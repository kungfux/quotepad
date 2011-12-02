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

        /// <summary>
        /// Init new instance of TabControl
        /// </summary>
        public TabControlPrototype()
        {
            this.ItemSize = new Size(this.ItemSize.Width, 20);
            this.Font = new Font(this.Font.FontFamily, 10);
            this.MouseUp += new MouseEventHandler(tabControl_MouseUp);
            this.Selected += new TabControlEventHandler(TabControlPrototype_Selected);
        }

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

        /// <summary>
        /// Set caption for main form
        /// </summary>
        /// <param name="text"></param>
        public void UpdateCaption(string text = "")
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
}
