using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuotePad.Model;

namespace QuotePad.Data
{
    public interface IDataProvider
    {
        IList<Author> GetAuthors();
        bool SaveAuthor(Author author);
        bool RemoveAuthor(Author author);

        IList<Theme> GetThemes();
        bool SaveTheme(Theme theme);
        bool RemoveTheme(Theme theme);
    }
}
