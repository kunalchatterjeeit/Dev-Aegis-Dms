using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class Chart
    {
        public Chart()
        {

        }

        public List<Entity.BarChartResponse> File_GetByDateRange(DateTime fromDate, DateTime toDate, int userId)
        {
            return DataLayer.Chart.File_GetByDateRange(fromDate, toDate, userId);
        }

        public decimal File_GetSize(int userId)
        {
            return DataLayer.Chart.File_GetSize(userId);
        }

        public int File_GetFileCount(int userId)
        {
            return DataLayer.Chart.File_GetFileCount(userId);
        }
    }
}
