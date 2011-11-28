using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public class AskPassword
    {
        private TabControlPrototype tabControl;
        private TableLayoutPanel panel;
        public UserType returnedUserType = UserType.Viewer;
        MaskedTextBox password;
        Button bOk;
        Button bCancel;
        int activeTab;

        public AskPassword(TabControlPrototype tabcontrol)
        {
            tabControl = tabcontrol;

            GroupBox g = new GroupBox();
            password = new MaskedTextBox();
            bOk = new Button();
            bCancel = new Button();
            Label l = new Label();

            TableLayoutPanel p = new TableLayoutPanel();
            p.Dock = DockStyle.Fill;
            p.ColumnCount = 2;
            p.RowCount = 3;
            p.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            p.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            p.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            p.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            p.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            p.Controls.Add(l, 0, 0);
            p.SetColumnSpan(l, 2);
            l.AutoSize = false;
            l.Dock = DockStyle.Fill;
            l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            p.Controls.Add(password, 0, 1);
            p.SetColumnSpan(password, 2);
            password.Dock = DockStyle.Fill;
            p.Controls.Add(bOk, 0, 2);
            bOk.Dock = DockStyle.Top;
            p.Controls.Add(bCancel, 1, 2);
            bCancel.Dock = DockStyle.Top;

            g.Text = " Авторизация ";
            g.Dock = DockStyle.Fill;
            password.PasswordChar = '*';
            password.BeepOnError = true;
            password.TextAlign = HorizontalAlignment.Center;
            bOk.Text = "OK";
            bOk.Enabled = false;
            bCancel.Text = "Отмена";
            l.Text = "Введите пароль для получения привилегированного доступа к программе.";

            g.Controls.Add(p);

            panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.ColumnCount = 3;
            panel.RowCount = 3;
            panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            panel.Controls.Add(g, 1, 1);

            activeTab = tabControl.SelectedIndex;
            tabControl.Visible = false;
            tabControl.SelectedIndex = -1;
            tabControl.Parent.Controls.Add(panel);

            bCancel.Click += new EventHandler(bCancel_Click);
            bOk.Click += new EventHandler(bOk_Click);
            password.TextChanged += new EventHandler(password_TextChanged);
            password.Select();
        }

        void password_TextChanged(object sender, EventArgs e)
        {
            if (password.TextLength > 0)
            {
                bOk.Enabled = true;
            }
            else
            {
                bOk.Enabled = false;
            }
        }

        void bOk_Click(object sender, EventArgs e)
        {
            Authorization.userType = new Authorization().CheckCredentials(password.Text);
            panel.Dispose();
            tabControl.Visible = true;
            tabControl.SelectedIndex = activeTab;
        }

        void bCancel_Click(object sender, EventArgs e)
        {
            panel.Dispose();
            tabControl.Visible = true;
            tabControl.SelectedIndex = activeTab;
        }
    }
}
