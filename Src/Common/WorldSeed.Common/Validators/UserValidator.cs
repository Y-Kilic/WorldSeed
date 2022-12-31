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
            RuleFor(user => user.UserName).NotNull();
            RuleFor(user => user.UserName).NotEmpty();
            RuleFor(user => user.UserName).Must(u => !u.Any(x => Char.IsWhiteSpace(x)));
        }

    }
}
