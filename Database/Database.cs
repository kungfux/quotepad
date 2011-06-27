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
        public ItWorks.OleDb connector = new ItWorks.OleDb();

        public bool Connect()
        {
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
            //connector.SetTrace(false);
            ClearDb();

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
                "pAUTHOR INTEGER NOT NULL CONSTRAINT fktAUTHORS REFERENCES tAUTHORS(pID)," +
                "pTHEME INTEGER NOT NULL CONSTRAINT fktTHEMES REFERENCES tTHEMES(pID)," +
                "prtfQUOTE MEMO NOT NULL," +
                "ptxtQUOTE MEMO NOT NULL," +
                "pFAVORITE LOGICAL NOT NULL DEFAULT FALSE," +
                "pDT DATETIME NOT NULL);";
                ;

            connector.ChangeData(tAuthor);
            connector.ChangeData(tThemes);
            connector.ChangeData(tQuotes);

            connector.SetTrace(true);
        }

        private void ClearDb()
        {
            connector.ChangeData("DROP TABLE tQUOTES");
            connector.ChangeData("DROP TABLE tTHEMES");
            connector.ChangeData("DROP TABLE tAUTHORS");
        }

        public string GetError()
        {
            return connector.LastErrorMessage;
        }
        #region Theme

        public Objects.Theme[] Theme_GetList()
        {
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

        public Objects.Theme Theme_Get(int ThemeID)
        {
            Objects.Theme theme = new Objects.Theme();
            string tname =  connector.SelectCell<string>("SELECT pNAME FROM tTHEMES WHERE pID = @id",
                new OleDbParameter("@id", ThemeID));
            if (tname != null)
            {
                theme.ID = ThemeID;
                theme.Name = tname;
            }
            return theme;
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

        public Int32 Quote_GetCount()
        {
            return connector.SelectCell<Int32>("SELECT COUNT(*) FROM tQUOTES");
        }

        public bool Quote_Create(int AuthorID, int ThemeID, string RTFQuote, string TXTQuote, bool IsFavorite)
        {
            if (connector.ChangeData("INSERT INTO tQUOTES (pAUTHOR, pTHEME, prtfQUOTE, ptxtQUOTE, pFAVORITE, pDT) " +
                "VALUES (@author, @theme, @rtf, @txt, @favorite, @datetime)",
                new OleDbParameter("@author", AuthorID),
                new OleDbParameter("@theme", ThemeID),
                new OleDbParameter("@rtf", RTFQuote),
                new OleDbParameter("@txt", TXTQuote),
                new OleDbParameter("@favorite", IsFavorite),
                new OleDbParameter("@datetime", DateTime.Now.ToString())) >= 0)
                return true;
            else return false;
        }

        public bool Quote_Modify(int QuoteID, int newAuthorID, int newThemeID, string newRTFQuote, string newTXTQuote, bool IsFavorite)
        {
            if (connector.ChangeData("UPDATE tQUOTES SET pAUTHOR=@author, pTHEME=@theme, prtfQUOTE = @rtf, "+
                "ptxtQUOTE = @txt, pFAVORITE = @favorite, pDT = @datetime) WHERE pID = @id",
                new OleDbParameter("@author", newAuthorID),
                new OleDbParameter("@theme", newThemeID),
                new OleDbParameter("@rtf", newRTFQuote),
                new OleDbParameter("@txt", newTXTQuote),
                new OleDbParameter("@favorite", IsFavorite),
                new OleDbParameter("@id", QuoteID)) >= 0)
                return true;
            else return false;
        }

        public bool Quote_Remove(int QuoteID)
        {
            if (connector.ChangeData("DELETE FROM tQUOTES WHERE pID = @id",
                new OleDbParameter("@id", QuoteID)) >= 0)
                return true;
            else return false;
        }

        public bool Quote_SetAsFavorite(int QuoteID)
        {
            if (connector.ChangeData("UPDATE tQUOTES SET pFAVORITE = TRUE WHERE pID = @id",
                new OleDbParameter("@id", QuoteID)) >= 0)
                return true;
            else return false;
        }

        public bool Quote_UnsetFavorite(int QuoteID)
        {
            if (connector.ChangeData("UPDATE tQUOTES SET pFAVORITE = FALSE WHERE pID = @id",
                new OleDbParameter("@id", QuoteID)) >= 0)
                return true;
            else return false;
        }

        public Objects.Quote Quote_ReadNext(int QuoteID, bool FavoriteOnly)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        public Objects.Quote Quote_ReadPrevious(int QuoteID, bool FavoriteOnly)
        {
            throw new Exception("Method is NOT implemented yet in QuotePad.Database.");
        }

        #region Random Read
        
        public int BufferSize = 20; // In persents
        public bool IsReady = false; // Is Buffer has been initialized and etc.

        private Random rnd = new Random();
        private Int32[] Buffer;
        private Int32 Max;
        private Int32 RandomValue;
        private Objects.Quote RandomQuote;

        private void RandomInit()
        {
            Max = connector.SelectCell<Int32>("SELECT MAX(*) FROM tQUOTES");
            Buffer = new Int32[(int)(Max * (BufferSize / 100))];
        
            // Init Buffer
            for (int a = 0; a < Buffer.Length; a++)
            {
                Buffer[a] = 0;
            }
        }

        private void UpdateBuffer(Int32 value)
        {
            for (int a = 1; a < Buffer.Length; a++)
            {
                Buffer[a - 1] = Buffer[a];
            }
            Buffer[Buffer.Length - 1] = value;
        }

        private bool IsValueInBuffer(Int32 value)
        {
            foreach (Int32 v in Buffer)
            {
                if (v == value) return true;
            }
            return false;
        }

        private void Random_FindNext()
        {
            RandomValue = rnd.Next(1, Max);
            while (IsValueInBuffer(RandomValue))
            {
                RandomValue = rnd.Next(1, Max);
            }
            UpdateBuffer(RandomValue);
            RandomQuote = Quote_FindByID(RandomValue);
        }

        public Objects.Quote Quote_RandomRead()
        {
            RandomQuote = new Objects.Quote();
            while (RandomQuote.ID == null)
            {
                Random_FindNext();
            }
            return RandomQuote;
        }

        #endregion

        public Objects.Quote Quote_FindByID(int QuoteID)
        {
            Objects.Quote quote = new Objects.Quote();
            DataRow row = connector.SelectRow("SELECT * FROM tQUOTES WHERE pID = @id",
                new OleDbParameter("@id", QuoteID));
            if (row != null)
            {
                quote.ID = (int)row.ItemArray[0];
                quote.QuoteAuthor = Author_Get((int)row.ItemArray[1]);
                quote.QuoteTheme = Theme_Get((int)row.ItemArray[2]);
                quote.RTF = (string)row.ItemArray[3];
                // ItemArray[4] is a Quote saved in text format
                quote.IsFavorite = (bool)row.ItemArray[5];
                quote.WhenCreated = (DateTime)row.ItemArray[6];
            }
            return quote;
        }

        public Objects.Quote[] Quote_FindByAuthor(int AuthorID)
        {
            DataTable found = connector.SelectTable("SELECT * FROM tQUOTES WHERE pAUTHOR = @author",
                new OleDbParameter("@author", AuthorID));
            Objects.Quote[] result = new Objects.Quote[found.Rows.Count];
            for (int a=0;a<found.Rows.Count;a++)
            {
                result[a] = new Objects.Quote();
                result[a].ID = (int)found.Rows[a].ItemArray[0];
                result[a].QuoteAuthor = Author_Get((int)found.Rows[a].ItemArray[1]);
                result[a].QuoteTheme = Theme_Get((int)found.Rows[a].ItemArray[2]);
                result[a].RTF = (string)found.Rows[a].ItemArray[3];
                // ItemArray[4] is a Quote saved in text format
                result[a].IsFavorite = (bool)found.Rows[a].ItemArray[5];
                result[a].WhenCreated = (DateTime)found.Rows[a].ItemArray[6];
            }
            return result;
        }

        public Objects.Quote[] Quote_FindByTheme(int ThemeID)
        {
            DataTable found = connector.SelectTable("SELECT * FROM tQUOTES WHERE pTHEME = @theme",
                new OleDbParameter("@theme", ThemeID));
            Objects.Quote[] result = new Objects.Quote[found.Rows.Count];
            for (int a = 0; a < found.Rows.Count; a++)
            {
                result[a] = new Objects.Quote();
                result[a].ID = (int)found.Rows[a].ItemArray[0];
                result[a].QuoteAuthor = Author_Get((int)found.Rows[a].ItemArray[1]);
                result[a].QuoteTheme = Theme_Get((int)found.Rows[a].ItemArray[2]);
                result[a].RTF = (string)found.Rows[a].ItemArray[3];
                // ItemArray[4] is a Quote saved in text format
                result[a].IsFavorite = (bool)found.Rows[a].ItemArray[5];
                result[a].WhenCreated = (DateTime)found.Rows[a].ItemArray[6];
            }
            return result;
        }

        public Objects.Quote[] Quote_FindByText(string Text)
        {
            DataTable found = connector.SelectTable("SELECT * FROM tQUOTES WHERE ptxtQUOTE LIKE %@text%",
                new OleDbParameter("@text", Text));
            Objects.Quote[] result = new Objects.Quote[found.Rows.Count];
            for (int a = 0; a < found.Rows.Count; a++)
            {
                result[a] = new Objects.Quote();
                result[a].ID = (int)found.Rows[a].ItemArray[0];
                result[a].QuoteAuthor = Author_Get((int)found.Rows[a].ItemArray[1]);
                result[a].QuoteTheme = Theme_Get((int)found.Rows[a].ItemArray[2]);
                result[a].RTF = (string)found.Rows[a].ItemArray[3];
                // ItemArray[4] is a Quote saved in text format
                result[a].IsFavorite = (bool)found.Rows[a].ItemArray[5];
                result[a].WhenCreated = (DateTime)found.Rows[a].ItemArray[6];
            }
            return result;
        }

        public Objects.Quote[] Quote_FindByDate(DateTime From, DateTime To)
        {
            DataTable found = connector.SelectTable("SELECT * FROM tQUOTES WHERE pDT >= @from and pDT <= @to",
                new OleDbParameter("@from", From),
                new OleDbParameter("@to", To));
            Objects.Quote[] result = new Objects.Quote[found.Rows.Count];
            for (int a = 0; a < found.Rows.Count; a++)
            {
                result[a] = new Objects.Quote();
                result[a].ID = (int)found.Rows[a].ItemArray[0];
                result[a].QuoteAuthor = Author_Get((int)found.Rows[a].ItemArray[1]);
                result[a].QuoteTheme = Theme_Get((int)found.Rows[a].ItemArray[2]);
                result[a].RTF = (string)found.Rows[a].ItemArray[3];
                // ItemArray[4] is a Quote saved in text format
                result[a].IsFavorite = (bool)found.Rows[a].ItemArray[5];
                result[a].WhenCreated = (DateTime)found.Rows[a].ItemArray[6];
            }
            return result;
        }
        #endregion
    }
}
