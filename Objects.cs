using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace QuotePad
{
    public static class Objects
    {
        public class Theme
        {
            public int ID;
            public string Name;
        }

        public class Author
        {
            public int ID;
            public string FIO;
            public string About;
            public Image Photo;
        }

        public class Quote
        {
            public int ID;
            public Author QuoteAuthor;
            public Theme QuoteTheme;
            public string RTF;
            public bool IsFavorite;
            public DateTime WhenCreated;
        }
    }
}
