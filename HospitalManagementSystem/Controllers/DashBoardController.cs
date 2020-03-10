using HospitalManagementSystem.Core.Interface;
using HospitalManagementSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace HospitalManagementSystem.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IDashBoardRepository dashBoardRepository;
        public DashBoardController(IDashBoardRepository dashBoardRepository)
        {
            this.dashBoardRepository = dashBoardRepository;
        }
        // GET: DashBoard
        public ActionResult DashBoard()
        {
            Appointment appointment = new Appointment();
            appointment.Id = -2;
            List<Appointment> data = new List<Appointment>();
            data = dashBoardRepository.InsertUpdate_Appointment(appointment).ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult BloodRequest()
        {

            return View();
        }
        [HttpPost]
        public ActionResult BloodRequest(DonarDetails data)
        {
            var Bloodgroup = data.BGroup;
            var Message = data.Message;
            List<DonarDetails> donarDetails = new List<DonarDetails>();
            donarDetails =  dashBoardRepository.Get_DonarList(Bloodgroup).ToList();
            var res = SendMailtopApplicant(donarDetails, Message);

            if(res=="Sent")
            {
                TempData["Message"] = "Email Sent successfully";
                TempData["Messageclass"] = "alert-success";
            }

            return View();
        }

        [HttpGet]
        public ActionResult AppointmentList()
        {
            
            Appointment appointment = new Appointment();
            
                appointment.Id = -1;
            
                
            List<Appointment> data = new List<Appointment>();
            data= dashBoardRepository.InsertUpdate_Appointment(appointment).ToList();
            return View(data);
        }
        
        public ActionResult AppointmentConfirmation(string id)
        {

            Appointment appointment = new Appointment();
            appointment.Id = Convert.ToInt32(id);
            dashBoardRepository.InsertUpdate_Appointment(appointment);
            var res = 0;
            if (res == 0)
            {
                TempData["Message"] = "Appointmentdetails inserted   successfully";
                TempData["Messageclass"] = "alert-success";
            }
            else
            {
                TempData["Message"] = "Appointmentdetails updated   successfully";
                TempData["Messageclass"] = "alert-success";
            }



            return RedirectToAction("DashBoard", "DashBoard");
        }

        [HttpPost]
        public ActionResult AppointmentList(Appointment appointment)
        {
            dashBoardRepository.InsertUpdate_Appointment(appointment);
            var res = 0;
            if (res == 0)
            {
                TempData["Message"] = "Appointmentdetails inserted   successfully";
                TempData["Messageclass"] = "alert-success";
            }
            else
            {
                TempData["Message"] = "Appointmentdetails updated   successfully";
                TempData["Messageclass"] = "alert-success";
            }



            return RedirectToAction("About", "DashBoard");
        }







        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ExcelUpload()
        {
            return View();
        }
        [HttpPost]
            public ActionResult ExcelUpload(HttpPostedFileBase File)
        {
            //path declaration
            string extension = System.IO.Path.GetExtension(File.FileName).ToLower();
            string[] validFileTypes = { ".xls", ".xlsx" };
            string connString = "";
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ExcelTemplates/");
            //checking path exists or not
            

            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;
               
            }
            if (!Directory.Exists(Server.MapPath("~/ExcelTemplates")))
            {
                Directory.CreateDirectory(Server.MapPath("~/ExcelTemplates"));
            }


            string path1 = string.Format("{0}\\{1}", Server.MapPath("~/ExcelTemplates/"), File.FileName);
            if (validFileTypes.Contains(extension))
            {
                if (System.IO.File.Exists(path1))
                {
                    System.IO.File.Delete(path1);
                }
                File.SaveAs(path1);     //For Saving The Excel File In the Path
                                              //Connection String to Excel Workbook  

                string consString = ConfigurationManager.ConnectionStrings["HMS"].ConnectionString;
                var Created_On = DateTime.Now;
                string excelConnectionString = string.Empty;
                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                //connection String for xls file format.
                if (extension == ".xls")
                {
                    excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                //connection String for xlsx file format.
                else if (extension == ".xlsx")
                {
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }

                OleDbConnection cnn = new OleDbConnection(excelConnectionString);
                //to get sheet name
                cnn.Open();
                DataTable dtSheet = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetname;
                sheetname = "";
                foreach (DataRow drSheet in dtSheet.Rows)
                {
                    if (drSheet["TABLE_NAME"].ToString().Contains("$"))
                    {
                        //getting sheet name
                        sheetname = drSheet["TABLE_NAME"].ToString();

                        OleDbConnection Conn = new OleDbConnection(excelConnectionString);
                        Conn.Open();
                        OleDbCommand oconn = new OleDbCommand("select * from [" + sheetname + "]", Conn);
                        OleDbDataAdapter adp = new OleDbDataAdapter(oconn);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            dashBoardRepository.ExcelUpload(dt);
                        }

                        Conn.Close();

                    }
                }
                
                TempData["Message"] = "files uploaded successfully";
                TempData["Messageclass"] = "alert-success";
                return View();



            }
            return View();



        }

        public static string SendMailtopApplicant(List<DonarDetails> donarDetails,string Message)
        {
            var res = "";
            try
            {
                string bodyTemplate = "";
                List<string> emails = new List<string>();
                foreach (var item in donarDetails)
                {
                    

                    emails.Add(item.Email);
                }
             return res=   SendEmailWithAttachments("Blood Required", emails, Message);




            }
            catch (Exception ex)
            {
                return res = ex.ToString();
            }




        }

        public static string SendEmailWithAttachments(string Subject, List<string> ToEMail, string HtmlBodyWithSignature)
        {
            try
            {
                MailMessage mm = new MailMessage
                {
                    IsBodyHtml = true
                };

                for (int i = 0; i < ToEMail.Count; i++)
                {
                    if (i == 0)
                    {
                       if(ToEMail[i] != null) { 
                        mm.To.Add(new MailAddress(ToEMail[i]));
                        }
                    }
                    else
                    {
                        if (ToEMail[i] != null)
                        {
                            //mm.CC.Add(new MailAddress(ToEMail[i]));
                            mm.To.Add(new MailAddress(ToEMail[i]));
                        }
                    }

                }

                
                string fromEmailId = ConfigurationManager.AppSettings["MainMailId"].ToString();
                string displayName = ConfigurationManager.AppSettings["MailDisplayName"].ToString();
                mm.From = new MailAddress(fromEmailId, displayName);
                mm.Subject = Subject;
                mm.Body = HtmlBodyWithSignature;
                SmtpClient smtp = new SmtpClient();
                smtp.Timeout = 1200000;
                smtp.Send(mm);
                mm.Dispose(); 
                return "Sent";
            }
            catch (Exception em)
            {
                return em.ToString();
                // var res = em.ToString();

            }
        }











    }
}