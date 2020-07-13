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
        public string Username { get; set; }
        public string Password { get; set; }
        public object Token { get; set; }
        public string Message { get; set; }
        public string[] Roles { get; set; }
    }

    public enum LoginStatus
    {
        Failed = 0,
        Success = 1,
        WrongPassword = 2
    }
}
