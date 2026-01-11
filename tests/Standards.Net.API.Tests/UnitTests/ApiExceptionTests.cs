using AutoFixture;
using Standards.Net.API.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Standards.Net.API.Tests.UnitTests.Exceptions;

[TestFixture]
public class ApiExceptionTests
{
    private Fixture _fixture = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void NotFoundException_ShouldHaveCorrectStatusCode()
    {
        // Arrange & Act
        var exception = new NotFoundException("User", 123);

        // Assert
        exception.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        exception.ResourceType.Should().Be("User");
        exception.ResourceId.Should().Be(123);
        exception.Message.Should().Contain("User");
        exception.Message.Should().Contain("123");
    }

    [Test]
    public void ConflictException_ShouldHaveCorrectStatusCode()
    {
        // Arrange & Act
        var exception = new ConflictException("Email", "test@example.com");

        // Assert
        exception.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        exception.ResourceType.Should().Be("Email");
        exception.ConflictingValue.Should().Be("test@example.com");
    }

    [Test]
    public void ValidationException_ShouldHandleSingleError()
    {
        // Arrange & Act
        var exception = new ValidationException("Email", "Invalid email format");

        // Assert
        exception.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        exception.Errors.Should().ContainKey("Email");
        exception.Errors["Email"].Should().Contain("Invalid email format");
    }

    [Test]
    public void ValidationException_ShouldHandleMultipleErrors()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            ["Email"] = new[] { "Email is required", "Invalid email format" },
            ["Password"] = new[] { "Password must be at least 8 characters" },
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        exception.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        exception.Errors.Should().HaveCount(2);
        exception.Errors["Email"].Should().HaveCount(2);
        exception.Errors["Password"].Should().HaveCount(1);
    }

    [Test]
    public void ForbiddenException_ShouldHaveCorrectStatusCode()
    {
        // Arrange & Act
        var exception = new ForbiddenException("You do not have permission");

        // Assert
        exception.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        exception.Message.Should().Be("You do not have permission");
    }

    [Test]
    public void UnauthorizedException_ShouldHaveCorrectStatusCode()
    {
        // Arrange & Act
        var exception = new UnauthorizedException();

        // Assert
        exception.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Test]
    public void BadRequestException_ShouldHaveCorrectStatusCode()
    {
        // Arrange & Act
        var exception = new BadRequestException("Invalid request");

        // Assert
        exception.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        exception.Message.Should().Be("Invalid request");
    }

    [Test]
    public void ApiException_ShouldAllowAddingErrorDetails()
    {
        // Arrange
        var exception = new NotFoundException("User", 123);

        // Act
        exception.AddErrorDetail("Timestamp", DateTime.UtcNow);
        exception.AddErrorDetail("RequestId", "abc-123");

        // Assert
        exception.ErrorDetails.Should().NotBeNull();
        exception.ErrorDetails.Should().ContainKey("Timestamp");
        exception.ErrorDetails.Should().ContainKey("RequestId");
    }
}
