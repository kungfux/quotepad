using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ItWorksTeam.UI;
using System.Diagnostics;

namespace QuotePad
{
    public class pageGreetings : TabPagePrototype
    {
        public pageGreetings()
        {
            RTFEditor textbox = new RTFEditor();
            textbox.RtfTextBox.ReadOnly = true;
            textbox.RtfTextBox.Dock = DockStyle.Fill;
            textbox.RtfTextBox.BorderStyle = BorderStyle.None;
            textbox.RtfTextBox.LinkClicked += new LinkClickedEventHandler(RtfTextBox_LinkClicked);

            this.Text = "Приветствие";
            this.captionText = "Приветствие";

            textbox.RtfTextBox.Rtf = Resources.Greetings;

            this.Controls.Add(textbox.RtfTextBox);
        }

        void RtfTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception)
            {
            }
        }
    }
}
