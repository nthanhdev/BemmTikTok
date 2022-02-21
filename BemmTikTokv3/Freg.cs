using Auto_LDPlayer;
using KAutoHelper;
using MadMilkman.Ini;
using Proxy_Client_Tinsoft;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using LDPlayer = Auto_LDPlayer.LDPlayer;

namespace BemmTikTokv3
{
    public partial class Freg : UserControl
    {
        public Freg()
        {
            InitializeComponent();

     
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        [DllImport("user32")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
   
        public string path;
        private TinsoftProxy myProxy;
        private static LDPlayer ldplayer = new LDPlayer();
        private string nameLD;
        private string[] linesName;
        private string[] filesImg;
        private string deviceID;
        Thread threadstart;
        Setting st = new Setting(Application.StartupPath);
        void RunNow(string name)
        {

            panelphone.Invoke(new Action(() =>
            {
                ldplayer.Open("name", name);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int timeout = 10 * 1000;
                //Thread.Sleep(5000);
                while (WinGetHandle(name) == IntPtr.Zero)
                {
                    System.Threading.Thread.Sleep(10);
                    if (sw.ElapsedMilliseconds > timeout)
                    {
                        sw.Stop();
                        return;
                    }
                }
                IntPtr ldplayerHandle = WinGetHandle(name);
                System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
                SetParent(ldplayerHandle, panel.Handle);

                MoveWindow(ldplayerHandle, -8, -36, 343, 616, true);
                panel.Dock = DockStyle.Fill;
                panelphone.Controls.Add(panel);
            }));

        }

        private string createFolder(string name)
        {
            string path = txtpathBackup.Text + @"\" + name;
            if (Directory.Exists(path)) Directory.Delete(path);

            Directory.CreateDirectory(path);
            return path;
        }

        private bool getBackUp(string name, string deviceID)
        {

            string path = createFolder(name);
            string r1 = ADBHelper.ExecuteCMD(string.Format("adb -s {0} pull /data/data/com.zhiliaoapp.musically.go/files {1}", deviceID, path + @"\files"));
            string r2 = ADBHelper.ExecuteCMD(string.Format("adb -s {0} pull /data/data/com.zhiliaoapp.musically.go/shared_prefs {1}", deviceID, path + @"\shared_prefs"));
            if (r1.Contains("successfully") || r2.Contains("successfully"))
                return true;
            else
                return false;

        }
        private bool setBackUp(string path, string deviceID)
        {

            string r1 = ADBHelper.ExecuteCMD(string.Format("adb -s {0} push {1} /data/data/com.zhiliaoapp.musically.go/", deviceID, path + @"\files"));
            string r2 = ADBHelper.ExecuteCMD(string.Format("adb -s {0} push {1} /data/data/com.zhiliaoapp.musically.go/", deviceID, path + @"\shared_prefs"));

            if (r1.Contains("pushed") || r2.Contains("pushed"))
                return true;
            else
                return false;
        }
        public bool turnOffIp6()
        {
            using (var powerShell = PowerShell.Create())
            {
                powerShell.AddScript("Disable-NetAdapterBinding -Name 'Wi-Fi' -ComponentID ms_tcpip6");
                powerShell.Invoke();
                if (powerShell.HadErrors)
                {
                    // Failed, do something
                    return false;
                }
                // Success, do something
                return true;
            }
        }
        public static IntPtr WinGetHandle(string wName)
        {
            IntPtr hWnd = IntPtr.Zero;
            var process = Process.GetProcesses();
            foreach (Process pList in process)
            {
                if (pList.MainWindowTitle.Length == 0) continue;


                if (pList.MainWindowTitle.Equals(wName))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;
        }

        private bool getproxy(string deviceID)
        {


            TinsoftProxy proxy;
            proxy = changeProxy(txtkey.Text);
    
            if (proxy.errorCode == "")
            {
                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell settings put global http_proxy " + proxy.proxy);
                return true;
            }
            else
            {
                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell settings put global http_proxy :0" );

            }
            return false;
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
        TinsoftProxy changeProxy(string api, int location = 0)
        {

            if (myProxy == null)
            {
                myProxy = new TinsoftProxy("");
            }
            myProxy.api_key = api; //input api key
            myProxy.location = location; //input location (0 for random)
            myProxy.changeProxy();
          
            return myProxy;
        }
        public string ExecuteLD_Result(string cmdCommand)
        {
            string result;
            try
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = @"D:\LDPlayer\LDPlayer4.0\ldconsole.exe",
                    Arguments = cmdCommand,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };
                process.Start();
                process.WaitForExit();
                string text = process.StandardOutput.ReadToEnd();
                result = text;
            }
            catch
            {
                result = null;
            }
            return result;
        }
        void saveST()
        {
            st.setSetting("application", "idtiktok",txtidtiktok.Text);
            st.setSetting("application", "password", txtmatkhau.Text);
            st.setSetting("application", "pathname", txtname.Text);
            st.setSetting("application", "pathavt", txtpathimg.Text);
            st.setSetting("application", "pathbackup", txtpathBackup.Text);
        }
        void showST()
        {
            string path = Application.StartupPath + @"\Data\";
            txtidtiktok.Text = st.getSetting("application", "idtiktok");
            txtmatkhau.Text = st.getSetting("application", "password");
            txtname.Text = st.getSetting("application", "pathname");
            if (txtname.Text == "path")
                txtname.Text = path + "Name.txt";
            linesName = File.ReadAllLines(txtname.Text);
            txtpathimg.Text = st.getSetting("application", "pathavt");
            if (txtpathimg.Text == "path")
            {
                txtpathimg.Text = path + "Images";
            }
            filesImg = Directory.GetFiles(txtpathimg.Text);

            txtpathBackup.Text = st.getSetting("application", "pathbackup");
            if (txtpathBackup.Text == "path")
            {
                txtpathBackup.Text = Application.StartupPath + "\\Backup";
            }

            string account = File.ReadAllText(path + "Account.txt");
            account = account.Replace("\r", "");
            account = account.Replace("\n", "");
            string[] list = account.Split('#');
            
            foreach (var item in list)
            {
                if (item != "")
                {
                    string[] infor = item.Replace("#", "").Split(',');
                  
                    string[] tik = { infor[0], infor[1], infor[2], infor[3] , infor[4] };
                    dataGridviewTik.Rows.Add(tik);
                }
            }
            designView();
        }
        void designView()
        {
            foreach (DataGridViewColumn item in dataGridviewTik.Columns)
            {
                item.DividerWidth = 2;
            }
        }
        string[] getInforDv()
        {
            string html;
            using (var web = new WebClient())
            {
                html = web.DownloadString("https://www.myfakeinfo.com/mobile/imei-generator.php");

            }

            string imei = regEx(@"(?<=imei=).*?(?="")", html);

            string[] result = new string[] { imei };
            return result;
        }
        void changeDevice(string name)
        {

            Change_Property("name", name, string.Format(" --model {0} --imei {1}", "SM-G977N", "auto"));

        }

        private void btnrun_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.check)
            {
                MessageBox.Show("Bạn kích hoạt phần mềm đi nha :(", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            if (threadstart is null || !threadstart.IsAlive)
            {
                if (lblstt.Text == "ĐANG CHỜ LỆNH")
                {
                    MessageBox.Show("Vui lòng chạy LD trước!", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!Directory.Exists(txtpathBackup.Text))
                {
                    MessageBox.Show("Path Backup không tồn tại!", "BemmTeam.Com", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                else if (checkuser.Checked)
                {
                    if (btnUserID.Text == "Thiết lập User ID")
                    {
                        MessageBox.Show("Vui lòng chọn tab user cần tương tác!", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (checkvideo.Checked)
                {
                    if (btnUserID.Text == "Thiết lập Video ID")
                    {
                        MessageBox.Show("Vui lòng chọn tab video cần tương tác!", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (txtname.Text == "" || txtpathimg.Text == "" || txtidtiktok.Text == "" || txtpathBackup.Text == "")
                {
                    MessageBox.Show("Thiếu trường thông tin", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!txtmatkhau.Text.Contains("@"))
                {
                    MessageBox.Show("Mật khẩu bắt buộc có kí tự @", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                saveST();
                btnrun.Text = "STOP CREATE CLONE";
                threadstart = new Thread(() =>
                {
                    while (true)
                    {
                        if (txtkey.Text != "")
                        {
                            setText("Đang đổi proxy");


                            if (getproxy(deviceID))
                            {
                                setText("đổi proxy thành công");
                                sleep(5000);
                            }
                            else
                            {
                                setText("đổi proxy không thành công",true);
                                sleep(5000);

                            }
                        }
                        else
                        {
                            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell settings put global http_proxy :0");

                        }
                        runCreateClone();
                    }
                });
                threadstart.IsBackground = true;

                threadstart.Start();
            }
            else
            {
                btnrun.Text = "TIẾN HÀNH TẠO CLONE";
                setText("Đã hủy tạo clone");
                threadstart.Abort();
            }

        }

        string dumpData(string devices)
        {
            string data = ADBHelper.ExecuteCMD("adb -s " + devices + " exec-out uiautomator dump");
            string path = regEx(@"(?<=to: ).*?(?=.xml)", data);
            ADBHelper.ExecuteCMD("adb -s " + devices + " pull " + path + ".xml " + "dataDump.xml");
            string result = File.ReadAllText("dataDump.xml");
            return result;
        }
        //D:\Code\Adb>adb push aweme_user.xml data/data/com.zhiliaoapp.musically.go/shared_prefs/aweme_user.xml
        Point findByText(string devices, string text, int _count = 6, bool isText = true)
        {
            bool check = true;
            Point point = new Point();
            int count = 1;
            do
            {
                string data = dumpData(devices);
                if (count == _count) return point;
                string query;
                if (isText)
                    query = @"(?<=text=""" + text + @""").*?(?=>)";
                else
                    query = text;
                string node = regEx(query, data);
                string bound = regEx(@"(?<=bounds="").*?(?="")", node);
                bound = bound.Replace("][", ",");
                bound = bound.Replace("[", "");
                bound = bound.Replace("]", "");
                string[] num = bound.Split(',');
                if (num.Count() == 4 && num != null)
                {
                    point.X = (int.Parse(num[0]) + int.Parse(num[2])) / 2;
                    point.Y = (int.Parse(num[1]) + int.Parse(num[3])) / 2;
                    check = true;

                }
                else
                {
                    check = false;
                    count += 1;
                    Thread.Sleep(10000);
                }
            } while (!check);

            return point;
        }
        bool checkNow(string devices, string text, int count = 10)
        {

            if (!findByText(devices, text, count).IsEmpty)
                return true;
            else
                return false;

        }
        void sleep(int num)
        {
            Thread.Sleep(num);
        }

        void SendText(string deviceID, string text)
        {
            string converttext = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
            string send = string.Format("adb -s " + deviceID + " shell am broadcast -a ADB_INPUT_B64 --es msg '{0}'", converttext);
            ADBHelper.ExecuteCMD(send);
        }
        string getDevices()
        {
            string result = "";
            string name = nameLD;
            do
            {
                ADBHelper.ExecuteCMD("adb kill-server");
                var listDevices = ldplayer.GetDevices2_Running();
                foreach (var item in listDevices)
                {
                    if (item.name == name)
                    {
                        result = item.adb_id;
                        break;
                    }
                }
            } while (result.Contains("offline"));
            return result;

        }

        void runCreateClone()
        {
            Point point = new Point();
           Mail mail = new Mail(Properties.Settings.Default.domain, Properties.Settings.Default.gid) ;
            //string name = cboLdName.SelectedItem.ToString();
            //changeDevice(name);

            //RunNow(name);

            //for (int i = 0; i <= 10; i++)
            //{
            //    sleep(10000);

            //    deviceID = getDevices();
            //    if (deviceID != "")
            //        break;

            //    else
            //    {
            //        if (i == 10)
            //        {
            //            // khong mở dc LD
            //            MessageBox.Show("không mở duoc LD");
            //            return;
            //        }

            //    }
            //}



            //if (!checkNow(deviceID, "TikTok Lite", 10))
            //{
            //    //load vào quá lâu
            //    MessageBox.Show("Load quá lâu");

            //    return;
            //}
            //  getproxy();

            setText("clear data tiktok");
            sleep(3000);

            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell pm clear com.zhiliaoapp.musically.go");
            sleep(3000);
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://login");
            sleep(3000);

            //check đã vào phần login chưa
            //if (!checkNow(deviceID, "Dành cho bạn",10))
            //{
            //    //khong vao dc tiktok
            //    return;
            //}

     
            setText("Tiến hành đăng ký");

            point = findByText(deviceID, "Sử dụng số điện thoại hoặc email");
            if (point.IsEmpty)
            {
                //khong vao dc tiktok
                setText("không vào được tiktok", true);

                return;
            }
            ADBHelper.Tap(deviceID, point.X, point.Y);

            sleep(5000);
            for (int i = 0; i <= 6; i++)
            {

                //swipe nam sinh
                ADBHelper.Swipe(deviceID, 256, 421, 266, 513, 200);
                //swipe thang
                ADBHelper.Swipe(deviceID, 67, 426, 83, 516, 200);
            }

            // nhấn tiếp
            sleep(3000);

            KAutoHelper.ADBHelper.Tap(deviceID, 174, 289);
            for (int i = 0; i <= 6; i++)
            {
                point = findByText(deviceID, "Tiếp",2);
                if (point.IsEmpty)
                {
               
                        break;
                    
                }
                if (i == 6)
                {
                    setText("không vào được tiktok!", true);
                    sleep(2000);
                    return;
                }
                else
                {
                    ADBHelper.Tap(deviceID, point.X, point.Y);
                    sleep(5000);
                }
            
            }

            setText("đang lấy email");
            //var tenMail = new TenMinuteMailNet();
            //tenMail.GenerateNewEmailAddress();
            int numne = new Random().Next(10000, 99999);
            string userID = txtidtiktok.Text + numne.ToString();
            string email = mail.getitemMail(userID) ;
            sleep(5000);
            //click qua email 
            if (email == "")
            {
                //khong vào được
                setText("không lấy được email", true);
                sleep(3000);
                return;
            }
            setText("nhập email");
            point = findByText(deviceID, "Email");
            if (point.IsEmpty)
            {
                //khong vào được
                setText("không vào được tiktok!", true);
                return;
            }
            ADBHelper.Tap(deviceID, point.X, point.Y);
            sleep(1000);
            ADBHelper.InputText(deviceID, email);
            
            KAutoHelper.ADBHelper.Tap(deviceID, 168, 313);
            sleep(5000);

            // mat khau 
            SendText(deviceID, txtmatkhau.Text);
            KAutoHelper.ADBHelper.Tap(deviceID, 168, 329);
            setText("chờ code");

            string code = mail.WailgetCode(email) ;
            //int countcode = 0;
            //do
            //{
            //    var listmail = tenMail.Emails;
            //    foreach (var item in listmail)
            //    {
            //        if (item.From.Contains("TikTok"))
            //        {
            //            code = regEx(@"(?<=font-size:20px;"">).*?(?=<)", item.Html);
            //            break;
            //        }
            //        else
            //        {
            //            code = "";
            //            countcode++;
            //        }
            //        if (countcode == 10)
            //        {

            //            {
            //                setText("không lấy được mã code", true);
            //                return;

            //            }
            //        }
            //    }
            //    sleep(3000);
            //} while (code == "");

            if (code == "")
            {

                {
                    setText("không lấy được mã code", true);
                    return;

                }
            }
            var tcode = code.ToArray();
            foreach (var item in tcode)
            {
                ADBHelper.InputText(deviceID, item.ToString());

            }

            sleep(5000);
            //check reg
            setText("sửa thông tin");

            point = findByText(deviceID, "Tạo TikTok ID", 3);
            if (point.IsEmpty)
            {
                setText("reg clone thất bại", true);

                return;
            }
            setText("sửa id tiktok");

            ADBHelper.InputText(deviceID, "camonmoinguoinhahehehe");

            sleep(4000);
            KAutoHelper.ADBHelper.Tap(deviceID, 293, 194);
            // convert 
    
            ADBHelper.InputText(deviceID, userID);

            sleep(2000);
            KAutoHelper.ADBHelper.Tap(deviceID, 169, 271);
            sleep(3000);
            setText("Tiến hành sửa hồ sơ");
        
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell pm grant com.zhiliaoapp.musically.go android.permission.WRITE_EXTERNAL_STORAGE");




            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://profile");
            sleep(3000);
            // sua ho so
            KAutoHelper.ADBHelper.Tap(deviceID, 154, 328);
            sleep(2000);
            KAutoHelper.ADBHelper.Tap(deviceID, 270, 296);
            sleep(1000);
            KAutoHelper.ADBHelper.Tap(deviceID, 317, 146);

            //input ten 
            int numname = new Random().Next(0, linesName.Count());
            SendText(deviceID, linesName[numname]);
            // click luu
            KAutoHelper.ADBHelper.Tap(deviceID, 312, 54);
            sleep(1000);
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://feed");

            // change img 
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://profile");

            int numimg = new Random().Next(0, filesImg.Count());
            ADBHelper.ExecuteCMD(string.Format("adb -s {0} push {1} /sdcard/pic.jpg", deviceID, filesImg[numimg]));
            ADBHelper.ExecuteCMD(string.Format("adb -s {0} shell am broadcast -a android.intent.action.MEDIA_MOUNTED -d file:///sdcard", deviceID));
            setText("thay đổi avt");
            //tap vao img
            KAutoHelper.ADBHelper.Tap(deviceID, 172, 146);
            sleep(1000);
            // tap chinh sua
            KAutoHelper.ADBHelper.Tap(deviceID, 223, 389);
            sleep(1000);
            KAutoHelper.ADBHelper.Tap(deviceID, 248, 364);
            sleep(1000);
            KAutoHelper.ADBHelper.Tap(deviceID, 55, 144);
            sleep(1000);
            KAutoHelper.ADBHelper.Tap(deviceID, 310, 60);
            sleep(1000);
            KAutoHelper.ADBHelper.Tap(deviceID, 291, 593);
            sleep(2000);

            KAutoHelper.ADBHelper.Tap(deviceID, 311, 559);
            sleep(10000);

            KAutoHelper.ADBHelper.Tap(deviceID, 168, 598);
            setText("tiến hành backup");
            string backup = txtpathBackup.Text + @"\" + userID;
            if (getBackUp(userID, deviceID)) { setText("Không backup được!", true); backup = "Không có"; }
            string cookie = getCookie(backup); 

            dataGridviewTik.Invoke(new Action(() =>
            {
               
                string[] info = { userID, txtmatkhau.Text,email, backup , cookie};
                dataGridviewTik.Rows.Add(info);
                string data = "#" + info[0] + "," + info[1] + "," + info[2] + ","+  info[3] + "," + info[4];
                string ola = File.ReadAllText(Application.StartupPath + @"\Data\Account.txt");
                ola = ola + Environment.NewLine + data;
                File.WriteAllText(Application.StartupPath + @"\Data\Account.txt", ola);
                designView();
            }));
            if (checkvideo.Checked)
                byVideo(deviceID);
            if (checkuser.Checked)
                byUser(deviceID);


            setText("Thành công! chờ 10s tạo mới");
            sleep(10000);

        }
        string getCookie(string path)
        {
            path = path + @"\shared_prefs\aweme_user.xml";
            string result = "";
            string data = File.ReadAllText(path);
            string session = regEx(@"(?<=session_key&quot;:&quot;).*?(?=&)", data);
            string uid = regEx(@"(?<=sec_uid&quot;:&quot;).*?(?=&)", data);
            result = File.ReadAllText(Application.StartupPath + @"\Cookie.txt");
            result = result.Replace("@uid", uid);
            result = result.Replace("@sid", session);

            return result;
        }



        void byUser(string deviceID)
        {
            List<userID> users = getUser();
            Point point = new Point();
            string[] cmt = File.ReadAllLines(Application.StartupPath + @"/Data/Cmt.txt");

            foreach (var item in users)
            {
                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://feed");
                setText("Truy cập vào user: " + item.name);

                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d " + "https://" + item.link);
                sleep(3000);
                point = findByText(deviceID, "Follow", 3);

                if (!point.IsEmpty)
                {


                    if (item.follow)
                    {
                        setText("Tiến hành follow");

                        ADBHelper.Tap(deviceID, point.X, point.Y);

                        sleep(2000);

                    }
                    if (item.tuongtac)
                    {
                        KAutoHelper.ADBHelper.Tap(deviceID, 55, 560);
                        sleep(1000);
                        setText("xem video " + item.time + "s");
                        Thread.Sleep(TimeSpan.FromSeconds(item.time));

                        for (int i = 0; i < item.sovideo; i++)
                        {
                            if (item.love)
                            {
                                setText("Tiến hành love");
                                KAutoHelper.ADBHelper.Tap(deviceID, 167, 299);
                                point = findByText(deviceID, @"(?<=id/tn).*?(?=>)", 3, false);
                                ADBHelper.Tap(deviceID, point.X, point.Y);
                            }
                            if (item.cmt)
                            {
                                // random cmt 
                                int num = new Random().Next(0, cmt.Count());
                                setText("Tiến hành cmt");
                                KAutoHelper.ADBHelper.Tap(deviceID, 83, 595);
                                KAutoHelper.ADBHelper.Tap(deviceID, 83, 595);

                                sleep(5000);
                                SendText(deviceID, cmt[num]);
                                sleep(3000);

                                KAutoHelper.ADBHelper.Tap(deviceID, 317, 529);
                                sleep(3000);

                            }
                            setText("xem video " + item.time + "s");
                            ADBHelper.Swipe(deviceID, 145, 451, 145, 48, 300);
                            Thread.Sleep(TimeSpan.FromSeconds(item.time));
                        }

                    }
                }
                else KAutoHelper.ADBHelper.Tap(deviceID, 147, 324);

            }

        }

        void byVideo(string deviceID)
        {
            List<videoID> videos = getVideo();
            Point point = new Point();
            string[] cmt = File.ReadAllLines(Application.StartupPath + @"/Data/Cmt.txt");

            foreach (var item in videos)
            {

                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://feed");
                setText("vào video của " + item.name);
                sleep(1000);

                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d " + "https://" + item.link);
                sleep(3000);
                setText("Xem video " + item.time + "s");

                Thread.Sleep(TimeSpan.FromSeconds(item.time));
                if (item.follow)
                {
                    setText("tiến hành follow");
                    sleep(1000);
                    ADBHelper.Swipe(deviceID, 273, 324, 17, 324, 400);

                    point = findByText(deviceID, "Follow", 3);
                    if (point.IsEmpty)
                    {
                        KAutoHelper.ADBHelper.Tap(deviceID, 147, 324);
                        setText("Không follow được | follow theo tọa độ!", true);
                    }
                    else ADBHelper.Tap(deviceID, point.X, point.Y);
                    sleep(3000);
                    ADBHelper.Swipe(deviceID, 17, 324, 273, 324, 400);

                }
                if (item.love)
                {
                    setText("Tiến hành love");
                    KAutoHelper.ADBHelper.Tap(deviceID, 167, 299);

                    point = findByText(deviceID, @"(?<=id/tn).*?(?=>)", 6, false);
                    ADBHelper.Tap(deviceID, point.X, point.Y);
                }
                if (item.cmt)
                {
                    // random cmt 
                    int num = new Random().Next(0, cmt.Count());
                    setText("Tiến hành cmt");
                    KAutoHelper.ADBHelper.Tap(deviceID, 83, 595);
                    KAutoHelper.ADBHelper.Tap(deviceID, 83, 595);

                    sleep(5000);
                    SendText(deviceID, cmt[num]);
                    sleep(3000);

                    KAutoHelper.ADBHelper.Tap(deviceID, 317, 529);
                    sleep(3000);
                }

            }
        }

    
        List<videoID> getVideo()
        {
            List<videoID> result = new List<videoID>();
            string[] list = File.ReadAllLines(st.getSetting("listvideo", "path"));
            foreach (var item in list)
            {
                string[] info = item.Split('|');
                if (bool.Parse(info[6]))
                {
                    videoID video = new videoID()
                    {
                        link = info[0],
                        name = info[1],
                        follow = bool.Parse(info[2]),
                        cmt = bool.Parse(info[3]),
                        love = bool.Parse(info[4]),
                        time = int.Parse(info[5]),
                        kichhoat = bool.Parse(info[6])
                    };
                    result.Add(video);
                }
            }
            return result;
        }

        List<userID> getUser()
        {
            List<userID> result = new List<userID>();
            string[] list = File.ReadAllLines(st.getSetting("listuser", "path"));//
            foreach (var item in list)
            {
                string[] info = item.Split('|');
                if (bool.Parse(info[8]))
                {
                    userID user = new userID()
                    {
                        link = info[0],
                        name = info[1],
                        follow = bool.Parse(info[2]),
                        tuongtac = bool.Parse(info[3]),
                        cmt = bool.Parse(info[4]),
                        love = bool.Parse(info[5]),
                        sovideo = int.Parse(info[6]),
                        time = int.Parse(info[7]),
                        kichhoat = bool.Parse(info[8])
                    };
                    result.Add(user);
                }
            }
            return result;
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {

            User boxuser = new User();
            boxuser.ShowDialog();
            btnUserID.Text = boxuser.comTab.Text;

        }

        public void Change_Property(string param, string NameOrId, string cmd)
        {
            ExecuteLD_Result(string.Format("modify --{0} {1} {2}", param, NameOrId, cmd));
        }

    

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {

            Video video = new Video();
            video.ShowDialog();
            btnVideoID.Text = video.comTab.Text;


        }



        public static Random generate = new Random();

        public String IMEICode()
        {
            int[] code = new int[14];
            StringBuilder IMEI = new StringBuilder();
            //irrelevant
            for (int i = 0; i < code.Length; i++)
            {
                code[i] = generate.Next(10);
            }
            //irrelevant
            var a = IMEI.ToString();
            return a;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //import name
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Text Files (*.TXT;)|*.TXT|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    linesName = File.ReadAllLines(dialog.FileName);
                    txtname.Text = dialog.FileName;

                }
            }
        }

        private void dataGridviewStu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        void setText(string text, bool isError = false)
        {
            text = text.ToUpper();
            lblstt.Invoke(new Action(() => { lblstt.Text = text; if (isError) lblstt.ForeColor = Color.Red; else lblstt.ForeColor = Color.Cyan; }));
        }

        private void runLD(object sender, EventArgs e)
        {

         
        }

        private void Freg_Load(object sender, EventArgs e)
        {
            showST();
          
        }

        private void openimg(object sender, EventArgs e)
        {
            using (var fbd = new OpenFileDialog())
            {
                fbd.ValidateNames = false;
                fbd.CheckFileExists = false;
                fbd.CheckPathExists = true;
                // Always default to Folder Selection.
                fbd.FileName = "Folder Selection.";
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK)
                {

                    txtpathimg.Text = Path.GetDirectoryName(fbd.FileName);

                    filesImg = Directory.GetFiles(txtpathimg.Text);
                }
            }
        }

        private void btnpathbackup_Click(object sender, EventArgs e)
        {
            using (var fbd = new OpenFileDialog())
            {
                fbd.ValidateNames = false;
                fbd.CheckFileExists = false;
                fbd.CheckPathExists = true;
                // Always default to Folder Selection.
                fbd.FileName = "Folder Selection.";
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK)
                {

                    txtpathBackup.Text = Path.GetDirectoryName(fbd.FileName);

                }
            }

        }



        private void dataGridviewTik_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridViewCell clickedCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];

                    // Here you can do whatever you want with the cell
                    this.dataGridviewTik.CurrentCell = clickedCell;  // Select the clicked cell, for instance

                    // Get mouse position relative to the vehicles grid
                    var relativeMousePosition = dataGridviewTik.PointToClient(Cursor.Position);

                    // Show the context menu
                    this.contextMenuStrip1.Show(dataGridviewTik, relativeMousePosition);
                }
            }
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            string path = dataGridviewTik.Rows[dataGridviewTik.CurrentCell.RowIndex].Cells[2].Value.ToString();
            Process.Start(path);
        }

        private void menuLogin_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => {
                string cookie = dataGridviewTik.Rows[dataGridviewTik.CurrentCell.RowIndex].Cells[3].Value.ToString();
                Selenium Selenium = new Selenium();

               string a =  Selenium.loginTikTok(cookie);
                MessageBox.Show(a);
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.RestoreDirectory = true;
            savefile.FileName = String.Format("{0}.txt", "account_day" + DateTime.Now.Day);
            savefile.DefaultExt = "*.txt*";
            savefile.Filter = "TEXT Files|*.txt";
            string text = "";

            foreach (DataGridViewRow item in dataGridviewTik.Rows)
            {

                dataGridviewTik.CurrentCell = dataGridviewTik.Rows[0].Cells[0];
                dataGridviewTik.Rows[0].Selected = true;

                string user = item.Cells[0].Value.ToString();
                string pass = item.Cells[1].Value.ToString();
                string path = item.Cells[2].Value.ToString();


                text += "#" + user + "," + pass + "," + path  + Environment.NewLine;
            }

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(savefile.FileName))
                {
                    sw.WriteLine(text);
                   
                }


            }
        }

        private void btnrunLD_Click(object sender, EventArgs e)
        {
            if (btnld.Text == "Nhấn vào để chọn LD")
            {
                MessageBox.Show("Vui lòng chọn tab LD cần chạy!", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            panelphone.Controls.Clear();
            panel1.Visible = false;

            Thread r = new Thread(() =>
            {
                //input ten 
                ADBHelper.ExecuteCMD("adb kill-server");
                Process[] processesByName = Process.GetProcessesByName("adb");
                foreach (Process p in processesByName)
                    if (p != null)
                        p.Kill();
                setText("đang kiểm tra path LD");
                string path =  st.getSetting("pathLD","path")+ @"\ldconsole.exe";
                if (!File.Exists(path))
                {
                    setText("Path LD không đúng!", true);
                    return;
                }
                btnld.Invoke(new Action(() => nameLD = btnld.Text));
                changeDevice(nameLD);
                LDPlayer.pathLD = path;

                ldplayer.ExecuteLD("modify --name " + nameLD + " --resolution 343,616,160");
                RunNow(nameLD);


                setText("đang khởi động LD");
                do
                {
                    deviceID = getDevices();
                    sleep(3000);
                } while (deviceID == "");

                sleep(10000);
                string result = "";
                if (checktik.Checked)
                {
                    setText("tiến hành cài đặt tiktok");
                    sleep(2000);

                    result = ADBHelper.ExecuteCMD("adb -s " + deviceID + " install TikTok.apk");
                    if (!result.Contains("Success"))
                    {
                        setText("không cài được tiktok.apk", false);
                        return;
                    }
                }
                if (checkkey.Checked)
                {
                    setText("tiến hành cài đặt ADBKeyboard");
                    sleep(2000);

                    result = ADBHelper.ExecuteCMD("adb -s " + deviceID + " install ADBKeyboard.apk");
                    if (!result.Contains("Success"))
                    {
                        setText("không cài được ADBKeyboard.apk", false);
                        return;
                    }
                }
                //set as defaultl 
                if (checkdefault.Checked)
                {
                    if (!checkNow(deviceID, "TikTok Lite", 7))
                    {
                        // khong vào dc 
                        setText("không set được! khởi động lại");
                        return;
                    }
                    setText("set as default tiktok");
                    ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -a android.settings.APPLICATION_DETAILS_SETTINGS package:com.zhiliaoapp.musically.go");
                    sleep(10000);
                    KAutoHelper.ADBHelper.Tap(deviceID, 96, 466);
                    sleep(4000);
                    KAutoHelper.ADBHelper.Tap(deviceID, 51, 220);
                    sleep(2000);
                    Point point = findByText(deviceID, "Mở trong ứng dụng này");
                    if (point.IsEmpty)
                    {
                        setText("không set as default được!");
                        return;
                    }

                    ADBHelper.Tap(deviceID, point.X, point.Y);

                    ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell input keyevent 3");
                }
                setText("khởi động ADBKeyboard");
                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell ime set com.android.adbkeyboard/.AdbIME");
                sleep(6000);
                setText("đã sẳn sàng tạo clone!");

            });
            r.IsBackground = true;
            r.Start();
        }

        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = true;
            ldplayer.Close("name", nameLD);
            panelphone.Controls.Clear();
            if (threadstart != null)
            {
                threadstart.Abort();
            }
        }

        private void btnld_Click(object sender, EventArgs e)
        {
            using (nameLD ld = new nameLD())
            {
                ld.ShowDialog();
                if (ld.name != "")
                {
                    btnld.Text = ld.name;
                }
            }
        }

        
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn xóa tất cả không ?", "BemmTeam", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
            File.WriteAllText(Application.StartupPath + @"\Data\Account.txt", "");

            }
            showST();

        }

        private void menuaddtab_Click(object sender, EventArgs e)
        {
            try
            {
                using (tabAccount tab = new tabAccount())
                {
                    tab.ShowDialog();
                    string name = tab.nameTab();
                    if (name != "")
                    {

                        foreach (DataGridViewRow row in dataGridviewTik.SelectedRows)
                        {
                            string user = row.Cells[0].Value.ToString();
                            string password = row.Cells[1].Value.ToString();
                            string pathBackup = row.Cells[3].Value.ToString();
                            string cookie = row.Cells[4].Value.ToString();
                            string email = row.Cells[2].Value.ToString();

                            string data = Environment.NewLine + "#" + user + "," + password + "," + pathBackup + "," + cookie + ",NameLD,Proxy," + email ;
                            string main = File.ReadAllText((Application.StartupPath + @"\Data\Account\" + name + ".txt")) + data;
                            File.WriteAllText(Application.StartupPath + @"\Data\Account\" + name + ".txt", main);

                            dataGridviewTik.Rows.Remove(row);
                        }
                        MessageBox.Show("Đã thêm account vào tab " + name, "BemmTeam");
                        save();
                    }
                 

                }
            }
            catch (Exception a)
            {

                MessageBox.Show("Lỗi: " + a.Message, "BemmTeam");

            }
        }

        void save()
        {
            string main= "";
            foreach (DataGridViewRow item in dataGridviewTik.Rows)
            {
                string data = "#" + item.Cells[0].Value + "," + item.Cells[1].Value + "," + item.Cells[2].Value + "," + item.Cells[3].Value;
                main = main + Environment.NewLine + data;
            }
            File.WriteAllText(Application.StartupPath + @"\Data\Account.txt", main);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mail mail = new Mail(Properties.Settings.Default.domain, Properties.Settings.Default.gid);
            MessageBox.Show(mail.getitemMail("bemmteamtest"));
        }
    }
}


//>adb push default/app_lib_application_tiktok_d53cf964cb785480 /data/data/com.zhiliaoapp.musically.go


/*
 * 
 * R6kq3TV7=ACln0fN7AQAAG-MM9Qjq59pSF3UqEyAsjsuBLRQiXZzRYZUVBsq4EcN1t9j6|1|0|000d2cf601731a99e4c614f3b26c82bd69da063d;tt_csrf_token=1IB6J9ojEbWG6OQOwaIImD6v;tt_webid=7008884845221824002;tt_webid_v2=7008884845221824002;ttwid=1%7CEB3V1p9iRHrgABx64WqPDCvdmEJsCww1SPbDrOGvV6k%7C1631883260%7C4205551dd83f94c0d7d0b55d14f892398de52e137d891da95cfe7ec570ad2c6c;csrf_session_id=c13d58496ed94d6098e7880fd3798d1c;msToken=tm-wxiLCP007vyAKCr6NPdZzsb5jsvN-BTodv0WKgwyqmbV4ZeMOwl_Pbip2zRWowRd4tAUsjI94jpwpp1x4vrpjO-YJU5c3UIHe-XEsNDq93A5lWHEJdbltfCmQcfj0o-yZ0Yg=;
s_v_web_id=verify_ktod6nlm_u8KcPDof_X0fv_45fS_B40j_hrVvtoimyB3P;sessionid=e1ed5fc461caf008ebf65370843cfb2c;sessionid_ss=e1ed5fc461caf008ebf65370843cfb2c

 * */

