using Chat.Api.Models.UserModel;
using FluentValidation;

namespace Chat.Api.ModelValidators
{
    public class LoginUserValidator : AbstractValidator<LoginUserModel>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Username)
               .NotNull().WithMessage("The username can not be null")
               .NotEmpty().WithMessage("The username can not be empty")
               .Length(3, 20).WithMessage("The username must be between 3 and 20 characters.")
               .Matches("^[a-zA-Z0-9]*$").WithMessage("Username must contain only letters and numbers.");


            RuleFor(x => x.Password)
                .NotNull().WithMessage("The password can not be null")
                .NotEmpty().WithMessage("The password can not be empty")
                .MinimumLength(8).WithMessage("The password must contain minimum 8 characters")
                .Must(HaveUpperLetterOrDigit).WithMessage("The password must contain minimum 1 uppder character or 1 digit")
                .Matches("[^a-zA-Z0-9]").WithMessage("The password must contain at least one special character.");
        }

        private bool HaveUpperLetterOrDigit(string password)
        {
            if (!string.IsNullOrEmpty(password))
                foreach (var letter in password)
                {
                    char.IsUpper(letter);
                    char.IsDigit(letter);
                }
            return false;
        }

    }
}