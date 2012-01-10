using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        TabControlPrototype tabControl;
        ToolStripPrototype toolBar;
        mainMenu mainMenu;

        private void mainForm_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(mainForm_FormClosing);
            // set form's caption
            this.Text = new assembly().AssemblyProduct;
            // check if default password is set
            new Authorization().AutoAuthorization();

            tabControl = new TabControlPrototype();
            mainMenu = new mainMenu(tabControl);
            toolBar = new ToolStripPrototype(tabControl, mainMenu);
            tabControl.toolStrip = toolBar;

            tabControl.Dock = DockStyle.Fill;
            this.Controls.Add(tabControl);
            this.Controls.Add(toolBar);
            this.Controls.Add(mainMenu);
        }

        void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!tabControl.CloseAllTabs())
            {
                e.Cancel = true;
            }
        }

        private void mainForm_Shown(object sender, EventArgs e)
        {
            if (Database.Quote_GetCount() > 10)
            {
                tabControl.AddPage(new pageQuoteView(tabControl));
            }
            else
            {
                tabControl.AddPage(new pageGreetings());
            }
        }

    }
}
