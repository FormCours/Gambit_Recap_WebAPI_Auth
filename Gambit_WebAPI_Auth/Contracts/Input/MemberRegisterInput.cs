namespace Gambit_WebAPI_Auth.Contracts.Input
{
    public class MemberRegisterInput
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
    }
}
