using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GPSNote.Models
{
    internal class UserModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Ename { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
