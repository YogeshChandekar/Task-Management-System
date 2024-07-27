using FluentValidation;
using TaskManagementPortal;
using TaskManagementPortal.Models.UserManagement;

namespace EKMSPortal.Models.UserModel.Validators
{
    public class InAddUserValidator : AbstractValidator<AddUser>
    {
        public InAddUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required")
                .Matches(RegexValidators.EmailRegex)
                .WithMessage("Email address is invalid")
                .MaximumLength(100).WithMessage("Only 100 characters are allowed");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .Matches(RegexValidators.NameRegex)
                .WithMessage("Name should start with alphabets and must be between 4 to 50 characters. Alphanumeric, dash and underscore are allowed");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Mobile number is required")
                .Matches(RegexValidators.MobileRegex).WithMessage("Only 10 digits are allowed");
            RuleFor(x => x.IsAdmin).LessThan(3).WithMessage("Please select");
            RuleFor(x => x.IsAdmin).NotEmpty().WithMessage("Please select");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter password")
            .Matches(RegexValidators.PasswordRegex)
            .WithMessage("Password must be between 8 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character and these !*@$%^&+?= allowed");
        }
    }
}
