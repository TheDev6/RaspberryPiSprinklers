namespace Jobz.RootContracts.Security
{
    using System;

    public interface ITicketUser
    {
        string Email { get; set; }
        Guid AppUserGuid { get; set; }
    }
}
