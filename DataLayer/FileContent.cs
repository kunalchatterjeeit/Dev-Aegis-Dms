using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataLayer
{
    public class FileContent
    {
        public FileContent() { }

        public static int FileContent_Save(Entity.FileContent fileContent)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileContent.FileGuid);
                oDm.Add("p_Content", MySqlDbType.LongText, fileContent.Content);

                oDm.CommandType = CommandType.StoredProcedure;
                int retVal = oDm.ExecuteNonQuery("usp_FileContent_Save");
                oDm.Dispose();
                return retVal;
            }
        }
    }
}
