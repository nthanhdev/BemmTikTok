using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace BemmTikTokv3
{
    class Selenium
    {
        public ChromeDriver Driver;
        public bool congkhai, banbe, riengtu, cmt, duet, stitch;
        public Selenium()
        {
            ChromeOptions options = new ChromeOptions();
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            options.AddExcludedArgument("enable-automation");
            options.AddArgument("--window-size=500,600");
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Safari/537.36");
            options.AddArgument("--upgrade-insecure-requests=1");

            // options.AddArgument("--user-agent=Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) CriOS/56.0.2924.75 Mobile/14E5239e Safari/602.1");
            Driver = new ChromeDriver(chromeDriverService,options);
        }

        public void ClickByXpath(ChromeDriver driver, string Xpath)
        {
            var item = Driver.FindElement(By.XPath(Xpath));
            item.Click();
        }
        public List<Cookie> readCookie(string cookie)
        {
            List<Cookie> result = new List<Cookie>();
            foreach (var item in cookie.Split(';'))
            {
                string[] obj = item.Split('=');
                string key = obj[0];
                string value = obj[1];
                key = key.Replace(" ", "");
                value = value.Replace(" ", "");

                Cookie cki = new Cookie(key, value);
                result.Add(cki);
            }

            return result;
        }
      public string loginTikTok(string cookie)
        {
            try
            {
                List<Cookie> cookies = readCookie(cookie);
              
                Driver.Url = "https://tiktok.com";
                Driver.Navigate();
                foreach (var item in cookies)
                {
                    Driver.Manage().Cookies.AddCookie(item);
                }
                if (cookies.Count == 0)
                    return "không định dạng được cookie";
             
                Driver.Url = "https://www.tiktok.com/?lang=vi";
                Driver.Navigate();


                return "Login thành công!";

            }
            catch (Exception e)
            {
                if (Driver!= null)
                {
                    Driver.Quit();

                    Driver.Close();


                }
                return e.Message;
            }
        }
        string up(string cookie, string pathvideo, string note)
        {
            //try
            //{
            List<Cookie> cookies = readCookie(cookie);

            Driver.Url = "https://www.tiktok.com/@thanhcutequadi?lang=vi";
            Driver.Navigate();

            if (cookies.Count == 0)
                return "không định dạng được cookie";
            foreach (var item in cookies)
            {
                Driver.Manage().Cookies.AddCookie(item);
            }
            Driver.Url = "https://www.tiktok.com/upload?lang=vi";
            Driver.Navigate();

            Thread.Sleep(2000);
            IWebElement btnup;

            try
            {
                Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div[2]/div[2]/div/div/input")).SendKeys(pathvideo);
                Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div[2]/div[3]/div[1]/div[1]/div[1]/div[2]/div/div[1]/div/div/div/div/div/div")).SendKeys(note);
                if (congkhai) ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[1]/div[2]/label[1]/span");
                if (banbe) ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[1]/div[2]/label[2]/span");
                if (riengtu) ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[1]/div[2]/label[3]/span");
                if (!cmt) ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[1]/span");
                if (!duet) ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[2]/span");
                if (!congkhai) ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[3]/span");
                do
                {
                    Thread.Sleep(2000);
                    try
                    {
                        btnup = Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div[2]/div[1]/div[2]/div[2]"));

                    }
                    catch (Exception)
                    {
                        Thread.Sleep(10000);
                        break;
                    }
                } while (btnup.Text != "Thay đổi video");
                Thread.Sleep(3000);

                Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div[2]/div[3]/div[6]/button[2]")).Click();
            }
            catch (Exception)
            {

                Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div/div[2]/div[2]/div/div/input")).SendKeys(pathvideo);
                Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[1]/div[1]/div[1]/div[2]/div/div[1]/div/div/div/div/div/div")).SendKeys(note);
                ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[4]/div[1]/div[2]/div");
                if (congkhai) ClickByXpath(Driver, "/html/body/div[2]/div/span[1]");
                if (banbe) ClickByXpath(Driver, "/html/body/div[2]/div/span[2]");
                if (riengtu) ClickByXpath(Driver, "/html/body/div[2]/div/span[3]");
                if (!cmt) ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[1]/span");
                if (!duet) ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[2]/span");
                if (!congkhai) ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[3]/span");
                do
                {
                    Thread.Sleep(2000);
                    try
                    {
                        btnup = Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div/div[2]/div[1]/div[2]/div[2]"));

                    }
                    catch (Exception)
                    {
                        Thread.Sleep(10000);
                        break;
                    }
                } while (btnup.Text != "Thay đổi video");
                Thread.Sleep(3000);

                Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[6]/button[2]")).Click();
            }


            Thread.Sleep(3000);

            Thread.Sleep(5000);

            Driver.Close();
            Driver.Dispose();

            return "Upload thành công!";
            //}
            //catch (Exception)
            //{
            //    //Driver.Close();
            //    //Driver.Dispose();
            //    return "Không tìm được element!";
            //}
        }

        public string upload(string cookie , string pathvideo, string note)
        {
            Thread r = new Thread(() => up(cookie,pathvideo,note));
            r.IsBackground = true;
            r.Start();
            return "a";
        }

    }
}
