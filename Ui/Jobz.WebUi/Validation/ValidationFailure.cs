
namespace Jobz.WebUi.Validation
{
    using RootContracts.DataContracts.Validation;
    public class ValidationFailure : IValidationFailure
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
    }
}