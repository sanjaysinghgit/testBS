using System.Data.Entity;

namespace MLM.DB
{
    public interface IDatabaseFactory
    {
        DbContext Get(IConnectionRetriever connRetreiver);
    }
}
