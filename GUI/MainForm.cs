using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using QuotePad.Properties;

namespace QuotePad.GUI
{
    public partial class MainForm: Form
    {
        bool isFirst = false;
        Point prevLeftClick;
        bool toBlock = false;
        ClientParams cParams = new ClientParams();
        const int cGrip = 16;
        const int cCaption = 0;
        Panel pWorkingArea;
        Panel pHead;

        public MainForm()
        {
            this.Text = "Цитатник";
            this.Icon = Resources._1297098121_notebook_boy;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            this.BackColor = Color.FromName(cParams.clientParams.HeaderColor);
            this.Size = cParams.clientParams.StartUpSize;
            this.MinimumSize = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            GraphicsPath gPath = new GraphicsPath();
            Bitmap bitmap = Bitmap.FromHbitmap(Resources.pero.GetHbitmap());
            Graphics grapthics = Graphics.FromImage(Resources.pero);
            gPath = CalculateControlGraphicsPath(bitmap);
            gPath.AddRectangle(new Rectangle(30, 140, 1920, 1080));
            gPath.FillMode = FillMode.Winding;
            Region region = new Region(gPath);
            this.Region = region;
            PictureBox picture = new PictureBox();
            picture.Image = Resources.pero;
            picture.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(picture);
            this.MouseMove += new MouseEventHandler(fForm_MouseMove);
            this.Resize += new EventHandler(MainForm_Resize);

            // Header
            pHead = ToolBox();
            this.Controls.Add(pHead);

            // Working Area
            pWorkingArea = new Panel();
            pWorkingArea.BackColor = Color.FromName(cParams.clientParams.WorkingAreaColor);
            pWorkingArea.Location = new Point(30, picture.Top + picture.Height);

            /*Font fButtons = new Font(FontFamily.GenericSansSerif, 14);
            Button bQoutes = new Button();
            bQoutes.Text = "Цитаты";
            bQoutes.Font = fButtons;
            bQoutes.BackColor = Color.Silver;
            bQoutes.Size = new Size(150,60);
            bQoutes.Location = new Point(0, 0);

            Button bAuthors = new Button();
            bAuthors.Font = fButtons;
            bAuthors.Text = "Авторы";
            bAuthors.BackColor = Color.Silver;
            bAuthors.Size = new Size(150, 60);
            bAuthors.Location = new Point(0, 60);

            Button bThemes = new Button();
            bThemes.Font = fButtons;
            bThemes.Text = "Тематики";
            bThemes.BackColor = Color.Silver;
            bThemes.Size = new Size(150, 60);
            bThemes.Location = new Point(0, 120);

            Button bSearch = new Button();
            bSearch.Font = fButtons;
            bSearch.Text = "Поиск";
            bSearch.BackColor = Color.Silver;
            bSearch.Size = new Size(150, 60);
            bSearch.Location = new Point(0, 180);

            RichTextBox rtf = new RichTextBox();
            rtf.Location = new Point(200, 0);
            rtf.Size = new Size(300, 300);
            rtf.BorderStyle = BorderStyle.None;
            
            //rtf.BackColor = Color.FromName(cParams.clientParams.HeaderColor);

            pWorkingArea.Controls.AddRange(new Control[] { bQoutes, bAuthors, bThemes, bSearch,rtf });*/
            ItWorks.RTFed editor = new ItWorks.RTFed();
            editor.RtfTextBox.Dock = DockStyle.Fill;
            pWorkingArea.Controls.Add(editor.RtfTextBox);
            pWorkingArea.Controls.Add(editor.RtfToolBar);
            this.Controls.Add(pWorkingArea);

            this.Load +=new EventHandler(MainForm_Load);
        }

        Panel ToolBox()
        {
            Panel panel = new Panel();
            panel.BackColor = Color.FromName(cParams.clientParams.HeaderColor);
            panel.Size = new Size(100, 27);

            Button[] btns = new Button[4];

            for (int a = 0; a < btns.Length; a++)
            {
                btns[a] = new Button();
                btns[a].Size = new Size(25, 25);
                btns[a].Location = new Point(25*a, 0);
                btns[a].FlatStyle = FlatStyle.Flat;
            }

            btns[0].Name = "pLogin";
            btns[1].Name = "pMimimaze";
            btns[2].Name = "pMaximaze";
            btns[3].Name = "pClose";
            btns[0].Image = Resources._1307193904_log_in;
            btns[1].Image = Resources._1307191900_Minimize_Square;
            btns[2].Image = Resources._1307191884_full_screen_16;
            btns[3].Image = Resources._1307191865_dialog_close;

            btns[1].Click += new EventHandler(bMinimize_Click);
            btns[2].Click += new EventHandler(bMaximaze_Click);
            btns[3].Click += new EventHandler(bClose_Click);

            panel.Controls.AddRange(btns);
            return panel;
        }

        void bMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        void bMaximaze_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                Button b = (Button)sender;
                b.Image = Resources._1307194145_minimize;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                Button b = (Button)sender;
                b.Image = Resources._1307191884_full_screen_16;
                this.WindowState = FormWindowState.Normal;
            }
        }

        void bClose_Click(object sender, EventArgs e)
        {
            cParams.Save();
            this.Close();
        }

        void MainForm_Resize(object sender, EventArgs e)
        {
            pWorkingArea.Size = new Size(this.ClientSize.Width-30,
                this.ClientSize.Height - pWorkingArea.Location.Y - cGrip);
            pHead.Location = new Point(this.ClientSize.Width - pHead.Width, 140);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, 32);
            e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            Color colorTransparent = bitmap.GetPixel(0, 0);
            int colOpaquePixel = 0;
            for (int row = 0; row < bitmap.Height; row++)
            {
                colOpaquePixel = 0;
                for (int col = 0; col < bitmap.Width; col++)
                {
                    if (bitmap.GetPixel(col, row) != colorTransparent)
                    {
                        colOpaquePixel = col;
                        int colNext = col;
                        for (colNext = colOpaquePixel; colNext < bitmap.Width; colNext++)
                            if (bitmap.GetPixel(colNext, row) == colorTransparent)
                                break;

                        graphicsPath.AddRectangle(new Rectangle(colOpaquePixel,
                                                   row, colNext - colOpaquePixel, 1));

                        col = colNext;
                    }
                }
            }
            return graphicsPath;
        }

        private void fForm_MouseMove(object sender,
                                        System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isFirst == true)
                {
                    prevLeftClick = new Point(e.X, e.Y);
                    isFirst = false;
                }
                else
                {
                    if (toBlock == false)
                        this.Location = new Point(this.Location.X + e.X -
                            prevLeftClick.X, this.Location.Y + e.Y - prevLeftClick.Y);

                    prevLeftClick = new Point(e.X, e.Y);
                    toBlock = !toBlock;
                }
            }
            else
                isFirst = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Database db = new Database();
            if (!db.Connect()) MessageBox.Show("not connected");
            db.Theme_GetList();
        }
    }
}
