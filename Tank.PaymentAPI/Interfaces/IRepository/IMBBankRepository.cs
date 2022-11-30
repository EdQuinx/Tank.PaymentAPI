using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tank.PaymentAPI.Interfaces.IRepository
{
    public interface IMBBankRepository
    {
        int PaymentState(string userName, string code, int serverID);
        Task<string> GeneratePaymentCode(int amount);
    }
}
