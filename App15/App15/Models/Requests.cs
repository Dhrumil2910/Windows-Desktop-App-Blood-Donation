using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App15.Models
{
    class Requests
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int IdDonor { get; set; }
        public string BloodGroup { get; set; }
        public string CreationDate { get; set; }
        public Requests()
        {
            //empty constructor
        }
        public Requests(int id, string blood_group)
        {
            IdDonor = id;
            BloodGroup = blood_group;
            CreationDate = DateTime.Now.ToString();
        }
    }

}
