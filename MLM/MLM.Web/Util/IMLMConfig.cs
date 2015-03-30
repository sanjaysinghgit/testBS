using System.Web;

namespace MLM.Util
{
    public interface IMLMConfig
    {
        string GetSetting(string settingName, HttpContextBase context);
        string GetSetting(string settingName);
    }
}