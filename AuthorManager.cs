﻿using System;
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
        Database db = new Database();
        TextBox author = new TextBox();
        TextBox about = new TextBox();
        PictureBox photo = new PictureBox();

        public AuthorManager()
        {
            this.Text = "Автора";
            author.MaxLength = 50;
            author.Multiline = true;
            author.Dock = DockStyle.Fill;
            //author.Width = 400;
            about.MaxLength = 1000;
            about.Multiline = true;
            about.Dock = DockStyle.Fill;
            photo.Dock = DockStyle.Right;
            photo.SizeMode = PictureBoxSizeMode.AutoSize;
            photo.BackColor = this.BackColor;
            comboBox = new ToolStripComboBoxPrototype();
            comboBox.AutoSize = false;
            comboBox.Width = 400;
            comboBox.DropDown += new EventHandler(comboBox_DropDown);
            comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            confToolStrip = new ConfigToolStrip(ConfigToolStrip.toolStripButtons.Add | 
                ConfigToolStrip.toolStripButtons.Edit | 
                ConfigToolStrip.toolStripButtons.Save | 
                ConfigToolStrip.toolStripButtons.Delete);
            confToolStrip.textboxesParent = this;
            confToolStrip.RequiredFields = new Control[] { author };
            confToolStrip.toolStripSave.Click += new EventHandler(toolStripSave_Click);
            confToolStrip.toolStripAdd.Click += new EventHandler(toolStripAdd_Click);
            confToolStrip.toolStripDelete.Click += new EventHandler(toolStripDelete_Click);
            this.AddToolStripItem(confToolStrip.toolStripAdd);
            this.AddToolStripItem(confToolStrip.toolStripEdit);
            this.AddToolStripItem(confToolStrip.toolStripSave);
            this.AddToolStripItem(confToolStrip.toolStripDelete);
            ToolStripButtonPrototype change_photo = new ToolStripButtonPrototype("Изменить фото", Resources._1315515132_edit);
            change_photo.Click += new EventHandler(change_photo_Click);
            this.AddToolStripItem(change_photo);
            ToolStripButtonPrototype clear_photo = new ToolStripButtonPrototype("Удалить фото", Resources._1315515132_edit);
            clear_photo.Click += new EventHandler(clear_photo_Click);
            this.AddToolStripItem(clear_photo);
            this.AddToolStripItem(comboBox);
            SplitContainer s1 = new SplitContainer();
            SplitContainer s2 = new SplitContainer();
            s1.Dock = DockStyle.Fill;
            s2.Dock = DockStyle.Fill;
            s2.Orientation = Orientation.Horizontal;
            s1.Panel1.Controls.Add(s2);
            s1.Panel2.Controls.Add(photo);
            s2.Panel1.Controls.Add(author);
            s2.Panel2.Controls.Add(about);
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
                db.Author_ClearImage(autors_ids[comboBox.SelectedIndex]);
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
                    if (TestImage(of.FileName) && !db.Author_SetImage(autors_ids[comboBox.SelectedIndex], of.FileName))
                    {
                        MessageBox.Show("Упс! Это ошибка! Сообщите разработчику.");
                    }
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
                if (db.Author_Remove(autors_ids[comboBox.SelectedIndex]))
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
                if (db.Author_Create(author.Text, about.Text)) confToolStrip.SaveExternalMethod();
            }
            else
            {
                if (db.Author_Modify(autors_ids[comboBox.SelectedIndex], author.Text, about.Text)) confToolStrip.SaveExternalMethod();
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
            }
        }

        Objects.Author[] autorsFound;

        void comboBox_DropDown(object sender, EventArgs e)
        {
            comboBox.Items.Clear();
            autorsFound = db.Author_GetList();
            autors_ids = new int[autorsFound.Length];
            for (int a=0;a<autorsFound.Length;a++)
            {
                comboBox.Items.Add(autorsFound[a].FIO);
                autors_ids[a] = autorsFound[a].ID;
            }
        }
    }
}
