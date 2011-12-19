using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public class ThemeManager : TabPagePrototype
    {
        ConfigToolStrip confToolStrip;
        ToolStripComboBoxPrototype comboBox;
        TabControlPrototype tabcontrol;
        int[] theme_ids;
        TextBox theme = new TextBox();

        public ThemeManager(TabControlPrototype tabControl)
        {
            tabcontrol = tabControl;
            this.Text = "Темы";
            this.captionText = "Редактор тем";
            this.BeforeDestroy += new Delete(ThemeManager_BeforeDestroy);
            GroupBox g = new GroupBox();
            g.Text = " Имя темы ";
            g.Dock = DockStyle.Fill;
            theme.Multiline = true;
            theme.Dock = DockStyle.Fill;
            theme.MaxLength = 50;
            theme.Name = "theme";
            theme.Enabled = false;
            comboBox = new ToolStripComboBoxPrototype();
            comboBox.Width = 320;
            comboBox.DropDown += new EventHandler(comboBox_DropDown);
            comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            confToolStrip = new ConfigToolStrip(ConfigToolStrip.toolStripButtons.Add | 
                ConfigToolStrip.toolStripButtons.Edit | 
                ConfigToolStrip.toolStripButtons.Save | 
                ConfigToolStrip.toolStripButtons.Delete);
            confToolStrip.textboxesParent = this;
            confToolStrip.RequiredFields = new Control[] { theme };
            confToolStrip.toolStripSave.Click += new EventHandler(toolStripSave_Click);
            confToolStrip.toolStripAdd.Click += new EventHandler(toolStripAdd_Click);
            confToolStrip.toolStripDelete.Click += new EventHandler(toolStripDelete_Click);
            confToolStrip.toolStripEdit.Click += new EventHandler(toolStripEdit_Click);
            this.AddToolStripItem(confToolStrip.toolStripAdd);
            this.AddToolStripItem(confToolStrip.toolStripEdit);
            this.AddToolStripItem(confToolStrip.toolStripSave);
            this.AddToolStripItem(confToolStrip.toolStripDelete);
            this.AddToolStripItem(comboBox);
            this.AddToolStripItem(new ToolStripSeparatorPrototype());
            //this.AddToolStripItem(new ToolStripLabelPrototype("Сохраненные темы:"));
            g.Controls.Add(theme);
            this.Controls.Add(g);
        }

        void toolStripEdit_Click(object sender, EventArgs e)
        {
            this.captionText = "Редактирование темы";
            tabcontrol.UpdateCaption();
        }

        /// <summary>
        /// Ask for saving changes before tab will be closed
        /// </summary>
        void ThemeManager_BeforeDestroy()
        {
            this.cancelClosing = false;
            if (confToolStrip.toolStripSave.Enabled && theme.Text != "")
            {
                DialogResult question = MessageBox.Show("Сохранить изменения перед закрытием?",
                    "Редактор тем", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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

        void toolStripDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Удалить тему {0}?", comboBox.Text),"Удаление темы", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Database.Theme_Remove(theme_ids[comboBox.SelectedIndex]))
                {
                    confToolStrip.DeleteExternalMethod();
                }
                else MessageBox.Show("Нельзя удалить тему!", "Удаление темы", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.captionText = "Редактор тем";
                tabcontrol.UpdateCaption();
            }
        }

        void toolStripAdd_Click(object sender, EventArgs e)
        {
            comboBox.SelectedIndex = -1;
            this.captionText = "Новая тема";
            tabcontrol.UpdateCaption();
        }

        void toolStripSave_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == -1)
            {
                if (confToolStrip.IsRequiredFilled())
                {
                    if (Database.Theme_Create(theme.Text))
                    {
                        confToolStrip.SaveExternalMethod();
                        this.captionText = "Редактор тем";
                        tabcontrol.UpdateCaption();
                    }
                    else MessageBox.Show("Ошибка сохранения!", "Редактор тем", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (confToolStrip.IsRequiredFilled())
                {
                    if (Database.Theme_Modify(theme_ids[comboBox.SelectedIndex], theme.Text))
                    {
                        confToolStrip.SaveExternalMethod();
                        this.captionText = "Редактор тем";
                        tabcontrol.UpdateCaption();
                    }
                    else MessageBox.Show("Ошибка сохранения!", "Редактор тем", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex != -1)
            {
                confToolStrip.OpenExternalMethod();
                theme.Text = comboBox.Text;
                this.captionText = "Просмотр темы";
                tabcontrol.UpdateCaption();
            }
        }

        void comboBox_DropDown(object sender, EventArgs e)
        {
            comboBox.Items.Clear();
            Objects.Theme[] themesFound = Database.Theme_GetList();
            theme_ids = new int[themesFound.Length];
            for (int a=0;a<themesFound.Length;a++)
            {
                comboBox.Items.Add(themesFound[a].Name);
                theme_ids[a] = themesFound[a].ID;
            }
        }
    }
}
