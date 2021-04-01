namespace Jobz.WebUi.Security
{
    using System;
    using RootContracts.Security;

    public class TicketUser : ITicketUser
    {
        public string Email { get; set; }
        public Guid AppUserGuid { get; set; }
    }
}