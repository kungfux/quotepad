using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public class ToolStripPrototype : ToolStrip
    {
        private TabControlPrototype tabControl;
        private string defSearchText = "Найти цитату?";
        ToolStripTextBox searchQuote;

        public ToolStripPrototype(TabControlPrototype tabcontrol)
        {
            this.AutoSize = false;
            this.Height = 68;
            tabControl = tabcontrol;

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
                        tabControl.AddPage(new pageQuoteView(tabControl, quoteId));
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
                        tabControl.AddPage(new pageQuoteView(tabControl, foundQuotes[0].ID));
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
            new AskPassword(tabControl);
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
