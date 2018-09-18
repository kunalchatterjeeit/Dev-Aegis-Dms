namespace Entity
{
    public class Auth
    {
        public int UserId { get; set; }
        public string IP { get; set; }
        public LoginStatus Status { get; set; }
        public string Client { get; set; }
        public string FailedUserName { get; set; }
        public string FailedPassword { get; set; }
    }

    public enum LoginStatus
    {
        Failed = 0,
        Success = 1,
        WrongPassword = 2
    }
}
