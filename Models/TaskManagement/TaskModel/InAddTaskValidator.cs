using FluentValidation;
using TaskManagementPortal.Models.UserManagement;

namespace TaskManagementPortal.Models.TaskManagement.TaskModel
{
    public class InAddTaskValidator : AbstractValidator<AddTask>
    {
        public InAddTaskValidator()
        {
            RuleFor(x => x.TaskName).NotEmpty().WithMessage("Task name is required");

            RuleFor(x => x.Assignee).NotEmpty().WithMessage("Email address is required")
               .Matches(RegexValidators.EmailRegex)
               .WithMessage("Email address is invalid")
               .MaximumLength(100).WithMessage("Only 100 characters are allowed");
            
            RuleFor(x => x.Reporter).NotEmpty().WithMessage("Email address is required")
               .Matches(RegexValidators.EmailRegex)
               .WithMessage("Email address is invalid")
               .MaximumLength(100).WithMessage("Only 100 characters are allowed");

            RuleFor(x => x.Status).LessThan(3).WithMessage("Please select");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Please select");

            RuleFor(x => x.AttachFile).NotEmpty().WithMessage("Please Choose File");

            RuleFor(x => x.Notes).NotEmpty().WithMessage("Notes is required");



        }
    }
}
