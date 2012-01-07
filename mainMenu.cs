using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ItWorksTeam.UI;

namespace QuotePad
{
    /// <summary>
    /// Main menu of the QuotePad
    /// </summary>
    public class mainMenu : menuStripPrototype
    {
        private TabControlPrototype tabcontrol;
        ToolStripMenuItem settings_config_setpassword;
        ToolStripMenuItem settings_config_changepassword;
        ToolStripMenuItem settings_config_clearpassword;

        public mainMenu(TabControlPrototype tabControl)
        {
            tabcontrol = tabControl;
            this.ShowItemToolTips = true;
            ToolStripMenuItem quote = new ToolStripMenuItem("Цитата");
            ToolStripMenuItem quote_view = new ToolStripMenuItem("Просмотр");
            quote_view.ToolTipText = "Открыть вкладку просмотра цитат";
            quote_view.Click += new EventHandler(quote_view_Click);
            ToolStripMenuItem quote_add = new ToolStripMenuItem("Добавить");
            quote_add.ToolTipText = "Создать новую цитату";
            quote_add.Click += new EventHandler(quote_add_Click);
            quote_add.Tag = this.adminTag;
            ToolStripMenuItem quote_quit = new ToolStripMenuItem("Выход");
            quote_quit.ToolTipText = "Закрыть Цитатник";
            quote_quit.Click += new EventHandler(quote_quit_Click);
            quote.DropDownItems.Add(quote_view);
            quote.DropDownItems.Add(quote_add);
            quote.DropDownItems.Add(quote_quit);
            ToolStripMenuItem edit = new ToolStripMenuItem("Правка");
            edit.Tag = this.adminTag;
            ToolStripMenuItem edit_theme = new ToolStripMenuItem("Темы");
            edit_theme.ToolTipText = "Открыть редактор тем";
            edit_theme.Click += new EventHandler(edit_theme_Click);
            ToolStripMenuItem edit_author = new ToolStripMenuItem("Авторы");
            edit_author.ToolTipText = "Открыть редактор авторов";
            edit_author.Click += new EventHandler(edit_author_Click);
            ToolStripMenuItem edit_quote = new ToolStripMenuItem("Цитаты");
            edit_quote.Visible = false; //// TEMPORARY
            edit.DropDownItems.Add(edit_theme);
            edit.DropDownItems.Add(edit_author);
            edit.DropDownItems.Add(edit_quote);
            ToolStripMenuItem search = new ToolStripMenuItem("Поиск");
            search.Visible = false; //// TEMPORARY
            ToolStripMenuItem search_author = new ToolStripMenuItem("По автору");
            ToolStripMenuItem search_theme = new ToolStripMenuItem("По теме");
            ToolStripMenuItem search_text = new ToolStripMenuItem("По тексту");
            ToolStripMenuItem search_date = new ToolStripMenuItem("По дате");
            search.DropDownItems.Add(search_author);
            search.DropDownItems.Add(search_theme);
            search.DropDownItems.Add(search_text);
            search.DropDownItems.Add(search_date);
            ToolStripMenuItem settings = new ToolStripMenuItem("Параметры");
            settings.Visible = false; //// TEMPORARY
            ToolStripMenuItem settings_config = new ToolStripMenuItem("Настройка");
            settings_config.Tag = this.adminTag;
            settings_config_setpassword = new ToolStripMenuItem("Установить пароль...");
            settings_config_setpassword.Click += new EventHandler(settings_config_setpassword_Click);
            settings_config_changepassword = new ToolStripMenuItem("Сменить пароль...");
            settings_config_changepassword.Click += new EventHandler(settings_config_changepassword_Click);
            settings_config_clearpassword = new ToolStripMenuItem("Удалить пароль...");
            settings_config_clearpassword.Click += new EventHandler(settings_config_clearpassword_Click);
            ChangePasswordOptionsVisibility();
            settings_config.DropDownItems.Add(settings_config_setpassword);
            settings_config.DropDownItems.Add(settings_config_changepassword);
            settings_config.DropDownItems.Add(settings_config_clearpassword);
            ToolStripMenuItem settings_export = new ToolStripMenuItem("Экспорт");
            settings_export.Visible = false; // TEMPORARY
            ToolStripMenuItem settings_login = new ToolStripMenuItem("Авторизоваться");
            settings_login.Visible = false; // NO LONGER NEEDED?
            settings.DropDownItems.Add(settings_config);
            settings.DropDownItems.Add(settings_export);
            settings.DropDownItems.Add(settings_login);
            ToolStripMenuItem help = new ToolStripMenuItem("Справка");
            ToolStripMenuItem help_about = new ToolStripMenuItem("О программе");
            help_about.ToolTipText = "Посмотреть информацию о программе";
            help_about.Click += new EventHandler(help_about_Click);
            help.DropDownItems.Add(help_about);
            ToolStripMenuItem closeTab = new ToolStripMenuItem("X");
            closeTab.ToolTipText = "Закрыть вкладку";
            closeTab.Click += new EventHandler(closeTab_Click);
            closeTab.Alignment = ToolStripItemAlignment.Right;

            this.Items.Add(quote);
            this.Items.Add(edit);
            this.Items.Add(search);
            this.Items.Add(settings_config);
            this.Items.Add(help);
            this.Items.Add(closeTab);

            tabcontrol.VisibleChanged += new EventHandler(tabcontrol_VisibleChanged);
        }

        private void ChangePasswordOptionsVisibility()
        {
            Authorization auth = new Authorization();
            bool IsDefaultPasswordSet = auth.IsDefaultPasswordSet;
            settings_config_setpassword.Visible = IsDefaultPasswordSet;
            settings_config_changepassword.Visible = !IsDefaultPasswordSet;
            settings_config_clearpassword.Visible = !IsDefaultPasswordSet;
        }

        void settings_config_clearpassword_Click(object sender, EventArgs e)
        {
            PasswordDialog pass = new PasswordDialog(true, 4, 0, 3, 1, 0, PasswordDialog.DialogType.AskPassword);
            pass.AskPassword();
            if (pass.EnteredOldPassword.Length > 0)
            {
                UserType utype= new Authorization().CheckCredentials(pass.EnteredOldPassword);
                if (utype == UserType.Viewer)
                {
                    if (MessageBox.Show("Вы указали неверный пароль!\rПопробовать еще раз?", new assembly().AssemblyProduct,
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        settings_config_clearpassword_Click(this, null);
                    }
                }
                else
                {
                    new Authorization().SetNewPassword("password");
                    MessageBox.Show("Пароль был удалён", new assembly().AssemblyProduct, MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            ChangePasswordOptionsVisibility();
        }

        void settings_config_changepassword_Click(object sender, EventArgs e)
        {
            PasswordDialog pass = new PasswordDialog(true, 4, 0, 0, 1, 0, PasswordDialog.DialogType.ChangePassword);
            pass.AskPassword();
            if (pass.EnteredOldPassword.Length > 0 && pass.EnteredNewPassword.Length > 0)
            {
                if (new Authorization().ChangeExistingPassword(pass.EnteredOldPassword, pass.EnteredNewPassword))
                {
                    MessageBox.Show("Пароль установлен", new assembly().AssemblyProduct, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось сменить пароль!", new assembly().AssemblyProduct, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void settings_config_setpassword_Click(object sender, EventArgs e)
        {
            PasswordDialog pass = new PasswordDialog(true, 4, 0, 0, 1, 0, PasswordDialog.DialogType.SetPassword);
            pass.AskPassword();
            if (pass.EnteredNewPassword.Length > 0)
            {
                if (new Authorization().SetNewPassword(pass.EnteredNewPassword))
                {
                    MessageBox.Show("Пароль установлен", new assembly().AssemblyProduct, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось установить пароль!", new assembly().AssemblyProduct, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                ChangePasswordOptionsVisibility();
            }
        }

        void closeTab_Click(object sender, EventArgs e)
        {
            tabcontrol.CloseActiveTab();
        }

        void tabcontrol_VisibleChanged(object sender, EventArgs e)
        {
            if (tabcontrol.Visible)
            {
                this.Enabled = true;
                this.SetVisibility();
            }
            else
            {
                this.Enabled = false;
            }
        }

        void help_about_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        void edit_author_Click(object sender, EventArgs e)
        {
            tabcontrol.AddPage(new AuthorManager(tabcontrol));
        }

        void edit_theme_Click(object sender, EventArgs e)
        {
            tabcontrol.AddPage(new ThemeManager(tabcontrol));
        }

        void quote_quit_Click(object sender, EventArgs e)
        {
            // Needs to send manually close all page before (to save job before exit)
            Application.Exit();
        }

        void quote_add_Click(object sender, EventArgs e)
        {
            tabcontrol.AddPage(new pageQuoteEditor(tabcontrol));
        }

        private void quote_view_Click(object sender, EventArgs e)
        {
            tabcontrol.AddPage(new pageQuoteView(tabcontrol));
        }
    }
}
