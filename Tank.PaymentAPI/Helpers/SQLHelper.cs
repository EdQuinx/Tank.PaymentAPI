using Microsoft.EntityFrameworkCore;

namespace Tank.PaymentAPI.Helpers
{
    public static class SQLHelper
    {
        public static DbContext CloneSQL(DbContext dbContext, string connectionString)
        {
            dbContext.Database.CloseConnection();//dong ket noi hien tai
            dbContext.Database.SetConnectionString(connectionString);//cau hinh lai chuoi ket noi
            dbContext.Database.OpenConnection();//mo lai ket noi
            return dbContext;
        }
    }
}
