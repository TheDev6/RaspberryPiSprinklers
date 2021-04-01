namespace Jobz.WebUi.Utilities
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    /// <summary>
    /// MVC4 doesn't have a simple way to set a different default Json Serializer so this is a solution from newtonsoft
    /// http://james.newtonking.com/archive/2008/10/16/asp-net-mvc-and-json-net.aspx
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        private readonly Encoding _contentEncoding = Encoding.Default;
        private readonly string _contentType = "application/json";

        public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml
        };

        private readonly JsonSerializer _serializer = JsonSerializer.Create(SerializerSettings);

        public JsonNetResult() : base()
        {
        }

        public JsonNetResult(object data, Encoding encoding = null, string contentType = null)
        {
            base.Data = data;
            if (encoding != null) _contentEncoding = encoding;
            if (contentType != null) _contentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = _contentType;

            if (_contentEncoding != null) response.ContentEncoding = _contentEncoding;

            if (base.Data != null)
            {
                var writer = new JsonTextWriter(response.Output);
                _serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}