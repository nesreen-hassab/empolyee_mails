using employeeInfo.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace employeeInfo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.Configure<SmtpOptions>(Configuration.GetSection("Smtp"));

            // SmtpClient is not thread-safe, hence transient
            services.AddTransient(provider =>
            {
                var smtpOptions = provider.GetService<IOptions<SmtpOptions>>().Value;
                return new SmtpClient(smtpOptions.Host, smtpOptions.Port)
                {
                    // Credentials and EnableSsl here when required
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=employee}/{action=Index}/{id?}");
            });
            RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env);
            /*   app.Run(async (context) =>
               {
                   //Demo: Basic ODP.NET Core application for ASP.NET Core
                   // to connect, query, and return results to a web page

                   //Create a connection to Oracle			
                   string conString = "User Id=hr;Password=hr;" +

                   //How to connect to an Oracle DB without SQL*Net configuration file
                   //  also known as tnsnames.ora.
                   "Data Source=192.168.1.60:1521/XEPDB1;";

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
                               cmd.CommandText = "select first_name from employees where department_id = :id";

                               // Assign id to the department number 50 
                               OracleParameter id = new OracleParameter("id", 110);
                               cmd.Parameters.Add(id);

                               //Execute the command and use DataReader to display the data
                               OracleDataReader reader = cmd.ExecuteReader();
                               while (reader.Read())
                               {

                                   await context.Response.WriteAsync("Employee First Name: " + reader.GetString(0) + "\n");
                               }

                               reader.Dispose();
                           }
                           catch (Exception ex)
                           {
                               await context.Response.WriteAsync(ex.Message);
                           }
                       }
                   }

               });*/



        }
    }
}
