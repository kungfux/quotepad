using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        TabControlPrototype tb;
        ToolStripPrototype toolbar;
        mainMenu menu;

        private void mainForm_Load(object sender, EventArgs e)
        {
            tb = new TabControlPrototype();
            toolbar = new ToolStripPrototype();
            menu = new mainMenu(ref tb);
            tb.toolStrip = toolbar;

            tb.Dock = DockStyle.Fill;
            this.Controls.Add(tb);
            this.Controls.Add(toolbar);
            this.Controls.Add(menu);

        }

    }
}
