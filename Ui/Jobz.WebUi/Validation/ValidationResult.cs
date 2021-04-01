namespace Jobz.WebUi.Validation
{
    using System.Collections.Generic;
    using RootContracts.DataContracts.Validation;
    public class ValidationResult 
    {
        public ValidationResult()
        {
            this.ValidationFailures = new List<ValidationFailure>();
        }

        public bool IsValid => this.ValidationFailures.Count == 0;

        public List<ValidationFailure> ValidationFailures { get; set; }
    }
}