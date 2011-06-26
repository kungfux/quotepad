using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Data.OleDb;

namespace QuotePad
{
    public class Database
    {
        // Instance of the OleDbQuery class
        public ItWorks.OleDb connector = new ItWorks.OleDb();

        public bool Connect()
        {
            // Connect to the database
            if (connector.TestConnection("Provider=Microsoft.Jet.OleDb.4.0;Data Source=db.mdb;", true, false))
            {
                connector.SetTrace(true, Application.StartupPath + @"\dberr.log", ItWorks.OleDb.TraceLevel.QueryWithMessage);
                InitDb();
                return true;
            }
            else return false;
        }

        public void InitDb()
        {
            connector.SetTrace(false); ;

            // Create tables and set trace enabled for queries
            string tAuthor =
                "CREATE TABLE tAUTHORS(" +
                "pID COUNTER CONSTRAINT pkTYPE PRIMARY KEY," +
                "pNAME CHAR(50) NOT NULL," +
                "pINFO MEMO," +
                "pPHOTO IMAGE);";
            string tThemes =
                "CREATE TABLE tTHEMES (" +
                "pID COUNTER CONSTRAINT pkTYPE PRIMARY KEY," +
                "pNAME TEXT(15) NOT NULL)";
            string tQuotes =
                "CREATE TABLE tQUOTES (" +
                "pID COUNTER PRIMARY KEY," +
                "pAUTHOR INT NOT NULL CONSTRAINT fktAUTHORS REFERENCES tAUTHORS(pID)," +
                "THEME INT NOT NULL CONSTRAINT fktTHEMES REFERENCES tTHEMES(pID)," +
                "prtfQUOTE MEMO NOT NULL," +
                "ptxtQUOTE MEMO NOT NULL," +
                "pFAVORITE BIT NOT NULL DEFAULT -1," +
                "pDT DATETIME NOT NULL);";

            connector.ChangeData(tAuthor);
            connector.ChangeData(tThemes);
            connector.ChangeData(tQuotes);

            connector.SetTrace(true);
        }

        public string GetError()
        {
            return connector.LastErrorMessage;
        }
        #region Theme

        public Objects.Theme[] Theme_GetList()
        {
            // Read all themes
            DataTable themes = connector.SelectTable("SELECT * FROM tTHEMES");
            if (themes == null) return new Objects.Theme[0];
            Objects.Theme[] list = new Objects.Theme[themes.Rows.Count];
            for (int theme=0;theme<themes.Rows.Count;theme++)
            {
                list[theme] = new Objects.Theme();
                list[theme].ID = (int)themes.Rows[theme].ItemArray[0];
                list[theme].Name = (string)themes.Rows[theme].ItemArray[1];
            }
            return list;
        }

        public string Theme_GetName(int ThemeID)
        {
            return connector.SelectCell<string>("SELECT pNAME FROM tTHEMES WHERE pID = @id",
                new OleDbParameter("@id", ThemeID));
        }

        public bool Theme_Create(string Theme)
        {
            if (connector.ChangeData("INSERT INTO tTHEMES (pNAME) VALUES (@name)", 
                new OleDbParameter("@name", Theme)) >= 0) return true;
            else return false;
        }

        public bool Theme_Modify(int ThemeID, string newName)
        {
            if (connector.ChangeData("UPDATE tTHEMES SET pNAME=@name WHERE pID = @id",
                new OleDbParameter("@name", newName),
                new OleDbParameter("@id", ThemeID)) >= 0) return true;
            else return false;
        }

        public bool Theme_Remove(int ThemeID)
        {
            if (connector.ChangeData("DELETE FROM tTHEMES WHERE pID = @id",
                new OleDbParameter("@id", ThemeID)) >= 0) return true;
            else return false;
        }

        #endregion

        #region Author

        public Objects.Author[] Author_GetList()
        {
            // Read all authors
            DataTable themes = connector.SelectTable("SELECT * FROM tAUTHORS");
            if (themes == null) return new Objects.Author[0];
            Objects.Author[] list = new Objects.Author[themes.Rows.Count];
            for (int theme = 0; theme < themes.Rows.Count; theme++)
            {
                list[theme] = new Objects.Author();
                list[theme].ID = (int)themes.Rows[theme].ItemArray[0];
                list[theme].FIO = (string)themes.Rows[theme].ItemArray[1];
                list[theme].About = (string)themes.Rows[theme].ItemArray[2];
                list[theme].Photo =
                    connector.GetImage("SELECT pPHOTO FROM tAUTHORS WHERE pID = " + 
                    themes.Rows[theme].ItemArray[0]);
            }
            return list;
        }

        public Objects.Author Author_Get(int AuthorID)
        {
            DataRow author = connector.SelectRow("SELECT * FROM tAUTHORS WHERE pID = @id",
                new OleDbParameter("@id", AuthorID));
            if (author == null) return new Objects.Author();
            Objects.Author item = new Objects.Author();
            item.ID = (int)author.ItemArray[0];
            item.FIO = (string)author.ItemArray[1];
            item.About = (string)author.ItemArray[2];
            item.Photo = connector.GetImage("SELECT pPHOTO FROM tAUTHORS WHERE pID = " +
                    AuthorID);
            return item;
        }

        public string Author_GetName(int AuthorID)
        {
            return connector.SelectCell<string>("SELECT pNAME FROM tAUTHORS WHERE pID = @id",
                new OleDbParameter("@id", AuthorID));
        }

        public bool Author_Create(string FIO, string About)
        {
            if (connector.ChangeData("INSERT INTO tAUTHORS (pNAME, pINFO) VALUES (@name, @info)",
                new OleDbParameter("@name", FIO),
                new OleDbParameter("@info", About)) >= 0) return true;
            else return false;
        }

        public bool Author_Modify(int AuthorID, string newFIO, string newAbout)
        {
            if (connector.ChangeData("UPDATE tAUTHORS SET pNAME=@name, pINFO = @info WHERE pID = @id",
                new OleDbParameter("@name", newFIO),
                new OleDbParameter("@info", newAbout),
                new OleDbParameter("@id", AuthorID)) >= 0) return true;
            else return false;
        }

        public bool Author_SetImage(int AuthorID, string Photo)
        {
            return connector.PutFile(Photo, "UPDATE tAUTHORS SET pPHOTO = @file WHERE pID = @id",
                new OleDbParameter("@id", AuthorID));
        }

        public bool Author_ClearImage(int AuthorID)
        {
            if (connector.ChangeData("UPDATE tAUTHORS SET pPHOTO = null WHERE pID = @id",
                new OleDbParameter("@id", AuthorID)) >= 0) return true;
            else return false;
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
