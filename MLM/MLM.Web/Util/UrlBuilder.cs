using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Util
{
    public static class UrlBuilder
    {
        public static string DataServiceUrl(string basePath, string apiBasePath,string path)
        {
            return path.Replace(basePath,apiBasePath);
        }

        public static Uri FullUrl(string host, string basePath, string apiBasePath, string path) {
            return new Uri(string.Format("{0}{1}",host,DataServiceUrl(basePath,apiBasePath,path)));
        }
    }
}
