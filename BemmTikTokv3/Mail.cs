using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Threading;

namespace BemmTikTokv3
{
    public class GetItem
    {
        public string items { get; set; }
        public int code { get; set; }
        public string msg { get; set; }
    }
    public class GetToken
    {
        public string Token { get; set; }
    }
    public class domain
    {
        public string dot { get; set; }
        public string gid { get; set; }

    }
    public class getCode
    {
        public Dictionary<string, string> textSubject;
    }

    public class WarframeStats
    {
        public Dictionary<string, string> items;
    }
    public class List
    {
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string mid { get; set; }

    }
    public class read
    {
        public string body { get; set; }
    }
    public class infoTikTok
    {
        public string userID { get; set; }

        public string password { get; set; }

        public string pathBackup { get; set; }

        public string cookie { get; set; }

    }
    public class userID
    {
        public string link { get; set; }

        public string name { get; set; }

        public bool follow { get; set; }

        public bool tuongtac { get; set; }

        public bool cmt { get; set; }

        public bool love { get; set;}

        public int sovideo { get; set; }

        public int time { get; set; }

        public bool kichhoat { get; set; }

    }

    public class videoID
    {
        public string link { get; set; }

        public string name { get; set; }

        public bool follow { get; set; }

        public bool cmt { get; set; }

        public bool love { get; set; }

        public int time { get; set; }

        public bool kichhoat { get; set; }
    }
    class Mail
    {
        string key;
        string gid;
        string domain;
        public Mail(string domain, string gid)
        {

            this.domain = domain;
            this.gid = gid;

        }
        string regEx(string query, string data)
        {
            string result = "";
            Regex regex = new Regex(query, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection matchCollection = regex.Matches(data);
            foreach (Match item in matchCollection)
            {
                result = item.ToString();
                break;
            }
            return result;
        }


        public   string getitemMail(string username)
        {

            try
            {
                string domain = this.domain;
                var restclient = new RestClient("https://smailpro.com/app/key");
                var restRequest = new RestRequest(Method.POST);
                var body = new { domain = domain, username = username};
                restRequest.AddHeader("content-type", "application/json");
                restRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");
                restRequest.AddHeader("cookie", "_ga=GA1.2.337157076.1626259413; _ga_GSFQ1G81R7=GS1.1.1627187390.6.1.1627187457.0; _gid=GA1.2.1811321858.1637045179; _gat_gtag_UA_197133567_1=1; XSRF-TOKEN=eyJpdiI6InRVZDZ1VDBqcVlQY3Eva0xMZ3hyQ2c9PSIsInZhbHVlIjoiRkJJWWY3ZEhqK0dYbnNXWEY4MVJ0MFBFM0w3dU5hUzZRUzdLaWVxYjM3bXFmZjlhV01OUHdpazd0K053RVpiZGdZc2xpdHBWc1NQS0poVG5uZ0tycnlTczJQRkRVNk9jZksrNTBxVllmc05NUVN4UHIwUlZ0Y1hrZnpHZkg3RU0iLCJtYWMiOiJmNmYzM2YxZDg5NmQwOTUwMjI1OGY1Yzk1MTNkMmJhYWMxOTRhNmRkZTk1YWZmNjkxMzI4YmY5YjQxNGQ2YzcxIn0%3D; sonjj_session=eyJpdiI6InRRcGZtMStHRVNmZlJESXgwbk5ld0E9PSIsInZhbHVlIjoiRzczaXNQczBjVk1xUEZZRUxieitKbEFxSFNDVVVkcURRVldrWjZjSDQwUGpDNSt2ODFjUmJMYTFjaGxySm9kTHozK2lOMjlaS0ZyZ3g1WG1mbUp1NjQxMlBmeHRjcC9GMFVpd2lXbjhXaHVyQUhjL3RzZ1dId0pYelplNFFEYzMiLCJtYWMiOiJjYTViMTUxM2I3MzcwMGIwYjE2YmMwZDdmYzc1ZDQxNTgxMjc5MTcwMzdjOTM4ZDIxYzBhNGE3MmRiZDUxY2MxIn0%3D");
                restRequest.AddHeader("x-xsrf-token", "eyJpdiI6InRVZDZ1VDBqcVlQY3Eva0xMZ3hyQ2c9PSIsInZhbHVlIjoiRkJJWWY3ZEhqK0dYbnNXWEY4MVJ0MFBFM0w3dU5hUzZRUzdLaWVxYjM3bXFmZjlhV01OUHdpazd0K053RVpiZGdZc2xpdHBWc1NQS0poVG5uZ0tycnlTczJQRkRVNk9jZksrNTBxVllmc05NUVN4UHIwUlZ0Y1hrZnpHZkg3RU0iLCJtYWMiOiJmNmYzM2YxZDg5NmQwOTUwMjI1OGY1Yzk1MTNkMmJhYWMxOTRhNmRkZTk1YWZmNjkxMzI4YmY5YjQxNGQ2YzcxIn0=");

                restRequest.AddJsonBody(body);

                IRestResponse restResponse = restclient.Execute(restRequest);
                var json = (restResponse.Content).ToString();
                GetItem get = JsonConvert.DeserializeObject<GetItem>(json);
                string item = get.items;


                string data = "https://public-sonjj.p.rapidapi.com/email/"+gid+"/get?key=@key&rapidapi-key=f871a22852mshc3ccc49e34af1e8p126682jsn734696f1f081&username="+username+"&domain=" + domain ;

                key = item;
                string result = "";
                data = data.Replace("@key", item);
                using (var web = new WebClient())
                {

                    web.Headers.Add("content-type", "application/json");
                    web.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");
                    web.Headers.Add("cookie", "_ga=GA1.2.337157076.1626259413; _ga_GSFQ1G81R7=GS1.1.1627187390.6.1.1627187457.0; _gid=GA1.2.1811321858.1637045179; _gat_gtag_UA_197133567_1=1; XSRF-TOKEN=eyJpdiI6InRVZDZ1VDBqcVlQY3Eva0xMZ3hyQ2c9PSIsInZhbHVlIjoiRkJJWWY3ZEhqK0dYbnNXWEY4MVJ0MFBFM0w3dU5hUzZRUzdLaWVxYjM3bXFmZjlhV01OUHdpazd0K053RVpiZGdZc2xpdHBWc1NQS0poVG5uZ0tycnlTczJQRkRVNk9jZksrNTBxVllmc05NUVN4UHIwUlZ0Y1hrZnpHZkg3RU0iLCJtYWMiOiJmNmYzM2YxZDg5NmQwOTUwMjI1OGY1Yzk1MTNkMmJhYWMxOTRhNmRkZTk1YWZmNjkxMzI4YmY5YjQxNGQ2YzcxIn0%3D; sonjj_session=eyJpdiI6InRRcGZtMStHRVNmZlJESXgwbk5ld0E9PSIsInZhbHVlIjoiRzczaXNQczBjVk1xUEZZRUxieitKbEFxSFNDVVVkcURRVldrWjZjSDQwUGpDNSt2ODFjUmJMYTFjaGxySm9kTHozK2lOMjlaS0ZyZ3g1WG1mbUp1NjQxMlBmeHRjcC9GMFVpd2lXbjhXaHVyQUhjL3RzZ1dId0pYelplNFFEYzMiLCJtYWMiOiJjYTViMTUxM2I3MzcwMGIwYjE2YmMwZDdmYzc1ZDQxNTgxMjc5MTcwMzdjOTM4ZDIxYzBhNGE3MmRiZDUxY2MxIn0%3D");
                    web.Headers.Add("x-xsrf-token", "eyJpdiI6InRVZDZ1VDBqcVlQY3Eva0xMZ3hyQ2c9PSIsInZhbHVlIjoiRkJJWWY3ZEhqK0dYbnNXWEY4MVJ0MFBFM0w3dU5hUzZRUzdLaWVxYjM3bXFmZjlhV01OUHdpazd0K053RVpiZGdZc2xpdHBWc1NQS0poVG5uZ0tycnlTczJQRkRVNk9jZksrNTBxVllmc05NUVN4UHIwUlZ0Y1hrZnpHZkg3RU0iLCJtYWMiOiJmNmYzM2YxZDg5NmQwOTUwMjI1OGY1Yzk1MTNkMmJhYWMxOTRhNmRkZTk1YWZmNjkxMzI4YmY5YjQxNGQ2YzcxIn0=");
                    result = web.DownloadString(data);
                    result = regEx(@"(?<=email"":"").*?(?="")", result);
                }
                return result;
            }
            catch (Exception e)
            {
                string a = e.Message;
                return "";
            }
           
     
        }
        string getkeyread(string email,string mid)
        {

            //1627049016
            var restclient = new RestClient("https://smailpro.com/app/key");
            var restRequest = new RestRequest(Method.POST);
            restRequest.AddHeader("content-type", "application/json");
            restRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");

            var body = new { email = email, message_id = mid };
            restRequest.AddJsonBody(body);


            IRestResponse restResponse = restclient.Execute(restRequest);
            var json = (restResponse.Content).ToString();
            GetItem get = JsonConvert.DeserializeObject<GetItem>(json);
            string item = get.items;

            return item;
        }
        string getKey(string email)
        {

            //1627049016
            var restclient = new RestClient("https://smailpro.com/app/key");
            var restRequest = new RestRequest(Method.POST);
            restRequest.AddHeader("content-type", "application/json");
            restRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");
           
            var body = new { email = email, timestamp = "1630997915" };
            restRequest.AddJsonBody(body);


            IRestResponse restResponse = restclient.Execute(restRequest);
            var json = (restResponse.Content).ToString();
            GetItem get = JsonConvert.DeserializeObject<GetItem>(json);
            string item = get.items;

            return item;
        }
        string read(string key , string domain, string mid)
        {
            string data = "https://public-sonjj.p.rapidapi.com/email/"+gid+"/read?key=@key&rapidapi-key=f871a22852mshc3ccc49e34af1e8p126682jsn734696f1f081&email=@email&message_id=@mid";

            data = data.Replace("@key", key);
            data = data.Replace("@email", domain);
            data = data.Replace("@mid", mid);

            var restclient = new RestClient(data);
            var restRequest = new RestRequest(Method.OPTIONS);
            restRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");
            restRequest.AddHeader("Access-Control-Request-Headers", "x-rapidapi-ua");
            restRequest.AddHeader("Access-Control-Request-Method", "GET");

            restclient.Execute(restRequest);
            restRequest = new RestRequest(Method.GET);
            restRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");
            restRequest.AddHeader("x-rapidapi-ua", "RapidAPI-Playground");
   

            restRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");

            var aa = restclient.Execute(restRequest);
            var json = (aa.Content).ToString();

            return json;
        }
      public  string WailgetCode(string email,bool iscode = false)
        {
            List getitem;
            int i = 0;
            try
            {
                do
                {
                    i++;
                    if (i == 7)
                    {
                        return "";

                    }
                    Thread.Sleep(10000);
                    string key = getKey(email);
                    string data = "https://public-sonjj.p.rapidapi.com/email/"+gid+"/check?key=@key&rapidapi-key=f871a22852mshc3ccc49e34af1e8p126682jsn734696f1f081&email=@email&timestamp=1627049016";
                    data = data.Replace("@key", key);
                    data = data.Replace("@email", email);

                    var restclient = new RestClient(data);
                    var restRequest = new RestRequest(Method.GET);
                    restRequest.AddHeader("content-type", "application/json");
                    restRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");

                    var restResponse = restclient.Execute(restRequest);
                    var json = (restResponse.Content).ToString();

                    getitem = JsonConvert.DeserializeObject<List>(json);
                    if (getitem.items is null)
                      return "";
                    
                }  while (getitem.items.Count == 0);

                string mid = getitem.items[0].mid;

                string keyread = getkeyread(email, mid);
                string jdata = read(keyread, email, mid);
                if (iscode)
                {
                    return regEx(@"(?<=""body"":"").*?(?=""})", jdata);
                }

                return regEx(@"(?<=\"">  ).*?(?= )", jdata);
            }
            catch (Exception)
            {

                return "";
            }

        }

    }
    public class user
    {
        
       public string uid { get; set; }
       public string time { get; set; }
       public string key { get; set; }
      public  string note { get; set; }
    }

}
