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
using Tank.PaymentAPI.Interfaces.IRepository;
using Tank.PaymentAPI.Models;
using Tank.PaymentAPI.Services;

namespace Tank.PaymentAPI.Interfaces
{
    public class MomoRepository : IMomoRepository
    {
        private readonly MyDbWebContext _contextWeb;
        private readonly MyDbTankContext _contextTank;
        private readonly AppSetting _appSettings;
        private readonly TankSetting _tankSetting;

        public MomoRepository(MyDbWebContext contextWeb, MyDbTankContext contextTank, IOptionsMonitor<AppSetting> appMonitor, IOptionsMonitor<TankSetting> tankMonitor)
        {
            _contextWeb = contextWeb;
            _contextTank = contextTank;
            _appSettings = appMonitor.CurrentValue;
            _tankSetting = tankMonitor.CurrentValue;
        }
       
        private async void LoadHistoryTransaction()
        {
            //request http
            string requestUrl = string.Format("{0}/{1}", _appSettings.APIMomo_Url, _appSettings.APIMomo_Token);//url/token

            //json parse
            var jsonParse = JsonConvert.DeserializeObject<dynamic>(BaseInterface.RequestContent(requestUrl));
            var transList = jsonParse.momoMsg.tranList;
            if (transList.Count <= 0)//return object null
                return;

            //fill to list
            foreach (var transaction in transList)
            {
                long tranID = transaction.tranId;
                string userID = transaction.user;
                double unixTime = transaction.clientTime;
                string partnerID = transaction.partnerId;
                string partnerName = transaction.partnerName;
                int amount = transaction.amount;
                string comment = "Nạp game bằng MOMO";
                DateTime payTime = BaseInterface.UnixTimeStampToDateTime(unixTime);

                bool chk = _contextWeb.Momos.Any(e => e.TranID == tranID);
                if (chk || transaction.io == 0)//PK check
                    continue;
                await _contextWeb.Momos.AddAsync(new MomoModel
                {
                    TranID = tranID,
                    UserID = userID,
                    PartnerID = partnerID,
                    PartnerName = partnerName,
                    Amount = amount,
                    Comment = comment,
                    PayTime = payTime,
                    Checked = false
                });
            }
            //add to database
            await _contextWeb.SaveChangesAsync();
        }
        private int CheckDatabase(long tranId, int serverID, ref ServerListModel serverList, ref ChargeValueModel chargeValue)
        {
            serverList = _contextWeb.ServerLists.Where(e => e.Id == serverID).SingleOrDefault();
            if (serverList == null)
                return (int)eResponseType.SERVER_NOTFOUND;

            //check momo
            LoadHistoryTransaction();
            var transaction = _contextWeb.Momos.Where(e => e.TranID == tranId && !e.Checked).SingleOrDefault();
            if (transaction == null)
                return (int)eResponseType.TRANSACTION_NOTFOUND;

            chargeValue = _contextWeb.ChargeValues.Where(e => e.RealAmount == transaction.Amount).SingleOrDefault();
            if (chargeValue == null || chargeValue.GameAmount <= 0)
                return (int)eResponseType.AMOUNT_VALUE_NOTFOUND;

            return (int)eResponseType.SUCCESS;
        }
        #region Methods
  
        public int PaymentState(string userName, long tranID, int serverID)
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
            eResponseType databaseType = (eResponseType)CheckDatabase(tranID, serverID, ref serverList, ref chargeValue);
            if (databaseType != eResponseType.SUCCESS)
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
                    return (int)eResponseType.USER_NOTFOUND;

                string chargeID = string.Format("{0}-{1}-{2}", userInfo.NickName, tranID, totalCharge);
                ChargeMoneyModel chargeMoney = new ChargeMoneyModel
                {
                    ChargeId = chargeID,
                    UserName = userName,
                    Money = chargeValue.GameAmount,
                    CanUse = true,
                    Date = DateTime.Now.ToLocalTime(),
                    PayWay = "MoMo",
                    NeedMoney = chargeValue.RealAmount,
                    NickName = userInfo.NickName,
                };
                _contextTank.ChargeMoneys.Add(chargeMoney);
                
                var transaction = _contextWeb.Momos.Where(e => e.TranID == tranID).SingleOrDefault();
                transaction.Checked = true;
                _contextWeb.SaveChanges();
                _contextTank.SaveChanges();

                BaseInterface.RequestContent(string.Format(serverList.RequestUrl + _appSettings.ChargeMoney_Url, userInfo.UserId, chargeID));
                return (int)eResponseType.SUCCESS;
            }
            catch (Exception ex)
            {
                return (int)eResponseType.SYSTEM_ERROR;
            }
        }
        #endregion
    }
}
