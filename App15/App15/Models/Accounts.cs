using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App15.Models
{
    class Accounts
    {
        //The Id property is marked as the Primary Key
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string BloodGroup { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CreationDate { get; set; }
        public Accounts()
        {
            //empty constructor
        }
        public Accounts(string name, string phone_no, string blood_group, string email, string password, string latitude, string longitude, string address)
        {
            Name = name;
            BloodGroup = blood_group;
            Email = email;
            ContactNumber = phone_no;
            Password = password;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
            CreationDate = DateTime.Now.ToString();
        }

    }
}
