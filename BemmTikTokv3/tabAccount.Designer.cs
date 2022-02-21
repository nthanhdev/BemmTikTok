
namespace BemmTikTokv3
{
    partial class tabAccount
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnXacNhan = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comTab = new Guna.UI2.WinForms.Guna2ComboBox();
            this.SuspendLayout();
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.CheckedState.Parent = this.btnXacNhan;
            this.btnXacNhan.CustomImages.Parent = this.btnXacNhan;
            this.btnXacNhan.FillColor = System.Drawing.Color.Turquoise;
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.btnXacNhan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnXacNhan.HoverState.Parent = this.btnXacNhan;
            this.btnXacNhan.Location = new System.Drawing.Point(3, 76);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.ShadowDecoration.Parent = this.btnXacNhan;
            this.btnXacNhan.Size = new System.Drawing.Size(259, 30);
            this.btnXacNhan.TabIndex = 5;
            this.btnXacNhan.Text = "XÁC NHẬN";
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-2, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Chọn tab Account name:";
            // 
            // comTab
            // 
            this.comTab.BackColor = System.Drawing.Color.Transparent;
            this.comTab.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comTab.FocusedColor = System.Drawing.Color.Empty;
            this.comTab.FocusedState.Parent = this.comTab;
            this.comTab.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.comTab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.comTab.FormattingEnabled = true;
            this.comTab.HoverState.Parent = this.comTab;
            this.comTab.ItemHeight = 30;
            this.comTab.ItemsAppearance.Parent = this.comTab;
            this.comTab.Location = new System.Drawing.Point(3, 33);
            this.comTab.Name = "comTab";
            this.comTab.ShadowDecoration.Parent = this.comTab;
            this.comTab.Size = new System.Drawing.Size(259, 36);
            this.comTab.TabIndex = 21;
            this.comTab.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabAccount
            // 
            this.AcceptButton = this.btnXacNhan;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 112);
            this.Controls.Add(this.comTab);
            this.Controls.Add(this.btnXacNhan);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "tabAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chọn tab account Name";
            this.Load += new System.EventHandler(this.tabAccount_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnXacNhan;
        private System.Windows.Forms.Label label1;
        public Guna.UI2.WinForms.Guna2ComboBox comTab;
    }
}