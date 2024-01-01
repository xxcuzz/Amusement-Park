using Lab06.BLL.EntitiesDTO;
using Lab06.BLL.Validation;
using NUnit.Framework;
using System;

namespace Lab06.BLL.Tests.Validation
{
    [TestFixture]
    public class AttractionValidatorTests
    {
        [Test]
        public void Validate_ShouldReturnTrue_WhenModelIsValid()
        {
            // Arrange
            var parkAttractionDto = new ParkAttractionDto
            {
                Id = 1,
                Name = "Name",
                Price = 3.00M,
                Image = Array.Empty<byte>(),
            };

            // Act
            var actualResult = AttractionValidator.Validate(parkAttractionDto);

            // Assert
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void Validate_ShouldReturnFalse_WhenNameIsNull()
        {
            // Arrange
            var parkAttractionDto = new ParkAttractionDto
            {
                Id = 1,
                Name = null,
                Price = 3.00M,
                Image = Array.Empty<byte>(),
            };

            // Act
            var actualResult = AttractionValidator.Validate(parkAttractionDto);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Validate_ShouldReturnFalse_WhenNameIsEmpty()
        {
            // Arrange
            var parkAttractionDto = new ParkAttractionDto
            {
                Id = 1,
                Name = string.Empty,
                Price = 3.00M,
                Image = Array.Empty<byte>(),
            };

            // Act
            var actualResult = AttractionValidator.Validate(parkAttractionDto);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Validate_ShouldReturnFalse_WhenPriceIsNegative()
        {
            // Arrange
            var parkAttractionDto = new ParkAttractionDto
            {
                Id = 1,
                Name = "Name",
                Price = -3.00M,
                Image = Array.Empty<byte>(),
            };

            // Act
            var actualResult = AttractionValidator.Validate(parkAttractionDto);

            // Assert
            Assert.IsFalse(actualResult);
        }
    }
}
