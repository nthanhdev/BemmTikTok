using Auto_LDPlayer;
using KAutoHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using LDPlayer = Auto_LDPlayer.LDPlayer;

namespace BemmTikTokv3
{
    public partial class FNuoiTikTok : UserControl
    {

        public FNuoiTikTok(FlowLayoutPanel panel)
        {
            InitializeComponent();
            this.panelPhone = panel;
        }
        string pathvideo;
        string pathuser;
        static string pathLD = new Setting(Application.StartupPath).getSetting("pathLD","path");
        FlowLayoutPanel panelPhone;
   

        private void btnVideoID_Click(object sender, EventArgs e)
        {

            Video video = new Video();
            video.ShowDialog();
            btnVideoID.Text = video.comTab.Text;
            pathvideo = Application.StartupPath + @"\Data\VideoID\" + video.comTab.Text + ".txt";
        }

        private void btnUserID_Click(object sender, EventArgs e)
        {
            User boxuser = new User();
            boxuser.ShowDialog();
            btnUserID.Text = boxuser.comTab.Text;
            pathuser = Application.StartupPath + @"\Data\UserID\" + boxuser.comTab.Text + ".txt";

        }

        private void showData(string path) 
        {
            string data = File.ReadAllText(path);
            data = data.Replace(Environment.NewLine, "");
            string[] read = data.Split('#');

            foreach (var item in read)
            {
                if (item != "")
                {
                    string[] info = item.Split('|');

                    dataGridviewTik.Rows.Add(info[0], info[1], info[3] + "|" + info[4] + "|" + info[5] + info[6], "đang chờ up", "UP VIDEO");
                }
            }
        }
        private void menuImprot_Click(object sender, EventArgs e)
        {
            int idex = dataGridviewTik.CurrentCell.RowIndex;
      

            try
            {
                if (threads.Count > 0)
                {
                    threads[idex].Abort() ;
                   
                    setText("", idex, "Đã thoát", true);
                    now++;
                    ParalleFor(idex);
                }
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.Message, "BemmTeam");
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

                    //Show the context menu
                    this.contextMenuStrip1.Show(dataGridviewTik, relativeMousePosition);
                }
            }
        }

        private void dataGridviewTik_Click(object sender, EventArgs e)
        {
            if (dataGridviewTik.Rows.Count == 0)
            {
                this.contextMenuStrip1.Show(dataGridviewTik, new Point(20, 50));

            }
        }

        private void showDataTab(string nametab)
        {
            if (nametab != "")
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

                        dataGridviewTik.Rows.Add(info[0], info[1], info[2], info[4], info[5], "Đang chờ");
                    }
                }
            }
        }
        string getDevices(string nameLD)
        {
            string result = "";
            int i = 0;
            Auto_LDPlayer.LDPlayer lD = new Auto_LDPlayer.LDPlayer();
            do
            {
                ADBHelper.ExecuteCMD("adb kill-server");
                var listDevices = lD.GetDevices2_Running();
                foreach (var  item in listDevices)
                {
                    if (item.name == nameLD)
                    {
                        result = item.adb_id;
                        break;
                    }
                    i++;
                    if (i == 10)
                    {
                        result = "";
                        break;
                    }else
                    sleep(3000);

                }
            } while (result.Contains("offline"));
            return result;

        }
        private int getIndex(string name)
        {
            int result = -1;
            Auto_LDPlayer.LDPlayer lDPlayer = new Auto_LDPlayer.LDPlayer();
            var list = lDPlayer.GetDevices2();
            foreach (var item in list)
            {
                if (item.name == name)
                {
                    result = item.index;
                    break;
                }
            }
            return result;
        }
        string tabname;
        private void menuOpen_Click(object sender, EventArgs e)
        {
            using (tabAccount tab = new tabAccount())
            {
                tab.ShowDialog();
                tabname = tab.nameTab();
                showDataTab(tabname);
            }
            
        }
        void setText(string iid,int index, string text, bool isError = false, bool isNew = false)
        {
            if (index < dataGridviewTik.Rows.Count)
            {
                text = text.ToUpper();
                dataGridviewTik.Invoke(new Action(() => {
                    dataGridviewTik.Rows[index].Cells[5].Value = text;
                    if (!isNew)
                    {
                        if (isError) dataGridviewTik.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                        else dataGridviewTik.Rows[index].DefaultCellStyle.BackColor = Color.Cyan;
                    }
                }));
                if (iid != "")
                {
                    ThongbaoLD(text, iid);

                }
            }
        }
        public void Change_Property(string param, string NameOrId, string cmd)
        {
             Auto_LDPlayer.LDPlayer ld = new LDPlayer();
                ld.ExecuteLD_Result(string.Format("modify --{0} {1} {2}", param, NameOrId, cmd));
        }

        void changeDevice(string name)
        {

            Change_Property("name", name, string.Format(" --model {0} --imei {1}", "SM-G977N", "auto"));

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
        string dumpData(string devices)
        {
            string data = ADBHelper.ExecuteCMD("adb -s " + devices + " exec-out uiautomator dump");
            string path = regEx(@"(?<=to: ).*?(?=.xml)", data);
            ADBHelper.ExecuteCMD("adb -s " + devices + " pull " + path + ".xml " + "dataDump.txt");
            string result = "";
            using (var stream = File.Open("dataDump.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                 var a = new StreamReader(stream);
                result = a.ReadToEnd();
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
                    Thread.Sleep(5000);
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
        
        private async void runTT(int index, string deviceID, string pathBackup, string nameLD, string iid,string proxy)
        {
            LDPlayer lD = new LDPlayer();

            try
            {

                await Task.Delay(10);
                if (proxy.Contains(":"))
                {
                    setText(iid, index, "Check proxy");
                    if (CanPing(proxy))
                    {
                        ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell settings put global http_proxy " + proxy);
                    }
                    else
                    {
                        setText(iid, index, "proxy die");
                        lD.Remove_Proxy(deviceID);

                    }
                }
                else
                {
                    lD.Remove_Proxy(deviceID);

                }
                int check = 0;

                setText(iid, index, "check load");
                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell input keyevent 164");

                if (checkNow(deviceID, "TikTok Lite", 10))
                {
                    ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://login");

                }
                else
                {
                    lD.Close("name", nameLD);
                    changeDevice(nameLD);
                    setText(iid, index, "không vào được !", true);
                    now++;
                    ParalleFor(index);

                    return;
                }
                bool istrue;
                setText(iid, index, "Tiến hành backup");

                do
                {
                    check++;
                    ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell pm clear com.zhiliaoapp.musically.go");

                    ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://login");

                    istrue = checkNow(deviceID, "Sử dụng số điện thoại hoặc email", 5);
                    if (istrue)
                    {

                        if (!setBackUp(pathBackup, deviceID))
                        {
                            lD.Close("name", nameLD);
                            changeDevice(nameLD);
                            setText(iid, index, "không backup được!", true);
                            now++;
                            ParalleFor(index);

                            return;

                        }
                        break;
                    }

                    if (!istrue)
                    {
                        setText(iid, index, "Mở lại tiktok");
                        if (check == 8)
                        {
                            lD.Close("name", nameLD);
                            changeDevice(nameLD);
                            setText(iid, index, "không mở tiktok được", true);
                            now++;
                            ParalleFor(index);
                            return;
                        }

                    }
                } while (!istrue);


                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am force-stop com.zhiliaoapp.musically.go");
                await Task.Delay(5);

                setText(iid, index, "Tiến hành login");

                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://login");
                await Task.Delay(1);

                setText(iid, index, "Check login");
                if (checkNow(deviceID, "Sử dụng số điện thoại hoặc email", 2))
                {
                    //khong vao dc tiktok
                    setText(iid, index, "không vào được tiktok", true);
                    now++;
                    ParalleFor(index);
                    return;
                }
                Point point = new Point();
                check = 0;
                setText(iid, index, "Check Video");

                do
                {
                    ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://feed");
                    await Task.Delay(5);
                    KAutoHelper.ADBHelper.Tap(deviceID, 167, 299);

                    point = findByText(deviceID, @"(?<=id/tn).*?(?=>)", 4, false);
                    if (point.IsEmpty)
                    {
                        ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am force-stop com.zhiliaoapp.musically.go");
                        setText(iid, index, "Mở lại tiktok " + check.ToString() + "/5");
                        check++;
                        if (check == 5)
                        {
                            setText(iid, index, "Lỗi mạng", true);
                            now++;
                            ParalleFor(index);
                            return;
                        }
                    }
                } while (point.IsEmpty);
                setText(iid, index, "Chuẩn bị tương tác");

                //tuong tac

                ADBHelper.Swipe(deviceID, 145, 530, 145, 111, 300);
                ADBHelper.Swipe(deviceID, 145, 447, 145, 102, 300);
                ADBHelper.Swipe(deviceID, 145, 447, 145, 102, 300);
                ADBHelper.Swipe(deviceID, 145, 447, 145, 102, 300);


                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://profile");
                if (checkView.Checked)
                {
                   
                    trenDing(deviceID, index, iid);
                }
                if (checkvideo.Checked)
                {
                    sleep(2000);
                    byVideo(deviceID, index, iid);
                }

                if (checkuser.Checked)
                {
                    sleep(2000);

                    byUser(deviceID, index, iid);
                }

             
                setText(iid, index, "Tương tác thành công!");
             
            }
            catch
            {

                setText(iid, index, "Tương tác không thành công!",true);

            }
            finally
            {
                lD.Close("name", nameLD);
                sleep(3000);
                now++;
                ParalleFor(index);
            }
      
           
        }

        void SendText(string deviceID, string text)
        {
            string converttext = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
            string send = string.Format("adb -s " + deviceID + " shell am broadcast -a ADB_INPUT_B64 --es msg '{0}'", converttext);
            ADBHelper.ExecuteCMD(send);
        }
        void byUser(string deviceID,int index,string iid)
        {
            Setting st = new Setting(Application.StartupPath);
            List<userID> users = getUser();
            Point point = new Point();
            string[] cmt = File.ReadAllLines(Application.StartupPath + @"/Data/Cmt.txt");

            foreach (var item in users)
            {
                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://feed");
                setText(iid, index,"Truy cập vào user: " + item.name);
                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d " + "https://" + item.link);
                sleep(3000);

                point = findByText(deviceID, "Follow", 3);

                if (!point.IsEmpty)
                {


                    if (item.follow)
                    {
                        setText(iid, index,"Tiến hành follow");

                        ADBHelper.Tap(deviceID, point.X, point.Y);

                        sleep(2000);

                    }
                    if (item.tuongtac)
                    {
                        KAutoHelper.ADBHelper.Tap(deviceID, 55, 560);
                        sleep(1000);
                        setText(iid, index,"xem video " + item.time + "s");
                        Thread.Sleep(TimeSpan.FromSeconds(item.time));

                        for (int i = 0; i < item.sovideo; i++)
                        {
                            if (item.love)
                            {
                                setText(iid, index,"Tiến hành love");
                                KAutoHelper.ADBHelper.Tap(deviceID, 167, 299);
                                point = findByText(deviceID, @"(?<=id/tn).*?(?=>)", 3, false);
                                ADBHelper.Tap(deviceID, point.X, point.Y);
                            }
                            if (item.cmt)
                            {
                                // random cmt 
                                int num = new Random().Next(0, cmt.Count());
                                setText(iid, index,"Tiến hành cmt");
                                KAutoHelper.ADBHelper.Tap(deviceID, 82, 593);
                                sleep(1000);
                                SendText(deviceID, cmt[num]);
                                sleep(1000);

                                KAutoHelper.ADBHelper.Tap(deviceID, 317, 529);
                                sleep(3000);

                            }
                            setText(iid, index,"xem video " + item.time + "s");
                            ADBHelper.Swipe(deviceID, 145, 451, 145, 48, 300);
                            Thread.Sleep(TimeSpan.FromSeconds(item.time));
                        }

                    }
                }
                else KAutoHelper.ADBHelper.Tap(deviceID, 147, 324);

            }

        }
        void trenDing(string deviceID, int index,string iid)
        {
            Point point = new Point();
            setText(iid, index, "Tương tác video trending");
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://feed");
            for (int i = 0; i < numVideo.Value; i++)
            {
                ADBHelper.Swipe(deviceID, 145, 451, 145, 48, 300);
                setText(iid, index, "xem video " + numtime.Value + "s");
                Thread.Sleep(TimeSpan.FromSeconds((int)numtime.Value));
                if (checkLike.Checked)
                {
                    setText(iid, index, "Tiến hành love");
                    KAutoHelper.ADBHelper.Tap(deviceID, 167, 299);
                    point = findByText(deviceID, @"(?<=id/tn).*?(?=>)", 2, false);
                    if (point.IsEmpty)
                    {
                        setText(iid, index, "Không love được", true);
                        ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://feed");
                    }
                     

                    else
                        ADBHelper.Tap(deviceID, point.X, point.Y);
                }
                if (checkCmt.Checked)
                {
                    setText(iid, index, "Tiến hành cmt");

                    string[] cmt = File.ReadAllLines(Application.StartupPath + @"\Data\Cmt.txt");

                    int num1 = new Random().Next(0, cmt.Count());
                    KAutoHelper.ADBHelper.Tap(deviceID, 314, 404);
                    sleep(5000);
                    KAutoHelper.ADBHelper.Tap(deviceID, 82, 592);
                    sleep(2000);
                    if (cmt[num1]!= "")
                    {
                        SendText(deviceID, cmt[num1]);

                    }
                    sleep(2000);
                    KAutoHelper.ADBHelper.Tap(deviceID, 316, 528);
                    sleep(5000);
                 

                    
                 
                 
                    KAutoHelper.ADBHelper.Tap(deviceID, 177, 95);
                    KAutoHelper.ADBHelper.Tap(deviceID, 177, 95);
                    KAutoHelper.ADBHelper.Tap(deviceID, 177, 95);
                }
            }

        }

        void upload(string deviceID, int index,  string iid)
        {
            Setting st = new Setting(Application.StartupPath);
            setText(iid, index, "Tiến hành upload video");
            sleep(2000);
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://feed");
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell pm grant com.zhiliaoapp.musically.go android.permission.CAMERA");
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell pm grant com.zhiliaoapp.musically.go android.permission.RECORD_AUDIO");
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell pm grant com.zhiliaoapp.musically.go android.permission.WRITE_EXTERNAL_STORAGE");

            sleep(2000);
            KAutoHelper.ADBHelper.Tap(deviceID, 171, 596);
            sleep(10000);
            setText(iid, index, "đang lấy video");

            bool random = (st.getSetting("setupvideo", "random").ToString() == "True") ? true : false;
            string path = st.getSetting("setupvideo", "path");
            string pathfilevideo = "";
            if (random)
            {
                string[] files;
                files = Directory.GetFiles(Path.GetDirectoryName(path)) ;
                int num = new Random().Next(0, files.Count());
                pathfilevideo = files[num];
            }
            else
            {
                pathfilevideo = path;
            }
            setText(iid, index, "push video");

            ADBHelper.ExecuteCMD(string.Format("adb -s {0} push {1} /sdcard/video.mp4", deviceID, pathfilevideo)) ;
            ADBHelper.ExecuteCMD(string.Format("adb -s {0} shell am broadcast -a android.intent.action.MEDIA_MOUNTED -d file:///sdcard", deviceID));
            KAutoHelper.ADBHelper.Tap(deviceID, 281,  531);
            sleep(5000);
            KAutoHelper.ADBHelper.Tap(deviceID, 49, 110);
            sleep(5000);

            KAutoHelper.ADBHelper.Tap(deviceID, 289,  78);
            sleep(5000);
            KAutoHelper.ADBHelper.Tap(deviceID, 304,  590);
            // bat dau vào ghi hastag
            setText(iid, index, "chỉnh sửa nội dung");
            KAutoHelper.ADBHelper.Tap(deviceID, 93, 100);
            string note = Properties.Settings.Default.note;
            if (note != "")
                 SendText(deviceID,note);
            bool checkcmt = (st.getSetting("setupvideo", "cmt").ToString() == "True") ? true : false;
            if (!checkcmt)
                KAutoHelper.ADBHelper.Tap(deviceID, 186, 314);
            bool checkduet = (st.getSetting("setupvideo", "duet").ToString() == "True") ? true : false;
            if (checkduet)
                KAutoHelper.ADBHelper.Tap(deviceID, 154, 368);

        }

        void byVideo(string deviceID,int index,string iid)
        {                                                                                  
            Setting st = new Setting(Application.StartupPath);
            List<videoID> videos = getVideo();
            Point point = new Point();
            string[] cmt = File.ReadAllLines(Application.StartupPath + @"/Data/Cmt.txt");

            foreach (var item in videos)
            {

                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d tiktok://feed");
                setText(iid, index,"vào video của " + item.name);
                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d " + "https://" + item.link);
                sleep(1000);
                setText(iid, index,"Xem video " + item.time + "s");

                Thread.Sleep(TimeSpan.FromSeconds(item.time));
                if (item.follow)
                {
                    setText(iid, index,"tiến hành follow");
                    sleep(1000);
                    ADBHelper.Swipe(deviceID, 273, 324, 17, 324, 400);

                    point = findByText(deviceID, "Follow", 3);
                    if (point.IsEmpty)
                    {
                        KAutoHelper.ADBHelper.Tap(deviceID, 147, 324);
                        setText(iid, index,"Không follow được | follow theo tọa độ!", true);
                    }
                    else ADBHelper.Tap(deviceID, point.X, point.Y);
                    ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am start -W -a android.intent.action.VIEW -d " + "https://" + item.link);
                    sleep(3000);
                    ADBHelper.Swipe(deviceID, 17, 324, 273, 324, 400);

                }
                if (item.love)
                {
                    setText(iid, index,"Tiến hành love");
                    KAutoHelper.ADBHelper.Tap(deviceID, 167, 299);

                    point = findByText(deviceID, @"(?<=id/tn).*?(?=>)", 3, false);
                    ADBHelper.Tap(deviceID, point.X, point.Y);
                }
                if (item.cmt)
                {
                    // random cmt 
                    int num = new Random().Next(0, cmt.Count());
                    setText(iid, index,"Tiến hành cmt");
                    KAutoHelper.ADBHelper.Tap(deviceID, 83, 595);
                    KAutoHelper.ADBHelper.Tap(deviceID, 83, 595);

                    sleep(5000);
                    SendText( deviceID, cmt[num]);
                    sleep(3000);

                    KAutoHelper.ADBHelper.Tap(deviceID, 317, 529);
                    sleep(3000);
                }

            }
        }
        List<videoID> getVideo()
        {
            List<videoID> result = new List<videoID>();
            string[] list = File.ReadAllLines(pathvideo);
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
            string[] list = File.ReadAllLines(pathuser);//
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
 
        List<Thread> threads = new List<Thread>();
        private int index = 0;
        private static bool CanPing(string address)
        {
            Ping ping = new Ping();

            try
            {
                PingReply reply = ping.Send(address, 2000);
                if (reply == null) return false;

                return (reply.Status == IPStatus.Success);
            }
            catch (PingException )
            {
                return false;
            }
        }

        private void btnrun_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.check)
            {
                MessageBox.Show("Bạn kích hoạt phần mềm đi nha :(", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            if (checkuser.Checked)
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
            if (btnrun.Text == "CHẠY TƯƠNG TÁC")
            {

                if (dataGridviewTik.Rows.Count < (int)numMain.Value || numMain.Value == 0)
                {
                    MessageBox.Show("Vui lòng nhập số account chạy <= account đang có", "BemmTeam");
                    return;
                }
                else
                {
                    index = 0;
                    now = 0;
                    threads.Clear();
                    btnrun.Text = "DỪNG TƯƠNG TÁC";
                    Parallel.For(index, (int)numMain.Value + index, RunTask);
                }
            }
            else
            {
                foreach (var item in threads)
                {
                    item.Abort();
                    
                }
                threads.Clear();
                index = 0;
                now = 0;
                btnrun.Text = "CHẠY TƯƠNG TÁC";
                panelPhone.Controls.Clear();
                showDataTab(tabname);


            }

        }
        private int now = 0;
        void ParalleFor(int i)
        {
            if (index >= dataGridviewTik.Rows.Count )
            {
                panelPhone.Invoke(new Action(()=> panelPhone.Controls.Clear())) ;

                try
                {
                    foreach (var item in threads)
                    {
                        item.Abort();
                    }
                    threads.Clear();
                    btnrun.Text = "CHẠY TƯƠNG TÁC";

                }
                catch (Exception)
                {
                }
            }
            else
            {
                if (now == numMain.Value)
                {
                    panelPhone.Invoke(new Action(() => panelPhone.Controls.Clear()));
                    int inde = (int)numMain.Value + index;
                    if (dataGridviewTik.Rows.Count < inde)
                    {
                        inde = dataGridviewTik.Rows.Count;
                        
                    }
                    threads.Clear();
                    now = 0;
                    var tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;
                    Parallel.For(index,inde , RunTask);
                }
            }

            dataGridviewTik.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
        }
     
        
        public async void RunTask(int i)
        {

            string nameLD = dataGridviewTik.Rows[i].Cells[3].Value.ToString();
            await Task.Delay(2);
            Thread thread = new Thread(()=> {
            

                string deviceID = "";
                LDPlayer ldplayer = new LDPlayer();
                setText("",i, "đang kiểm tra path LD");
                string path = pathLD + @"\ldconsole.exe";
                if (!File.Exists(path))
                {
                    setText("", i , "Path LD không đúng!", true);
                    now++;
                    ParalleFor(index);
                    return;
                }
                LDPlayer.pathLD = path;
                var list = ldplayer.GetDevices2();
                bool checkname = false;
                foreach (var item in list)
                {
                    if (nameLD == item.name)
                    {
                        checkname = true;
                        break;
                    }
         
                }
                if (!checkname)
                {
                    setText("", i, "Không tồn tại Name LD", true);
                    now++;
                    ParalleFor(index);
                    return;
                }
                ldplayer.Close("name", nameLD);

                changeDevice(nameLD);
                ldplayer.ExecuteLD("modify --name " + nameLD + " --resolution 343,616,160");
                setText("",i, "đang khởi động LD");
                Thread.Sleep(3000);
                int iid = getIndex(nameLD);
                if (iid== -1)
                {
                    setText("",i , "Không nhận dạnh được LD!", true);
                    now++;
                    ParalleFor(index);
                    return;
                }
                //   Phone phone = new Phone(dataGridviewTik.Rows[i].Cells[0].Value.ToString(),nameLD);
                // panel = phone.panel;
                //panelPhone.Invoke(new Action(() => panelPhone.Controls.Add(panel))); 
                OpenLD(iid.ToString(),nameLD, dataGridviewTik.Rows[i].Cells[0].Value.ToString());
                int check = 0;

                do
                {
      
                    do
                    {
                        ADBHelper.ExecuteCMD("adb kill-server");
                        sleep(2000);
                        ADBHelper.ExecuteCMD("adb start-server");
                        var listDevices = ldplayer.GetDevices2_Running();
                        check++;

                        foreach (var item in listDevices)
                        {
                            if (item.name == nameLD)
                            {
                                deviceID = item.adb_id;
                                break;
                            }
                     
                        }
                        if (check == 10 )
                        {
                            deviceID = "";
                            break;
                        }
                    } while (deviceID.Contains("offline"));
                    if (check == 10)
                    {
                        deviceID = "";
                        break;
                    }
                    else { sleep(3000); }
                } while (deviceID == "");
                if (deviceID == "")
                {
                    setText("", i, "Không mở LD được", true);
                    now++;
                    ParalleFor(index);
                    return;
                }
                setText("index " + iid.ToString(), i, deviceID);

                Thread.Sleep(10000);
                setText("khởi động adbkeyboard " + iid.ToString(), i, deviceID);

                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell ime set com.android.adbkeyboard/.AdbIME");

            string pathBackup = dataGridviewTik.Rows[i].Cells[2].Value.ToString();
                runTT(i, deviceID, pathBackup, nameLD, "index " + iid.ToString(), dataGridviewTik.Rows[i].Cells[4].Value.ToString());  ;

            });
            thread.IsBackground = true;
            thread.Start();
            index++;
            threads.Add(thread);
        }
      
           
        
        private void menuLD_Click(object sender, EventArgs e)
        {
            if (dataGridviewTik.Rows.Count >0)
            {
                using (nameLD ld = new nameLD())
                {
                    ld.ShowDialog();
                    if (ld.name != "")
                    {
                        dataGridviewTik.Rows[dataGridviewTik.CurrentCell.RowIndex].Cells[3].Value = ld.name;

                    }
                }
            }
            
        }

        private void FNuoiTikTok_Load(object sender, EventArgs e)
        {
       
        }

        ///




        private void OpenLD(string index, string name,string user)
        {

            Thread r = new Thread(() =>
            {


                panelPhone.Invoke(new Action(delegate ()
                {
                    index = "index " + index;
                    Panel panel = new Panel();
                    //panel.Width = 300;
                    panel.BackColor = Color.White;
                    panel.Height = 575;
                    panel.Size = new Size(this.LDSize[0], this.LDSize[1]);
                    panel.Name = "panel_" + index;
                    panel.BorderStyle = BorderStyle.FixedSingle;
            

                    ////////////////////

                    Label label = new Label
                    {
                        Text = "Mở LDPlayer...",
                        //Location = new Point(0, this.LDSize[1]),
                        Size = new Size(this.LDSize[0], 20),
                        BackColor = Color.FromArgb(60, 63, 81),
                        Dock = DockStyle.Top,
                        BorderStyle = BorderStyle.None,
                        ForeColor = Color.White,

                        Name = "label_" + index,
                        AutoSize = false
                    };


                    Label label2 = new Label
                    {
                        Text = "LD name: " + name + " - User: " + user,
                        // Location = new Point(0, this.LDSize[1]),
                        Size = new Size(this.LDSize[0], 20),
                        Dock = DockStyle.Top,

                        BackColor = Color.FromArgb(60, 63, 81),
                        BorderStyle = BorderStyle.None,
                        ForeColor = Color.White,
                        Name = "label_" + index + 1,
                        AutoSize = false
                    };


                    /////////////////
                    Panel panel2 = new Panel();
                    panel2.Width = 335;
                    panel2.Dock = DockStyle.Top;
                    panel2.Height = 575;
                    panel2.AutoSize = false;
                    panel2.BackgroundImage = Properties.Resources.logopng;
                    panel2.BackgroundImageLayout = ImageLayout.Stretch;
                    panel.Controls.Add(label2);
                    panel.Controls.Add(label);
                    panel.Controls.Add(panel2);

                    panelPhone.Controls.Add(panel);


                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            Thread.Sleep(1000);

                            Process process = Process.Start(pathLD + "\\dnplayer.exe", index.Replace(" ", "="));
                            process.WaitForInputIdle();
                            IntPtr mainWindowHandle = process.MainWindowHandle;
                            while (mainWindowHandle == IntPtr.Zero)
                            {
                                Thread.Sleep(200);
                                mainWindowHandle = process.MainWindowHandle;
                            }


                            SetParent(mainWindowHandle, panel2.Handle);
                            //   SetWindowLong(mainWindowHandle, -16, 268435456);
                            MoveWindow(mainWindowHandle, this.LDSize[2], this.LDSize[3], this.LDSize[4], this.LDSize[5], true);


                            break;
                        }
                        catch
                        {
                            Thread.Sleep(2000);

                        }
                    }
                }));
            });
            r.IsBackground = true;
            r.Start();
        }

        public int[] LDSize = new int[]
        {
            343,
            616,
            -1,
            -36,
            343,
            616
        };
        private void ThongbaoLD(string text, string index)
        {
         
            foreach (object obj in this.panelPhone.Controls)
            {
                Control control = (Control)obj;
                bool flag = control.Name == "panel_" + index;
                if (flag)
                {
                    IEnumerator enumerator2 = control.Controls.GetEnumerator();

                    while (enumerator2.MoveNext())
                    {
                        Control iteme = (Control)enumerator2.Current;
                        bool flag2 = iteme.Name == "label_" + index;
                        if (flag2)
                        {
                            this.panelPhone.Invoke(new Action(delegate ()
                            {
                                iteme.Text = text;
                            }));
                        }
                    }

                }
            }
        }



        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        [DllImport("user32")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            inforVideo inforVideo = new inforVideo();
            inforVideo.ShowDialog();
        }

        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {
            frmCmt frm = new frmCmt();
            frm.ShowDialog();
        }
    }
}
