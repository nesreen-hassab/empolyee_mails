using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using employeeInfo.Models;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace employeeInfo.Controllers
{
    public class param
    {
        public int to { get; set; }
        public int from { get; set; }
    }

    
    public class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DemoController : Controller
    {




        private const string PdfFileName = "employee_information.pdf";

        private readonly SmtpClient _smtpClient;

        public DemoController(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }


        public employee_sData getEmployeeInfo(string id)
        {
            employee_sData l  = new employee_sData();
            string conString = "User Id=hr;Password=hr;" + "Data Source=192.168.1.2:1521/XEPDB1;";



            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;


                        cmd.CommandText = "select employee_id,first_name,last_name,email,hire_date,job_id,salary,department_id from employees where employee_id=:id";


                        //Execute the command and use DataReader to display the data
                        OracleParameter input_id = new OracleParameter("id", id);
                        cmd.Parameters.Add(input_id);
                        OracleDataReader reader = cmd.ExecuteReader();
                        Console.WriteLine("id " + id);
                        while (reader.Read())
                        {
                            l.employeeId = reader.GetString(0);
                            l.firstName = reader.GetString(1);
                            l.LastName = reader.GetString(2);
                            l.Email = reader.GetString(3);

                            l.hire_date = reader.GetString(4);
                            l.job_id = reader.GetString(5);
                            l.salary = reader.GetString(6);
                            //l.comm= reader.GetString(7);
                            // l.Manager_id= reader.GetString(9);
                            l.department_id = reader.GetString(7);
                        }

                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console
                            .WriteLine("errooorrrrr");
                    }


                }
            }


            return l;

        }
        [HttpPost]        public void sendToAll(param parameters)        {            for (int i = parameters.from; i <= parameters.to; i++)            {                int id = i;                DemoViewAsPdf(id.ToString(), "hagar.elsherif999@gmail.com"); //getEmployeeInfo(id).Email            }        }

        public async void DemoViewAsPdf(string id, string email)
        {
            MailMessage message = new MailMessage();
            message.To.Add(email);
            message.Subject = "employee's data";
            message.Body = " employee's data is written in the pdf file bellow";
            message.From = new MailAddress("csgsales@csgegypt.com");
            message.IsBodyHtml = true;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("csgsales@csgegypt.com", "UPLaZ5%CSG"),
                EnableSsl = true,

                //csgsales@csgegypt.com
                //UPLaZ5%CSG
            };
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            //------------------------------------------------------------------------------------------------------------
            employee_sData h = getEmployeeInfo(id);
            //ViewBag.data = h;
            // ViewData["data"] = h;
            //---------------------------------------------------------------------------------------
            var viewAsPdf = new ViewAsPdf("index", h)
            {
                FileName = PdfFileName,
                PageSize = Size.A4,
                PageMargins = { Left = 1, Right = 1 }

            };
            var pdfBytes = await viewAsPdf.BuildFile(ControllerContext);

            using var attachment = new Attachment(new MemoryStream(pdfBytes), PdfFileName);
            message.Attachments.Add(attachment);
            //  System.Net.ServicePointManager.Expect100Continue = false;
            // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            smtpClient.Send(message);

        }

        //[HttpGet]
        ///*
        // ?id=${data}
        // */
        //public async Task<IActionResult> DemoViewAsPdf(string id)
        //{

        //    MailMessage message = new MailMessage();
        //    message.To.Add("esraahassab18@gmail.com"); //getEmployeeInfo(id).Email
        //    message.Subject = "employee's data";
        //    message.Body = " employee's data is written in the pdf file bellow";
        //    message.From = new MailAddress("nesreenramdan75@gmail.com");
        //    message.IsBodyHtml = true;
        //    var smtpClient = new SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential("nesreenramdan75@gmail.com", "xnjhgxvzjnqixzrl"),
        //        EnableSsl = true,

        //        //csgsales@csgegypt.com
        //        //UPLaZ5%CSG
        //    };
        //    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //    {
        //        return true;
        //    };

        //    //------------------------------------------------------------------------------------------------------------
        //    employee_sData h = getEmployeeInfo(id);
        //    //ViewBag.data = h;
        //    // ViewData["data"] = h;
        //    //---------------------------------------------------------------------------------------
        //    var viewAsPdf = new ViewAsPdf("index", h)
        //    {
        //        FileName = PdfFileName,
        //        PageSize = Size.A4,
        //        PageMargins = { Left = 1, Right = 1 }

        //    };
        //    var pdfBytes = await viewAsPdf.BuildFile(ControllerContext);

        //    using var attachment = new Attachment(new MemoryStream(pdfBytes), PdfFileName);
        //    message.Attachments.Add(attachment);
        //    //  System.Net.ServicePointManager.Expect100Continue = false;
        //    // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        //    smtpClient.Send(message);
        //    return View("index", h);
        //}
    }
}
