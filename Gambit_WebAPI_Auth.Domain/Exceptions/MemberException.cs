namespace Gambit_WebAPI_Auth.Domain.Exceptions
{
    public class MemberException : Exception
    {
        public MemberException(string? message) : base(message) { }
    }

    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException() : base("Member not found !") { }
    }

    public class MemberBadCredentialException : Exception
    {
        public MemberBadCredentialException() : base("Invalid credential !") { }
    }
    public class MemberAlReadyExistsException : Exception
    {
        public MemberAlReadyExistsException() : base("Member already exists !") { }
    }
}
