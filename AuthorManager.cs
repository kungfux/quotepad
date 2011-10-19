using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace QuotePad
{
    public class AuthorManager : TabPagePrototype
    {
        ConfigToolStrip confToolStrip;
        ToolStripComboBoxPrototype comboBox;
        int[] autors_ids;
        TextBox author = new TextBox();
        TextBox about = new TextBox();
        PictureBox photo = new PictureBox();
        ToolStripButtonPrototype change_photo;
        ToolStripButtonPrototype clear_photo;

        public AuthorManager()
        {
            this.Text = "Авторы";
            GroupBox g1 = new GroupBox();
            GroupBox g2 = new GroupBox();
            GroupBox g3 = new GroupBox();
            g1.Text = " ФИО автора";
            g2.Text = " Об авторе ";
            g3.Text = " Фото автора ";
            g1.Dock = DockStyle.Fill;
            g2.Dock = DockStyle.Fill;
            g3.Dock = DockStyle.Fill;
            author.MaxLength = 50;
            author.Name = "author";
            author.Multiline = true;
            author.Dock = DockStyle.Fill;
            author.Enabled = false;
            //author.Width = 400;
            about.MaxLength = 1000;
            about.Multiline = true;
            about.ScrollBars = ScrollBars.Vertical;
            about.Dock = DockStyle.Fill;
            about.Enabled = false;
            about.Name = "about";
            photo.Dock = DockStyle.Right;
            photo.Dock = DockStyle.Fill;
            photo.SizeMode = PictureBoxSizeMode.Zoom;
            photo.BackColor = this.BackColor;
            comboBox = new ToolStripComboBoxPrototype();
            comboBox.AutoSize = false;
            comboBox.Width = 320;
            comboBox.DropDown += new EventHandler(comboBox_DropDown);
            comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            confToolStrip = new ConfigToolStrip(ConfigToolStrip.toolStripButtons.Add | 
                ConfigToolStrip.toolStripButtons.Edit | 
                ConfigToolStrip.toolStripButtons.Save | 
                ConfigToolStrip.toolStripButtons.Delete);
            confToolStrip.textboxesParent = this;
            confToolStrip.RequiredFields = new Control[] { author };
            confToolStrip.NonRequiredFields = new Control[] { about };
            confToolStrip.toolStripSave.Click += new EventHandler(toolStripSave_Click);
            confToolStrip.toolStripAdd.Click += new EventHandler(toolStripAdd_Click);
            confToolStrip.toolStripDelete.Click += new EventHandler(toolStripDelete_Click);
            this.AddToolStripItem(confToolStrip.toolStripAdd);
            this.AddToolStripItem(confToolStrip.toolStripEdit);
            this.AddToolStripItem(confToolStrip.toolStripSave);
            this.AddToolStripItem(confToolStrip.toolStripDelete);
            this.AddToolStripItem(new ToolStripSeparatorPrototype());
            this.AddToolStripItem(new ToolStripLabelPrototype("Сохраненные авторы:"));
            change_photo = new ToolStripButtonPrototype("Изменить фото", Resources._1315515132_edit);
            change_photo.Click += new EventHandler(change_photo_Click);
            this.AddToolStripItem(change_photo);
            clear_photo = new ToolStripButtonPrototype("Удалить фото", Resources._1315515132_edit);
            clear_photo.Click += new EventHandler(clear_photo_Click);
            this.AddToolStripItem(clear_photo);
            this.AddToolStripItem(comboBox);
            SplitContainer s1 = new SplitContainer();
            SplitContainer s2 = new SplitContainer();
            s1.Dock = DockStyle.Fill;
            s2.Dock = DockStyle.Fill;
            s2.Orientation = Orientation.Horizontal;
            g1.Controls.Add(author);
            g2.Controls.Add(about);
            g3.Controls.Add(photo);
            s1.Panel1.Controls.Add(s2);
            s1.Panel2.Controls.Add(g3);
            s2.Panel1.Controls.Add(g1);
            s2.Panel2.Controls.Add(g2);
            //this.Controls.Add(author);
            //this.Controls.Add(about);
            //this.Controls.Add(photo);
            this.Controls.Add(s1);
        }

        void clear_photo_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex != -1 &&
                MessageBox.Show("Удалить фото этого автора?", "Фото автора", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Database.Author_ClearImage(autors_ids[comboBox.SelectedIndex]);
            }
        }

        void change_photo_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex != -1 &&
                MessageBox.Show("Добавить новое фото для автора?", "Фото автора", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OpenFileDialog of = new OpenFileDialog();
                of.ShowDialog();
                if (of.FileName != "")
                {
                    if (TestImage(of.FileName) && !Database.Author_SetImage(autors_ids[comboBox.SelectedIndex], of.FileName))
                    {
                        MessageBox.Show("Упс! Это ошибка! Сообщите разработчику.");
                    }
                    else comboBox_SelectedIndexChanged(this, null);
                }
            }
        }

        private bool TestImage(string file)
        {
            PictureBox test = new PictureBox();
            try
            {
                test.Load(file);
                return true;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Выбранный файл не может использоваться в качестве фото!", "Фото автора", MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                test.Dispose();
            }
        }

        void toolStripDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Удалить автора {0}?", comboBox.Text),"Удаление автора", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Database.Author_Remove(autors_ids[comboBox.SelectedIndex]))
                {
                    confToolStrip.DeleteExternalMethod();
                }
                else MessageBox.Show("Нельзя удалить автора!", "Удаление автора", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void toolStripAdd_Click(object sender, EventArgs e)
        {
            comboBox.SelectedIndex = -1;
        }

        void toolStripSave_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == -1)
            {
                if (confToolStrip.IsRequiredFilled())
                {
                    if (Database.Author_Create(author.Text, about.Text)) confToolStrip.SaveExternalMethod();
                }
            }
            else
            {
                if (confToolStrip.IsRequiredFilled())
                {
                    if (Database.Author_Modify(autors_ids[comboBox.SelectedIndex], author.Text, about.Text)) confToolStrip.SaveExternalMethod();
                }
            }
        }

        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex != -1)
            {
                confToolStrip.OpenExternalMethod();
                author.Text = comboBox.Text;
                about.Text = autorsFound[comboBox.SelectedIndex].About;
                photo.Image = autorsFound[comboBox.SelectedIndex].Photo;
                change_photo.Enabled = true;
                if (photo.Image != null) clear_photo.Enabled = true;
                else clear_photo.Enabled = false;
            }
            else
            {
                change_photo.Enabled = false;
                clear_photo.Enabled = false;
            }
        }

        Objects.Author[] autorsFound;

        void comboBox_DropDown(object sender, EventArgs e)
        {
            comboBox.Items.Clear();
            autorsFound = Database.Author_GetList();
            autors_ids = new int[autorsFound.Length];
            for (int a=0;a<autorsFound.Length;a++)
            {
                comboBox.Items.Add(autorsFound[a].FIO);
                autors_ids[a] = autorsFound[a].ID;
            }
        }
    }
}
