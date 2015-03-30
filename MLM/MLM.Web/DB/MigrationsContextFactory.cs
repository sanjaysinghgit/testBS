using System.Data.Entity.Infrastructure;

namespace MLM.DB
{
    public class MigrationsContextFactory : IDbContextFactory<MLMDbContext>
    {
        public MLMDbContext Create()
        {
            return new MLMDbContext("MLMCon");
        }
    }
}