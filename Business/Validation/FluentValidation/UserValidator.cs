using Core.Entities.Concrete;
using FluentValidation;
using FluentValidation.Results;

namespace Business.Validation.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Validation error: First name is empty");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Validation error: Last name is empty");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Validation error: Email name is empty");
            RuleFor(x => x.PasswordSalt).NotEmpty().WithMessage("Validation error: PasswordS is empty");
            RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("Validation error: PasswordH is empty");
        }

        protected override bool PreValidate(ValidationContext<User> context, ValidationResult result)
        {
            if(context.InstanceToValidate==null)
            {
                result.Errors.Add(new ValidationFailure("", "Validation error: UserM data is null"));
                return false;
            }
            return true;
        }
    }
}
