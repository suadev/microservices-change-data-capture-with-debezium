namespace Services.Identity.Events
{
    public class CustomerUpdatedEvent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}