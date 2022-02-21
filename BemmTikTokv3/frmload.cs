using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
namespace BemmTikTokv3
{
 
    public partial class frmload : Form
    {
        public frmload()
        {
            InitializeComponent();
        }
        protected string key = "6190cb2541928b001768fe49";

        private IRestResponse rest(string li)
        {
            try
            {
                var res = new RestClient("https://" + key + ".mockapi.io/key/user/" + li);
                var method = new RestRequest(method: Method.GET);
                method.AddHeader("content-type", "application/json");
                IRestResponse result = res.Execute(method);
                return result;
            }
            catch
            {
                return null;
            }
        }

        private void domain()
        {
            try
            {
                var res = new RestClient("https://" + key + ".mockapi.io/key/infor/domain");
                var method = new RestRequest(method: Method.GET);
                method.AddHeader("content-type", "application/json");
                IRestResponse result = res.Execute(method);
                domain domain = new domain();
                domain = JsonConvert.DeserializeObject<domain>(result.Content);
                if(domain != null)
                {
                    Properties.Settings.Default.domain = domain.dot;
                    Properties.Settings.Default.gid = domain.gid;

                    Properties.Settings.Default.Save();
                }else
                {
                       Properties.Settings.Default.domain = "gmail.com";
                Properties.Settings.Default.gid = "gm";


                Properties.Settings.Default.Save();
                }
            }
            catch
            {
                Properties.Settings.Default.domain = "gmail.com";
                Properties.Settings.Default.gid = "gm";


                Properties.Settings.Default.Save();
            }
        }
        int time = 5;
        private void frmload_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
        string getuid()
        {
            //string key = "Win32_BIOS";
            //ManagementObjectSearcher selectvalue = new ManagementObjectSearcher("select * from " + key);
            //string serialId = "";
            //foreach (ManagementObject getserial in selectvalue.Get())
            //{
            //    serialId += getserial["SerialNumber"].ToString();
            //}
            return "FREE";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (time ==0 )
            {
                timer1.Stop();
                domain();

                string li = Properties.Settings.Default.key;
                bool check = false;
                if (li != "")
                {
                    var result = rest(li);
                    if (result.IsSuccessful && result != null)
                    {
                        user info = JsonConvert.DeserializeObject<user>(result.Content);
                        if (info.key == li)
                        {
                            check = true;
                        }
                        else check = false;
                        if (info.uid != getuid())
                        {
                            check = false;
                        }
                    }
                    else
                    {
                        check = false;
                    }


                }
                else check = false;
                Properties.Settings.Default.check = check;
                Properties.Settings.Default.Save();
                Main main = new Main(check);
                this.Hide();
                main.ShowDialog();
                this.Close();
            }
            else
            {
                time--;
            }
        }
    }
}
