namespace MLM.UtilityClasses
{
    public interface IImpersonationProvider
    {
        bool IsImpersonated();
        long GetImpersonated();
        long GetImpersonator();
        string GetAppDomain();
    }
}
