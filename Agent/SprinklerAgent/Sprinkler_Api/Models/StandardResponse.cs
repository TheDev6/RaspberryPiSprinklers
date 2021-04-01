namespace SprinklerAgent.Sprinkler_Api.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class StandardResponse<T>
    {
        public StandardResponse()
        {
            this.ValidationMessages = new List<string>();
        }

        public T Payload { get; set; }
        public bool IsSuccess => this.ValidationMessages.Count == 0;

        private int? _statusCode;
        public int StatusCode
        {
            get
            {
                var result = _statusCode
                             ?? (this.ValidationMessages?.Any() == true
                                 ? 400
                                 : 200);
                return result;
            }
            set => _statusCode = value;
        }

        public List<string> ValidationMessages { get; set; }
    }
}