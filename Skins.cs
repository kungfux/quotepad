using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace QuotePad
{
    public class Skins
    {
        public string[] SkinsAvailable;
        private string RuntimeFolder = Application.StartupPath + @"\runtime\";
        private string SkinsFolder = Application.StartupPath + @"\skins\";
        private List<SkinInfo.EventImage.cLeft> EvL = new List<SkinInfo.EventImage.cLeft>();
        private List<SkinInfo.EventImage.cTop> EvT = new List<SkinInfo.EventImage.cTop>();


        public Skins()
        {
            DirectoryInfo skins = new DirectoryInfo(SkinsFolder);
            try
            {
                SkinsAvailable = new string[skins.GetFiles("*.zip").Length];
                int skinNumber = 0;
                foreach (FileInfo skin in skins.GetFiles("*.zip"))
                {
                    SkinsAvailable[skinNumber] = skin.Name;
                    skinNumber++;
                }
            }
            catch (DirectoryNotFoundException) { }
        }

        private bool PrepareSkin(string SkinName)
        {
            if (!Directory.Exists(RuntimeFolder))
            {
                try
                {
                    Directory.CreateDirectory(RuntimeFolder);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            ICSharpCode.SharpZipLib.Zip.FastZip z = new ICSharpCode.SharpZipLib.Zip.FastZip();

            try
            {
                z.ExtractZip(SkinsFolder + SkinName + ".zip", RuntimeFolder, ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite.Always,
                     null, null, null, false);
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            return true;
        }

        private SkinInfo ReadSkin(string SkinName)
        {
            if (PrepareSkin(SkinName))
            {
                ItWorks.XML xml = new ItWorks.XML();
                SkinInfo skin = xml.DeserializeClass<SkinInfo>(RuntimeFolder + "SkinInfo.xml");
                return skin;
            }
            else return null;
        }

        public void ApplySkin(string SkinName, Control Control)
        {
            SkinInfo skin = ReadSkin(SkinName);
            EvL.Clear();
            EvT.Clear();
            if (skin != null)
            {
                if (skin.BackImages != null)
                {
                    foreach (SkinInfo.BackImage image in skin.BackImages)
                    {
                        if (File.Exists(RuntimeFolder + image.Image))
                        {
                            PictureBox p = new PictureBox();
                            p.Location = image.Location;
                            p.Size = image.Size;
                            p.Dock = image.DockStyle;
                            p.BackgroundImage = Image.FromFile(RuntimeFolder + image.Image);
                            p.BackgroundImageLayout = image.ImageLayout;
                            Control.Controls.Add(p);
                            p.BringToFront();
                        }
                        else DisplayError(image.Image);
                    }
                }

                if (skin.EventImages != null)
                {
                    foreach (SkinInfo.EventImage ev in skin.EventImages)
                    {
                        if (File.Exists(RuntimeFolder + ev.Image))
                        {
                            PictureBox p = new PictureBox();
                            p.Name = EvL.Count.ToString();
                            p.Image = Image.FromFile(RuntimeFolder + ev.Image);
                            EvL.Add(ev.Left);
                            EvT.Add(ev.Top);
                            p.Size = ev.Size;
                            Control.Resize += new EventHandler(c_Resize);
                            Control.Controls.Add(p);
                            p.BringToFront();
                            c_Resize(Control, null);
                        }
                        else DisplayError(ev.Image);
                    }
                }

                Control.MinimumSize = skin.MinimalSkinSize;
            }
            else DisplayError("");
        }

        private void DisplayError(string text)
        {
            if (text != "") text = "\r\nФайл " + text + " не найден!";
            MessageBox.Show("Ошибка загрузки обложки!" + text,
                About.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void c_Resize(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                Control c = (Control)sender;
                foreach (PictureBox p in c.Controls)
                {
                    if (p.Name != "")
                    {
                        int pi = Convert.ToInt16(p.Name);
                        if (EvL[pi].Calculate)
                        {
                            p.Left = c.ClientSize.Width * EvL[pi].Persents / 100;
                            switch (EvL[pi].SelfMinus)
                            {
                                case SkinInfo.EventImage.CalculateSize.Full:
                                    p.Left -= p.Width;
                                    break;
                                case SkinInfo.EventImage.CalculateSize.Half:
                                    p.Left -= p.Width / 2;
                                    break;
                            }
                        }
                        if (EvT[pi].Calculate)
                        {
                            p.Top = c.ClientSize.Height * EvT[pi].Persents / 100;
                            switch (EvT[pi].SelfMinus)
                            {
                                case SkinInfo.EventImage.CalculateSize.Full:
                                    p.Top -= p.Height;
                                    break;
                                case SkinInfo.EventImage.CalculateSize.Half:
                                    p.Top -= p.Height / 2;
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Skin Information structure
    /// </summary>
    public class SkinInfo
    {
        public BackImage[] BackImages;
        public EventImage[] EventImages;
        public Size MinimalSkinSize;

        /// <summary>
        /// PictureBox
        /// </summary>
        public class BackImage
        {
            public DockStyle DockStyle;
            public Point Location;
            public Size Size;
            public string Image;
            public ImageLayout ImageLayout;
        }

        public class EventImage
        {
            public Size Size;
            public string Image;
            public cTop Top = new cTop();
            public cLeft Left = new cLeft();

            public class cTop
            {
                public bool Calculate = false;
                public CalculateSize SelfMinus = CalculateSize.None;
                public int Persents = 0;
            }

            public class cLeft
            {
                public bool Calculate = false;
                public CalculateSize SelfMinus = CalculateSize.None;
                public int Persents = 0;
            }

            public enum CalculateSize { None, Full, Half }

        }
    }
}
