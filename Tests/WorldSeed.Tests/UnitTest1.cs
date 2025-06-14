using WorldSeed.Common.Validators;
using WorldSeed.Application.DTOS;

namespace WorldSeed.Tests;

public class UnitTest1
{
    [Fact]
    public void UserName_WithWhitespace_ShouldFailValidation()
    {
        var validator = new UserValidator();
        var dto = new CreateUserDTO { UserName = "Invalid Name" };

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
    }
}
