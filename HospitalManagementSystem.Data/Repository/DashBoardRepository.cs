using Dapper;
using HospitalManagementSystem.Core.Interface;
using HospitalManagementSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Repository
{
    public class DashBoardRepository: IDashBoardRepository
    {
        private readonly IDbConnection con;
        private readonly string connectionString;
        /// <summary>
        /// To initialize connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public DashBoardRepository(string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.connectionString = connectionString;
        }

        public void ExcelUpload(DataTable connString)
        {
            DynamicParameters param = new DynamicParameters();
            
            try
            {
                param.Add("@DataTable", connString.AsTableValuedParameter("[dbo].[UserMapping]"));
                con.Query("dbo.usp_Upload_UserMapping", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<DonarDetails> Get_DonarList(string bloodGroup)
        {
            
                DynamicParameters param = new DynamicParameters();
                param.Add("@BloodGroup", bloodGroup);
                return con.Query<DonarDetails>("[dbo].[Get_DonarData]", param, commandType: CommandType.StoredProcedure);
            
        }

       
        

        public IEnumerable<Appointment> InsertUpdate_Appointment(Appointment appointment)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", appointment.Id);
            param.Add("@Name", appointment.Name);
            param.Add("@MobileNumber", appointment.MobileNumber);
            param.Add("@Service", appointment.Services);
            param.Add("@Time", appointment.Time);
            param.Add("@Date", appointment.Date);
            param.Add("@Note", appointment.Note);
            
            return con.Query<Appointment>("dbo.Insert_Appointment", param, commandType: CommandType.StoredProcedure);
            
            
        }
    }
}
