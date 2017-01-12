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
    class RequestsDatabaseHelperClass
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
                        dbConn.CreateTable<Requests>();
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

        // Retrieve the specific request from the database. 
        public Requests ReadRequests(int requestid)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingrequest = dbConn.Query<Requests>("select * from Requests where Id =" + requestid).FirstOrDefault();
                return existingrequest;
            }
        }

        // Retrieve the all request list from the database. 
        public ObservableCollection<Requests> ReadRequests()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Requests> myCollection = dbConn.Table<Requests>().ToList<Requests>();
                ObservableCollection<Requests> RequestsList = new ObservableCollection<Requests>(myCollection);
                return RequestsList;
            }
        }

        //Update existing request 
        public void UpdateRequest(Requests request)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<Requests>("select * from Requests where Id =" + request.Id).FirstOrDefault();
                if (existingconact != null)
                {
                    existingconact.IdDonor = request.IdDonor;
                    existingconact.BloodGroup = request.BloodGroup;
                    existingconact.CreationDate = request.CreationDate;
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existingconact);
                    });
                }
            }
        }

        // Insert the new request in the Requests table. 
        public void Insert(Requests newrequest)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newrequest);
                });
            }
        }

        //Delete specific request 
        public void DeleteRequest(int Id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<Requests>("select * from Requests where Id =" + Id).FirstOrDefault();
                if (existingconact != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingconact);
                    });
                }
            }
        }
        //Delete all requestlist or delete Requests table 
        public void DeleteAllRequest()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() => 
                //   { 
                dbConn.DropTable<Requests>();
                dbConn.CreateTable<Requests>();
                dbConn.Dispose();
                dbConn.Close();
                //}); 
            }
        }

    }
}
