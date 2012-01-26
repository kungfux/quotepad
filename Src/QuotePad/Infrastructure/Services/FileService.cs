using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace QuotePad.Infrastructure.Services
{
    public class FileService : IFileService
    {
        public bool SelectFile(out string selectedFilePath, string filter = null, string initialDirectory = null)
        {
            selectedFilePath = null;
            var openFileDialog = new OpenFileDialog
                                     {
                                         CheckFileExists = true,
                                         CheckPathExists = true,
                                         Filter = filter,
                                         InitialDirectory = initialDirectory
                                     };
            if (openFileDialog.ShowDialog() == true)
            {
                selectedFilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }
    }
}
