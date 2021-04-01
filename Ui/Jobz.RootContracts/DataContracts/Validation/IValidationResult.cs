namespace Jobz.RootContracts.DataContracts.Validation
{
    using System.Collections.Generic;

    public interface IValidationResult
    {
        bool IsValid { get; }
        List<IValidationFailure> ValidationFailures { get; set; }
    }
}