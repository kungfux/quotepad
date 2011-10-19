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

        public ToolStripPrototype(TabControlPrototype tabcontrol)
        {
            tabControl = tabcontrol;
            ToolStripButtonPrototype viewQuote = new ToolStripButtonPrototype("Просмотр цитат", Resources._1315511677_old_edit_find);
            viewQuote.Click += new EventHandler(viewQuote_Click);
            this.Items.Add(viewQuote);
            ToolStripTextBox searchQuote = new ToolStripTextBox();
            searchQuote.Text = defSearchText;
            searchQuote.Font = new System.Drawing.Font(searchQuote.Font.FontFamily, searchQuote.Font.Size, System.Drawing.FontStyle.Italic);
            searchQuote.Alignment = ToolStripItemAlignment.Right;
            searchQuote.GotFocus += new EventHandler(searchQuote_GotFocus);
            searchQuote.LostFocus += new EventHandler(searchQuote_LostFocus);
            this.Items.Add(searchQuote);
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
            bool isSuperUser = true; // Here is should be external call to check isAdminLoggedIn?
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
