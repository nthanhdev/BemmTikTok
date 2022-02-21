using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace BemmTikTokv3
{
    public class DBVideo
    {
       public bool check { get; set; }
      
        public string nameFile { get; set; }
        public string videoID { get; set; }
        public string userName { get; set; }
        public string time { get; set; }
        public string love { get; set; }
        public string cmt { get; set; }
        public string share { get; set; }
        public string trangthai { get; set; }
    }
    public class RQPAGE
    {
        public string user_id { get; set; }
        public string sec_uid { get; set; }
        public string cursor { get; set; }
        public string lang { get; set; }
        public int page { get; set; }
        public string x { get; set; }
    }
    public class RQPAGEHASH
    {
        public string hash { get; set; }
        public string id { get; set; }
        public string cursor { get; set; }
        public string lang { get; set; }
        public int page { get; set; }
        public string x { get; set; }
    }
    class RQVideo
    {
        private RQPAGE videoPage = new RQPAGE();
        private RQPAGEHASH videoHash = new RQPAGEHASH();

        public string max_cursor = "";
        public DBVideo toOjectVideo(bool check,  string nameFile , string videoID , string userName, string love, string cmt,string share, string des)
        {
           

            DBVideo result = new DBVideo()
            {
                check = check,
                nameFile = nameFile,
                videoID = videoID,
                userName = userName,
                love = love,
                cmt = cmt,
                share = share,
                trangthai = des
            };
            return result;
        }
        private void setvideoPage(string data)
        {
           videoPage.user_id = regEx(@"(?<=data-user-id="").*?(?="")", data);
            videoPage.sec_uid = regEx(@"(?<=data-sec-uid="").*?(?="")", data);
            videoPage.cursor = regEx(@"(?<=data-cursor="").*?(?="")", data);
            videoPage.x = regEx(@"(?<=data-x="").*?(?="")", data);
            videoPage.lang = regEx(@"(?<=data-lang"").*?(?="")", data);
            videoPage.page = 2;
        }
        private void setvideoHash(string data)
        {
            videoHash.hash= regEx(@"(?<=data-hash="").*?(?="")", data);
            videoHash.id = regEx(@"(?<=data-id="").*?(?="")", data);
            videoHash.cursor = regEx(@"(?<=data-cursor="").*?(?="")", data);
            videoHash.x = regEx(@"(?<=data-x="").*?(?="")", data);
            videoHash.lang = regEx(@"(?<=data-lang"").*?(?="")", data);
            videoHash.page = 2;
        }



        public string getUrlDown(string id)
        {
         
              var Client = new RestClient("https://tikmate.online/download_plugin.php");
    
            var reQuest = new RestRequest(Method.POST);
            reQuest.AddHeader("content-type", "application/json");
            reQuest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; rv:23.0) Gecko/20100101 Firefox/23.0");
            reQuest.AddHeader("referer", "https://tikmate.online/dl_plugin.php?video="+id+"?src=profile&utm_source=extension-profile");
            var restResponse = Client.Execute(reQuest);
            var text = (restResponse.Content).ToString();
            text = regEx(@"(?<=download_video_Source).*?(?=>)", text);
            text = regEx(@"(?<=href="").*?(?="")", text);
            return text;
        }
        private string getthumbpage(int page, bool isByuser)
        {
            string result = "";
         
            var Client = new RestClient("https://urlebird.com/ajax/");
            var reQuest = new RestRequest(method: Method.POST);
            reQuest.AddHeader("content-type", "application/x-www-form-urlencoded");
            if (isByuser)
            {
                videoPage.page = page + 1;
                string json = JsonConvert.SerializeObject(videoPage);
                reQuest.AddParameter("action", "user");
                reQuest.AddParameter("data", json);
            }
            else
            {
                videoHash.page = page ;
                string json = JsonConvert.SerializeObject(videoHash);
                reQuest.AddParameter("action", "hash");
                reQuest.AddParameter("data", json);
            }

            var response = Client.Execute(reQuest);
            result = (response.Content).ToString();
            if (result.Contains("no videos"))
            {
                result = "no videos";
            }
            else
            {
                if (isByuser)
                {
                    videoPage.cursor = regEx(@"(?<=cursor"":"").*?(?="")", result);
                    videoPage.x = regEx(@"(?<=x"":"").*?(?="")", result);
                }
                else
                {
                    videoHash.cursor= regEx(@"(?<=cursor"":).*?(?=,)", result);
                    videoHash.x = regEx(@"(?<=x"":"").*?(?="")", result); 
                }

            }
            return result;
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
        public List<DBVideo> getByi4(bool isByUser, string value, int page = 0)
        {
            List<DBVideo> result = new List<DBVideo>();
            string  username = "",  Id = "" ,love = "", cmt = "", share = "";
            string url = "https://urlebird.com/user/" + value + "/";
            if (!isByUser) url = value ;
            
            using (var web = new WebClient())
            {
                try
                {
                    string data = "";
                    if (page == 0)
                    {
                        web.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");

                        data = web.DownloadString(url);
                        if (isByUser)
                        {
                            setvideoPage(data);

                        }
                        else
                        {
                            setvideoHash(data);
                        }

                    }
                    else
                    {
                        data =   getthumbpage(page,isByUser);
                        if (data == "no videos") { return null; }
                        // data = data.Replace(@"\""thumb\""", @"""thumb""");
                        data = data.Replace(@"\", "");
                    }
                    MatchCollection collection = getThump(true, data);

                    if (collection.Count != 0)
                    {
                        foreach (Match item in collection)
                        {
                            string dataq = item.ToString();

                            username = regEx(@"(?<=user/).*?(?=/)", dataq);
                            Id = regEx(@"(?<=video/).*?(?=/)", dataq);
                            string time = "";
                            Id = Id.Substring(Id.Length - 19, 19);
                            MatchCollection traffic = getThump(false, dataq);
                            int i = 1;
                            foreach (var match in traffic)
                            {
                                if (i == 1)
                                {
                                    time = match.ToString();
                                    i++;

                                }
                                else if (i == 2)
                                {
                                    love = match.ToString();
                                    i++;
                                }
                                else if (i == 3)
                                {
                                    cmt = match.ToString();
                                    i++;
                                }
                                else share = match.ToString();
                              
                            }

                            DBVideo i4video = new DBVideo()
                            {
                                check = false,
                                nameFile = "by_" + username + Id.Substring(16, 3),
                                videoID = Id,
                                userName = username,
                                time = time,
                                love = love,
                                cmt = cmt,
                                share = share,
                                trangthai = "Có thể tải"
                            };
                            result.Add(i4video);
                        }
                    }
                  
                }
                catch 
                {
                    return result;
                }
               
              

            }
            return result;
        }
        MatchCollection getThump(bool isthumb,  string data)
        {
            string query = "";
            if (isthumb) query = @"(?<=class=""thumb wc"">).*?(?=overlay)";
            else query = @"(?<=></i> ).*?(?=<)";
            Regex regex = new Regex(query, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection matchCollection = regex.Matches(data);
  
            return matchCollection;

        }
        public DBVideo GetbyVideoID(string Id)
        {
             string url = "https://m.tiktok.com/v/" + Id;
          //  try
          //  {
        
                using (WebClient web = new WebClient())
                {
                web.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");

                string data = web.DownloadString(url);
                    string query = "";
                    string des = "", username = "", love = "", cmt = "", share = "";
                    for (int i = 0; i <= 3; i++)
                    {
                        if (i == 0) query = @"(?<=<title>).*?(?=<)"; // lay hashtag
                        else if (i == 1) query = @"(?<={""diggCount"":).*?(?=,)"; // lay love
                        else if (i == 2) query = @"(?<=""commentCount"":).*?(?=,)"; // lay cmt
                        else if (i == 3) query = @"(?<=com/@).*?(?=/)"; // lay username

                         Regex regex = new Regex(query, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                        MatchCollection matchCollection = regex.Matches(data);
                        foreach (Match item in matchCollection)
                        {
                            if (i == 0) des = item.ToString();
                            else if (i == 1) love = item.ToString();
                            else if (i == 2) cmt = item.ToString();
                            else if (i == 3) username = item.ToString();
                        break;
                        }
                    }
                    return toOjectVideo(true, "by_" + username + Id.Substring(16,3), Id, username, love, cmt,share, des);

                }
         //   }
          //  catch (Exception)
          //  {
          //      return null;
           // }

        }

    }

}

//<div class="author"><a href="https://urlebird.com/user/hongtruc264/"><img src="https://p16-amd-va.tiktokcdn.com/tos-useast2a-avt-0068-aiso/bce811b76fd78c3d3bc42cfc911b3204~c5_100x100.jpeg?x-expires=1630069000&x-signature=GW8tTkFSL-G9l8ggkytVOM5V9AH%3D" alt="Hong Truc Nguyen"></a></div>
//<div class="author-name"><a href="https://urlebird.com/user/hongtruc264/">Hong Truc Nguyen</a><br><span class="ago">4 weeks ago</span></div>
//<div class="img"><img src="https://p16-amd-va.ibyteimg.site/obj/tos-maliva-p-0068/d45031147f02b1b0289e67c4ebe9cde5.jpg?x-expires=1630563147&x-signature=o27GwObTqHxpmj7z2q0h-T6XvZD5b5B1oP-Ec06N%3D" class="img-fluid" alt="@Hong Truc Nguyen"></div>
//<div class="info">
//<span><i class="far fa-heart" aria-hidden="true"></i> 5</span>
//<span><i class="far fa-comment" aria-hidden="true"></i> 0</span>
//<span><i class="far fa-share-square" aria-hidden="true"></i> 0</span>
//</div>
//<a href="https://urlebird.com/video/6990339650275544347/"><div class="