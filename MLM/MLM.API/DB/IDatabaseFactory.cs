using System.Data.Entity;

namespace MLM.API.DB
{
    public interface IDatabaseFactory
    {
        DbContext Get(IConnectionRetriever connRetreiver);
    }
}
