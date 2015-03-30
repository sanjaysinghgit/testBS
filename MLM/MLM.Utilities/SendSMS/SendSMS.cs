using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Net;
using System.Text;
using System.Net;

namespace MLM.Utilities
{
    public class SendSMS
    {
        public string SendMessage(string Mobile_Number, string Message, [System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute("N")]  // ERROR: Optional parameters aren't supported in C#
string MType)
        {
            //Get values from config
            //ConfigurationManager.AppSettings["User"];
            string User = "brightfuture";
            string password = "brightfuture";
            string SenderName = "BRIGHT";
            string stringpost = "User=" + User + "&passwd=" + password + "&mobilenumber=" + Mobile_Number + "&message=" + Message + "&sid=" + SenderName + "&MTYPE=" + MType;
            //Response.Write(stringpost)
            string functionReturnValue = null;
            functionReturnValue = "";
            
            

            HttpWebRequest objWebRequest = null;
            HttpWebResponse objWebResponse = null;
            StreamWriter objStreamWriter = null;
            StreamReader objStreamReader = null;

            try
            {
                string stringResult = null;

                objWebRequest = (HttpWebRequest)WebRequest.Create("http://sms.clientsprojects.com/WebServiceSMS.aspx");
                //domain name: Domain name Replace With Your Domain  
                objWebRequest.Method = "POST";

                // Response.Write(objWebRequest)

                // Use below code if you want to SETUP PROXY.
                //Parameters to pass: 1. ProxyAddress 2. Port
                //You can find both the parameters in Connection settings of your internet explorer.


                // If You are In the proxy Then You Uncomment the below lines and Enter IP And Port Number


                //System.Net.WebProxy myProxy = new System.Net.WebProxy("192.168.1.108", 6666);
                //myProxy.BypassProxyOnLocal = true;
                //objWebRequest.Proxy = myProxy;

                objWebRequest.ContentType = "application/x-www-form-urlencoded";

                objStreamWriter = new StreamWriter(objWebRequest.GetRequestStream());
                objStreamWriter.Write(stringpost);
                objStreamWriter.Flush();
                objStreamWriter.Close();

                objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();


                objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();

                objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
                stringResult = objStreamReader.ReadToEnd();
                objStreamReader.Close();
                return (stringResult);
            }
            catch (System.Exception ex)
            {
                return (ex.ToString());

            }
            finally
            {
                if ((objStreamWriter != null))
                {
                    objStreamWriter.Close();
                }
                if ((objStreamReader != null))
                {
                    objStreamReader.Close();
                }
                objWebRequest = null;
                objWebResponse = null;

            }
        }

        //protected void btnsubmit_Click(object sender, System.EventArgs e)
        //{
        //    string str = null;
        //    str = SendSMS(User.Value, Passwd.Value, mobilenumber.Value, message.Value, "N");
        //    Response.Write(str);
        //}
    }
}
