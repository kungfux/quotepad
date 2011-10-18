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

        public pageQuoteView(int displayQuote = -1)
        {
            this.Text = "Просмотр";            
            g.Text = " Текст цитаты ";
            g.Dock = DockStyle.Fill;
            g.Controls.Add(rtfed.RtfTextBox);
            this.AddToolStripItem(new ToolStripButtonPrototype("Редактировать цитату", Resources._1315515132_edit));
            ToolStripButtonPrototype prevQuote = new ToolStripButtonPrototype("Предыдущая цитата", Resources._1315515132_edit);
            prevQuote.Click += new EventHandler(prevQuote_Click);
            ToolStripButtonPrototype randomQuote = new ToolStripButtonPrototype("Случайная цитата", Resources._1315515132_edit);
            randomQuote.Click += new EventHandler(randomQuote_Click);
            ToolStripButtonPrototype nextQuote = new ToolStripButtonPrototype("Следующая цитата", Resources._1315515132_edit);
            nextQuote.Click += new EventHandler(nextQuote_Click);
            this.AddToolStripItem(prevQuote);
            this.AddToolStripItem(randomQuote);
            this.AddToolStripItem(nextQuote);
            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            rtfed.RtfTextBox.ReadOnly = true;
            this.Controls.Add(g);
            if (displayQuote == -1)
            {
                randomQuote_Click(this, null);
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
            }
            else MessageBox.Show("больше цитат нет :)");
        }
    }
}
