﻿using System;
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

        TabControlPrototype tabControl;
        ToolStripPrototype toolBar;
        mainMenu mainMenu;

        private void mainForm_Load(object sender, EventArgs e)
        {
            tabControl = new TabControlPrototype();
            toolBar = new ToolStripPrototype(tabControl);
            mainMenu = new mainMenu(tabControl);
            tabControl.toolStrip = toolBar;

            tabControl.Dock = DockStyle.Fill;
            this.Controls.Add(tabControl);
            this.Controls.Add(toolBar);
            this.Controls.Add(mainMenu);

        }

    }
}
