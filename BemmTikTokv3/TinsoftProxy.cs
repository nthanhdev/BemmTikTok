using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Proxy_Client_Tinsoft
{
    class TinsoftProxy
    {
        public string api_key { get; set; }
        public string proxy { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public int timeout { get; set; }
        public int next_change { get; set; }
        public string errorCode = "";
        public int location { get; set; }
        private string svUrl = "http://proxy.tinsoftsv.com";
        private int lastRequest = 0;

        public TinsoftProxy(string api_key, int location = 0)
        {
            this.api_key = api_key;
            this.proxy = "";
            this.ip = "";
            this.port = 0;
            this.timeout = 0;
            this.next_change = 0;
            this.location = location;
        }
        public bool changeProxy()
        {
            if (checkLastRequest())
            {
                errorCode = "";
                this.next_change = 0;
                this.proxy = "";
                this.ip = "";
                this.port = 0;
                this.timeout = 0;
                string rs = getSVContent(svUrl + "/api/changeProxy.php?key=" + this.api_key + "&location=" + this.location);
                if (rs != "")
                {
                    try
                    {
                        JObject rsObject = JObject.Parse(rs);
                        if (bool.Parse(rsObject["success"].ToString()))
                        {
                            this.proxy = rsObject["proxy"].ToString();
                            string[] proxyArr = this.proxy.Split(':');
                            this.ip = proxyArr[0];
                            this.port = int.Parse(proxyArr[1]);
                            this.timeout = int.Parse(rsObject["timeout"].ToString());
                            this.next_change = int.Parse(rsObject["next_change"].ToString());
                            this.errorCode = "";
                            return true;
                        }
                        else
                        {
                            this.errorCode = rsObject["description"].ToString();
                        }
                    }
                    catch { }
                }
                else
                {
                    this.errorCode = "request server timeout!";
                }
            }
            else
            {
                errorCode = "Request so fast!";
            }
            return false;
        }
        public void stopProxy()
        {
            errorCode = "";
            this.proxy = "";
            this.ip = "";
            this.port = 0;
            this.timeout = 0;
            if (this.api_key != "")
            {
                getSVContent(svUrl + "/api/stopProxy.php?key=" + this.api_key);
            }


        }
        public bool getProxyStatus()
        {
            if (checkLastRequest())
            {
                errorCode = "";
                this.proxy = "";
                this.ip = "";
                this.port = 0;
                this.timeout = 0;
                string rs = getSVContent(svUrl + "/api/getProxy.php?key=" + this.api_key);
                if (rs != "")
                {
                    try
                    {
                        JObject rsObject = JObject.Parse(rs);
                        if (bool.Parse(rsObject["success"].ToString()))
                        {
                            this.proxy = rsObject["proxy"].ToString();
                            string[] proxyArr = this.proxy.Split(':');
                            this.ip = proxyArr[0];
                            this.port = int.Parse(proxyArr[1]);
                            this.timeout = int.Parse(rsObject["timeout"].ToString());
                            this.next_change = int.Parse(rsObject["next_change"].ToString());
                            this.errorCode = "";
                            return true;
                        }
                        else
                        {
                            this.errorCode = rsObject["description"].ToString();
                        }
                    }
                    catch { }
                }
            }
            else
            {
                errorCode = "Request so fast!";
            }

            return false;
        }
        private bool checkLastRequest()
        {

            try
            {
                DateTime centuryBegin = new DateTime(2001, 1, 1);
                DateTime currentDate = DateTime.Now;

                long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                int now = (int)elapsedSpan.TotalSeconds;

                if ((now - lastRequest) >= 10)
                {

                    lastRequest = now;

                    return true;
                }
            }
            catch { }

            return false;
        }
        private string getSVContent(string url)
        {
            Console.WriteLine(url);
            string rs = "";
            try
            {

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                  "Windows NT 5.2; .NET CLR 1.0.3705;)");
                    rs = client.DownloadString(url);
                }
                if (string.IsNullOrEmpty(rs)) { rs = ""; }

            }
            catch { rs = ""; }
            return rs;
        }

    }
}
