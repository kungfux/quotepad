using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public class pageQuoteView : TabPagePrototype
    {
        ItWorks.RTFed rtfed = new ItWorks.RTFed();
        Objects.Quote currentQuote;
        PictureBox authorImage = new PictureBox();
        Label quoteInfo = new Label();
        GroupBox gInfo = new GroupBox();
        private TabControlPrototype tabcontrol;
        ToolStripButtonPrototype editQuote;
        ToolStripButtonPrototype infoAuthor;
        ToolStripButtonPrototype deleteQuote;
        ToolStripButtonPrototype prevQuote;
        ToolStripButtonPrototype nextQuote;

        public pageQuoteView(TabControlPrototype tabControl, int displayQuote = -1)
        {
            tabcontrol = tabControl;
            this.Text = "Просмотр";            
            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            gInfo.Text = " Информация ";
            gInfo.Dock = DockStyle.Right;
            gInfo.Width = 250;
            authorImage.SizeMode = PictureBoxSizeMode.Zoom;
            authorImage.Size = new System.Drawing.Size(250,250);
            quoteInfo.AutoSize = false;
            quoteInfo.Bounds = new System.Drawing.Rectangle(0, 250, 250, 500);
            quoteInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            gInfo.Controls.Add(authorImage);
            gInfo.Controls.Add(quoteInfo);

            editQuote = new ToolStripButtonPrototype("Редактировать цитату", Resources.edit_64);
            editQuote.Enabled = false;
            editQuote.isForSupervisorOnly = true;
            editQuote.Click += new EventHandler(editQuote_Click);
            this.AddToolStripItem(editQuote);

            infoAuthor = new ToolStripButtonPrototype("Информация", Resources.info_64);
            infoAuthor.CheckOnClick = true;
            infoAuthor.Checked = true;
            infoAuthor.Enabled = false;
            infoAuthor.Click += new EventHandler(infoAuthor_Click);
            this.AddToolStripItem(infoAuthor);

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
            this.Controls.Add(rtfed.RtfTextBox);
            this.Controls.Add(gInfo);
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
            gInfo.Visible = !gInfo.Visible;
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
            currentQuote = Database.Quote_ReadNext(currentQuote.ID);
            if (currentQuote.ID == 0) nextQuote.Enabled = false;
            qRefresh();
        }

        void randomQuote_Click(object sender, EventArgs e)
        {
            currentQuote = Database.Quote_RandomRead();
            qRefresh();
        }

        void prevQuote_Click(object sender, EventArgs e)
        {
            currentQuote = Database.Quote_ReadPrevious(currentQuote.ID);
            if (currentQuote.ID == 0) prevQuote.Enabled = false;
            qRefresh();
        }

        private void qRefresh()
        {
            if (currentQuote.ID != 0)
            {
                prevQuote.Enabled = true;
                nextQuote.Enabled = true;
                editQuote.Enabled = true;
                infoAuthor.Enabled = true;
                deleteQuote.Enabled = true;
                rtfed.RtfTextBox.Rtf = currentQuote.RTF;
                //g.Text = " Текст цитаты №" + currentQuote.ID.ToString();
                authorImage.Image = currentQuote.QuoteAuthor.Photo;
                quoteInfo.Text = currentQuote.QuoteAuthor.FIO + Environment.NewLine + Environment.NewLine
                    + currentQuote.QuoteAuthor.About + Environment.NewLine + Environment.NewLine
                    + "Дата модификации: " + currentQuote.WhenCreated.ToShortDateString() + " "
                    + currentQuote.WhenCreated.ToShortTimeString();
            }
            else
            {
                editQuote.Enabled = false;
                infoAuthor.Enabled = false;
                deleteQuote.Enabled = false;
                rtfed.RtfTextBox.Rtf = "";
                //g.Text = " Текст цитаты ";
                authorImage.Image = null;
                quoteInfo.Text = "";
                MessageBox.Show("Цитата не найдена!", new assembly().AssemblyProduct, MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            }
        }
    }
}
