using System.Collections.Generic;
using System.Threading.Tasks;
using Tank.PaymentAPI.Interfaces.IModels;

namespace Tank.PaymentAPI.Interfaces.IRepository
{
    public interface IMBBankRepository
    {
        List<IMBBankModel> GetAllTransaction();
        IMBBankModel GetSingleTransactionById(string transactionID);
        void Update(IMBBankModel transaction);
        int PaymentState(string userName, string code, int serverID);
        Task<string> GeneratePaymentCode(int amount);
    }
}
