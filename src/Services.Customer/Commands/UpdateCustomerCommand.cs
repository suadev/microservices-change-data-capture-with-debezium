using System;
using MediatR;

namespace Services.Customer.Commands
{
    public class UpdateCustomerCommand : IRequest
    {
        public string Email { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }

        public UpdateCustomerCommand SetEmail(string email)
        {
            Email = email;
            return this;
        }
    }
}