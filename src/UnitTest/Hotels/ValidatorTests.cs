using FluentValidation.TestHelper;
using Lemax.Application.Hotels;
using Lemax.Application.Validation;
using Xunit;

namespace Lemax.UnitTests.Hotels;

public class ValidatorTests
{
    private readonly CreateHotelRequestValidator _createValidator = new();
    private readonly SearchHotelRequestValidator _searchValidator = new();

    [Fact]
    public void CreateHotelRequest_ShouldHaveError_WhenCoordinatesAreInvalid()
    {
        // 1. Arrange: Postavljamo neispravne koordinate (100, 200)
        // Ime je obavezno zbog 'required' modifikatora
        var model = new CreateHotelRequest
        {
            Name = "Test Hotel",
            Latitude = 100,
            Longitude = 200,
            Price = 100
        };

        // 2. Act
        var result = _createValidator.TestValidate(model);

        // 3. Assert: Provjera da li validator hvata koordinate izvan opsega
        result.ShouldHaveValidationErrorFor(x => x.Latitude);
        result.ShouldHaveValidationErrorFor(x => x.Longitude);
    }

    [Fact]
    public void CreateHotelRequest_ShouldHaveError_WhenPriceIsNegative()
    {
        // 1. Arrange: Postavljamo negativnu cijenu (-10)
        // Moramo postaviti i Name jer je 'required'
        var model = new CreateHotelRequest
        {
            Name = "Test Hotel",
            Price = -10, // Ovo aktivira .GreaterThan(0) pravilo
            Latitude = 45.8,
            Longitude = 15.9
        };

        // 2. Act
        var result = _createValidator.TestValidate(model);

        // 3. Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void SearchHotelRequest_ShouldBeValid_WhenDataIsCorrect()
    {
        // Provjera ispravnosti Search zahtjeva s koordinatama i paginacijom
        var model = new SearchHotelRequest
        {
            Latitude = 45.8060,
            Longitude = 15.9780,
            Page = 1,
            PageSize = 10
        };

        var result = _searchValidator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}