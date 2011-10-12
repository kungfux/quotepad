using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using QuotePad;

namespace QuotePad
{
    public class ConfigToolStrip
    {
        //public ToolStrip toolStrip = new ToolStrip();
        public Control textboxesParent;
        public ToolStripButtonPrototype toolStripAdd;
        public ToolStripButtonPrototype toolStripEdit;
        public ToolStripButtonPrototype toolStripSave;
        public ToolStripButtonPrototype toolStripDelete;
        public Color WarningColor = Color.Yellow;
        public Color NormalColor = SystemColors.Window;
        public bool UseWarningImages = false;
        public bool UseRequiredHighlighting = true;
        public Control[] RequiredFields;
        public Control[] NonRequiredFields;

        private string s_add = "Создать";
        private string s_edit = "Изменить";
        private string s_save = "Сохранить";
        private string s_delete = "Удалить";

        public enum toolStripButtons
        {
            Add = 1, 
            Edit = 2, 
            Save = 4, 
            Delete = 8
        }

        /// <summary>
        /// Initializing toolStrip
        /// </summary>
        /// <param name="AddButtons">What buttons should be added?</param>
        public ConfigToolStrip(toolStripButtons selectedButtons)
        {
            if ((int)selectedButtons != 0)
            {
                char[] _pattern = Convert.ToString((int)selectedButtons, 2).ToCharArray();
                int index = 0;
                while (index < _pattern.Length)
                {
                    if (_pattern[index] == '1')
                    {
                        switch (index)
                        {
                            case 0:
                                toolStripAdd = new ToolStripButtonPrototype(s_add, Resources.toolbar_add);
                                toolStripAdd.Click += new EventHandler(add_Click);
                                //toolStrip.Items.Add(toolStripAdd);
                                break;
                            case 1:
                                toolStripEdit = new ToolStripButtonPrototype(s_edit, Resources.toolbar_edit);
                                toolStripEdit.Click += new EventHandler(edit_Click);
                                toolStripEdit.Enabled = false;
                                //toolStrip.Items.Add(toolStripEdit);
                                break;
                            case 2:
                                toolStripSave = new ToolStripButtonPrototype(s_save, Resources.toolbar_save);
                                //toolStripSave.Click += new EventHandler(save_Click);
                                toolStripSave.Enabled = false;
                                //toolStrip.Items.Add(toolStripSave);
                                break;
                            case 3:
                                toolStripDelete = new ToolStripButtonPrototype(s_delete, Resources.toolbar_delete);
                                toolStripDelete.Enabled = false;
                                //toolStrip.Items.Add(toolStripDelete);
                                break;
                        }
                    }
                    index++;
                }
            }
        }

        public void OpenExternalMethod()
        {
            Search(ref s_add, true);
            Search(ref s_edit, true);
            Search(ref s_save, false);
            Search(ref s_delete, true);
            ClearFields();
            DisableFields();
        }

        public void DeleteExternalMethod()
        {
            Search(ref s_add, true);
            Search(ref s_edit, false);
            Search(ref s_save, false);
            Search(ref s_delete, false);
            ClearFields();
            DisableFields();
        }

        public void SaveExternalMethod()
        {
            save_Click(this, null);
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (CheckRequired())
            {
                Search(ref s_add, true);
                Search(ref s_edit, false);
                Search(ref s_save, false);
                Search(ref s_delete, false);
                ClearFields();
                DisableFields();
            }
        }

        private void edit_Click(object sender, EventArgs e)
        {
            Search(ref s_add, false);
            Search(ref s_edit, false);
            Search(ref s_save, true);
            Search(ref s_delete, false);
            EnableFields();
        }

        private void add_Click(object sender, EventArgs e)
        {
            Search(ref s_add, false);
            Search(ref s_edit, false);
            Search(ref s_save, true);
            Search(ref s_delete, false);
            ClearFields();
            EnableFields();
        }

        private void Search(ref string key, bool enable)
        {
            switch(key)
            {
                case "Создать": toolStripAdd.Enabled = enable; break;
                case "Изменить": toolStripEdit.Enabled = enable; break;
                case "Сохранить": toolStripSave.Enabled = enable; break;
                case "Удалить": toolStripDelete.Enabled = enable; break;
            }
        }

        private bool CheckRequired()
        {
            if (RequiredFields != null)
            {
                foreach (Control c in RequiredFields)
                {
                    IEnumerator i = textboxesParent.Controls.GetEnumerator();
                    while (i.MoveNext())
                    {
                        Control co = (Control)i.Current;
                        if (co.Tag == c.Tag)
                        {
                            if (c.Text == "")
                            {
                                MarkErrors();
                                return false;
                            }
                        }
                    }
                }
                MarkErrors();
            }
            return true;
        }

        private void MarkErrors()
        {
            foreach (Control rC in RequiredFields)
            {
                IEnumerator i = textboxesParent.Controls.GetEnumerator();
                while (i.MoveNext())
                {
                    Control co = (Control)i.Current;
                    if (co.Name == rC.Name)
                    {
                        if (rC.Text == "")
                        {
                            ApplyHighlight(rC);
                            ApplyWarningImage(rC);
                        }
                        else
                        {
                            ClearHighlight(rC);
                            ClearWarningImage(rC);
                        }
                    }
                }
            }
        }

        private void ApplyHighlight(Control c)
        {
            if (UseRequiredHighlighting) c.BackColor = WarningColor;
        }

        private void ClearHighlight(Control c)
        {
            if (UseRequiredHighlighting) c.BackColor = NormalColor;
        }

        private void ApplyWarningImage(Control c)
        {
            if (UseWarningImages)
            {
                PictureBox w = new PictureBox();
                w.Image = Resources.error;
                w.Name = "WarningImage_" + c.Name;
                w.Location = new Point(c.Left + c.Width + 1, c.Top + ((c.Height - Resources.error.Height) / 2));
                c.Parent.Controls.Add(w);
            }
        }

        private void ClearWarningImage(Control c)
        {
            if (UseWarningImages)
            {
                foreach (Control c2 in c.Parent.Controls)
                {
                    if (c2.Name == "WarningImage_" + c.Name) c2.Dispose();
                }
            }
        }

        private void ClearFields()
        {
            if (RequiredFields != null)
            {
                foreach (Control c in RequiredFields)
                {
                    IEnumerator i = textboxesParent.Controls.GetEnumerator();
                    while (i.MoveNext())
                    {
                        Control co = (Control)i.Current;
                        if (co.Name == c.Name)
                        {
                            co.Text = "";
                            ClearHighlight(co);
                            ClearWarningImage(co);
                        }
                    }
                }
            }

            if (NonRequiredFields != null)
            {
                foreach (Control c in NonRequiredFields)
                {
                    IEnumerator i = textboxesParent.Controls.GetEnumerator();
                    while (i.MoveNext())
                    {
                        Control co = (Control)i.Current;
                        if (co.Name == c.Name)
                            co.Text = "";
                    }
                }
            }
        }

        private void EnableFields()
        {
            if (RequiredFields != null)
            {
                foreach (Control c in RequiredFields)
                {
                    IEnumerator i = textboxesParent.Controls.GetEnumerator();
                    while (i.MoveNext())
                    {
                        Control co = (Control)i.Current;
                        if (co.Name == c.Name)
                        {
                            co.Enabled = true;
                        }
                    }
                }
            }

            if (NonRequiredFields != null)
            {
                foreach (Control c in NonRequiredFields)
                {
                    IEnumerator i = textboxesParent.Controls.GetEnumerator();
                    while (i.MoveNext())
                    {
                        Control co = (Control)i.Current;
                        if (co.Name == c.Name)
                        {
                            co.Enabled = true;
                        }
                    }
                }
            }
        }

        private void DisableFields()
        {
            if (RequiredFields != null)
            {
                foreach (Control c in RequiredFields)
                {
                    IEnumerator i = textboxesParent.Controls.GetEnumerator();
                    while (i.MoveNext())
                    {
                        Control co = (Control)i.Current;
                        if (co.Name == c.Name)
                        {
                            co.Enabled = false;
                        }
                    }
                }
            }

            if (NonRequiredFields != null)
            {
                foreach (Control c in NonRequiredFields)
                {
                    IEnumerator i = textboxesParent.Controls.GetEnumerator();
                    while (i.MoveNext())
                    {
                        Control co = (Control)i.Current;
                        if (co.Name == c.Name)
                        {
                            co.Enabled = false;
                        }
                    }
                }
            }
        }
    }
}