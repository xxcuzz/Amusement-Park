using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.BLL.Mapper;
using Lab06.BLL.Services;
using Lab06.BLL.Services.Interfaces;
using Lab06.DAL.Entities;
using Lab06.DAL.Uow;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Lab06.BLL.Tests.Services
{
    [TestFixture]
    public class UserTicketServiceTests
    {
        private UserTicketService _sut;
        private readonly Mock<IUserService> _userService = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        
        [SetUp]
        public void Setup()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BllMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            _sut = new UserTicketService(_userService.Object, _unitOfWork.Object, mapper);
        }

        [Test]
        public async Task CreateTicket_ShouldReturnTicket_WhenUserAndParkAttractionExists()
        {
            // Arrange
            var userId = "test";
            var attractionId = 1;
            var expectedResult = new UserTicketDto()
            {
                ApplicationUserId = userId,
                ParkAttractionId = attractionId,
            };

            _userService.Setup(x => x.GetUserById(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser() { Id = userId });

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ParkAttraction() { Id = attractionId });

            _unitOfWork.Setup(x => x.Repository<UserTicket>().AddAsync(It.IsAny<UserTicket>()))
                .ReturnsAsync(new UserTicket { ApplicationUserId = userId, ParkAttractionId = attractionId });

            var expectedResultSerialized = JsonConvert.SerializeObject(expectedResult);

            // Act
            var actualResult = await _sut.CreateTicket(userId, attractionId);

            // Assert
            _userService.Verify(_ => _.GetUserById(It.Is<string>(_ => _.Contains(userId))), Times.AtLeastOnce);
            _unitOfWork.Verify(_ => _.Repository<ParkAttraction>().GetByIdAsync(It.Is<int>(_ => _.Equals(attractionId))), Times.AtLeastOnce);
            _unitOfWork.Verify(_ => _.Repository<UserTicket>().AddAsync(It.Is<UserTicket>(_ => _.ApplicationUserId == userId && _.ParkAttractionId == attractionId)), Times.Once);
            _unitOfWork.Verify(_ => _.Complete(), Times.Once);
            _userService.VerifyNoOtherCalls();
            _unitOfWork.VerifyNoOtherCalls();

            Assert.AreEqual(expectedResultSerialized, JsonConvert.SerializeObject(actualResult));
        }

        [Test]
        public async Task CreateTicket_ShouldReturnNull_WhenUserDoesNotExists()
        {
            // Arrange
            var userId = "test";
            var attractionId = 1;

            _userService.Setup(x => x.GetUserById(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ParkAttraction() { Id = attractionId });


            // Act
            var actualResult = await _sut.CreateTicket(userId, attractionId);

            // Assert
            Assert.IsNull(actualResult);
        }

        [Test]
        public async Task CreateTicket_ShouldReturnNull_WhenParkAttractionDoesNotExists()
        {
            // Arrange
            var userId = "test";
            var attractionId = 1;

            _userService.Setup(x => x.GetUserById(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser() { Id = userId });

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var actualResult = await _sut.CreateTicket(userId, attractionId);

            // Assert
            Assert.IsNull(actualResult);
        }

        [Test]
        public void DeleteTicket_ShouldReturnZero_WhenTicketDoesNotExists()
        {
            // Arrange
            var userId = "test";
            var attractionId = 1;
            var expectedResult = 0;
            
            _unitOfWork.Setup(x => x.Repository<UserTicket>().FindByPredicate(It.IsAny<Func<UserTicket, bool>>()))
                .Returns(() => null);

            // Act
            var actualResult = _sut.DeteleTicket(userId, attractionId);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void GetByUserId_ShouldReturnNull_WhenTicketsDoesNotExists()
        {
            // Arrange
            var userId = "test";
            _unitOfWork.Setup(x => x.Repository<UserTicket>().FindByPredicate(It.IsAny<Func<UserTicket, bool>>()))
                .Returns(() => null);
            // Act
            var actualResult = _sut.GetByUserId(userId);

            // Assert
            Assert.IsNull(actualResult);
        }
    }
}
