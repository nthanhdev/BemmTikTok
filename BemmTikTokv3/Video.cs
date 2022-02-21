using MadMilkman.Ini;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BemmTikTokv3
{
    public partial class Video : Form
    {
        public Video()
        {
            InitializeComponent();
        }

        BindingList<videoID> listvideos = new BindingList<videoID>();

        private void btnimport_Click(object sender, EventArgs e)
        {

        }
        private void showData(string nametab)
        {
            string path = Application.StartupPath + @"\Data\VideoID\" + nametab + ".txt";
            string[] lines = File.ReadAllLines(path);
            dataGridViewVideo.Rows.Clear();

            Thread r = new Thread(() =>
            {
                foreach (var item in lines)
                {
                    string[] info = item.Split('|');
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
                    dataGridViewVideo.Invoke(new Action(() =>
                    {
                        listvideos.Add(video);

                    }));
                }

                dataGridViewVideo.Invoke(new Action(() => dataGridViewVideo.DataSource = listvideos));
                designView();
            });
            r.IsBackground = true;
            r.Start();
        }
        void designView()
        {
            dataGridViewVideo.Invoke(new Action(() =>
            {
                dataGridViewVideo.Columns[0].FillWeight = 400;
                dataGridViewVideo.Columns[1].FillWeight = 150;

                dataGridViewVideo.Columns[5].HeaderText = "time";
                dataGridViewVideo.Columns[6].HeaderText = "Activate";

            }));

        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (comTab.Text == "")
            {
                MessageBox.Show("Vui lòng thêm tab mới!","BemmTeam");

            }
            if (txtlink.Text != "" && txtlink.Text.Contains("vm") || txtlink.Text.Contains("vt"))
            {
                videoID video = new videoID()
                {
                    link = txtlink.Text,
                    name = txtname.Text,
                    follow = checkfollow.Checked,
                    cmt = checkcmt.Checked,
                    love = checklove.Checked,
                    time = int.Parse(txttime.Text),
                    kichhoat = checkkichhoat.Checked
                };
                listvideos.Add(video);
                dataGridViewVideo.DataSource = null;
                dataGridViewVideo.DataSource = listvideos;
                designView();
            }
            else
                MessageBox.Show("Link không đúng định dạng!","BemmTeam");
        }
       
        private void Video_Load(object sender, EventArgs e)
        {

            string[] files = Directory.GetFiles(Application.StartupPath + @"\Data\VideoID");
            foreach (var item in files)
            {
                comTab.Items.Add(Path.GetFileName(item.Replace(".txt", "")));
            }
            if (comTab.Items.Count != 0)
            {
                string nameTab = comTab.Items[0].ToString();
                comTab.Text = nameTab;
            }
        }
        void save()
        {
            string text = "";
            string path = "";
            if (comTab.Text != "")
              path = Application.StartupPath + @"\Data\VideoID\" + comTab.SelectedItem.ToString() + ".txt";

            if (File.Exists(path))
            {

                foreach (DataGridViewRow item in dataGridViewVideo.Rows)
                {

                    dataGridViewVideo.CurrentCell = dataGridViewVideo.Rows[0].Cells[0];
                    dataGridViewVideo.Rows[0].Selected = true;


                    string link = item.Cells[0].Value.ToString();
                    string name = item.Cells[1].Value.ToString();
                    string follow = item.Cells[2].Value.ToString();
                    string cmt = item.Cells[3].Value.ToString();
                    string love = item.Cells[4].Value.ToString();
                    string time = item.Cells[5].Value.ToString();

                    string kichhoat = item.Cells[6].Value.ToString();
                    text += link + "|" + name + "|" + follow + "|" + cmt + "|" + love + "|" + time + "|" + kichhoat + Environment.NewLine;
                    setSetting("listvideo", "path", path);
                }

                File.WriteAllText(path, text);
            }
        }

        private void Video_FormClosing(object sender, FormClosingEventArgs e)
        {
            save();
        }
        string getSetting(string sub, string key)
        {
            var ini = new IniFile();
            ini.Load(Application.StartupPath + "\\Setting.ini");
            IniSection iniSection = ini.Sections[sub];
            string result = iniSection.Keys[key].Value;
            return result;
        }

        void setSetting(string sub, string key, string data)
        {
            var ini = new IniFile();
            ini.Load(Application.StartupPath + "\\Setting.ini");
            IniSection iniSection = ini.Sections[sub];
            iniSection.Keys[key].Value = data;
            ini.Save(Application.StartupPath + "\\Setting.ini");
        }

        private void dataGridViewVideo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridViewCell clickedCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];

                    // Here you can do whatever you want with the cell
                    this.dataGridViewVideo.CurrentCell = clickedCell;  // Select the clicked cell, for instance

                    // Get mouse position relative to the vehicles grid
                    var relativeMousePosition = dataGridViewVideo.PointToClient(Cursor.Position);

                    // Show the context menu
                    this.contextMenuStrip1.Show(dataGridViewVideo, relativeMousePosition);
                }
            }
        }

        private void menuDownLoadVideo_Click(object sender, EventArgs e)
        {
            dataGridViewVideo.Rows.RemoveAt(dataGridViewVideo.CurrentCell.RowIndex);

        }

        private void txtlink_TextChanged(object sender, EventArgs e)
        {
            if (txtlink.Text.Contains("https://") || txtlink.Text.Contains("https://"))
            {
                txtlink.Text = txtlink.Text.Replace("https://", "");
                txtlink.Text = txtlink.Text.Replace("http://", "");

            }
        }

        private void checkcmt_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnimport_Click_1(object sender, EventArgs e)
        {
            using (var frm = new AddTab())
            {
                string name = "";
                frm.ShowDialog();

                name = frm.txtname.Text;
                if (name != "")
                {
                    using (var file = File.CreateText(Application.StartupPath + @"\Data\VideoID\" + name + ".txt")) { }
                    comTab.Items.Add(name);
                    comTab.SelectedItem = name;
                    showData(name);

                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (comTab.Text != "")
            {
                var result = MessageBox.Show("Bạn có muốn xóa tab: " + comTab.SelectedItem.ToString() + " không ?", "BemmTeam", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string path = Application.StartupPath + @"\Data\VideoID\" + comTab.Text + ".txt";
                    File.Delete(path);
                    comTab.Items.RemoveAt(comTab.SelectedIndex);
                    if (comTab.Items.Count != 0)
                    {
                        comTab.SelectedIndex = 0;
                    }
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            save();
        }

        private void comTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comTab.Text != "")
            {
                showData(comTab.SelectedItem.ToString());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmCmt frm = new frmCmt();
            frm.ShowDialog();
        }
    }


}
