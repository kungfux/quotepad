using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuotePad.Infrastructure.Services
{
    public interface IFileService
    {
        bool SelectFile(out string selectedFilePath, string filter = null, string initialDirectory = null);
    }
}
