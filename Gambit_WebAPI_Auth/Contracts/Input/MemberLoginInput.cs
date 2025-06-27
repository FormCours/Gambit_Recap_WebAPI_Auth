namespace Gambit_WebAPI_Auth.Contracts.Input
{
    public class MemberLoginInput
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
