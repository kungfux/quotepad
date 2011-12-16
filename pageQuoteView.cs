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
        //ToolStripButtonPrototype infoAuthor;
        ToolStripButtonPrototype deleteQuote;
        ToolStripButtonPrototype prevQuote;
        ToolStripButtonPrototype nextQuote;
        Button favoriteQuote;
        Label favoriteText;
        Panel pFavorite;
        string sFavorite = "Цитата отмечена как \"любимая\"";
        string sNonFavorite = "Цитата не отмечена как \"любимая\"";
        SplitContainer s;
        TableLayoutPanel backpanel; // back panel for displaying info about author
        ItWorks.Registry regValue = new ItWorks.Registry();

        public pageQuoteView(TabControlPrototype tabControl, int displayQuote = -1)
        {
            tabcontrol = tabControl;

            this.Text = "Просмотр";
            this.captionText = "Просмотр цитат";

            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            rtfed.RtfTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            rtfed.RtfTextBox.BackColor = authorFIO.BackColor;

            authorImage.SizeMode = PictureBoxSizeMode.Zoom;
            authorImage.Dock = DockStyle.Fill;

            //authorFIO.AutoSize = false;
            authorFIO.Dock = DockStyle.Fill;
            authorFIO.Bounds = new System.Drawing.Rectangle(0, 250, 250, authorFIO.Font.Height);
            authorFIO.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            authorAbout.ReadOnly = true;
            authorAbout.BackColor = authorFIO.BackColor;
            authorAbout.Multiline = true;
            authorAbout.ScrollBars = ScrollBars.Vertical;
            authorAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            authorAbout.Dock = DockStyle.Fill;
            authorAbout.TextAlign = HorizontalAlignment.Center;

            favoriteQuote = new Button();
            favoriteQuote.Size = new System.Drawing.Size(65, 65);
            //favoriteQuote.Location = new Point(20, 20);
            favoriteQuote.Image = Resources.nonFavorite64;
            favoriteQuote.FlatStyle = FlatStyle.Flat;
            favoriteQuote.FlatAppearance.BorderSize = 0;
            favoriteQuote.FlatAppearance.MouseOverBackColor = favoriteQuote.BackColor;
            favoriteQuote.FlatAppearance.MouseDownBackColor = favoriteQuote.BackColor;
            favoriteQuote.Cursor = Cursors.Hand;


            favoriteQuote.Click += new EventHandler(favoriteQuote_Click);
            favoriteText = new Label();
            favoriteText.Left = favoriteQuote.Left + favoriteQuote.Width + 5;
            favoriteText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            favoriteText.Bounds = new System.Drawing.Rectangle(favoriteQuote.Left + favoriteQuote.Width + 5, 0, 150, 65);
            favoriteText.Text = sNonFavorite;

            pFavorite = new Panel();
            pFavorite.Visible = false;
            pFavorite.Dock = DockStyle.Fill;
            pFavorite.Controls.Add(favoriteQuote);
            pFavorite.Controls.Add(favoriteText);

            backpanel = new TableLayoutPanel();
            backpanel.Dock = DockStyle.Fill;
            backpanel.ColumnCount = 1;
            backpanel.RowCount = 4;
            backpanel.RowStyles.Add(new RowStyle(SizeType.Absolute, favoriteQuote.Height+7));
            backpanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            backpanel.RowStyles.Add(new RowStyle(SizeType.Percent, authorFIO.Height));
            backpanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            backpanel.Controls.Add(pFavorite, 0, 0);
            backpanel.Controls.Add(authorImage, 0, 1);
            backpanel.Controls.Add(authorFIO, 0, 2);
            backpanel.Controls.Add(authorAbout, 0, 3);
            backpanel.Visible = regValue.ReadKey<bool>(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE, "Software\\ItWorksTeam\\QuotePad",
                "BackPanelVisible", true);

			/*	
            infoAuthor = new ToolStripButtonPrototype("Информация", Resources.info_64);
            infoAuthor.CheckOnClick = true;
            infoAuthor.Checked = backpanel.Visible;
            //infoAuthor.Enabled = false;
            infoAuthor.Click += new EventHandler(infoAuthor_Click);
            this.AddToolStripItem(infoAuthor);
			*/

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
            rtfed.RtfTextBox.KeyDown += new KeyEventHandler(RtfTextBox_KeyDown);

            s = new SplitContainer();
            s.Panel1MinSize = 0;
            s.Panel2MinSize = 0;
            s.Dock = DockStyle.Fill;
            s.Panel1.Controls.Add(rtfed.RtfTextBox);
            s.Panel2.Controls.Add(backpanel);
            this.Controls.Add(s);
            int x = regValue.ReadKey<int>(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE, "Software\\ItWorksTeam\\QuotePad",
                "BackPanelWidth", -1);
            if (x >= 0 && x <= 100)
            {
                s.SplitterDistance = (s.Width - s.SplitterWidth) * x / 100;
            } 
            else 
            {
                s.SplitterDistance = (s.Width - s.SplitterWidth) * 75 / 100; // Default position is 75%
            }
            s.SplitterMoved += new SplitterEventHandler(s_SplitterMoved);
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

        void favoriteQuote_Click(object sender, EventArgs e)
        {
            if (currentQuote != null && Authorization.userType == UserType.Editor)
            {
                if (currentQuote.IsFavorite)
                {
                    // Unset favorite
                    Database.Quote_UnsetFavorite(currentQuote.ID);
                }
                else
                {
                    // Set favorite
                    Database.Quote_SetAsFavorite(currentQuote.ID);
                }
                currentQuote = Database.Quote_FindByID(currentQuote.ID);
                qRefresh();
            }
        }

        void RtfTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    nextQuote_Click(this, null);
                    break;
                case Keys.Left:
                    prevQuote_Click(this, null);
                    break;
                case Keys.Space:
                    randomQuote_Click(this, null);
                    break;
            }
        }

        void s_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (s.Focused)
            {
               regValue.SaveKey(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE, "Software\\ItWorksTeam\\QuotePad",
                   "BackPanelWidth", s.SplitterDistance * 100 / (s.Width - s.SplitterWidth));
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

        /*
		void infoAuthor_Click(object sender, EventArgs e)
        {
            backpanel.Visible = !backpanel.Visible;
            regValue.SaveKey(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE, "Software\\ItWorksTeam\\QuotePad",
                "BackPanelVisible", backpanel.Visible);
        }
		*/

        void editQuote_Click(object sender, EventArgs e)
        {
            if (currentQuote.ID != 0)
            {
                tabcontrol.AddPage(new pageQuoteEditor(tabcontrol, currentQuote.ID));
            }
        }

        void nextQuote_Click(object sender, EventArgs e)
        {
            tempQuote = Database.Quote_ReadNext(currentQuote.ID);
            if (tempQuote.ID == 0)
            {
                nextQuote.Enabled = false;
                currentQuote = new Objects.Quote();
                MessageBox.Show("Следующая цитата не найдена!", new assembly().AssemblyProduct, MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            }
            else
            {
                currentQuote = tempQuote;
            }
            qRefresh();
        }

        void randomQuote_Click(object sender, EventArgs e)
        {
            tempQuote = Database.Quote_RandomRead();
            if (tempQuote.ID != 0)
            {
                currentQuote = tempQuote;
            }
            else
            {
                prevQuote.Enabled = false;
                nextQuote.Enabled = false;
                currentQuote = new Objects.Quote();
                MessageBox.Show("Цитата не найдена!", new assembly().AssemblyProduct, MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            }
            qRefresh();
        }

        void prevQuote_Click(object sender, EventArgs e)
        {
            tempQuote = Database.Quote_ReadPrevious(currentQuote.ID);
            if (tempQuote.ID == 0)
            {
                prevQuote.Enabled = false;
                currentQuote = new Objects.Quote();
                MessageBox.Show("Предыдущая цитата не найдена!", new assembly().AssemblyProduct, MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            }
            else
            {
                currentQuote = tempQuote;
            }
            qRefresh();
        }

        private void qRefresh()
        {
            if (currentQuote.ID != 0)
            {
                prevQuote.Enabled = (Database.Quote_GetMinID() != currentQuote.ID);
                nextQuote.Enabled = (Database.Quote_GetMaxID() != currentQuote.ID);
                editQuote.Enabled = true;
                deleteQuote.Enabled = true;
                rtfed.RtfTextBox.Rtf = currentQuote.RTF;
                this.captionText = "Просмотр цитаты #" + currentQuote.ID.ToString();
                tabcontrol.UpdateCaption("Просмотр цитаты #" + currentQuote.ID.ToString());
                authorImage.Image = currentQuote.QuoteAuthor.Photo;
                if (authorImage.Image == null) authorImage.Image = Resources.noPhoto_128;
                authorFIO.Text = currentQuote.QuoteAuthor.FIO.TrimEnd(new char[] { ' ' });
                authorAbout.Text = currentQuote.QuoteAuthor.About;
                pFavorite.Visible = true;
                UpdateFavoriteStar(currentQuote.IsFavorite);
            }
            else
            {
                pFavorite.Visible = false;
                authorImage.Image = null;
                authorFIO.Text = "";
                authorAbout.Text = "";
                rtfed.RtfTextBox.Text = "";
                editQuote.Enabled = false;
                deleteQuote.Enabled = false;
                if (captionText != "")
                {
                    captionText = "";
                    tabcontrol.UpdateCaption("");
                }
            }
        }

        private void UpdateFavoriteStar(bool isGoldStar)
        {
            if (isGoldStar)
            {
                favoriteQuote.Image = Resources.favorite64;
                favoriteText.Text = sFavorite;
            }
            else
            {
                favoriteQuote.Image = Resources.nonFavorite64;
                favoriteText.Text = sNonFavorite;
            }
        }
    }
}
