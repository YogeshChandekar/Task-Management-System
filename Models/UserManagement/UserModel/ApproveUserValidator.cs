using FluentValidation;
using TaskManagementPortal.Models.UserManagement;

namespace EKMSPortal.Models.UserModel.Validators
{
    public class ApproveUserValidator : AbstractValidator<ApproveUser>
    {
        public ApproveUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.ApprovedBy).NotEmpty().WithMessage("Approver username is required");
        }
    }
}
