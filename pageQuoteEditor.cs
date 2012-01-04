using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;
using System.IO;
using ItWorksTeam.UI;

namespace QuotePad
{
    public class pageQuoteEditor: TabPagePrototype
    {
        RTFEditor rtfed = new RTFEditor();
        ConfigToolStrip confToolStrip;
        ToolStripComboBoxPrototype cAuthors;
        ToolStripComboBoxPrototype cThemes;
        TabControlPrototype tabcontrol;
        int[] theme_ids;
        int[] autors_ids;
        bool editMode = false;
        int quoteID = 0;
        // context menu items
        ToolStripMenuItem cUndo;
        ToolStripMenuItem cCut;
        ToolStripMenuItem cCopy;
        ToolStripMenuItem cPaste;
        ToolStripMenuItem cDelete;
        ToolStripMenuItem cSelectAll;
        ToolStripMenuItem cSaveFormat;

        public pageQuoteEditor(TabControlPrototype tabControl,int editQuote = -1)
        {
            tabcontrol = tabControl;
            this.Text = "Редактор";
            this.captionText = "Новая цитата";
            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            this.BeforeDestroy += new Delete(pageQuoteEditor_BeforeDestroy);

            confToolStrip = new ConfigToolStrip(ConfigToolStrip.toolStripButtons.Add | ConfigToolStrip.toolStripButtons.Save);
            confToolStrip.textboxesParent = this;
            confToolStrip.RequiredFields = new Control[] { rtfed.RtfTextBox };
            confToolStrip.toolStripAdd.PerformClick();
            confToolStrip.toolStripSave.Click += new EventHandler(toolStripSave_Click);
            confToolStrip.toolStripAdd.Click += new EventHandler(toolStripAdd_Click);

            cAuthors = new ToolStripComboBoxPrototype();
            cAuthors.Width = 220;
            cAuthors.DropDown += new EventHandler(cAuthors_DropDown);
            cAuthors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cThemes = new ToolStripComboBoxPrototype();
            cThemes.Width = 220;
            cThemes.DropDown += new EventHandler(cThemes_DropDown);
            cThemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Contex menu
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Opening += new System.ComponentModel.CancelEventHandler(contextMenu_Opening);
            cUndo = new ToolStripMenuItem("Отменить");
            cCut = new ToolStripMenuItem("Вырезать");
            cCopy = new ToolStripMenuItem("Копировать");
            cPaste = new ToolStripMenuItem("Вставить");
            cDelete = new ToolStripMenuItem("Удалить");
            cSelectAll = new ToolStripMenuItem("Выделить всё");
            cSaveFormat = new ToolStripMenuItem("Сохранить форматирование");
            cUndo.Click += new EventHandler(cUndo_Click);
            cCut.Click += new EventHandler(cCut_Click);
            cCopy.Click += new EventHandler(cCopy_Click);
            cPaste.Click += new EventHandler(cPaste_Click);
            cDelete.Click += new EventHandler(cDelete_Click);
            cSelectAll.Click += new EventHandler(cSelectAll_Click);
            cSaveFormat.Click += new EventHandler(cSaveFormat_Click);
            contextMenu.Items.Add(cUndo);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(cCut);
            contextMenu.Items.Add(cCopy);
            contextMenu.Items.Add(cPaste);
            contextMenu.Items.Add(cDelete);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(cSelectAll);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(cSaveFormat);
            rtfed.RtfTextBox.ContextMenuStrip = contextMenu;

            this.Controls.Add(rtfed.RtfTextBox);
            this.Controls.Add(rtfed.RtfToolBar);
            this.Controls.Add(rtfed.StatusBar);
            this.AddToolStripItem(confToolStrip.toolStripAdd);
            this.AddToolStripItem(confToolStrip.toolStripSave);
            this.AddToolStripItem(new ToolStripSeparatorPrototype());
            //this.AddToolStripItem(new ToolStripLabelPrototype("Укажите автора и тему"));
            this.AddToolStripItem(cAuthors);
            this.AddToolStripItem(cThemes);

            if (editQuote != -1)
            {
                editMode = true;
                Objects.Quote quote = Database.Quote_FindByID(editQuote);
                if (quote.ID != 0)
                {
                    quoteID = quote.ID;
                    cAuthors_DropDown(this, null);
                    cThemes_DropDown(this, null);
                    cAuthors.Text = quote.QuoteAuthor.FIO;
                    cThemes.Text = quote.QuoteTheme.Name;
                    rtfed.RtfTextBox.Rtf = quote.RTF;
                    this.captionText = "Редактирование цитаты #" + quote.ID.ToString();
                }
            } 
            else
            {
                RtfFormat format = GetFormatting();
                if (format.FontName != null)
                {
                    rtfed.RtfTextBox.SelectionAlignment = format.TextAlign;
                    rtfed.RtfTextBox.SelectionColor = Color.FromName(format.FontColor);
                    rtfed.RtfTextBox.SelectionBackColor = Color.FromName(format.TextBackColor);
                    rtfed.RtfTextBox.SelectionFont = new Font(format.FontName, format.FontSize, format.FontStyle);
                    rtfed.RefreshToolbar();
                }
            }
        }

        /////////////////////////
        // Contex menu methods //
        /////////////////////////
        void contextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cUndo.Enabled = rtfed.RtfTextBox.CanUndo;
            cCut.Enabled = (rtfed.RtfTextBox.SelectionLength != 0);
            cCopy.Enabled = (rtfed.RtfTextBox.SelectionLength != 0);
            cPaste.Enabled = Clipboard.ContainsText(TextDataFormat.Rtf | TextDataFormat.Text);
            cDelete.Enabled = (rtfed.RtfTextBox.SelectionLength != 0);
            cSelectAll.Enabled = (rtfed.RtfTextBox.SelectionLength != rtfed.RtfTextBox.Text.Length);
        }

        void cUndo_Click(object sender, EventArgs e)
        {
            rtfed.RtfTextBox.Undo();
        }

        void cCut_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtfed.RtfTextBox.SelectedRtf, TextDataFormat.Rtf);
            rtfed.RtfTextBox.SelectedRtf = "";
        }

        void cCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtfed.RtfTextBox.SelectedRtf, TextDataFormat.Rtf);
        }

        void cPaste_Click(object sender, EventArgs e)
        {
            ClipboardAsync ca = new ClipboardAsync();
            rtfed.RtfTextBox.SelectedRtf = ca.GetText(TextDataFormat.Rtf);
        }

        void cDelete_Click(object sender, EventArgs e)
        {
            rtfed.RtfTextBox.SelectedRtf = "";
        }

        void cSelectAll_Click(object sender, EventArgs e)
        {
            rtfed.RtfTextBox.SelectAll();
        }

        void cSaveFormat_Click(object sender, EventArgs e)
        {
            SaveFormatting(new RtfFormat(rtfed.RtfTextBox.SelectionAlignment,
                rtfed.RtfTextBox.SelectionBackColor, rtfed.RtfTextBox.SelectionColor,
                rtfed.RtfTextBox.SelectionFont));
        }
        /////// end of context menu ////////

        /// <summary>
        /// Ask for saving changes before tab will be closed
        /// </summary>
        void pageQuoteEditor_BeforeDestroy()
        {
            this.cancelClosing = false;
            if (confToolStrip.toolStripSave.Enabled && rtfed.RtfTextBox.Text != "")
            {
                DialogResult question = MessageBox.Show("Сохранить изменения перед закрытием?",
                    "Редактор цитат", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (question)
                {
                    case DialogResult.Yes:
                        toolStripSave_Click(this, null);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        this.cancelClosing = true;
                        break;
                }
            }
        }

        void toolStripAdd_Click(object sender, EventArgs e)
        {
            //confToolStrip
            this.captionText = "Новая цитата";
            tabcontrol.UpdateCaption();
        }

        void cThemes_DropDown(object sender, EventArgs e)
        {
            cThemes.Items.Clear();
            Objects.Theme[] themesFound = Database.Theme_GetList();
            theme_ids = new int[themesFound.Length];
            for (int a = 0; a < themesFound.Length; a++)
            {
                cThemes.Items.Add(themesFound[a].Name);
                theme_ids[a] = themesFound[a].ID;
            }
        }

        void cAuthors_DropDown(object sender, EventArgs e)
        {
            cAuthors.Items.Clear();
            Objects.Author[] autorsFound = Database.Author_GetList();
            autors_ids = new int[autorsFound.Length];
            for (int a = 0; a < autorsFound.Length; a++)
            {
                cAuthors.Items.Add(autorsFound[a].FIO);
                autors_ids[a] = autorsFound[a].ID;
            }
        }

        void toolStripSave_Click(object sender, EventArgs e)
        {
            if (cAuthors.SelectedIndex != -1)
            {
                if (cThemes.SelectedIndex != -1)
                {
                    if (confToolStrip.IsRequiredFilled())
                    {
                        if (editMode)
                        {
                            if (Database.Quote_Modify(quoteID, autors_ids[cAuthors.SelectedIndex], theme_ids[cThemes.SelectedIndex],
                            rtfed.RtfTextBox.Rtf, rtfed.RtfTextBox.Text, false))
                            {
                                confToolStrip.SaveExternalMethod();
                                editMode = false;
                                quoteID = 0;
                            }
                            else MessageBox.Show("Ошибка сохранения!", "Редактор цитат", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (Database.Quote_Create(autors_ids[cAuthors.SelectedIndex], theme_ids[cThemes.SelectedIndex],
                                rtfed.RtfTextBox.Rtf, rtfed.RtfTextBox.Text, false))
                            {
                                confToolStrip.SaveExternalMethod();
                            }
                            else MessageBox.Show("Ошибка сохранения!", "Редактор цитат", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Укажите тему цитаты!", "Редактор цитат", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cancelClosing = true;
                }
            }
            else
            {
                MessageBox.Show("Укажите автора цитаты!", "Редактор цитат",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cancelClosing = true;
            }
        }

        private bool SaveFormatting(RtfFormat format)
        {
            XmlSerializer xs = new XmlSerializer(typeof(RtfFormat));
            ItWorksTeam.Utils.Registry r = new ItWorksTeam.Utils.Registry();
            StringWriter sw = new StringWriter();
            xs.Serialize(sw, format);
            bool result = r.SaveKey(ItWorksTeam.Utils.Registry.BaseKeys.HKEY_LOCAL_MACHINE,
                @"Software\ItWorksTeam\QuotePad", "RtfFormat", sw.ToString());
            sw.Close();
            return result;
        }

        private RtfFormat GetFormatting()
        {
            ItWorksTeam.Utils.Registry r = new ItWorksTeam.Utils.Registry();
            string savedFormatting = r.ReadKey<string>(ItWorksTeam.Utils.Registry.BaseKeys.HKEY_LOCAL_MACHINE, 
                @"Software\ItWorksTeam\QuotePad", "RtfFormat", null);
            if (savedFormatting != null)
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(RtfFormat));
                    StringReader sr = new StringReader(savedFormatting);
                    RtfFormat restoredFormat = (RtfFormat)xs.Deserialize(sr);
                    sr.Close();
                    return restoredFormat;
                }
                catch (InvalidOperationException)
                {
                    return new RtfFormat();
                }
            }
            else return new RtfFormat();

        }

        [Serializable]
        public class RtfFormat
        {
            public HorizontalAlignment TextAlign;
            public string TextBackColor;
            public string FontColor;
            public string FontName;
            public float FontSize;
            public FontStyle FontStyle;

            public RtfFormat(HorizontalAlignment horizontalAlign, Color backColor,
                Color fontColor, Font font)
            {
                TextAlign = horizontalAlign;
                TextBackColor = backColor.Name;
                FontColor = fontColor.Name;
                FontName = font.Name;
                FontSize = font.Size;
                FontStyle = font.Style;
            }

            public RtfFormat()
            {
            }
        }
    }
}
