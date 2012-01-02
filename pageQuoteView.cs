using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace QuotePad
{
    public class pageQuoteView : TabPagePrototype
    {
        ItWorks.RTFed rtfed = new ItWorks.RTFed();
        Objects.Quote currentQuote = new Objects.Quote();
        Objects.Quote tempQuote;
        PictureBox authorImage = new PictureBox();
        PictureBox goldStar = new PictureBox();
        Label authorFIO = new Label();
        TextBox authorAbout = new TextBox();
        private TabControlPrototype tabcontrol;
        ToolStripButtonPrototype editQuote;
        ToolStripButtonPrototype deleteQuote;
        ToolStripButtonPrototype prevQuote;
        ToolStripButtonPrototype nextQuote;
        ToolStripButtonPrototype favoriteUnfavorite;
        SplitContainer s;
        TableLayoutPanel backpanel; // back panel for displaying info about author
        ItWorks.Registry regValue = new ItWorks.Registry();
        Objects.Quote[] displayOnly;
        int displayOnlyCurrent;

        public pageQuoteView(TabControlPrototype tabControl, params Objects.Quote[] displayOnlyQuotes)
        {
            tabcontrol = tabControl;
            if (displayOnlyQuotes != null && displayOnlyQuotes.Length > 0)
            {
                displayOnly = displayOnlyQuotes;
            }

            this.Text = "Просмотр";
            this.captionText = "Просмотр цитат";

            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            rtfed.RtfTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            rtfed.RtfTextBox.BackColor = authorFIO.BackColor;

            authorImage.SizeMode = PictureBoxSizeMode.Zoom;
            authorImage.Dock = DockStyle.Fill;

            goldStar.SizeMode = PictureBoxSizeMode.Zoom;
            goldStar.Size = new System.Drawing.Size(32, 32);
            goldStar.Location = new System.Drawing.Point(0, 0);
            goldStar.BackColor = Color.Transparent;
            authorImage.Controls.Add(goldStar);

            authorFIO.Dock = DockStyle.Fill;
            authorFIO.Font = new System.Drawing.Font(authorFIO.Font.FontFamily, authorFIO.Font.Size+3,
                System.Drawing.FontStyle.Bold);
            authorFIO.Bounds = new System.Drawing.Rectangle(0, 250, 250, authorFIO.Font.Height);
            authorFIO.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            authorAbout.ReadOnly = true;
            authorAbout.BackColor = authorFIO.BackColor;
            authorAbout.Multiline = true;
            authorAbout.ScrollBars = ScrollBars.Vertical;
            authorAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            authorAbout.Dock = DockStyle.Fill;
            authorAbout.TextAlign = HorizontalAlignment.Center;

            backpanel = new TableLayoutPanel();
            backpanel.Dock = DockStyle.Fill;
            backpanel.ColumnCount = 1;
            backpanel.RowCount = 3;
            backpanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            backpanel.RowStyles.Add(new RowStyle(SizeType.Absolute, authorFIO.Height));
            backpanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            backpanel.Controls.Add(authorImage, 0, 0);
            backpanel.Controls.Add(authorFIO, 0, 1);
            backpanel.Controls.Add(authorAbout, 0, 2);
            backpanel.Visible = regValue.ReadKey<bool>(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE, "Software\\ItWorksTeam\\QuotePad",
                "BackPanelVisible", true);

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

            favoriteUnfavorite = new ToolStripButtonPrototype("Отметить как \"любимую\"", Resources.favoriteUnfavorite);
            favoriteUnfavorite.Enabled = false;
            favoriteUnfavorite.isForSupervisorOnly = true;
            favoriteUnfavorite.Click += new EventHandler(favoriteUnfavorite_Click);
            this.AddToolStripItem(favoriteUnfavorite);

            this.AddToolStripItem(prevQuote);
            if (displayOnly == null)
            {
                this.AddToolStripItem(randomQuote);
            }
            this.AddToolStripItem(nextQuote);
            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            rtfed.RtfTextBox.ReadOnly = true;
            rtfed.RtfTextBox.KeyDown += new KeyEventHandler(RtfTextBox_KeyDown);

            s = new SplitContainer();
            s.SplitterWidth = 1;
            s.SplitterIncrement = 1;
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
                s.SplitterDistance = Convert.ToInt32(Convert.ToDouble(s.Width) * (x / 100.0));
            } 
            else 
            {
                s.SplitterDistance = (s.Width - s.SplitterWidth) * 75 / 100; // Default position is 75%
            }
            s.SplitterMoved += new SplitterEventHandler(s_SplitterMoved);
            if (displayOnly == null)
            {
                randomQuote_Click(this, null);
            }
            else
            {
                currentQuote = displayOnly[0];
                displayOnlyCurrent = 0;
                qRefresh();
            }
        }

        void favoriteUnfavorite_Click(object sender, EventArgs e)
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
                    if (nextQuote.Enabled)
                        nextQuote_Click(this, null);
                    break;
                case Keys.Left:
                    if (prevQuote.Enabled)
                        prevQuote_Click(this, null);
                    break;
                case Keys.Space:
                    randomQuote_Click(this, null);
                    break;
            }
            e.SuppressKeyPress = true;
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

        void editQuote_Click(object sender, EventArgs e)
        {
            if (currentQuote.ID != 0 && Database.Quote_FindByID_IsExist(currentQuote.ID))
            {
                tabcontrol.AddPage(new pageQuoteEditor(tabcontrol, currentQuote.ID));
            }
            else
            {
                MessageBox.Show("Цитата не найдена!\nВозможно цитата уже была удалена?", "Редактирование цитаты",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void nextQuote_Click(object sender, EventArgs e)
        {
            if (displayOnly == null)
            {
                tempQuote = Database.Quote_ReadNext(currentQuote.ID);
            }
            else
            {
                displayOnlyCurrent++;
                tempQuote = displayOnly[displayOnlyCurrent];
            }
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
                MessageBox.Show("Не найдено ни одной цитаты!", new assembly().AssemblyProduct, MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            }
            qRefresh();
        }

        void prevQuote_Click(object sender, EventArgs e)
        {
            if (displayOnly == null)
            {
                tempQuote = Database.Quote_ReadPrevious(currentQuote.ID);
            }
            else
            {
                displayOnlyCurrent--;
                tempQuote = displayOnly[displayOnlyCurrent];
            }
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
                if (displayOnly == null)
                {
                    prevQuote.Enabled = (Database.Quote_GetMinID() != currentQuote.ID);
                    nextQuote.Enabled = (Database.Quote_GetMaxID() != currentQuote.ID);
                    this.captionText = "Просмотр цитаты #" + currentQuote.ID.ToString();
                    tabcontrol.UpdateCaption("Просмотр цитаты #" + currentQuote.ID.ToString());
                }
                else
                {
                    prevQuote.Enabled = (displayOnly[0].ID != currentQuote.ID);
                    nextQuote.Enabled = (displayOnly[displayOnly.Length - 1].ID != currentQuote.ID);
                    this.captionText = string.Format("Просмотр цитаты #{0} [Просмотр цитаты {1} из найденных {2} цитат]",
                        currentQuote.ID.ToString(), (displayOnlyCurrent + 1).ToString(), displayOnly.Length.ToString());
                    tabcontrol.UpdateCaption(string.Format("Просмотр цитаты #{0} [Просмотр цитаты {1} из найденных {2} цитат]",
                        currentQuote.ID.ToString(), (displayOnlyCurrent + 1).ToString(), displayOnly.Length.ToString()));
                }
                editQuote.Enabled = true;
                deleteQuote.Enabled = true;
                rtfed.RtfTextBox.Rtf = currentQuote.RTF;
                authorImage.Image = currentQuote.QuoteAuthor.Photo;
                if (authorImage.Image == null) authorImage.Image = Resources.noPhoto_128;
                authorFIO.Text = currentQuote.QuoteAuthor.FIO.TrimEnd(new char[] { ' ' });
                authorAbout.Text = currentQuote.QuoteAuthor.About;
                favoriteUnfavorite.Enabled = true;
                UpdateFavoriteStar(currentQuote.IsFavorite);
            }
            else
            {
                favoriteUnfavorite.Enabled = false;
                authorImage.Image = null;
                authorFIO.Text = "";
                authorAbout.Text = "";
                rtfed.RtfTextBox.Text = "";
                editQuote.Enabled = false;
                deleteQuote.Enabled = false;
                goldStar.Image = null;
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
                favoriteUnfavorite.Text = "Убрать отметку \"любимая\"";
                goldStar.Image = Resources.favorite64;
            }
            else
            {
                favoriteUnfavorite.Text = "Отметить как \"любимую\"";
                goldStar.Image = null;
            }
        }
    }
}
