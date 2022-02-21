using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BemmTikTokv3
{
    public partial class inforVideo : Form
    {
        public inforVideo()
        {
            InitializeComponent();
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
                    groupBox1.BackgroundImage = img;
                    groupBox1.BackgroundImageLayout = ImageLayout.Stretch;

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

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked)
            
            
                {
             
                    string path = Path.GetDirectoryName(txtpathvideo.Text);
                    if (Directory.Exists(path))
                    {
                        var a = Directory.GetFiles(path);
                        MessageBox.Show("Đã bật random video: có " + a.Count() + " Video", "BemmTeam");
                    }
            


            }
          

        }
        void save()
        {
            Setting st = new Setting(Application.StartupPath);
            st.setSetting("setupvideo", "path", txtpathvideo.Text);
            Properties.Settings.Default.note = txtchuthich.Text;
            Properties.Settings.Default.Save();
            string pub = rdoCongKhai.Checked ? "1" : rdoBanBe.Checked ? "2" : "3";
            st.setSetting("setupvideo", "pub", pub);
            st.setSetting("setupvideo", "cmt", checkCmt.Checked.ToString()) ;
            st.setSetting("setupvideo", "duet", checkDuet.Checked.ToString());
            st.setSetting("setupvideo", "stitch", checkStitch.Checked.ToString());
            st.setSetting("setupvideo", "random", guna2ToggleSwitch1.Checked.ToString());
        }
        void show()
        {
            Setting st = new Setting(Application.StartupPath);
            txtpathvideo.Text = st.getSetting("setupvideo", "path");
           txtchuthich.Text = Properties.Settings.Default.note;

            string pub = st.getSetting("setupvideo", "pub");
            if (pub == "1")
            {
                rdoCongKhai.Checked = true;
            }
            else if (pub == "2") rdoBanBe.Checked = true;
            else rdoRiengTu.Checked = true;
            checkCmt.Checked = (st.getSetting("setupvideo", "cmt") == "True") ? true : false;
            checkDuet.Checked = (st.getSetting("setupvideo", "duet") == "True") ? true : false;
            checkStitch.Checked = (st.getSetting("setupvideo", "stitch") == "True") ? true : false;
            guna2ToggleSwitch1.Checked = (st.getSetting("setupvideo", "random") == "True") ? true : false;
            if (File.Exists(txtpathvideo.Text))
            {
                Image img = GetThumbnail(txtpathvideo.Text, "thumb.png");
                groupBox1.BackgroundImage = img;
                groupBox1.BackgroundImageLayout = ImageLayout.Stretch;
            }

        }
        private void inforVideo_Load(object sender, EventArgs e)
        {
            show();
        }

        private void inforVideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            save();
        }


    }
}
