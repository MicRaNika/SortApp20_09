using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SortApp.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public List<User> Users { get; set; }
        public Company()
        {
            Users = new List<User>();
        }
    }
}
