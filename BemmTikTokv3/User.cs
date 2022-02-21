using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using MadMilkman.Ini;

namespace BemmTikTokv3
{
    public partial class User : Form
    {
        public User()
        {
            InitializeComponent();
        }
        BindingList<userID> listusers = new BindingList<userID>();
        private void User_Load(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(Application.StartupPath + @"\Data\UserID");
            foreach (var item in files)
            {
                comTab.Items.Add(Path.GetFileName(item.Replace(".txt","")));
            }
            if (comTab.Items.Count != 0)
            {
                string nameTab = comTab.Items[0].ToString();
                comTab.Text = nameTab;

            }
        }
        private void showData(string nametab)
        {
            string path = Application.StartupPath + @"\Data\UserID\" + nametab + ".txt";
            string[] lines = File.ReadAllLines(path);
            dataGridViewUser.Rows.Clear();
            Thread r = new Thread(() =>
            {
                foreach (var item in lines)
                {
                    string[] info = item.Split('|');
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
                    dataGridViewUser.Invoke(new Action(() =>
                    {
                        listusers.Add(user);

                    }));
                }

                dataGridViewUser.Invoke(new Action(() => dataGridViewUser.DataSource = listusers));
                designView();
            });
            r.IsBackground = true;
            r.Start();
        }
        private void btnadd_Click(object sender, EventArgs e)
        {
            if (txtlink.Text != "" && txtlink.Text.Contains("vm"))
            {
                userID user = new userID()
                {
                    link = txtlink.Text,
                    name = txtname.Text,
                    follow = checkfollow.Checked,
                    tuongtac = checktuongtac.Checked,
                    cmt = checkcmt.Checked,
                    love = checklove.Checked,
                    sovideo = int.Parse(txtsovideo.Text),
                    time = int.Parse(txttime.Text),
                    kichhoat = checkkichhoat.Checked
                };
                listusers.Add(user);
                dataGridViewUser.DataSource = null;
                dataGridViewUser.DataSource = listusers;
                designView();
            }
            else
                MessageBox.Show("Link không đúng định dạng!");


        }


        private void btnimport_Click(object sender, EventArgs e)
        {
            using (var frm = new AddTab())
            {
                string name = "";
                frm.ShowDialog();
                
                   name = frm.txtname.Text;
                if (name != "")
                {
                    using (var file = File.CreateText(Application.StartupPath + @"\Data\UserID\" + name + ".txt")) { }
                    comTab.Items.Add(name);
                    comTab.SelectedItem = name;
                    showData(name);
                   
                }
            }


        }
        void designView()
        {
            dataGridViewUser.Invoke(new Action(() =>
            {
                save();

                dataGridViewUser.Columns[0].FillWeight = 400;
                dataGridViewUser.Columns[1].FillWeight = 150;


                dataGridViewUser.Columns[3].HeaderText = "TT";

                dataGridViewUser.Columns[6].HeaderText = "video";
                dataGridViewUser.Columns[7].HeaderText = "time";
                dataGridViewUser.Columns[8].HeaderText = "Activate";




            }));

        }
        void save()
        {
            if (comTab.Items.Count != 0)
            {
                string text = "";
                string path = Application.StartupPath + @"\Data\UserID\" + comTab.SelectedItem.ToString() + ".txt";
                if (File.Exists(path))
                {
                    foreach (DataGridViewRow item in dataGridViewUser.Rows)
                    {

                        dataGridViewUser.CurrentCell = dataGridViewUser.Rows[0].Cells[0];
                        dataGridViewUser.Rows[0].Selected = true;


                        string link = item.Cells[0].Value.ToString();
                        string name = item.Cells[1].Value.ToString();
                        string follow = item.Cells[2].Value.ToString();
                        string tuongtac = item.Cells[3].Value.ToString();
                        string cmt = item.Cells[4].Value.ToString();
                        string love = item.Cells[5].Value.ToString();
                        string sovideo = item.Cells[6].Value.ToString();
                        string time = item.Cells[7].Value.ToString();

                        string kichhoat = item.Cells[8].Value.ToString();
                        text += link + "|" + name + "|" + follow + "|" + tuongtac + "|" + cmt + "|" + love + "|" + sovideo + "|" + time + "|" + kichhoat + Environment.NewLine;
                        setSetting("listuser", "path", path);
                    }

                    File.WriteAllText(path, text);
                }
            }
        }
        private void User_FormClosing(object sender, FormClosingEventArgs e)
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

        private void checktuongtac_CheckedChanged(object sender, EventArgs e)
        {
            if (!checktuongtac.Checked)
            {
                checkcmt.Checked = false;
                checklove.Checked = false;
                txtsovideo.Text = "0";
                checkcmt.Enabled = false;
                checklove.Enabled = false;
                txtsovideo.Enabled = false;
                txttime.Enabled = false;
            }
            else
            {
               
                checkcmt.Enabled = true;
                checklove.Enabled = true;
                txtsovideo.Enabled = true;
                txttime.Enabled = true;

            }
        }

        private void dataGridViewUser_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Ignore if a column or row header is clicked
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridViewCell clickedCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];

                    // Here you can do whatever you want with the cell
                    this.dataGridViewUser.CurrentCell = clickedCell;  // Select the clicked cell, for instance

                    // Get mouse position relative to the vehicles grid
                    var relativeMousePosition = dataGridViewUser.PointToClient(Cursor.Position);

                    // Show the context menu
                    this.contextMenuStrip1.Show(dataGridViewUser, relativeMousePosition);
                }
            }
        }

        private void menuDownLoadVideo_Click(object sender, EventArgs e)
        {
            dataGridViewUser.Rows.RemoveAt(dataGridViewUser.CurrentCell.RowIndex);
         
        }

        private void txtlink_TextChanged(object sender, EventArgs e)
        {
            if (txtlink.Text.Contains("https://") || txtlink.Text.Contains("https://"))
            {
                txtlink.Text = txtlink.Text.Replace("https://", "");
                txtlink.Text = txtlink.Text.Replace("http://", "");

            }
        }

        private void btnDelete(object sender, EventArgs e)
        {
            if (comTab.Text != "")
            {
                var result = MessageBox.Show("Bạn có muốn xóa tab: " + comTab.SelectedItem.ToString() + " không ?" , "BemmTeam" , MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string path = Application.StartupPath + @"\Data\UserID\" + comTab.Text + ".txt";
                    File.Delete(path);
                    comTab.Items.RemoveAt(comTab.SelectedIndex);
                    if (comTab.Items.Count != 0)
                    {
                        comTab.SelectedIndex = 0;
                    }
                }
            }
        }

        private void comTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comTab.Text != "")
            {
                showData(comTab.SelectedItem.ToString()) ;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            save();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmCmt frm = new frmCmt();
            frm.ShowDialog();
        }
    }
}
