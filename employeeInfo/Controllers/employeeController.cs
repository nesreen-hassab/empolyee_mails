using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using employeeInfo.Models;
using System.Net.Mail;
using System.Net;


namespace employeeInfo.Controllers
{
    public class employeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string conString = "User Id=hr;Password=hr;" + "Data Source=192.168.1.2:1521/XEPDB1;";

            //How to connect to an Oracle DB without SQL*Net configuration file
            //  also known as tnsnames.ora.


            var l = new List<viewEmployee>();

            //How to connect to an Oracle DB with a DB alias.
            //Uncomment below and comment above.
            //"Data Source=<service name alias>;";

            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;

                        //Use the command to display employee names from 
                        // the EMPLOYEES table
                        cmd.CommandText = "select employee_id,first_name from employees ";

                        OracleDataReader reader = cmd.ExecuteReader();
                        //Execute the command and use DataReader to display the data

                        while (reader.Read())
                        {
                            l.Add(new viewEmployee(reader.GetString(0), reader.GetString(1)));
                            // await context.Response.WriteAsync("Employee First Name: " + reader.GetString(0) + "\n");
                        }

                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console
                            .WriteLine("errooorrrrr");
                        // await context.Response.WriteAsync(ex.Message);
                    }
                }
            }

            return Json(new { data = l });
        }



        //[HttpDelete]

        /*  public async Task<IActionResult> Delete(int id)
          {
              var bookFromDb = await _db.Books.FirstOrDefaultAsync(u => u.Id == id);
              if (bookFromDb == null)
              {
                  return Json(new { success = false, message = "Error while Deleting" });
              }
              _db.Books.Remove(bookFromDb);
              await _db.SaveChangesAsync();
              return Json(new { success = true, message = "Delete successful" });
          }*/
        #endregion

        //}

    }
}
