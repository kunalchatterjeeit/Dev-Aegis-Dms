using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer
{
    public class Chart
    {
        public static List<Entity.BarChartResponse> File_GetByDateRange(DateTime fromDate, DateTime toDate, int userId)
        {
            List<Entity.BarChartResponse> barChartResponse = new List<Entity.BarChartResponse>();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FromDate", MySqlDbType.DateTime, fromDate);
                oDm.Add("p_ToDate", MySqlDbType.DateTime, toDate);
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);

                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_File_GetByDateRange"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime entryDate = new DateTime();
                            barChartResponse.Add( new Entity.BarChartResponse() {
                                Date = DateTime.TryParse(reader["EntryDate"].ToString(), out entryDate) ? entryDate.ToString("dd/MM") : string.Empty,
                                FileCount = Convert.ToInt32(reader["count"].ToString())
                            });
                        }
                        oDm.Dispose();
                    }
                }
            }
            return barChartResponse;
        }

        public static decimal File_GetSize(int userId)
        {
            decimal fileSize = 0;
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);

                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_File_GetSize"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            fileSize += Convert.ToDecimal(reader["TotalSizeInKb"].ToString());
                        }
                        oDm.Dispose();
                    }
                }
            }
            return fileSize;
        }

        public static int File_GetFileCount(int userId)
        {
            int fileCount = 0;
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);

                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_File_GetFileCount"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            fileCount += Convert.ToInt32(reader["TotalUploadedFile"].ToString());
                        }
                        oDm.Dispose();
                    }
                }
            }
            return fileCount;
        }

    }
}
