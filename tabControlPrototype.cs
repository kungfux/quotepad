using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace QuotePad
{
    /// <summary>
    /// Prototype of extended TabControl
    /// 
    /// List of definitions:
    /// 1. Only TabPagePrototypes can be added as TabPages to TabControlPrototype;
    /// 2. TabPage which is going to be added should has empty Tag;
    /// 
    /// </summary>
    public class TabControlPrototype : TabControl
    {
        public ToolStripPrototype toolStrip; // Associated ToolStrip element
        private List<TabPagePrototype> tabPages = new List<TabPagePrototype>(); // List of opened TabPages
        private int sequenceNumber = 1; // Unique sequence number of TabPage
        //private Image closeImage = Resources.close;

        /// <summary>
        /// Init new instance of TabControl
        /// </summary>
        public TabControlPrototype()
        {
            this.ItemSize = new Size(this.ItemSize.Width, 20);
            this.Font = new Font(this.Font.FontFamily, 10);
            this.MouseUp += new MouseEventHandler(tabControl_MouseUp);
            this.Selected += new TabControlEventHandler(TabControlPrototype_Selected);
            // Close tab func.
            //this.DrawMode = TabDrawMode.OwnerDrawFixed;
            //this.DrawItem += new DrawItemEventHandler(TabControlPrototype_DrawItem);
            //this.MouseClick += new MouseEventHandler(TabControlPrototype_MouseClick);
            //this.MouseMove += new MouseEventHandler(TabControlPrototype_MouseMove);
        }

        //void TabControlPrototype_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Point p = e.Location;
        //    for (int i = 0; i < TabCount; i++)
        //    {
        //        Rectangle r = GetTabRect(i);
        //        r.Offset(2, 2);
        //        r = new Rectangle(r.X + r.Width - closeImage.Width - 4,
        //            r.Top + ((r.Height - r.Top) / 2 - closeImage.Height / 2),
        //            closeImage.Width, 
        //            closeImage.Height);
        //        if (r.Contains(p))
        //        {
        //            closeImage = Resources.closeActive;
        //            this.Refresh();
        //        }
        //        else
        //        {
        //            closeImage = Resources.close;
        //            this.Refresh();
        //        }
        //    }
        //}

        //void TabControlPrototype_MouseClick(object sender, MouseEventArgs e)
        //{
        //    Point p = e.Location;
        //    for (int i = 0; i < TabCount; i++)
        //    {
        //        Rectangle r = GetTabRect(i);
        //        r.Offset(2, 2);
        //        r = new Rectangle(r.X + r.Width - closeImage.Width - 4,
        //            r.Top + ((r.Height - r.Top) / 2 - closeImage.Height / 2),
        //            closeImage.Width, 
        //            closeImage.Height);
        //        if (r.Contains(p))
        //        {
        //            tabControl_MouseUp(this, new MouseEventArgs(MouseButtons.Right, 1, p.X, p.Y, 0));
        //        }
        //    }
        //}

        //void TabControlPrototype_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    Rectangle r = e.Bounds;
        //    r = GetTabRect(e.Index);
        //    r.Offset(2, 2);
        //    Pen p = new Pen(Brushes.Red);
        //    e.Graphics.DrawImage(closeImage,
        //        r.X + r.Width - closeImage.Width - 4,
        //        r.Top + ((r.Height - r.Top) / 2 - closeImage.Height / 2),
        //        closeImage.Width, 
        //        closeImage.Height);
        //    e.Graphics.DrawString(this.TabPages[e.Index].Text, this.Font, 
        //        SystemBrushes.ControlText, new PointF(r.X + 5, r.Y));

        //}

        /// <summary>
        /// Re-fill associated ToolStrip with associated buttons when some TabPage is selected
        /// </summary>
        private void TabControlPrototype_Selected(object sender, EventArgs e)
        {
            if (toolStrip != null)
            {
                RemoveRedundantItems();
                if (this.TabPages.Count > 0 && this.SelectedTab != null)
                {
                    ToolStripSeparator separator = new ToolStripSeparator();
                    separator.Tag = "separator";
                    toolStrip.Items.Add(separator);
                    // Add items on toolstrip for selected tab
                    TabPagePrototype page = tabPages.Find(p => p.Tag == this.SelectedTab.Tag);

                    foreach (ToolStripButtonPrototype item in page.toolStripItemsB)
                    {
                        toolStrip.Items.Add(item);
                    }
                    foreach (ToolStripSeparatorPrototype item in page.toolStripItemsS)
                    {
                        toolStrip.Items.Add(item);
                    }
                    foreach (ToolStripLabelPrototype item in page.toolStripItemsL)
                    {
                        toolStrip.Items.Add(item);
                    }
                    
                    foreach (ToolStripComboBoxPrototype item in page.toolStripItemsC)
                    {
                        toolStrip.Items.Add(item);
                    }
                    UpdateCaption();
                }
            }
        }

        /// <summary>
        /// Remove all elements from ToolStrip which are marked as associated
        /// </summary>
        private void RemoveRedundantItems()
        {
            bool done = false;
            while (!done)
            {
                for (int a = 0; a < toolStrip.Items.Count; a++)
                {
                    if (toolStrip.Items[a].Tag != null)
                    {
                        toolStrip.Items.RemoveAt(a);
                        break;
                    }
                    if (a == toolStrip.Items.Count - 1)
                    {
                        done = true;
                    }
                }
            }
        }

        /// <summary>
        /// Add new TabPage (to List and to TabControl)
        /// </summary>
        public void AddPage(TabPagePrototype Page)
        {
            if (Page.Tag != null) throw new Exception("This page do not fit for internal logic.");
            Page.Tag = sequenceNumber++.ToString(); // Define unique number of page
            tabPages.Add(Page);
            this.TabPages.Add(tabPages[tabPages.Count - 1]);
            //this.SelectedIndex = tabPages.Count - 1;
            this.SelectedIndex = this.TabPages.Count - 1;
            TabControlPrototype_Selected(this, null);
        }

        /// <summary>
        /// Realize closing functionality by pressing right mouse button on TabPage
        /// </summary>
        private void tabControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < this.TabCount; i++)
                {
                    Rectangle r = this.GetTabRect(i);
                    if (r.Contains(e.Location))
                    {
                        tabPages.Find(p => p.Tag == this.TabPages[i].Tag).Destroy();
                        if (this.SelectedTab == null)
                        {
                            this.Parent.Text = new assembly().AssemblyProduct;
                        }
                        break;
                    }
                }
            }
        }

        public void CloseActiveTab()
        {
            if (SelectedIndex != -1)
            {
                tabPages.Find(p => p.Tag == this.TabPages[SelectedIndex].Tag).Destroy();
                if (this.SelectedTab == null)
                {
                    this.Parent.Text = new assembly().AssemblyProduct;
                }
            }
        }

        /// <summary>
        /// Set caption for main form
        /// </summary>
        /// <param name="text"></param>
        public void UpdateCaption(string text = "")
        {
            if (this.SelectedTab != null)
            {
                if (text == "")
                {
                    if (tabPages.Find(p => p.Tag == this.SelectedTab.Tag).captionText != "")
                    {
                        this.Parent.Text = new assembly().AssemblyProduct + " - " + tabPages.Find(p => p.Tag == this.SelectedTab.Tag).captionText;
                    }
                    else
                    {
                        this.Parent.Text = new assembly().AssemblyProduct;
                    }
                }
                else
                {
                    this.Parent.Text = new assembly().AssemblyProduct + " - " + text;
                }
            }
        }

        /// <summary>
        /// This methods close page by page and provide answer is all tabs are closed
        /// </summary>
        public bool CloseAllTabs()
        {
            int a = this.TabPages.Count;
            while (a > 0)
            {
                tabPages.Find(p => p.Tag == this.TabPages[0].Tag).Destroy();
                if (a == this.TabPages.Count)
                {
                    return false;
                }
                else
                {
                    a = this.TabPages.Count;
                }
                
            }
            return true;
        }
    }
}
