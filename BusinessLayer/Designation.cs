using System.Collections.Generic;

namespace BusinessLayer
{
    public class Designation
    {
        public Designation()
        {

        }

        public List<Entity.Designation> Designation_GetAll()
        {
            return DataLayer.Designation.Designation_GetAll();
        }
    }
}
