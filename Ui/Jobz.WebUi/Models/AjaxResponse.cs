namespace Jobz.WebUi.Models
{
    using System;
    using RootContracts.DataContracts.Validation;
    using Validation;

    public class AjaxResponse<T>
    {
        public AjaxResponse()
        {
           this.ValidationResult = new ValidationResult();
        }
        public ValidationResult ValidationResult { get; set; }
        public T Payload { get; set; }
        public Exception Exception { get; set; }
    }
}