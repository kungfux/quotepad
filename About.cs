using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace QuotePad
{
    public static class About
    {
        public static string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        public static string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
