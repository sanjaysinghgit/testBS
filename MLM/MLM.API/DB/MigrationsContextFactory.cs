using System.Data.Entity.Infrastructure;

namespace MLM.API.DB
{
    public class MigrationsContextFactory : IDbContextFactory<MLMDbContext>
    {
        public MLMDbContext Create()
        {
            return new MLMDbContext("MLMCon");
        }
    }
}