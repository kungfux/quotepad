using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public class pageQuoteEditor: TabPagePrototype
    {
        ItWorks.RTFed rtfed = new ItWorks.RTFed();
        ConfigToolStrip confToolStrip;
        ToolStripComboBoxPrototype cAuthors;
        ToolStripComboBoxPrototype cThemes;
        int[] theme_ids;
        int[] autors_ids;

        public pageQuoteEditor()
        {
            this.Text = "Редактор";
            rtfed.RtfTextBox.Dock = DockStyle.Fill;

            confToolStrip = new ConfigToolStrip(ConfigToolStrip.toolStripButtons.Add | ConfigToolStrip.toolStripButtons.Save);
            confToolStrip.textboxesParent = this;
            confToolStrip.RequiredFields = new Control[] { rtfed.RtfTextBox };
            confToolStrip.toolStripAdd.PerformClick();
            confToolStrip.toolStripSave.Click += new EventHandler(toolStripSave_Click);
            confToolStrip.toolStripAdd.Click += new EventHandler(toolStripAdd_Click);

            cAuthors = new ToolStripComboBoxPrototype();
            cAuthors.DropDown += new EventHandler(cAuthors_DropDown);
            cAuthors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cThemes = new ToolStripComboBoxPrototype();
            cThemes.DropDown += new EventHandler(cThemes_DropDown);
            cThemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.Controls.Add(rtfed.RtfTextBox);
            this.Controls.Add(rtfed.RtfToolBar);
            this.Controls.Add(rtfed.StatusBar);
            this.AddToolStripItem(confToolStrip.toolStripAdd);
            this.AddToolStripItem(confToolStrip.toolStripSave);
            this.AddToolStripItem(cAuthors);
            this.AddToolStripItem(cThemes);
        }

        void toolStripAdd_Click(object sender, EventArgs e)
        {
            //confToolStrip
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
                        if (Database.Quote_Create(autors_ids[cAuthors.SelectedIndex], theme_ids[cThemes.SelectedIndex],
                            rtfed.RtfTextBox.Rtf, rtfed.RtfTextBox.Text, false))
                        {
                            confToolStrip.SaveExternalMethod();
                        }
                    }
                }
                else MessageBox.Show("define theme");
            }
            else
                MessageBox.Show("define author");
        }
    }
}
