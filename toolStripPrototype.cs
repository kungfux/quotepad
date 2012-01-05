using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ItWorksTeam.UI;

namespace QuotePad
{
    public class ToolStripPrototype : ToolStrip
    {
        private TabControlPrototype tabControl;
        private string defSearchText = "Найти цитату?";
        ToolStripTextBox searchQuote;
        mainMenu mainMenu;

        public ToolStripPrototype(TabControlPrototype tabcontrol, mainMenu MainMenu)
        {
            this.AutoSize = false;
            this.Height = 68;
            tabControl = tabcontrol;
            mainMenu = MainMenu;

            ToolStripButtonPrototype viewQuote = new ToolStripButtonPrototype("Просмотр цитат", Resources.viewQuotes_64);
            viewQuote.Click += new EventHandler(viewQuote_Click);
            this.Items.Add(viewQuote);

            ToolStripButtonPrototype addQuote = new ToolStripButtonPrototype("Добавить новую цитату", Resources.createNew_64);
            addQuote.isForSupervisorOnly = true;
            addQuote.Click += new EventHandler(addQuote_Click);
            this.Items.Add(addQuote);

            ToolStripButtonPrototype auth = new ToolStripButtonPrototype("Авторизоваться", Resources.authorize_64);
            auth.Visible = Authorization.userType == UserType.Viewer;
            auth.Click += new EventHandler(auth_Click);
            this.Items.Add(auth);

            searchQuote = new ToolStripTextBox();
            searchQuote.ToolTipText = "Выполнить поиск цитаты.\nДля просмотра цитаты №1, введите в поле "+
                "поиска единицу;\nДля поиска цитаты по тексту введите уникальное словосочетание.";
            searchQuote.Visible = true;
            searchQuote.Width = 220;
            searchQuote.Text = defSearchText;
            searchQuote.Font = new System.Drawing.Font(searchQuote.Font.FontFamily, 14, System.Drawing.FontStyle.Italic);
            searchQuote.Alignment = ToolStripItemAlignment.Right;
            searchQuote.GotFocus += new EventHandler(searchQuote_GotFocus);
            searchQuote.LostFocus += new EventHandler(searchQuote_LostFocus);
            searchQuote.KeyDown += new KeyEventHandler(searchQuote_KeyDown);
            this.Items.Add(searchQuote);

            tabControl.VisibleChanged += new EventHandler(tabControl_VisibleChanged);
        }

        void searchQuote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && searchQuote.Text.Length > 0)
            {
                int quoteId = 0;
                if (int.TryParse(searchQuote.Text, out quoteId))
                {
                    if (Database.Quote_FindByID_IsExist(quoteId))
                    {
                        tabControl.AddPage(new pageQuoteView(tabControl, Database.Quote_FindByID(quoteId)));
                        searchQuote.Text = defSearchText;
                    }
                    else MessageBox.Show("Цитата не найдена!", new assembly().AssemblyProduct,
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    Objects.Quote[] foundQuotes = Database.Quote_FindByText(searchQuote.Text);
                    if (foundQuotes.Length > 0)
                    {
                        tabControl.AddPage(new pageQuoteView(tabControl, foundQuotes));
                        searchQuote.Text = defSearchText;
                    }
                    else MessageBox.Show("Цитата не найдена!", new assembly().AssemblyProduct,
                           MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        void tabControl_VisibleChanged(object sender, EventArgs e)
        {
            if (tabControl.Visible)
            {
                this.Enabled = true;
                this.SetVisibility();
            }
            else
            {
                this.Enabled = false;
            }
        }

        void auth_Click(object sender, EventArgs e)
        {
            PasswordDialog pass = new PasswordDialog(true, 4, 0, 3, 1, 0, PasswordDialog.DialogType.AskPassword);
            pass.AskPassword();
            Authorization.userType = new Authorization().CheckCredentials(pass.EnteredOldPassword);
            if (Authorization.userType == UserType.Viewer)
            {
                if (MessageBox.Show("Вы указали неверный пароль!\rПопробовать еще раз?", new assembly().AssemblyProduct,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    auth_Click(this, null);
                }
            }
            tabControl.RefreshItems();
            mainMenu.SetVisibility();
        }

        void addQuote_Click(object sender, EventArgs e)
        {
            tabControl.AddPage(new pageQuoteEditor(tabControl));
        }

        void searchQuote_LostFocus(object sender, EventArgs e)
        {
            if (sender is ToolStripTextBox)
            {
                ToolStripTextBox textBox = (ToolStripTextBox)sender;
                if (textBox.Text == "")
                    textBox.Text = defSearchText;
            }
        }

        void searchQuote_GotFocus(object sender, EventArgs e)
        {
            if (sender is ToolStripTextBox)
            {
                ToolStripTextBox textBox = (ToolStripTextBox)sender;
                if (textBox.Text == defSearchText)
                    textBox.Text = "";
            }
        }

        void viewQuote_Click(object sender, EventArgs e)
        {
            tabControl.AddPage(new pageQuoteView(tabControl));
        }

        // Perform setting correct visibility on load
        protected override void SetDisplayedItems()
        {
            SetVisibility();
            base.SetDisplayedItems();
        }

        // Set Visible property for each item in main menu
        public void SetVisibility()
        {
            bool isSuperUser = (Authorization.userType == UserType.Editor);
            foreach (ToolStripItem item in this.Items)
            {
                if (item is ToolStripButtonPrototype)
                {
                    ToolStripButtonPrototype item2 = (ToolStripButtonPrototype)item;
                    if (item2.isForSupervisorOnly)
                    {
                        item2.Visible = isSuperUser;
                    }
                }
                else
                    if (item is ToolStripComboBoxPrototype)
                    {
                        ToolStripComboBoxPrototype item2 = (ToolStripComboBoxPrototype)item;
                        if (item2.isForSupervisorOnly)
                        {
                            item2.Visible = isSuperUser;
                        }
                    }
                    else
                        if (item is ToolStripLabelPrototype)
                        {
                            ToolStripLabelPrototype item2 = (ToolStripLabelPrototype)item;
                            if (item2.isForSupervisorOnly)
                            {
                                item2.Visible = isSuperUser;
                            }
                        }
                        else
                            if (item is ToolStripSeparatorPrototype)
                            {
                                ToolStripSeparatorPrototype item2 = (ToolStripSeparatorPrototype)item;
                                if (item2.isForSupervisorOnly)
                                {
                                    item2.Visible = isSuperUser;
                                }
                            }
            }
        }
    }
}
