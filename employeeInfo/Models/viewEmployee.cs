using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace employeeInfo.Models
{
    public class viewEmployee
    {
       // [Key]
        public string id { get; set; }
        public string name { get; set; }
       

       public viewEmployee(string i, string n) {
            this.id = i;
            this.name = n;
        }

       
    }
}
