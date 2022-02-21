using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BemmTikTokv3
{
    public partial class frmtiktokTQ : Form
    {
        public frmtiktokTQ()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            if (guna2Button1.Text == "BẮT ĐẦU TẢI")
            {
                if (Directory.Exists(txtpath.Text))
                {
                    guna2Button1.Text = "Đang tải";
                    progressBar1.Style = ProgressBarStyle.Marquee ;
                    string link = txtLink.Text + "@";
                    link = link.Replace("?", "@");
                    string secuid = regEx(@"(?<=user/).*?(?=@)", link);


                    Thread reupThread = new Thread(() => ReupTiktokTQ(numericUpDown1.Value.ToString() + "|" + secuid));
                    reupThread.IsBackground = true;
                    reupThread.Start();
                }
                else
                    MessageBox.Show("Đường dẫn lưu video không tồn tại");
            }
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
        private bool DownLoadVideo(string url, string folderPath)
        {

            Thread.Sleep(2000);

            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
         
                wc.DownloadFileAsync(new System.Uri(url), folderPath);

            }
            return true;
        }
        int count = 0;
        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
         
        
            


        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Invoke(new Action(() => progressBar1.Value = e.ProgressPercentage));
            

        }

        private void ReupTiktokTQ(string param)
        {
           

                string[] settings = param.ToString().Split('|');
            try
                {
                    ReupTiktokTQ reup = new ReupTiktokTQ(settings[1].ToString(), txtpath.Text, int.Parse(settings[0]));
                    var myVideos = reup.DownloadLatestVideos();
                    if (myVideos == null)
                    {
                        MessageBox.Show("không nhận dạng được! vui lòng copy link đúng định dạng", "BemmTeam");
                    guna2Button1.Invoke(new Action(() =>
                    {
                        guna2Button1.Text = "BẮT ĐẦU TẢI";
                        progressBar1.Style = ProgressBarStyle.Blocks;
                    }));
                    }
                    else if (myVideos.Count == 0)
                    {
                        MessageBox.Show("không có video nào", "BemmTeam");
                    guna2Button1.Invoke(new Action(() =>
                    {
                        guna2Button1.Text = "BẮT ĐẦU TẢI";
                        progressBar1.Style = ProgressBarStyle.Blocks;
                    }));
                }
                    else
                    {
                        count = 0;
                        foreach (var item in myVideos)
                        {
                        Thread.Sleep(2000);
                            count++;
                            DownLoadVideo(item.Url, txtpath.Text + @"\" + item.Vid + ".mp4");
                        guna2Button1.Invoke(new Action(() => {

                            if (count == numericUpDown1.Value)
                            {
                                guna2Button1.Text = "BẮT ĐẦU TẢI";
                                progressBar1.Style = ProgressBarStyle.Blocks;

                                MessageBox.Show("Tải video thành công", "BemmTeam");
                            }
                            else
                            {
                                guna2Button1.Text = string.Format("Đã tải thành công {0}/{1}", count, numericUpDown1.Value.ToString());

                            }

                        }));
                    }

                }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
      


        }

        private void frmtiktokTQ_Load(object sender, EventArgs e)
        {
            txtpath.Text = Application.StartupPath + @"\Video";
        }
    }
}
