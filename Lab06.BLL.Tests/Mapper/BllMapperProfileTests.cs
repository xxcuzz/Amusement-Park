using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.BLL.Mapper;
using Lab06.DAL.Entities;
using NUnit.Framework;

namespace Lab06.BLL.Tests.Mapper
{
    [TestFixture]
    public class BllMapperProfileTests
    {
        [Test]
        public void AutoMapper_Configuration_IsValid()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new BllMapperProfile()));

            configuration.AssertConfigurationIsValid();
        }

        [Test]
        public void AutoMapper_ConvertFromParkAttractionToParkAttractionDto_IsValid()
        {
            // Arrange
            byte[] fakeImage = { 0x32, 0x00, 0x1E, 0x00 };

            var parkAttraction = new ParkAttraction
            {
                Id = 1,
                Name = "Test",
                Price = 3.00M,
                AttractionImage = new AttractionImage
                {
                    Id = 1,
                    Payload = fakeImage,
                    AttractionId = 1,
                }
            };

            var expectedResult = new ParkAttractionDto
            {
                Id = 1,
                Name = "Test",
                Price = 3.00M,
                Image = fakeImage,
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BllMapperProfile>());
            var mapper = config.CreateMapper();

            // Act
            var parkAttractionDto = mapper.Map<ParkAttraction, ParkAttractionDto>(parkAttraction);

            // Assert
            Assert.AreEqual(parkAttractionDto.Id, expectedResult.Id);
            Assert.AreEqual(parkAttractionDto.Name, expectedResult.Name);
            Assert.AreEqual(parkAttractionDto.Price, expectedResult.Price);
            Assert.AreEqual(parkAttractionDto.Image, expectedResult.Image);
        }

        [Test]
        public void AutoMapper_ConvertFromParkAttractionDtoToParkAttraction_IsValid()
        {
            // Arrange
            var expectedId = 1;
            var expectedName = "Test";
            var expectedPrice = 3.00M;
            byte[] expectedImage = { 0x32, 0x00, 0x1E, 0x00 };

            var parkAttractionDto = new ParkAttractionDto
            {
                Id = 1,
                Name = "Test",
                Price = 3.00M,
                Image = new byte[] { 0x32, 0x00, 0x1E, 0x00 },
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BllMapperProfile>());
            var mapper = config.CreateMapper();

            // Act
            var parkAttraction = mapper.Map<ParkAttractionDto, ParkAttraction>(parkAttractionDto);

            // Assert
            Assert.AreEqual(parkAttraction.Id, expectedId);
            Assert.AreEqual(parkAttraction.Name, expectedName);
            Assert.AreEqual(parkAttraction.Price, expectedPrice);
            Assert.AreEqual(parkAttraction.AttractionImage.Payload, expectedImage);
        }

        [Test]
        public void AutoMapper_ConvertFromUserTicketDtoToUserTicket_IsValid()
        {
            // Arrange
            var userTicketDto = new UserTicketDto
            {
                ParkAttractionId = 1,
                ApplicationUserId = "Test",
            };

            var expectedResult = new UserTicket
            {
                ParkAttractionId = 1,
                ApplicationUserId = "Test",
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BllMapperProfile>());
            var mapper = config.CreateMapper();

            // Act
            var userTicket = mapper.Map<UserTicketDto, UserTicket>(userTicketDto);

            // Assert
            Assert.AreEqual(userTicket.ApplicationUserId, expectedResult.ApplicationUserId);
            Assert.AreEqual(userTicket.ParkAttractionId, expectedResult.ParkAttractionId);
        }

        [Test]
        public void AutoMapper_ConvertFromUserTicketToUserTicketDto_IsValid()
        {
            // Arrange
            var userTicket = new UserTicket
            {
                ParkAttractionId = 1,
                ApplicationUserId = "Test",
            };

            var expectedResult = new UserTicketDto
            {
                ParkAttractionId = 1,
                ApplicationUserId = "Test",
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BllMapperProfile>());
            var mapper = config.CreateMapper();

            // Act
            var userTicketDto = mapper.Map<UserTicket, UserTicketDto>(userTicket);

            // Assert
            Assert.AreEqual(userTicketDto.ApplicationUserId, expectedResult.ApplicationUserId);
            Assert.AreEqual(userTicketDto.ParkAttractionId, expectedResult.ParkAttractionId);
        }
    }
}
