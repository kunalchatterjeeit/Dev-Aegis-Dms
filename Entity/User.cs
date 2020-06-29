using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class User
    {
        public User()
        { }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int CreatedBy { get; set; }
        public int Status { get; set; }
        public string EmailId { get; set; }
        public int DesignationId { get; set; }
    }

    public enum UserStatus
    {
        InActive = 0,
        Active = 1
    }
}
