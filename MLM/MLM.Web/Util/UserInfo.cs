using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MLM.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MLM.Util
{
    public class UserInfo
    {
        private readonly IClientFactory _clientFactory;
        private readonly IMLMConfig _bazingaConfig;

        public UserInfo(IClientFactory clientFactory, IMLMConfig bazingaConfig)
        {
            _clientFactory = clientFactory;
            _bazingaConfig = bazingaConfig;
        }
        //public Person GetCurrentUserInfo(HttpContextBase context)
        //{
        //    HttpClient client = _clientFactory.Create(context);
        //    string url = "/data/users/self";
        //    var userResponse = client.GetAsync(UrlBuilder.DataServiceUrl(_bazingaConfig.GetSetting("ProxyBasePath"),
        //    _bazingaConfig.GetSetting("ApiBasePath"), url)).Result;
        //    if (!userResponse.IsSuccessStatusCode)
        //        throw new HttpResponseException(userResponse.StatusCode);
        //    var userInfo = userResponse.Content.ReadAsStringAsync().Result;
        //    JObject userJsonObject = JObject.Parse(userInfo);
        //    Person currentPerson = JsonConvert.DeserializeObject<Person>(userInfo);
        //    return currentPerson;
        //}
    }
}