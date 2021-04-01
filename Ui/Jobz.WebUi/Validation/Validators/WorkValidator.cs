namespace Jobz.WebUi.Validation.Validators
{
    using FluentValidation;
    using Models;
    using RootContracts.DataContracts.Domain;

    public partial class WorkValidator : AbstractValidator<WorkUpdateModel>
    {
        public WorkValidator()
        {
            RuleFor(obj => obj.WorkGuid).NotEmpty();
            RuleFor(obj => obj.ContractorGuid).NotEmpty();
            RuleFor(obj => obj.JobGuid).NotEmpty();
            RuleFor(obj => obj.DateOfWork).NotEmpty();
            RuleFor(obj => obj.Hours).NotEmpty();
        }
    }
}

