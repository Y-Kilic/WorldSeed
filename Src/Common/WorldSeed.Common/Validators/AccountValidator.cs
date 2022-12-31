using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.DTOS;

namespace WorldSeed.Common.Validators
{
    public class AccountValidator : AbstractValidator<AccountRegisterDTO>
    {
        public AccountValidator()
        {
            RuleFor(account => account.Username).NotNull().NotEmpty().Must(u => !u.Any(x => Char.IsWhiteSpace(x)));
            RuleFor(account => account.Email).NotNull().NotEmpty().EmailAddress().Must(u => !u.Any(x => Char.IsWhiteSpace(x)));
            RuleFor(account => account.Password).MinimumLength(8).Must(u => !u.Any(x => Char.IsWhiteSpace(x)));
        }

    }
}
