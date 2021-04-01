namespace SprinklerApi.Models
{
    using System.Collections.Generic;

    public class AppUserCreateModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Claims { get; set; }
    }
}
