using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Tank.PaymentAPI.Datas;
using Tank.PaymentAPI.Helpers;
using Tank.PaymentAPI.Interfaces.IModels;
using Tank.PaymentAPI.Interfaces.IRepository;
using Tank.PaymentAPI.Models;
using Tank.PaymentAPI.Services;

namespace Tank.PaymentAPI.Interfaces
{
    public class MBBankRepository : IMBBankRepository
    {
        private readonly MyDbWebContext _contextWeb;
        private readonly MyDbTankContext _contextTank;
        private readonly AppSetting _appSettings;
        private readonly TankSetting _tankSetting;

        public MBBankRepository(MyDbWebContext contextWeb, MyDbTankContext contextTank, IOptionsMonitor<AppSetting> appMonitor, IOptionsMonitor<TankSetting> tankMonitor)
        {
            _contextWeb = contextWeb;
            _contextTank = contextTank;
            _appSettings = appMonitor.CurrentValue;
            _tankSetting = tankMonitor.CurrentValue;
        }
        private DateTime ConvertDateTime(dynamic needConvert)
        {
            string[] splitDateTimes = Convert.ToString(needConvert).Split(' ');//0 - date, 1 - time
            string[] splitDates = splitDateTimes[0].Split('/');
            string[] splitTimes = splitDateTimes[1].Split(':');
            string completeString = string.Format("{0}-{1}-{2} {3}", splitDates[2], splitDates[1], splitDates[0], splitDateTimes[1]);
            DateTime dateTime = new DateTime();
            DateTime.TryParse(completeString, out dateTime);
            return dateTime;//DateTime.ParseExact(completeString, "G", CultureInfo.CurrentUICulture.DateTimeFormat);

        }
        private async void LoadHistoryTransaction()
        {
            //decrypt and request httpclient
            string decryptAccountID = BaseInterface.AESDecrypt(_appSettings.APIMBBank_AccountID);//giai ma accountid
            string decryptPassword = BaseInterface.AESDecrypt(_appSettings.APIMBBank_Pwd);//giai ma pwd
            string requestUrl = string.Format("{0}/{1}/{2}/{3}", _appSettings.APIMBBank_Url, decryptPassword, decryptAccountID, _appSettings.APIMBBank_Token);//url/pass/id/token

            //json parse
            var jsonParse = JsonConvert.DeserializeObject<dynamic>(BaseInterface.RequestContent(requestUrl));
            if (jsonParse.message != "Thành công")//return another success
                return;

            //fill to list
            foreach (var transaction in jsonParse.transactions)
            {
                string cID = transaction.transactionID;//dynamic to string
                string cType = transaction.type;//OUT = continue
                int cAmount = Convert.ToInt32(transaction.amount);
                DateTime cDateTime = ConvertDateTime(transaction.transactionDate);
                if (_contextWeb.MBBankModels.Any(e => e.TransactionID == cID) || cType == "OUT")//PK check
                    continue;
                await _contextWeb.MBBankModels.AddAsync(new MBBankModel
                {
                    TransactionID = transaction.transactionID,
                    Amount = cAmount,
                    Description = transaction.description,
                    TransactionDate = cDateTime,
                    Type = transaction.type,
                    Checked = false
                });
            }
            //add to database
            await _contextWeb.SaveChangesAsync();
        }
        private int CheckDatabase(string code, int serverID, ref ServerListModel serverList, ref ChargeValueModel chargeValue)
        {
            serverList = _contextWeb.ServerLists.Where(e => e.Id == serverID).SingleOrDefault();
            if (serverList == null)
                return (int)eDatabaseType.SERVER_NOTFOUND;

            var paymentCode = _contextWeb.PaymentCodes.Where(e => e.Code == code).SingleOrDefault();
            if (paymentCode == null)
                return (int)eDatabaseType.PAYMENT_CODE_NOTFOUND;

            if (paymentCode.EndTime < DateTime.UtcNow)
                return (int)eDatabaseType.PAYMENT_CODE_EXPIRED;

            //check bank
            LoadHistoryTransaction();
            var transaction = _contextWeb.MBBankModels.Where(e => e.Description.Contains(code)).SingleOrDefault();
            if (transaction == null)
                return (int)eDatabaseType.TRANSACTION_NOTFOUND;

            paymentCode.EndTime = DateTime.UtcNow.AddMinutes(-20);
            _contextWeb.SaveChanges();

            chargeValue = _contextWeb.ChargeValues.Where(e => e.RealAmount == paymentCode.Amount).SingleOrDefault();
            if (chargeValue == null || chargeValue.GameAmount <= 0)
                return (int)eDatabaseType.AMOUNT_VALUE_NOTFOUND;

            return (int)eDatabaseType.SUCCESS;
        }
        #region Methods
        public async Task<string> GeneratePaymentCode(int amount)
        {
            Random random = new Random();
            string code = random.Next(0, 1000000).ToString("D6");

            //check exist
            var amountExist = _contextWeb.ChargeValues.Where(e => e.RealAmount == amount).SingleOrDefault();
            if (amountExist == null)
                return "false1";

            var paymentExist = _contextWeb.PaymentCodes.Where(e => e.Code == code).SingleOrDefault();
            if (paymentExist != null)
            {
                if (paymentExist.EndTime > DateTime.UtcNow)
                    return "false2";

                paymentExist.EndTime = DateTime.UtcNow.AddMinutes(5);
                paymentExist.Amount = amount;

                await _contextWeb.SaveChangesAsync();
                return string.Format("{0},{1}", paymentExist.Code, paymentExist.EndTime.ToString("dd-MM-yyyy hh:mm:ss"));
            }

            //add to db
            PaymentCodeModel paymentCode = new PaymentCodeModel()
            {
                Code = code,
                Amount = amount,
                EndTime = DateTime.UtcNow.AddMinutes(5)
            };
            await _contextWeb.PaymentCodes.AddAsync(paymentCode);
            await _contextWeb.SaveChangesAsync();
            return string.Format("{0},{1}", paymentCode.Code, paymentCode.EndTime.ToString("dd-MM-yyyy hh:mm:ss"));
        }
        public List<IMBBankModel> GetAllTransaction()
        {
            var transactions = _contextWeb.MBBankModels.Select(e => new IMBBankModel
            {
                TransactionID = e.TransactionID,
                Amount = e.Amount,
                Description = e.Description,
                TransactionDate = e.TransactionDate,
                Checked = e.Checked
            }).ToList();
            return transactions.ToList();
        }

        public IMBBankModel GetSingleTransactionById(string transactionID)
        {
            var data = _contextWeb.MBBankModels.SingleOrDefault(e => e.TransactionID == transactionID.Trim());
            if (data == null)
                return null;
            return new IMBBankModel
            {
                TransactionID = data.TransactionID,
                Amount = data.Amount,
                Description = data.Description,
                TransactionDate = data.TransactionDate,
                Checked = data.Checked
            };
        }

        public void Update(IMBBankModel transaction)
        {
            var data = _contextWeb.MBBankModels.Where(e => e.TransactionID == transaction.TransactionID.Trim()).SingleOrDefault();
            if (data != null)
            {
                data.Amount = transaction.Amount;
                data.Description = transaction.Description;
                data.TransactionDate = transaction.TransactionDate;
                data.Checked = transaction.Checked;
            }
            _contextWeb.SaveChanges();
        }
        public int PaymentState(string userName, string code, int serverID)
        {
            //khai báo
            ServerListModel serverList = new ServerListModel();
            ChargeValueModel chargeValue = new ChargeValueModel();

            string connectString = "";
            string dataSource = "127.0.0.1";
            string catalog = "";
            string userID = "sa";
            string password = "123456";
            int port = 1433;

            //check database web
            eDatabaseType databaseType = (eDatabaseType)CheckDatabase(code, serverID, ref serverList, ref chargeValue);
            if (databaseType != eDatabaseType.SUCCESS)
                return (int)databaseType;

            dataSource = serverList.DataSource;
            port = serverList.Port.GetValueOrDefault();
            catalog = serverList.Catalog;
            userID = serverList.UserId;
            password = serverList.Password;
            connectString = string.Format(_tankSetting.Tank_ConnectionString, dataSource, port, catalog, userID, password);

            try
            {
                //check database tank/user
                _contextTank.Database.CloseConnection();//dong ket noi hien tai
                _contextTank.Database.SetConnectionString(connectString);//cau hinh lai chuoi ket noi
                _contextTank.Database.OpenConnection();//mo lai ket noi

                SysUsersDetailModel userInfo = _contextTank.SysUsersDetails.Where(e => e.UserName == userName).SingleOrDefault<SysUsersDetailModel>();
                int totalCharge =  _contextTank.ChargeMoneys.Count();
                if (userInfo == null)
                    return (int)eDatabaseType.USER_NOTFOUND;

                string chargeID = string.Format("{0}-{1}-{2}", userInfo.NickName, code, totalCharge);
                ChargeMoneyModel chargeMoney = new ChargeMoneyModel
                {
                    ChargeId = chargeID,
                    UserName = userName,
                    Money = chargeValue.GameAmount,
                    CanUse = true,
                    Date = DateTime.UtcNow,
                    PayWay = "MBBank",
                    NeedMoney = chargeValue.RealAmount,
                    NickName = userInfo.NickName,
                };
                _contextTank.ChargeMoneys.Add(chargeMoney);
                _contextTank.SaveChanges();

                BaseInterface.RequestContent(string.Format(serverList.RequestUrl + _appSettings.ChargeMoney_Url, userInfo.UserId, chargeID));
                return (int)eDatabaseType.SUCCESS;
            }
            catch (Exception ex)
            {
                return (int)eDatabaseType.SYSTEM_ERROR;
            }
        }
        #endregion
    }
}
