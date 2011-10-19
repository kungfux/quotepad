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
        GroupBox g = new GroupBox();
        PictureBox authorImage = new PictureBox();
        Label quoteInfo = new Label();
        GroupBox gInfo = new GroupBox();
        private TabControlPrototype tabcontrol;

        public pageQuoteView(TabControlPrototype tabControl, int displayQuote = -1)
        {
            tabcontrol = tabControl;
            this.Text = "Просмотр";            
            g.Text = " Текст цитаты ";
            g.Dock = DockStyle.Fill;
            g.Controls.Add(rtfed.RtfTextBox);
            gInfo.Text = " Информация ";
            gInfo.Dock = DockStyle.Right;
            gInfo.Width = 250;
            gInfo.Visible = false;
            authorImage.SizeMode = PictureBoxSizeMode.Zoom;
            authorImage.Size = new System.Drawing.Size(250,250);
            quoteInfo.AutoSize = false;
            quoteInfo.Bounds = new System.Drawing.Rectangle(0, 250, 250, 500);
            quoteInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            gInfo.Controls.Add(authorImage);
            gInfo.Controls.Add(quoteInfo);
            ToolStripButtonPrototype editQuote = new ToolStripButtonPrototype("Редактировать цитату", Resources._1315515132_edit);
            editQuote.Click += new EventHandler(editQuote_Click);
            this.AddToolStripItem(editQuote);
            ToolStripButtonPrototype infoAuthor = new ToolStripButtonPrototype("Информация", Resources.infoAuthor);
            infoAuthor.Click += new EventHandler(infoAuthor_Click);
            this.AddToolStripItem(infoAuthor);
            ToolStripButtonPrototype prevQuote = new ToolStripButtonPrototype("Предыдущая цитата", Resources.arrowLeft);
            prevQuote.Click += new EventHandler(prevQuote_Click);
            ToolStripButtonPrototype randomQuote = new ToolStripButtonPrototype("Случайная цитата", Resources.refresh);
            randomQuote.Click += new EventHandler(randomQuote_Click);
            ToolStripButtonPrototype nextQuote = new ToolStripButtonPrototype("Следующая цитата", Resources.arrowRight);
            nextQuote.Click += new EventHandler(nextQuote_Click);
            this.AddToolStripItem(prevQuote);
            this.AddToolStripItem(randomQuote);
            this.AddToolStripItem(nextQuote);
            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            rtfed.RtfTextBox.ReadOnly = true;
            this.Controls.Add(g);
            this.Controls.Add(gInfo);
            if (displayQuote == -1)
            {
                randomQuote_Click(this, null);
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
            qRefresh();
        }

        private void qRefresh()
        {
            if (currentQuote.ID != 0)
            {
                rtfed.RtfTextBox.Rtf = currentQuote.RTF;
                g.Text = " Текст цитаты №" + currentQuote.ID.ToString();
                authorImage.Image = currentQuote.QuoteAuthor.Photo;
                quoteInfo.Text = currentQuote.QuoteAuthor.FIO + Environment.NewLine + Environment.NewLine
                    + currentQuote.QuoteAuthor.About + Environment.NewLine + Environment.NewLine
                    + "Дата модификации: " + currentQuote.WhenCreated.ToShortDateString() + " "
                    + currentQuote.WhenCreated.ToShortTimeString();
            }
            else MessageBox.Show("больше цитат нет :)");
        }
    }
}
