using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HospitalManagementSystem.Core.Model
{
    public class DashBoard
    {
    }
    public class BloodRequest
    {
        public string  Message { get; set; }
        public string BGroupId { get; set; }
        public string BGroup { get; set; }

    }
    public class Appointment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string BGroupId { get; set; }
        public string BGroup { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Services { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }

    }
    public class DonarDetails
    {
        public HttpPostedFileBase Files { get; set; }
        public string Message { get; set; }
        public string BGroupId { get; set; }
        public string BGroup { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
       
    


    }
}
