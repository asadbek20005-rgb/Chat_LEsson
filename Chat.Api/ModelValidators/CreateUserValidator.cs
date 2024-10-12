using Chat.Api.Models;
using FluentValidation;

namespace Chat.Api.ModelValidators
{
    public class CreateUserValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull().WithMessage("The first name can not be null")
                .WithErrorCode("The first name can not be null")
                .Length(2, 50).WithMessage("The firt name must be between 2 and 50 characters.")
                .Must(BeUpperLetter).WithMessage("The first letter must be upper in the first name");

            RuleFor(x => x.LastName)
                .NotNull().WithMessage("The last name can not be null")
                .Length(2, 50).WithMessage("The last name must be between 2 and 50 characters.")
                .Must(BeUpperLetter).WithMessage("The first letter must be upper in the last name");

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


            RuleFor(x => x).Custom((x, context) =>
            {
                if(x.Password != x.ConfirmPassword)
                    context.AddFailure(nameof(x.Password), "The password should match");
            });


            RuleFor(x => x.Gender)
                .Must(BeValidGender).WithMessage("The gender is incorrect.");
               
               
        }

        private bool BeUpperLetter(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
                return false;

            return char.IsUpper(firstName[0]);
        }

        private bool HaveUpperLetterOrDigit(string password)
        {
            if(!string.IsNullOrEmpty(password))
                foreach (var letter in password)
                {
                    char.IsUpper(letter);
                    char.IsDigit(letter);
                }
            return false;
        }


        private bool BeValidGender(string gender)
        {
            string[] genders = ["male", "female"];

            if (!string.IsNullOrEmpty(gender))
                return genders.Contains(gender);
            return false;
        }
    }
}
