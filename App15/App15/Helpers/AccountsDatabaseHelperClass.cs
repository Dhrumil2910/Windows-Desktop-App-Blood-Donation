using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Collections.ObjectModel;
using App15.Models;
using System.Diagnostics;

namespace App15.Helpers
{
    class AccountsDatabaseHelperClass
    {
        SQLiteConnection dbConn;

        //Create Table
        public async Task<bool> onCreate(string DB_PATH)
        {
            try
            {
                if (!CheckFileExists(DB_PATH).Result)
                {
                    using (dbConn = new SQLiteConnection(DB_PATH))
                    {
                        dbConn.CreateTable<Accounts>();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Retrieve the specific account from the database. 
        public Accounts ReadAccounts(int accountid)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingaccount = dbConn.Query<Accounts>("select * from Accounts where Id =" + accountid).FirstOrDefault();
                return existingaccount;
            }
        }

        // Retrieve userId after successful Login
        public Accounts LoginUser(string userEmail, string password)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                Debug.WriteLine("select * from Accounts where Email ='" + userEmail + "'and Password = '" + password + "'");
                var existingaccount = dbConn.Query<Accounts>("select * from Accounts where Email ='" + userEmail + "' and Password = '" + password + "'").FirstOrDefault();
                return existingaccount;
            }
        }
        // Retrieve the all account list from the database. 
        public ObservableCollection<Accounts> ReadAccounts()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Accounts> myCollection = dbConn.Table<Accounts>().ToList<Accounts>();
                ObservableCollection<Accounts> AccountsList = new ObservableCollection<Accounts>(myCollection);
                return AccountsList;
            }
        }

        //Update existing account 
        public void UpdateAccount(Accounts account)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<Accounts>("select * from Accounts where Id =" + account.Id).FirstOrDefault();
                if (existingconact != null)
                {
                    existingconact.Name = account.Name;
                    existingconact.ContactNumber = account.ContactNumber;
                    existingconact.BloodGroup = account.BloodGroup;
                    existingconact.Email = account.Email;
                    existingconact.Latitude = account.Latitude;
                    existingconact.Longitude = account.Longitude;
                    existingconact.Address = account.Address;
                    existingconact.CreationDate = account.CreationDate;
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existingconact);
                    });
                }
            }
        }

        // Insert the new account in the Accounts table. 
        public void Insert(Accounts newaccount)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newaccount);
                });
            }
        }

        //Delete specific account 
        public void DeleteAccount(int Id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<Accounts>("select * from Accounts where Id =" + Id).FirstOrDefault();
                if (existingconact != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingconact);
                    });
                }
            }
        }
        //Delete all accountlist or delete Accounts table 
        public void DeleteAllAccount()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() => 
                //   { 
                dbConn.DropTable<Accounts>();
                dbConn.CreateTable<Accounts>();
                dbConn.Dispose();
                dbConn.Close();
                //}); 
            }
        }

    }
}
