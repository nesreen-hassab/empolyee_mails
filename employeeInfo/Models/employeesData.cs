using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace employeeInfo.Models
{
    public class employee_sData
    {
        public employee_sData()
        {
        }

        public employee_sData(string employeeId, string firstName)
        
            {
                this.employeeId = employeeId;
                this.firstName = firstName;
            }

        public employee_sData(string employeeId, string firstName, string lastName, string email, string phone_number, string hire_date, string job_id, string salary, string comm, string manager_id, string department_id)
        {
            this.employeeId = employeeId;
            this.firstName = firstName;
           this. LastName = lastName;
           this. Email = email;
            this.hire_date = hire_date;
            this.job_id = job_id;
            this.salary = salary;
          
            this.department_id = department_id;
        }

        public string employeeId { get; set; }
        public string firstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
     
        public string hire_date { get; set; }
        public string job_id { get; set; }
        public string salary { get; set; }
       
        public string department_id { get; set; }

    }
}
