using Entities.Dtos;
using FluentValidation;
using FluentValidation.Results;

namespace Business.Validation.FluentValidation
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x=>x.Email).NotEmpty().WithMessage("Validation error: Email name is empty").EmailAddress().WithMessage("A valid email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Validation error: Password name is empty");
        }

        protected override bool PreValidate(ValidationContext<UserLoginDto> context, ValidationResult result)
        {
            if (context.InstanceToValidate==null)
            {
                result.Errors.Add(new ValidationFailure("", "Validation error: User login data is null"));
                return false;
            }
            return true;
        }
    }
}
