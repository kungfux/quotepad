using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace QuotePad
{
    public class AboutBox : Form
    {
        assembly a = new assembly();
        PictureBox pic = new PictureBox();
        ExtendedComponents.Scroller scroll = new ExtendedComponents.Scroller();
        System.Media.SoundPlayer snd;

        public AboutBox()
        {
            this.Text = "О программе";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(400, 400);

            scroll.Dock = DockStyle.Top;
            scroll.BackColor = Color.Black;
            scroll.ForeColor = Color.White;
            scroll.TextToScroll = string.Concat(
                " \n \n \n",
                a.AssemblyProduct, "\n",
                a.AssemblyVersion, "\n\n",
                "Дизайн и разработка:", "\n",
                a.AssemblyCompany, "\n\n",
                "Copyright:", "\n",
                a.AssemblyCopyright, "\n\n",
                "Заказчик:", "\n",
                "Стрелкова Татьяна", "\n\n",
                "Менеджер проекта:", "\n",
                "Фукс Александр", "\n\n",
                "Программист:", "\n",
                "Фукс Александр", "\n\n",
                "Огромная благодарность:", "\n",
                "Стрелкова Татьяна", "\n",
                "Малицкий Андрей", "\n",
                "Вельма Антон", "\n",
                "Сержантов Сергей", "\n",
                "Капшук Александр", "\n",
                "Малицкий Антон", "\n\n\n",
                "Программа распрастраняется под\nлицензией GNU/GPL\nПользуйтесь все!", "\n\n",
                "Отзывы и пожелания\nпишите по адресу:\nkungfux2010@gmail.com", "\n\n",
                "Приятного пользования!");

            //pic.Image = Resources.logo;
            //pic.SizeMode = PictureBoxSizeMode.AutoSize;
            //pic.Left = (this.Width / 2) - (pic.Width / 2);

            //TableLayoutPanel tp = new TableLayoutPanel();
            //tp.Dock = DockStyle.Fill;
            //tp.ColumnCount = 1;
            //tp.RowCount = 2;
            //tp.RowStyles.Add(new RowStyle(SizeType.Absolute, pic.Height));
            //tp.RowStyles.Add(new RowStyle(SizeType.Absolute, this.ClientSize.Height - pic.Height));
            //tp.Controls.Add(pic, 0, 0);
            //tp.Controls.Add(scroll, 0, 1);

            this.Shown += new EventHandler(AboutBox_Shown);
            this.FormClosing += new FormClosingEventHandler(AboutBox_FormClosing);
            this.Controls.Add(scroll);
        }

        void AboutBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            snd.Stop();
        }

        void AboutBox_Shown(object sender, EventArgs e)
        {
            snd = new System.Media.SoundPlayer();
            snd.Stream = Resources.starwars;
            snd.Play();
            scroll.Start();
        }
    }
}
