namespace Jobz.WebUi.Sprinkler_Api.Models
{
    using System;
    using System.Collections.Generic;

    public class AppUserModel
    {
        public Guid UserUid { get; set; }
        public string Username { get; set; }
        public List<string> Claims { get; set; }
    }
}
