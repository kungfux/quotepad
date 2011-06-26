using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace QuotePad
{
    public class Database
    {
        public readonly ItWorks.OleDb connector = new ItWorks.OleDb();

        public bool Connect()
        {
            if (connector.TestConnection("Provider=Microsoft.Jet.OleDb.4.0;Data Source=db.mdb;", true, false))
                return true;
            else return false;
        }

        public void InitDb()
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        #region Theme

        public Objects.Theme[] Theme_GetList()
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Theme_Create(string Theme)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Theme_Modify(int ThemeID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Theme_Remove(int ThemeID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        #endregion

        #region Author

        public Objects.Author[] Author_GetList()
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public Objects.Author Author_Get(int AuthorID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public string Author_GetName(int AuthorID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Author_Create(string FIO, string About)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Author_Modify(int AuthorID, string newFIO, string newAbout)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Author_SetImage(int AuthorID, Image Photo)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Author_ClearImage(int AuthorID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }
        #endregion

        #region Quote

        public int Quote_GetCount()
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Quote_Create(int AuthorID, int ThemeID, string RTFQuote, string TXTQuote, bool IsFavorite)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Quote_Modify(int QuoteID, int newAuthorID, int newThemeID, string newRTFQuote, string newTXTQuote, bool IsFavorite)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Quote_Remove(int QuoteID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Quote_SetAsFavorite(int QuoteID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public bool Quote_UnsetFavorite(int QuoteID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public Objects.Quote Quote_ReadNext(int QuoteID, bool FavoriteOnly)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public Objects.Quote Quote_ReadPrevious(int QuoteID, bool FavoriteOnly)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public Objects.Quote Quote_RandomRead(bool FavoriteOnly)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public Objects.Quote[] Quote_FindByID(int QuoteID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public Objects.Quote[] Quote_FindByAuthor(int AuthorID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public Objects.Quote[] Quote_FindByTheme(int ThemeID)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public Objects.Quote[] Quote_FindByText(string Text)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public Objects.Quote[] Quote_FindByDate(DateTime From, DateTime To)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }
        #endregion
    }
}
