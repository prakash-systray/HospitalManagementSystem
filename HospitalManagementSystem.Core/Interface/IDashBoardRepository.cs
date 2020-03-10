using HospitalManagementSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Interface
{
   public interface IDashBoardRepository
    {
        void ExcelUpload(DataTable connString);
        IEnumerable<Appointment> InsertUpdate_Appointment(Appointment appointment);
        
        IEnumerable<DonarDetails> Get_DonarList(string bloodGroup);
    }
}
