
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium.Support.UI;
using System.Runtime.InteropServices;
using System.Collections;
using Auto_LDPlayer;

namespace BemmTikTokv3
{
    public partial class Fupload : UserControl
    {
        public Fupload()
        {
            InitializeComponent();
          
        }
        
        public class DBaccount
        {
            public string uid { get; set; }
            public string pass { get; set; }
            public string cookie
            {
                get; set;
            }
            public DataGridViewButtonColumn btnlogin { get; set; }
        }
        private void showDataTab(string nametab)
        {
            dataGridviewTik.Rows.Clear();

            string path = Application.StartupPath + @"\Data\Account\" + nametab + ".txt";
            string data = File.ReadAllText(path);
            data = data.Replace(Environment.NewLine, "");
            string[] read = data.Split('#');

            foreach (var item in read)
            {
                if (item != "")
                {
                    string[] info = item.Split(',');

                    dataGridviewTik.Rows.Add(info[0], info[1], info[3], info[5], "Đang chờ","UP video");
                }
            }
        }
        private void btnimportvideo_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Video File (*.mp4;)|*.mp4|All files (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    txtpathvideo.Text = dialog.FileName;
                    if (File.Exists("thumb.png"))
                          File.Delete("thumb.png");
                    Image img = GetThumbnail(dialog.FileName, "thumb.png");
                    picthump.Image = img;
                    GetThumbnail(dialog.FileName, "thumb.png");

                }
            }
        }



        public static Image GetThumbnail(string video, string thumbnail)
        {
            var cmd = "ffmpeg  -itsoffset -4  -i " + '"' + video + '"' + " -vcodec mjpeg -vframes 1 -an -f rawvideo -s 320x240 " + '"' + thumbnail + '"';

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = Application.StartupPath,
                FileName = "cmd.exe",
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                Verb = "runas"
            };
            process.Start();
            process.StandardInput.WriteLine(cmd);
            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();
            return LoadImage(thumbnail);
        }

        static Image LoadImage(string path)
        {
            var ms = new MemoryStream(File.ReadAllBytes(path));
            return Image.FromStream(ms);
        }

        private void btnimport_Click(object sender, EventArgs e)
        {
            using (tabAccount tab = new tabAccount())
            {
                tab.ShowDialog();
               string tabname = tab.nameTab();
                if (tabname != "" && tabname != null)
                {
                    showDataTab(tabname);

                }
            }
        }

   

        private void Fupload_Load(object sender, EventArgs e)
        {
           
        }

        private void picthump_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(txtpathvideo.Text);
            }
            catch (Exception)
            {

              
            }
        }
        void run(int index)


        {
            string cookie = dataGridviewTik.Rows[index].Cells[2].Value.ToString();
            Selenium sle = new Selenium();
            sle.congkhai = rdoCongKhai.Checked;
            sle.riengtu = rdoRiengTu.Checked;
            sle.banbe = rdoBanBe.Checked;
            sle.cmt = checkCmt.Checked;
            sle.duet = checkDuet.Checked;
            sle.stitch = checkStitch.Checked;
            setText(index, "Đang upload...");
            setText(index,up(cookie, txtpathvideo.Text, txtchuthich.Text,sle));

        }
        void setText(int index ,string text)
        {
            dataGridviewTik.Invoke(new Action(() =>
            {
                dataGridviewTik.Rows[index].Cells[4].Value = text;

            }));
        }
        string up(string cookie, string pathvideo, string note , Selenium sle)
        {
            var Driver = sle.Driver;

            try
            {
            List<Cookie> cookies = sle.readCookie(cookie);
            Driver.Url = "https://google.com";
            Driver.Navigate();

            Driver.Url = "https://tiktok.com";
            Driver.Navigate();
            foreach (var item in cookies)
            {
                Driver.Manage().Cookies.AddCookie(item);
            }
            if (cookies.Count == 0)
                return "không định dạng được cookie";

            Driver.Url = "https://www.tiktok.com/upload?lang=vi-VN";
            Driver.Navigate();

            Thread.Sleep(2000);
            IWebElement btnup;

            try
            {
                
                Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div[2]/div[2]/div/div/input")).SendKeys(pathvideo);
                Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div[2]/div[3]/div[1]/div[1]/div[1]/div[2]/div/div[1]/div/div/div/div/div/div")).SendKeys(note);
                if (sle. congkhai) sle.ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[1]/div[2]/label[1]/span");
                if (sle.banbe) sle.ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[1]/div[2]/label[2]/span");
                if (sle.riengtu) sle.ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[1]/div[2]/label[3]/span");
                if (!sle.cmt) sle.ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[1]/span");
                if (!sle.duet) sle.ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[2]/span");
                if (!sle.congkhai) sle.ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[3]/span");
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
                sle.ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[4]/div[1]/div[2]/div");
                if (sle.congkhai) sle.ClickByXpath(Driver, "/html/body/div[2]/div/span[1]");
                if (sle.banbe) sle.ClickByXpath(Driver, "/html/body/div[2]/div/span[2]");
                if (sle.riengtu) sle.ClickByXpath(Driver, "/html/body/div[2]/div/span[3]");
                if (!sle.cmt) sle.ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[1]/span");
                if (!sle.duet) sle.ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[2]/span");
                if (!sle.congkhai) sle.ClickByXpath(Driver, "//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[4]/div[2]/div[2]/label[3]/span");
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
                    //*[@id="main"]/div[2]/div/div/div[2]/div[3]/div[6]/button[2]
                    try
                    {
                        Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[6]/button[2]")).Click();

                    }
                    catch 
                    {

                        Driver.FindElement(By.XPath("//*[@id='main']/div[2]/div/div/div[2]/div[3]/div[8]/button[2]")).Click();

                    }
                }


            Thread.Sleep(3000);

            Thread.Sleep(5000);

       


                return "Upload thành công!";
            }
            catch (Exception)
            {
             
               
                return "Không tìm được element!";
            }
            finally
            {
             
                    try
                    {
                        Driver.Close();
                        Driver.Quit();

                    }
                    catch (Exception)
                    {

                 
                    }

            }
        }
    

        private void dataGridviewTik_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridviewTik.Columns[5].Index)
            {
                Thread r = new Thread(() => run(e.RowIndex));
                r.IsBackground = true;
                r.Start();
            }
        }

     
    }
}
