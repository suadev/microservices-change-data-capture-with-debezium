
using System;
using MediatR;

namespace Services.Identity.Commands
{
    public class RegisterUserCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public RegisterUserCommand()
        {
            Id = Guid.NewGuid();
        }
    }
}