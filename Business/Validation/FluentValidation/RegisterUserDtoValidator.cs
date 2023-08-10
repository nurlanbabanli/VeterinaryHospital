using Entities.Dtos;
using FluentValidation;
using FluentValidation.Results;

namespace Business.Validation.FluentValidation
{
    public class RegisterUserDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Validation error: First name is empty");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Validation error: Last name is empty");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Validation error: Email name is empty").EmailAddress().WithMessage("A valid email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Validation error: Password name is empty");
        }

        protected override bool PreValidate(ValidationContext<UserRegisterDto> context, ValidationResult result)
        {
            if (context.InstanceToValidate==null)
            {
                result.Errors.Add(new ValidationFailure("", "Validation error: User data is null"));
                return false;
            }
            return true;
        }
    }
}
