namespace Gambit_WebAPI_Auth.Domain.Models
{
    public class Member
    {
        public enum RoleEnum
        {
            None = 0,
            Admin = 1,
            SuperAdmin = 2,
        }

        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required RoleEnum Role { get; set; }
        public required string  Firstname { get; set; }
        public required string Lastname { get; set; }
    }
}
