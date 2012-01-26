using System;
using System.Collections.Generic;
using System.Linq;
using QuotePad.Model;
using PetaPoco;
using System.Data.SQLite;

namespace QuotePad.Data
{
    public class DataProvider : IDataProvider
    {
        public const string ConnectionString = @"Data Source=Quotes.s3db;Pooling=true;FailIfMissing=true";
        private readonly Database _database;

        public DataProvider()
        {
            _database = new Database(ConnectionString,new SQLiteFactory());
        }

        #region Author Methods
        private IList<Author> _authors;

        public IList<Author> GetAuthors()
        {
            if (_authors == null)
            {
                _authors = _database.Fetch<Author>("select * from authors");
            }
            return _authors.OrderBy(t => t.Name).ToList();
        }

        public bool SaveAuthor(Author author)
        {
            try
            {
                if (author.Id <= 0)
                {
                    _database.Insert(author);
                }
                else
                {
                    _database.Update(author);
                    var oldAuthor = _authors.FirstOrDefault(t => t.Id == author.Id);
                    if (oldAuthor != null) _authors.Remove(oldAuthor);
                }
                _authors.Add(author);
                return true;
            }
            catch (Exception)
            {
                _authors = null;
                return false;
            }
        }

        public bool RemoveAuthor(Author author)
        {
            try
            {
                _database.Delete(author);
                _authors.Remove(author);
                return true;
            }
            catch (Exception)
            {
                _authors = null;
                return false;
            }
        } 
        #endregion

        #region Theme Methods
        private IList<Theme> _themes;

        public IList<Theme> GetThemes()
        {
            if (_themes == null)
            {
                _themes = _database.Fetch<Theme>("select * from themes");
            }
            return _themes.OrderBy(t => t.Name).ToList();
        }

        public bool SaveTheme(Theme theme)
        {
            try
            {
                if (theme.Id <= 0)
                {
                    _database.Insert(theme);
                }
                else
                {
                    _database.Update(theme);
                    var oldTheme = _themes.FirstOrDefault(t => t.Id == theme.Id);
                    if (oldTheme != null) _themes.Remove(oldTheme);
                }
                _themes.Add(theme);
                return true;
            }
            catch (Exception)
            {
                _themes = null;
                return false;
            }
        }

        public bool RemoveTheme(Theme theme)
        {
            try
            {
                _database.Delete(theme);
                _themes.Remove(theme);
                return true;
            }
            catch (Exception)
            {
                _themes = null;
                return false;
            }
        }
        #endregion
    }
}
