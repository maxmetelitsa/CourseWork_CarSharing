using CourseWork_CarSharing.SQL_Manager;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseWork_CarSharing.UsersInfo
{
    public class CurrentUserManager
    {
        private static CurrentUser currentUser;
        public SQLiteManager manager = new SQLiteManager();

        public static CurrentUser CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
    }
}
