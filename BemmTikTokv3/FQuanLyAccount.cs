using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BemmTikTokv3
{
    public partial class FQuanLyAccount : UserControl
    {
       
        public class DBAcount
        {
            public string userID { get; set; }
            public string password { get; set; }
            public string pathBackup { get; set; }
            public string cookie { get; set; }
            public string NameLD { get; set; }
            public string proxy { get; set; }
        }
        public FQuanLyAccount()
        {
            InitializeComponent();
        }
        private string nameselect = "";
        Guna.UI2.WinForms.Guna2GradientButton btnselect;
        List<DBAcount> listAccount = new List<DBAcount>();
        private void Btn_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2GradientButton btn = sender as Guna.UI2.WinForms.Guna2GradientButton;
            showData(btn.Text);
            nameselect = btn.Text;
            btnselect = btn;
        }

        void addTab(string nameTab)
        {
            Guna.UI2.WinForms.Guna2GradientButton btn = newTab(nameTab);
            btn.Click += Btn_Click;
            panelTab.Controls.Add(btn);
            
        }

        Guna.UI2.WinForms.Guna2GradientButton newTab(string nameTab)
        {
            Guna.UI2.WinForms.Guna2GradientButton btn = new Guna.UI2.WinForms.Guna2GradientButton();
            btn.Animated = true;
            btn.BackColor = System.Drawing.Color.Transparent;
            btn.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            btn.BorderStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;

     
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.CustomImages.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            btn.CustomImages.Parent = btn;
            btn.FillColor = System.Drawing.SystemColors.Highlight;
            btn.FillColor2 = System.Drawing.Color.Empty;
            this.panelTab.SetFlowBreak(btn, true);
            btn.Font = new System.Drawing.Font("Segoe UI", 12F);
            btn.ForeColor = System.Drawing.Color.White;
            btn.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(62)))), ((int)(((byte)(103)))));
            btn.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(52)))));
            btn.HoverState.Parent = btn;
            btn.Location = new System.Drawing.Point(3, 3);
            btn.Name = "btnTab";
            btn.PressedColor = System.Drawing.Color.BlueViolet;
            btn.ShadowDecoration.Parent = btn;
            btn.Size = new System.Drawing.Size(170, 45);
            btn.TabIndex = 3;
            btn.Text = nameTab;
            btn.UseTransparentBackground = true;

            return btn;
      
        }
        private void showTab()
        {
            string[] files = Directory.GetFiles(Application.StartupPath + @"\Data\Account");
            foreach (var item in files)
            {
                addTab(Path.GetFileName(item.Replace(".txt", "")));
            }
        }
        private void showData(string nametab , bool isClear = true)
        {
            if (isClear)
            {
                dataGridviewTik.Rows.Clear();

            }
            string path = Application.StartupPath + @"\Data\Account\" + nametab + ".txt";
            string data = File.ReadAllText(path);
            data = data.Replace(Environment.NewLine, "");
            string[] read = data.Split('#');

            foreach (var item in read)
            {
                if (item != "")
                {
                    string[] info = item.Split(',');
                    if (info.Count() == 6)
                    {
                        dataGridviewTik.Rows.Add(info[0], info[1], info[2], info[3], info[4], info[5]);

                    }
                    else
                        dataGridviewTik.Rows.Add(info[0], info[1], info[2], info[3], info[4], info[5], info[6]);
                }
            }
        }

        private void readData(string data)
        {

            if (data != null)
            {
                data = data.Replace(Environment.NewLine, "");
                data = data.Replace("\n", "");
                string[] read = data.Split('#');

                foreach (var item in read)
                {
                    if (item != "")
                    {
                        string[] info = item.Split(',');
                        if (info[4] == "")
                        {
                            info[4] = "None";
                        }
                        if (info[5] == "")
                        {
                            info[5] = "None";
                        }
                        if (info.Count() == 6)
                        {
                            dataGridviewTik.Rows.Add(info[0], info[1], info[2], info[3], info[4], info[5]);

                        }else
                            dataGridviewTik.Rows.Add(info[0], info[1], info[2], info[3], info[4], info[5],info[6]);



                    }
                }
            }
        }
      void import(string path)
        {
            string data = File.ReadAllText(path);
            data = data.Replace(Environment.NewLine, "");
            string[] read = data.Split('#');

            foreach (var item in read)
            {
                if (item != "")
                {
                    string[] info = item.Split(',');
                    if (info.Count() == 6)
                    {
                        dataGridviewTik.Rows.Add(info[0], info[1], info[2], info[3], info[4], info[5]);

                    }
                    else
                        dataGridviewTik.Rows.Add(info[0], info[1], info[2], info[3], info[4], info[5], info[6]);
                }
            }
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
                    using (var file = File.CreateText(Application.StartupPath + @"\Data\Account\" + name + ".txt")) { }
                    addTab(name);

                }
            }
        }

        private void FQuanLyAccount_Load(object sender, EventArgs e)
        {
            showTab();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn xóa tab: " + nameselect + " không ?", "BemmTeam", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string path = Application.StartupPath + @"\Data\Account\" + nameselect + ".txt";
                File.Delete(path);
                panelTab.Controls.Remove(btnselect);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (nameselect != "")
                {
                    using (addAccount account = new addAccount(nameselect))
                    {
                        account.ShowDialog();

                        string a = account.data;
                        if (a != null)
                        {
                            string[] b = a.Split('$');
                            int num = int.Parse(b[0]);
                            if (num == 0 || num == 1)
                            {
                                readData(b[1]);
                            }
                            else if (num == 2)
                            {
                                import(b[1]);
                            }
                            else
                            {
                                showData(b[1], false);
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng kiểm tra lại dữ liệu", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
       
            }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string text = "";
            string path = Application.StartupPath + @"\Data\Account\" + nameselect + ".txt";
            foreach (DataGridViewRow item in dataGridviewTik.Rows)
            {

                dataGridviewTik.CurrentCell = dataGridviewTik.Rows[0].Cells[0];
                dataGridviewTik.Rows[0].Selected = true;

                string user = item.Cells[0].Value.ToString();
                string pass = item.Cells[1].Value.ToString();
                string pathbackup = item.Cells[2].Value.ToString();
                string cookie = item.Cells[3].Value.ToString();
                string nameld = item.Cells[4].Value.ToString();
                string proxy = item.Cells[5].Value.ToString();
                string email = item.Cells[6].Value.ToString();
                if (proxy == "")
                     proxy = "None";
                if (nameld == "")
                    nameld = "None";
                text += Environment.NewLine + "#" + user + "," + pass + "," + pathbackup + "," + cookie + "," + nameld + "," + proxy + "," + email;
            }
            File.WriteAllText(path, text);

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridviewTik.Rows.Count > 0)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa account?", "BemmTeam", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dataGridviewTik.SelectedRows)
                    {
                    dataGridviewTik.Rows.Remove(row);

                    }
                    btnSave.PerformClick();
                }
            }
        }



        private void chonld(object sender, EventArgs e)
        {
            if (dataGridviewTik.Rows.Count > 0)
            {
                using (nameLD ld = new nameLD())
                {
                    ld.ShowDialog();
                    if (ld.name != "")
                    {
                        dataGridviewTik.Rows[dataGridviewTik.CurrentCell.RowIndex].Cells[4].Value = ld.name;
                    }
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

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

        private void copy(object sender, EventArgs e)
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
                            string pathBackup = row.Cells[2].Value.ToString();
                            string cookie = row.Cells[3].Value.ToString();
                            string nameLD = row.Cells[4].Value.ToString();
                            string proxy = row.Cells[5].Value.ToString();
                            string email = row.Cells[6].Value.ToString();
                            string data = Environment.NewLine + "#" + user + "," + password + "," + pathBackup + "," + cookie + "," + nameLD + "," + proxy + "," + email ;
                            string main = File.ReadAllText((Application.StartupPath + @"\Data\Account\" + name + ".txt")) + data;
                            File.WriteAllText(Application.StartupPath + @"\Data\Account\" + name + ".txt", main);

                        }
                        MessageBox.Show("Đã thêm account vào tab " + name, "BemmTeam");

                        btnSave.PerformClick();

                    }


                }
            }
            catch (Exception a)
            {

                MessageBox.Show("Lỗi: " + a.Message, "BemmTeam");

            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
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
                            string pathBackup = row.Cells[2].Value.ToString();
                            string cookie = row.Cells[3].Value.ToString();
                            string nameLD = row.Cells[4].Value.ToString();
                            string proxy = row.Cells[5].Value.ToString();
                            string email = row.Cells[6].Value.ToString();

                            string data = Environment.NewLine + "#" + user + "," + password + "," + pathBackup + "," + cookie + "," + nameLD + "," + proxy + "," + email;
                            string main = File.ReadAllText((Application.StartupPath + @"\Data\Account\" + name + ".txt")) + data;
                            File.WriteAllText(Application.StartupPath + @"\Data\Account\" + name + ".txt", main);

                            dataGridviewTik.Rows.Remove(row);
                        }
                        MessageBox.Show("Đã thêm account vào tab " + name, "BemmTeam");

                        btnSave.PerformClick();

                    }


                }
            }
            catch (Exception a)
            {

                MessageBox.Show("Lỗi: " + a.Message, "BemmTeam");

            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            string cookie = dataGridviewTik.Rows[dataGridviewTik.CurrentCell.RowIndex].Cells[3].Value.ToString();
            Selenium selenium = new Selenium();
            selenium.loginTikTok(cookie);

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (dataGridviewTik.Rows.Count > 0)
            {
                using (nameLD ld = new nameLD())
                {
                    ld.ShowDialog();
                    if (ld.name != "")
                    {
                        dataGridviewTik.Rows[dataGridviewTik.CurrentCell.RowIndex].Cells[4].Value = ld.name;
                    }
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            frmgetcode frmgetcode = new frmgetcode();
            frmgetcode.Show();
        }
    }
    }


