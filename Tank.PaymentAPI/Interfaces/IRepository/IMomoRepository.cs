using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tank.PaymentAPI.Interfaces.IRepository
{
    public interface IMomoRepository
    {
        int PaymentState(string userName, long tranID, int serverID);
    }
}
