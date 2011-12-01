using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public class pageQuoteView : TabPagePrototype
    {
        ItWorks.RTFed rtfed = new ItWorks.RTFed();
        Objects.Quote currentQuote = new Objects.Quote();
        Objects.Quote tempQuote;
        PictureBox authorImage = new PictureBox();
        Label authorFIO = new Label();
        TextBox authorAbout = new TextBox();
        private TabControlPrototype tabcontrol;
        ToolStripButtonPrototype editQuote;
        ToolStripButtonPrototype infoAuthor;
        ToolStripButtonPrototype deleteQuote;
        ToolStripButtonPrototype prevQuote;
        ToolStripButtonPrototype nextQuote;
        SplitContainer s;
        TableLayoutPanel backpanel; // back panel for displaying info about author
        ItWorks.Registry regValue = new ItWorks.Registry();

        public pageQuoteView(TabControlPrototype tabControl, int displayQuote = -1)
        {
            tabcontrol = tabControl;
            this.Text = "Просмотр";            
            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            rtfed.RtfTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            rtfed.RtfTextBox.BackColor = authorFIO.BackColor;
            authorImage.SizeMode = PictureBoxSizeMode.Zoom;
            authorImage.Dock = DockStyle.Fill;
            //authorFIO.AutoSize = false;
            authorFIO.Dock = DockStyle.Fill;
            authorFIO.Bounds = new System.Drawing.Rectangle(0, 250, 250, 500);
            authorFIO.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            authorAbout.ReadOnly = true;
            authorAbout.BackColor = authorFIO.BackColor;
            authorAbout.Multiline = true;
            authorAbout.ScrollBars = ScrollBars.Vertical;
            authorAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            authorAbout.Dock = DockStyle.Fill;

            backpanel = new TableLayoutPanel();
            backpanel.Dock = DockStyle.Fill;
            backpanel.ColumnCount = 1;
            backpanel.RowCount = 3;
            backpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            backpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            backpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            backpanel.Controls.Add(authorImage, 0, 0);
            backpanel.Controls.Add(authorFIO, 0, 1);
            backpanel.Controls.Add(authorAbout, 0, 2);
            backpanel.Visible = regValue.ReadKey<bool>(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE, "Software\\ItWorksTeam\\QuotePad",
                "BackPanelVisible", true);

            infoAuthor = new ToolStripButtonPrototype("Информация", Resources.info_64);
            infoAuthor.CheckOnClick = true;
            infoAuthor.Checked = backpanel.Visible;
            //infoAuthor.Enabled = false;
            infoAuthor.Click += new EventHandler(infoAuthor_Click);
            this.AddToolStripItem(infoAuthor);

            editQuote = new ToolStripButtonPrototype("Редактировать цитату", Resources.edit_64);
            editQuote.Enabled = false;
            editQuote.isForSupervisorOnly = true;
            editQuote.Click += new EventHandler(editQuote_Click);
            this.AddToolStripItem(editQuote);

            deleteQuote = new ToolStripButtonPrototype("Удалить цитату", Resources.delete_64);
            deleteQuote.Enabled = false;
            deleteQuote.isForSupervisorOnly = true;
            deleteQuote.Click += new EventHandler(deleteQuote_Click);
            this.AddToolStripItem(deleteQuote);

            prevQuote = new ToolStripButtonPrototype("Предыдущая цитата", Resources.previous_64);
            prevQuote.Click += new EventHandler(prevQuote_Click);

            ToolStripButtonPrototype randomQuote = new ToolStripButtonPrototype("Случайная цитата", Resources.random_64);
            randomQuote.Click += new EventHandler(randomQuote_Click);

            nextQuote = new ToolStripButtonPrototype("Следующая цитата", Resources.next_64);
            nextQuote.Click += new EventHandler(nextQuote_Click);

            this.AddToolStripItem(prevQuote);
            this.AddToolStripItem(randomQuote);
            this.AddToolStripItem(nextQuote);
            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            rtfed.RtfTextBox.ReadOnly = true;

            s = new SplitContainer();
            s.SplitterDistance = (s.Width / 2) + (s.Width / 2) / 2;
            s.Dock = DockStyle.Fill;
            int x = regValue.ReadKey<int>(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE, "Software\\ItWorksTeam\\QuotePad",
                "BackPanelWidth", -1);
            if (x >= 0)
            {
                s.SplitterDistance = s.Width - x;
            }
            s.Panel1.Controls.Add(rtfed.RtfTextBox);
            s.Panel2.Controls.Add(backpanel);
            s.SplitterMoved += new SplitterEventHandler(s_SplitterMoved);
            this.Controls.Add(s);
            if (displayQuote == -1)
            {
                randomQuote_Click(this, null);
            }
            else
            {
                currentQuote = Database.Quote_FindByID(displayQuote);
                qRefresh();
            }
        }

        void s_SplitterMoved(object sender, SplitterEventArgs e)
        {
            regValue.SaveKey(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE, "Software\\ItWorksTeam\\QuotePad",
               "BackPanelWidth", s.Panel2.Width);
        }
   
        void deleteQuote_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить цитату?", new assembly().AssemblyProduct, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Database.Quote_Remove(currentQuote.ID))
                {
                    randomQuote_Click(this,null);
                }
            }
        }

        void infoAuthor_Click(object sender, EventArgs e)
        {
            backpanel.Visible = !backpanel.Visible;
            regValue.SaveKey(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE, "Software\\ItWorksTeam\\QuotePad",
                "BackPanelVisible", backpanel.Visible);
        }

        void editQuote_Click(object sender, EventArgs e)
        {
            if (currentQuote.ID != 0)
            {
                tabcontrol.AddPage(new pageQuoteEditor(currentQuote.ID));
            }
        }

        void nextQuote_Click(object sender, EventArgs e)
        {
            tempQuote = Database.Quote_ReadNext(currentQuote.ID);
            if (tempQuote.ID == 0)
            {
                nextQuote.Enabled = false;
                MessageBox.Show("Следующая цитата не найдена!", new assembly().AssemblyProduct, MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            }
            else
            {
                currentQuote = tempQuote;
                qRefresh();
            }
        }

        void randomQuote_Click(object sender, EventArgs e)
        {
            tempQuote = Database.Quote_RandomRead();
            if (tempQuote.ID != 0)
            {
                currentQuote = tempQuote;
                qRefresh();
            }
            else
            {
                MessageBox.Show("Цитата не найдена!", new assembly().AssemblyProduct, MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            }
        }

        void prevQuote_Click(object sender, EventArgs e)
        {
            tempQuote = Database.Quote_ReadPrevious(currentQuote.ID);
            if (tempQuote.ID == 0)
            {
                prevQuote.Enabled = false;
                MessageBox.Show("Предыдущая цитата не найдена!", new assembly().AssemblyProduct, MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            }
            else
            {
                currentQuote = tempQuote;
                qRefresh();
            }
        }

        private void qRefresh()
        {
            prevQuote.Enabled = true;
            nextQuote.Enabled = true;
            editQuote.Enabled = true;
            //infoAuthor.Enabled = true;
            deleteQuote.Enabled = true;
            rtfed.RtfTextBox.Rtf = currentQuote.RTF;
            //g.Text = " Текст цитаты №" + currentQuote.ID.ToString();
            authorImage.Image = currentQuote.QuoteAuthor.Photo;
            if (authorImage.Image == null) authorImage.Image = Resources.noPhoto_128;
            authorFIO.Text = currentQuote.QuoteAuthor.FIO.TrimEnd(new char[] { ' ' });
            authorAbout.Text = currentQuote.QuoteAuthor.About;
        }
    }
}
