using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BemmTikTokv3
{
    class ReupTiktokTQ
    {
        int limitFile = 3, totalFileCount = 0, doneFileCount = 0;
        string secuid = "";
        string path = "";
        public ReupTiktokTQ(string secuid,string path, int limitFile = 3)
        {
            this.limitFile = limitFile;
            this.secuid = secuid;
            this.path = path;
        }
        public void Run()
        {
            this.DownloadLatestVideos();
            this.ReupVideos();
        }

        private void log(string content)
        {
            Console.WriteLine(content);
        }

        private void ReupVideos()
        {

        }

        public class MyVideo
        {
            public string Vid;
            public string Url;
            public string AuthorId;
        }

        public List<MyVideo> DownloadLatestVideos()
        {
           
                    VideoList result = new VideoList();
                    List<MyVideo> allVideos = new List<MyVideo>();
                    long maxCursor = 0;
                    do
                    {
                        result = getVideoUrls(secuid, maxCursor);
                if (result.AwemeList == null)
                return null;
                     foreach (var video in result.AwemeList)
                        {
                            try
                            {
                                MyVideo vd = new MyVideo();
                                vd.Url = video.Video.PlayAddr.UrlList[0].ToString();
                                vd.Vid = video.Video.Vid.ToString();
                                vd.AuthorId = video.Author.Uid.ToString();

                                // Nếu trong thư mục chưa tồn tại video này thì mới thêm vào list
                                if (!File.Exists(path +@"\"+  vd.Vid + ".mp4"))
                                {
                                    allVideos.Add(vd);
                                    if (allVideos.Count >= limitFile)
                                    {
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                                continue;
                            }

                        }
                        maxCursor = result.MaxCursor;
                    } while (result.HasMore == true && allVideos.Count < limitFile);

              
                    // Echo link
            
                    return allVideos ;
              //  }
          //  }
        }


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            log(e.ProgressPercentage.ToString() + "%");
        }

        public void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.doneFileCount++;
            log("Downloading " + doneFileCount.ToString() + "/" + totalFileCount.ToString() + " video(s)");
        }

        private bool DownloadFile(MyVideo video, string folderPath)
        {
            string filename = video.Vid + ".mp4";
            if (File.Exists(folderPath + @"\" + filename))
            {
                return true;
            }

            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += wc_DownloadFileCompleted;
                wc.DownloadFileAsync(
                    new System.Uri(video.Url),
                    folderPath + @"\" + filename
                );
            }
            return true;
        }


        private string RedirectPath(string url)
        {
            StringBuilder sb = new StringBuilder();
            string location = string.Copy(url);
            while (!string.IsNullOrWhiteSpace(location))
            {
                sb.AppendLine(location); // you can also use 'Append'
                HttpWebRequest request = HttpWebRequest.CreateHttp(location);
                request.AllowAutoRedirect = false;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    location = response.GetResponseHeader("Location");
                }
            }
            return sb.ToString();
        }

        private dynamic getVideoUrls(string secUid, long maxCursor)
        {
            try
            {
                var client = new RestClient("https://www.iesdouyin.com/web/api/v2/aweme/post/?sec_uid=" + secUid + "&count=36&max_cursor=" + maxCursor + "&aid=1128&_signature=xX5OIQAApdEdWiVTpkBHycV-Tj&dytk=");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("authority", "www.iesdouyin.com");
                request.AddHeader("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
                request.AddHeader("accept", "application/json");
                request.AddHeader("x-requested-with", "XMLHttpRequest");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                client.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.77 Safari/537.36";
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("referer", "https://www.iesdouyin.com/share/user/2453725739491389?u_code=4k279gcm95g&sec_uid=MS4wLjABAAAAAjbu9fOUwa2avjqLWBVH6OTwkYnHBV2O-5BgM3Cx2H07pGddbSh00risKRRWey-A&did=MS4wLjABAAAA_luHKKI_ChTUwxzlduNnHmmCutkbuQ0g1XqxMkEKqz0&iid=MS4wLjABAAAAX4sbL8HmnKYYIH3x3-OM3CL9pxfDi0MTBKphNFDMa3Y&with_sec_did=1&app=aweme&utm_campaign=client_share&utm_medium=ios&tt_from=copy&utm_source=copy");
                request.AddHeader("accept-language", "vi-VN,vi;q=0.9,en-US;q=0.8,en;q=0.7,zh-CN;q=0.6,zh;q=0.5");
                request.AddHeader("cookie", "_tea_utm_cache_1243={%22utm_source%22:%22copy%22%2C%22utm_medium%22:%22ios%22%2C%22utm_campaign%22:%22client_share%22}");
                IRestResponse response = client.Execute(request);

                VideoList videoList = JsonConvert.DeserializeObject<VideoList>(response.Content);
                return videoList;
            }
            catch
            {
                return new VideoList();
            }

        }
    }
    public class MyJob
    {
        public string TiktokTQLink = "";
        public int NumberOfVideo = 1;
    }
}
