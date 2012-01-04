using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using ItWorksTeam.NET;

namespace QuotePad
{
    public static class Database
    {
        static Database()
        {
            //connector.SetTrace(true, "db.err", ItWorks.OleDb.TraceLevel.QueryWithMessage);
            if (!Database.Connect()) MessageBox.Show("Не удалось установить соединение с базой данных!",
                new assembly().AssemblyProduct, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        //public static void Disconnect()
        //{
        //    connector.Disconnect();
        //}

        private static bool UseTracing = new ItWorksTeam.Utils.Registry().ReadKey<bool>(ItWorksTeam.Utils.Registry.BaseKeys.HKEY_LOCAL_MACHINE,
            @"Software\ItWorksTeam\QuotePad", "TraceEnabled", false);

        public static OleDb connector = new OleDb();

        public static bool IsConnected { get { return connector.IsActiveConnection(); } }

        public static bool Connect()
        {
            string databaseSource = new ItWorksTeam.Utils.Registry().ReadKey<string>(ItWorksTeam.Utils.Registry.BaseKeys.HKEY_LOCAL_MACHINE,
                @"Software\ItWorksTeam\QuotePad", "Database", null);
            if (connector.TestConnection("Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + databaseSource, true, true))
            {
                connector.SetTrace(UseTracing, Application.StartupPath + @"\dberr.log", OleDb.TraceLevel.QueryWithMessage);
                InitDb();
                return true;
            }
            else return false;
        }

        public static void InitDb()
        {
            connector.SetTrace(false);

            string tAuthor =
                "CREATE TABLE tAUTHORS(" +
                "pID COUNTER CONSTRAINT pkTYPE PRIMARY KEY," +
                "pNAME TEXT(100) NOT NULL," +
                "pINFO MEMO," +
                "pPHOTO IMAGE);";
            string tThemes =
                "CREATE TABLE tTHEMES (" +
                "pID COUNTER CONSTRAINT pkTYPE PRIMARY KEY," +
                "pNAME TEXT(50) NOT NULL)";
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

            connector.SetTrace(UseTracing);
        }

        private static void ClearDb()
        {
            connector.ChangeData("DROP TABLE tQUOTES");
            connector.ChangeData("DROP TABLE tTHEMES");
            connector.ChangeData("DROP TABLE tAUTHORS");
        }

        public static string GetError()
        {
            return connector.LastErrorMessage;
        }

        #region Theme

        public static Objects.Theme[] Theme_GetList()
        {
            DataTable themes = connector.SelectTable("SELECT * FROM tTHEMES");
            if (themes == null) return new Objects.Theme[0];
            Objects.Theme[] list = new Objects.Theme[themes.Rows.Count];
            for (int theme = 0; theme < themes.Rows.Count; theme++)
            {
                list[theme] = new Objects.Theme();
                list[theme].ID = (int)themes.Rows[theme].ItemArray[0];
                list[theme].Name = (string)themes.Rows[theme].ItemArray[1];
            }
            return list;
        }

        public static Objects.Theme Theme_Get(int ThemeID)
        {
            Objects.Theme theme = new Objects.Theme();
            string tname;
            try
            {
                tname = connector.SelectCell<string>("SELECT pNAME FROM tTHEMES WHERE pID = @id",
                    new OleDbParameter("@id", ThemeID));
                theme.ID = ThemeID;
                theme.Name = tname;
            }
            catch (IndexOutOfRangeException) { }
            return theme;
        }

        public static bool Theme_Create(string Theme)
        {
            if (connector.ChangeData("INSERT INTO tTHEMES (pNAME) VALUES (@name)",
                new OleDbParameter("@name", Theme)) >= 0) return true;
            else return false;
        }

        public static bool Theme_Modify(int ThemeID, string newName)
        {
            if (connector.ChangeData("UPDATE tTHEMES SET pNAME=@name WHERE pID = @id",
                new OleDbParameter("@name", newName),
                new OleDbParameter("@id", ThemeID)) >= 0) return true;
            else return false;
        }

        public static bool Theme_Remove(int ThemeID)
        {
            if (connector.ChangeData("DELETE FROM tTHEMES WHERE pID = @id",
                new OleDbParameter("@id", ThemeID)) >= 0) return true;
            else return false;
        }

        #endregion

        #region Author

        public static Objects.Author[] Author_GetList()
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
                connector.SetTrace(false);
                list[theme].Photo =
                    connector.GetImage("SELECT pPHOTO FROM tAUTHORS WHERE pID = " +
                    themes.Rows[theme].ItemArray[0]);
                connector.SetTrace(UseTracing);
            }
            return list;
        }

        public static Objects.Author Author_Get(int AuthorID)
        {
            DataRow author = connector.SelectRow("SELECT * FROM tAUTHORS WHERE pID = @id",
                new OleDbParameter("@id", AuthorID));
            if (author == null) return new Objects.Author();
            Objects.Author item = new Objects.Author();
            item.ID = (int)author.ItemArray[0];
            item.FIO = (string)author.ItemArray[1];
            item.About = (string)author.ItemArray[2];
            connector.SetTrace(false);
            item.Photo = connector.GetImage("SELECT pPHOTO FROM tAUTHORS WHERE pID = " +
                    AuthorID);
            connector.SetTrace(UseTracing);
            return item;
        }

        public static string Author_GetName(int AuthorID)
        {
            try
            {
                return connector.SelectCell<string>("SELECT pNAME FROM tAUTHORS WHERE pID = @id",
                    new OleDbParameter("@id", AuthorID));
            }
            catch (IndexOutOfRangeException) { }
            return null;
        }

        public static bool Author_Create(string FIO, string About)
        {
            if (connector.ChangeData("INSERT INTO tAUTHORS (pNAME, pINFO) VALUES (@name, @info)",
                new OleDbParameter("@name", FIO),
                new OleDbParameter("@info", About)) >= 0) return true;
            else return false;
        }

        public static bool Author_Modify(int AuthorID, string newFIO, string newAbout)
        {
            if (connector.ChangeData("UPDATE tAUTHORS SET pNAME=@name, pINFO = @info WHERE pID = @id",
                new OleDbParameter("@name", newFIO),
                new OleDbParameter("@info", newAbout),
                new OleDbParameter("@id", AuthorID)) >= 0) return true;
            else return false;
        }

        public static bool Author_Remove(int AuthorID)
        {
            if (connector.ChangeData("DELETE FROM tAUTHORS WHERE pID = @id",
                new OleDbParameter("@id", AuthorID)) >= 0) return true;
            else return false;
        }

        public static bool Author_SetImage(int AuthorID, string Photo)
        {
            return connector.PutFileSpecifically(Photo, "UPDATE tAUTHORS SET pPHOTO = @file WHERE pID = @id", 1,
                new OleDbParameter("@id", AuthorID));
        }

        public static bool Author_ClearImage(int AuthorID)
        {
            if (connector.ChangeData("UPDATE tAUTHORS SET pPHOTO = null WHERE pID = @id",
                new OleDbParameter("@id", AuthorID)) >= 0) return true;
            else return false;
        }
        #endregion

        #region Quote

        public static Int32 Quote_GetCount()
        {
            return connector.SelectCell<Int32>("SELECT COUNT(*) FROM tQUOTES");
        }

        public static Int32 Quote_GetMaxID()
        {
            return Max;
        }

        public static Int32 Quote_GetMinID()
        {
            return Min;
        }

        public static bool Quote_Create(int AuthorID, int ThemeID, string RTFQuote, string TXTQuote, bool IsFavorite)
        {
            if (connector.ChangeData("INSERT INTO tQUOTES (pAUTHOR, pTHEME, prtfQUOTE, ptxtQUOTE, pFAVORITE, pDT) " +
                "VALUES (@author, @theme, @rtf, @txt, @favorite, @datetime)",
                new OleDbParameter("@author", AuthorID),
                new OleDbParameter("@theme", ThemeID),
                new OleDbParameter("@rtf", RTFQuote),
                new OleDbParameter("@txt", TXTQuote),
                new OleDbParameter("@favorite", IsFavorite),
                new OleDbParameter("@datetime", DateTime.Now.ToString())) >= 0)
            {
                ReInitRandomPlease();
                return true;
            }
            else return false;
        }

        public static bool Quote_Modify(int QuoteID, int newAuthorID, int newThemeID, string newRTFQuote, string newTXTQuote, bool IsFavorite)
        {
            if (connector.ChangeData("UPDATE tQUOTES SET pAUTHOR=@author, pTHEME=@theme, prtfQUOTE = @rtf, " +
                "ptxtQUOTE = @txt, pFAVORITE = @favorite, pDT = @datetime WHERE pID = @id",
                new OleDbParameter("@author", newAuthorID),
                new OleDbParameter("@theme", newThemeID),
                new OleDbParameter("@rtf", newRTFQuote),
                new OleDbParameter("@txt", newTXTQuote),
                new OleDbParameter("@favorite", IsFavorite),
                new OleDbParameter("@datetime", DateTime.Now.ToString()),
                new OleDbParameter("@id", QuoteID)) >= 0)
                return true;
            else return false;
        }

        public static bool Quote_Remove(int QuoteID)
        {
            if (connector.ChangeData("DELETE FROM tQUOTES WHERE pID = @id",
                new OleDbParameter("@id", QuoteID)) >= 0)
            {
                ReInitRandomPlease();
                return true;
            }
            else return false;
        }

        public static bool Quote_SetAsFavorite(int QuoteID)
        {
            if (connector.ChangeData("UPDATE tQUOTES SET pFAVORITE = TRUE WHERE pID = @id",
                new OleDbParameter("@id", QuoteID)) >= 0)
                return true;
            else return false;
        }

        public static bool Quote_UnsetFavorite(int QuoteID)
        {
            if (connector.ChangeData("UPDATE tQUOTES SET pFAVORITE = FALSE WHERE pID = @id",
                new OleDbParameter("@id", QuoteID)) >= 0)
                return true;
            else return false;
        }

        public static Objects.Quote Quote_ReadNext(int QuoteID, bool FavoriteOnly = false)
        {
            Objects.Quote quote = new Objects.Quote();
            DataRow row;
            if (!FavoriteOnly)
            {
                row = connector.SelectRow("SELECT TOP 1 * FROM tQUOTES WHERE pID > @id", 0,
                  new OleDbParameter("@id", QuoteID));
            }
            else
            {
                row = connector.SelectRow("SELECT TOP 1 * FROM tQUOTES WHERE pID > @id AND pFAVORITE = TRUE", 0,
                  new OleDbParameter("@id", QuoteID));
            }
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

        public static Objects.Quote Quote_ReadPrevious(int QuoteID, bool FavoriteOnly = false)
        {
            Objects.Quote quote = new Objects.Quote();
            DataRow row;
            if (!FavoriteOnly)
            {
                row = connector.SelectRow("SELECT * FROM tQUOTES WHERE pID = (SELECT MAX(pID) FROM tQUOTES WHERE pID < @id)", 0,
                  new OleDbParameter("@id", QuoteID));
            }
            else
            {
                row = connector.SelectRow("SELECT * FROM tQUOTES WHERE pID = (SELECT MAX(pID) FROM tQUOTES WHERE pID < @id AND pFAVORITE = TRUE)", 0,
                  new OleDbParameter("@id", QuoteID));
            }
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

        #region Random Read

        public static double BufferSize = 0.99; // In persents
        public static int BufferSizeLimit = 200; // In number

        private static bool IsReady = false; // Is Buffer has been initialized and etc.
        private static Random rnd = new Random();
        private static Int32[] Buffer;
        private static Int32 Min;
        private static Int32 Max;
        private static Int32 RandomValue;
        private static Objects.Quote RandomQuote;

        private static void ReInitRandomPlease()
        {
            IsReady = false;
        }

        private static void RandomInit()
        {
            if (IsConnected)
            {
                Int32 RecordsCount = connector.SelectCell<Int32>("SELECT COUNT(*) FROM tQUOTES", 0);
                if (RecordsCount > 0)
                {
                    if (RecordsCount * BufferSize < BufferSizeLimit)
                    {
                        Buffer = new Int32[(Int32)(RecordsCount * BufferSize)];
                    }
                    else
                    {
                        Buffer = new Int32[BufferSizeLimit];
                    }

                    Min = connector.SelectCell<Int32>("SELECT MIN(pID) FROM tQUOTES");
                    Max = connector.SelectCell<Int32>("SELECT MAX(pID) FROM tQUOTES");

                    // Init Buffer
                    for (int a = 0; a < Buffer.Length; a++)
                    {
                        Buffer[a] = 0;
                    }
                }
                else Max = 0;
            }
        }

        private static void UpdateBuffer(Int32 value)
        {
            if (Buffer.Length > 0)
            {
                for (int a = 1; a < Buffer.Length; a++)
                {
                    Buffer[a - 1] = Buffer[a];
                }
                Buffer[Buffer.Length - 1] = value;
            }
        }

        private static bool IsValueInBuffer(Int32 value)
        {
            foreach (Int32 v in Buffer)
            {
                if (v == value) return true;
            }
            return false;
        }

        private static void Random_FindNext()
        {
            RandomValue = rnd.Next(1, Max + 1);
            while (IsValueInBuffer(RandomValue))
            {
                RandomValue = rnd.Next(1, Max + 1);
            }
            UpdateBuffer(RandomValue);
            RandomQuote = Quote_FindByID(RandomValue);
        }

        public static Objects.Quote Quote_RandomRead()
        {
            if (!IsReady)
            {
                RandomInit();
                IsReady = !IsReady;
            }
            if (Max > 0)
            {
                RandomQuote = new Objects.Quote();
                while (RandomQuote.ID == 0)
                {
                    Random_FindNext();
                }
                return RandomQuote;
            }
            else return new Objects.Quote();
        }

        #endregion

        public static bool Quote_FindByID_IsExist(int QuoteID)
        {
            DataRow row = connector.SelectRow("SELECT * FROM tQUOTES WHERE pID = @id",
                new OleDbParameter("@id", QuoteID));
            if (row != null)
            {
                return true;
            }
            else return false;
        }

        public static Objects.Quote Quote_FindByID(int QuoteID)
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

        public static Objects.Quote[] Quote_FindByAuthor(int AuthorID)
        {
            DataTable found = connector.SelectTable("SELECT * FROM tQUOTES WHERE pAUTHOR = @author",
                new OleDbParameter("@author", AuthorID));
            if (found != null)
            {
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
            else return new Objects.Quote[0];
        }

        public static Objects.Quote[] Quote_FindByTheme(int ThemeID)
        {
            DataTable found = connector.SelectTable("SELECT * FROM tQUOTES WHERE pTHEME = @theme",
                new OleDbParameter("@theme", ThemeID));
            if (found != null)
            {
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
            else return new Objects.Quote[0];
        }

        public static Objects.Quote[] Quote_FindByText(string Text)
        {
            DataTable found = connector.SelectTable("SELECT * FROM tQUOTES WHERE ptxtQUOTE LIKE @text",
                new OleDbParameter("@text", "%" + Text + "%"));
            if (found != null)
            {
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
            else return new Objects.Quote[0];
        }

        public static Objects.Quote[] Quote_FindByDate(DateTime From, DateTime To)
        {
            DataTable found = connector.SelectTable("SELECT * FROM tQUOTES WHERE pDT >= @from and pDT <= @to",
                new OleDbParameter("@from", From.ToString()),
                new OleDbParameter("@to", To.ToString()));
            if (found != null)
            {
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
            else return new Objects.Quote[0];
        }
        #endregion
    }
}
