using FluentValidation;
using TaskManagementPortal;
using TaskManagementPortal.Models.LoginManagement;

namespace TaskManagementPortal.Models.LoginManagement.LoginModel
{
    public class AuthenticateUserValidator : AbstractValidator<AuthenticateUser>
    {
        public AuthenticateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter email")
                .Matches(RegexValidators.EmailRegex).WithMessage("Please enter valid email address");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter password");
        }
    }
}