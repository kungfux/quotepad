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
        private string RuntimeFolder = Application.StartupPath + @"\runtime";
        private string SkinsFolder = Application.StartupPath + @"\skins";

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
                z.ExtractZip(SkinsFolder + @"\" + SkinName + ".zip", RuntimeFolder, ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite.Always,
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
                SkinInfo skin = xml.DeserializeClass<SkinInfo>(RuntimeFolder+@"\SkinInfo.xml");
                return skin;
            }
            else return null;
        }

        public void SaveSkin(SkinInfo skin)
        {
            ItWorks.XML xml = new ItWorks.XML();
            xml.SerializeClass<SkinInfo>(skin, RuntimeFolder+@"\SkinInfo.xml");
        }

        public void ApplySkin(string SkinName, Control Control)
        {
            SkinInfo skin = ReadSkin(SkinName);
            foreach (SkinInfo.ImagePanel panel in skin.ImagePanels)
            {
                Panel p = new Panel();
                p.Size = panel.Size;
                p.BackgroundImage = Image.FromFile(Application.StartupPath + @"\runtime\" + panel.BackImage);
                Control.Controls.Add(p);
            }
        }
    }

    public class SkinInfo
    {
        public ImagePanel[] ImagePanels;

        public class ImagePanel
        {
            public Size Size;
            public string BackImage;
        }
    }
}
