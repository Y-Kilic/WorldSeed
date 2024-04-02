using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.DTOS;

namespace WorldSeed.Common.Validators
{
    public class UserValidator : AbstractValidator<CreateUserDTO>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName).NotNull().NotEmpty();
            // No whitespaces
            RuleFor(user => user.UserName).Must(u => !u.Any(x => Char.IsWhiteSpace(x)));
            // Only letters and numbers
            RuleFor(user => user.UserName).Matches(@"^[a-zA-Z0-9]+$");
        }

    }
}
