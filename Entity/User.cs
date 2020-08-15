using System;
using System.Collections.Generic;

namespace Entity
{
    public class User : BaseEntity
    {
        public User()
        {
            SelectedUserGroups = new List<int>();
            SelectedUserRoles = new List<int>();
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int CreatedBy { get; set; }
        public UserStatus Status { get; set; }
        public string EmailId { get; set; }
        public int DesignationId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DesignationName { get; set; }
        public ApplicationLoginStatus LoginStatus { get; set; }
        public List<int> SelectedUserGroups { get; set; }
        public List<int> SelectedUserRoles { get; set; }
    }

    public enum UserStatus
    {
        InActive = 0,
        Active = 1
    }

    public enum ApplicationLoginStatus
    {
        InActive = 0,
        Active = 1
    }
}
