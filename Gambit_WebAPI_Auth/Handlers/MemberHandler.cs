using Gambit_WebAPI_Auth.Contracts.Input;
using Gambit_WebAPI_Auth.Domain.Exceptions;
using Gambit_WebAPI_Auth.Domain.Models;
using Isopoh.Cryptography.Argon2;

namespace Gambit_WebAPI_Auth.Handlers
{
    public class MemberHandler
    {
        private static List<Member> _internalMembers = [
            new Member() {
                Id = 1,
                Username = "max",
                Password = "$argon2id$v=19$m=16,t=2,p=1$eE42TnR1OGZmSDVMTk84TQ$RR8AcsQH5ZfzKIepMRuLJA",
                Firstname = "Maxime",
                Lastname = "Gambit",
                Role = Member.RoleEnum.Admin
            },
            new Member() {
                Id = 2,
                Username = "della",
                Password = "$argon2id$v=19$m=16,t=2,p=1$M0gwM256NWJSTDJNNkFGYQ$Yx/YiBTWmpcbQFD5JsntUQ",
                Firstname = "Della",
                Lastname = "Duck",
                Role = Member.RoleEnum.SuperAdmin
            },
            new Member() {
                Id = 3,
                Username = "lena",
                Password = "$argon2id$v=19$m=16,t=2,p=1$RGV4dTF3R2Nra1V5RlVsbA$81+tkM67H4e9v41hG2SddQ",
                Firstname = "Lena",
                Lastname = "De Sortilege",
                Role = Member.RoleEnum.None
            },
        ];
        private static int _nextMemberId = 4;

        internal async Task<Member> Login(MemberLoginInput memberLogin)
        {
            await Task.Delay(100);

            Member? member = _internalMembers.SingleOrDefault(m => m.Username.ToLower().Trim() == memberLogin.Username.ToLower().Trim());

            if (member is null) {
                throw new MemberNotFoundException();
            }

            if(!Argon2.Verify(member.Password, memberLogin.Password))
            {
                throw new MemberBadCredentialException();
            }

            return member;
        }

        internal async Task<Member> Register(MemberRegisterInput memberRegister)
        {
            await Task.Delay(100);

            if(_internalMembers.Any(m => m.Username.ToLower().Trim() == memberRegister.Username.ToLower().Trim()))
            {
                throw new MemberAlReadyExistsException();
            }

            Member memberCreated = new Member()
            {
                Id = _nextMemberId++,
                Username = memberRegister.Username.ToLower().Trim(),
                Password = Argon2.Hash(memberRegister.Password),
                Role = Member.RoleEnum.None,
                Firstname = memberRegister.Firstname.Trim(),
                Lastname = memberRegister.Lastname.Trim(),
            };
            _internalMembers.Add(memberCreated);

            return memberCreated;
        }
    }
}
