namespace Jobz.WebUi.Sprinkler_Api.Models
{
    using System.Collections.Generic;

    public class StandardResponse<T>
    {
        public StandardResponse()
        {
            this.ValidationMessages = new List<string>();
        }

        public T Payload { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public List<string> ValidationMessages { get; set; }
    }
}
