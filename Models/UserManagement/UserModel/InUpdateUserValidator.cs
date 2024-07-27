using FluentValidation;
using TaskManagementPortal;
using TaskManagementPortal.Models.UserManagement;

namespace EKMSPortal.Models.UserModel.Validators
{
    public class InUpdateUserValidator : AbstractValidator<UpdateUser>
    {
        public InUpdateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required")
               .Matches(RegexValidators.EmailRegex)
               .WithMessage("Email address is invalid")
               .MaximumLength(100).WithMessage("Only 100 characters are allowed");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                    .Matches(RegexValidators.NameRegex)
                    .WithMessage("Name must be between 4 and 50 characters. Alphanumeric, dash and underscore are allowed");
            RuleFor(x => x.ContactNo).NotEmpty().WithMessage("Mobile number is required")
                    .Matches(RegexValidators.MobileRegex).WithMessage("Only 10 digits are allowed");
            RuleFor(x => x.IsAdmin).NotNull().WithMessage("Please notify whether user is normal or approver");
        }
    }
}
