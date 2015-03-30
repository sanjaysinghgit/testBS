using MLM.Util.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Optimization;
using System.Web.Razor.Text;
using YamlDotNet.RepresentationModel;

namespace MLM.Util
{
    public static class Localization
    {
        public static Bundle LocalizationJs(this Bundle bundle, string lang)
        {
            //bundle.Include("~/App/localization/en.js");
            var sr = new StringBuilder();
            var yaml = new YamlStream();


            var rootpath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(rootpath,"App\\localization\\resources\\en.yml");


            if (!File.Exists(path))
                return bundle;
            yaml.Load(new StreamReader(path));
            var fileHash = SecurityUtil.ComputeMD5Hash(yaml.Documents[0].RootNode.ToString());
            var jsFileName = string.Format("{0}_en.js", fileHash);
            var filePath = System.IO.Path.Combine(rootpath,string.Format("App\\localization\\{0}",jsFileName));
            if (File.Exists(filePath))
            {
                bundle.Include(string.Format("~/App/Localization/{0}", jsFileName));
                return bundle;
            }
        
                
            sr.AppendLine("var resources = {}");
            parseNode((YamlMappingNode) yaml.Documents[0].RootNode,sr);
            //sr.AppendLine("function t(key){return resources[key];}");
            using (var outfile = new StreamWriter(filePath))
            {
                outfile.Write(sr.ToString());
            }
            
            bundle.Include(string.Format("~/App/Localization/{0}",jsFileName));
            return bundle;
        }

        private static void parseNode(YamlMappingNode node, StringBuilder sr, string parent = "")
        {
            
            foreach (var child in node.Children)
            {
                if (child.Value.GetType().Name == "YamlMappingNode")
                {
                    if (parent != "" && !parent.EndsWith("."))
                    {
                        parent = parent + ".";
                    }
                    var p = parent == "" ? child.Key.ToString() : parent + child.Key;
                    parseNode((YamlMappingNode) child.Value, sr,p);
                }
                else
                {
                    if (parent != ""&&!parent.EndsWith("."))
                        parent = parent + ".";
                    sr.AppendLine(string.Format("resources['{0}'] = \"{1}\";", parent + child.Key, child.Value));
                }

            }
        }
    }
}