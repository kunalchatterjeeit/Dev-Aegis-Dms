using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public static class ExceptionLog
    {
        public static int ErrorLog_Save(string exceptionMessage, string exceptionType, string exceptionSource, string exceptionUrl, int userId)
        {
            int errorLogId = 0;
            using (DataManager oDm = new DataManager())
            {
                MySqlParameter errorLog = oDm.Add("p_ErrorLogId", MySqlDbType.Int64, errorLogId, ParameterDirection.InputOutput);
                oDm.Add("p_ExceptionMessage", MySqlDbType.LongText, exceptionMessage);
                oDm.Add("p_ExceptionType", MySqlDbType.VarChar, 100, exceptionType);
                oDm.Add("p_ExceptionSource", MySqlDbType.LongText, exceptionSource);
                oDm.Add("p_ExceptionUrl", MySqlDbType.VarChar, 100, exceptionUrl);
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("usp_ErrorLog_Save");
                int.TryParse(errorLog.Value.ToString(),out errorLogId);
                oDm.Dispose();
                return errorLogId;
            }
        }
    }
}
