using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.BLL.Mapper;
using Lab06.BLL.Services;
using Lab06.DAL.Entities;
using Lab06.DAL.Uow;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lab06.BLL.Tests.Services
{
    [TestFixture]
    public class ParkAttractionsServiceTests
    {
        private ParkAttractionsService _sut;
        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();

        [SetUp]
        public void Setup()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BllMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            _sut = new ParkAttractionsService(_unitOfWork.Object, mapper);
        }

        [Test]
        public async Task Create_ShouldReturnCreatedItem_WhenModelIsValid()
        {
            // Arrange
            var workingDirectory = Environment.CurrentDirectory;
            var imagePath = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName + "/Lab06.DAL/Resources/carousel.png";

            var expectedItem = new ParkAttractionDto
            {
                Id = 1,
                Name = "Test",
                Price = 3.00M,
                Image = File.ReadAllBytes(imagePath),
            };
            var parkAttraction = new ParkAttraction
            {
                Id = 1,
                Name = "Test",
                Price = 3.00M,
                AttractionImage = new AttractionImage
                {
                    Payload = File.ReadAllBytes(imagePath),
                }
            };

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().AddAsync(It.IsAny<ParkAttraction>()))
                .ReturnsAsync(parkAttraction);

            var expectedItemSerialized = JsonConvert.SerializeObject(expectedItem);

            // Act
            var actualResult = await _sut.Create(expectedItem);

            // Assert
            _unitOfWork.Verify(x => x.Repository<ParkAttraction>().AddAsync(It.Is<ParkAttraction>(_ => _.Name == expectedItem.Name)), Times.Once);
            _unitOfWork.Verify(_ => _.Complete(), Times.Once);
            _unitOfWork.VerifyNoOtherCalls();
            Assert.AreEqual(expectedItemSerialized, JsonConvert.SerializeObject(actualResult));
        }

        [Test]
        public async Task Create_ShouldReturnNull_WhenModelIsNull()
        {
            // Arrange
            ParkAttractionDto item = null;

            // Act
            var actualResult = await _sut.Create(item);

            // Assert
            Assert.IsNull(actualResult);
        }

        [Test]
        public async Task Create_ShouldReturnNull_WhenModelPriceIsNegative()
        {
            // Arrange
            var item = new ParkAttractionDto
            {
                Id = 1,
                Name = "Test",
                Price = -3.00M,
            };

            // Act
            var actualResult = await _sut.Create(item);

            // Assert
            Assert.IsNull(actualResult);
        }

        [Test]
        public async Task Create_ShouldReturnNull_WhenModelNameIsNull()
        {
            // Arrange
            var item = new ParkAttractionDto
            {
                Id = 1,
                Name = null,
                Price = 3.00M,
            };

            // Act
            var actualResult = await _sut.Create(item);

            // Assert
            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetAll_ShouldReturnEmptyEnumerable_WhenDbSetContainsNoItems()
        {
            // Arrange
            var expectedResult = Enumerable.Empty<ParkAttractionDto>();

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().GetAll())
                .Returns(() => null);

            // Act
            var actualResult = _sut.GetAll();

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void GetAll_ShouldReturnEnumerable_WhenDbSetContainsItems()
        {
            // Arrange
            var expectedResult = new List<ParkAttractionDto>
            {
                new ParkAttractionDto
                {
                    Id = 1,
                    Name = "Test1",
                    Price = 1.00M,
                    Image = Array.Empty<byte>(),
                },
                new ParkAttractionDto
                {
                    Id = 2,
                    Name = "Test2",
                    Price = 2.00M,
                    Image = Array.Empty<byte>(),
                }
            };

            var attractionImage = new List<AttractionImage>
            {
                new AttractionImage
                {
                    Name = "Test1",
                    Payload = Array.Empty<byte>(),
                },
            };

            var parkAttractions = new List<ParkAttraction>
            {
                new ParkAttraction
                {
                    Id = 1,
                    Name = "Test1",
                    Price = 1.00M,
                },
                new ParkAttraction
                {
                    Id = 2,
                    Name = "Test2",
                    Price = 2.00M,
                },
            };

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().GetAll())
                .Returns(parkAttractions.AsQueryable());
            _unitOfWork.Setup(x => x.Repository<AttractionImage>().FindByPredicate(It.IsAny<Func<AttractionImage, bool>>()))
               .Returns(attractionImage.AsQueryable());

            var expectedResultSerialized = JsonConvert.SerializeObject(expectedResult);

            // Act
            var actualResult = _sut.GetAll().ToList();

            // Assert
            Assert.AreEqual(expectedResultSerialized, JsonConvert.SerializeObject(actualResult));
        }

        [Test]
        public async Task GetPrice_ShouldReturnPrice_WhenAttractionExists()
        {
            // Arrange
            var attractionId = 1;
            var attractionPrice = 3.00M;
            var attraction = new ParkAttraction
            {
                Id = attractionId,
                Price = attractionPrice,
            };

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().GetByIdAsync(attractionId))
                .ReturnsAsync(attraction);

            // Act
            var actualPrice = await _sut.GetPriceAsync(attractionId);

            // Assert
            Assert.AreEqual(attractionPrice, actualPrice);
        }

        [Test]
        public async Task GetPrice_ShouldReturnZero_WhenAttractionDoesNotExists()
        {
            // Arrange
            var attractionId = 1;
            var expectedPrice = 0.00M;

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().GetByIdAsync(attractionId))
                .ReturnsAsync(() => null);

            // Act
            var actualPrice = await _sut.GetPriceAsync(attractionId);

            // Assert
            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnParkAttraction_WhenAttractionExists()
        {
            // Arrange
            var attractionId = 1;
            var attractionName = "Castle";
            var attractionPrice = 3.00M;
            var attraction = new ParkAttraction
            {
                Id = attractionId,
                Name = attractionName,
                Price = attractionPrice,
            };

            var attractionImage = new List<AttractionImage>()
            {
                new AttractionImage
                {
                    Id = 2,
                    AttractionId = 1,
                }
            };

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().GetByIdAsync(attractionId))
                .ReturnsAsync(attraction);
            _unitOfWork.Setup(x => x.Repository<AttractionImage>().FindByPredicate(It.IsAny<Func<AttractionImage, bool>>()))
                .Returns(attractionImage.AsQueryable());

            // Act
            var attractionDto = await _sut.GetByIdOrNullAsync(attractionId);

            // Assert
            Assert.AreEqual(attractionId, attractionDto.Id);
            Assert.AreEqual(attractionName, attractionDto.Name);
            Assert.AreEqual(attractionPrice, attractionDto.Price);
            Assert.IsEmpty(attractionDto.Image);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNothing_WhenAttractionDoesNotExists()
        {
            // Arrange
            var attractionId = 1;

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var attractionDto = await _sut.GetByIdOrNullAsync(attractionId);

            // Assert
            Assert.Null(attractionDto);
        }

        [Test]
        public void Update_ShouldReturnNull_WhenPassingNullItem()
        {
            // Arrange
            ParkAttractionDto item = null;

            // Act
            var actualResult = _sut.Update(item);

            // Assert
            Assert.IsNull(actualResult);
        }

        [Test]
        public void Update_ShouldReturnUpdatedAttraction_WhenPassingValidItem()
        {
            // Arrange
            byte[] fakeImage = { 0x32, 0x00, 0x1E, 0x00 };

            var expectedResult = new ParkAttractionDto
            {
                Id = 1,
                Name = "Test1",
                Price = 1.00M,
                Image = fakeImage,
            };

            var newAttraction = new ParkAttraction
            {
                Id = 1,
                Name = "Test1",
                Price = 1.00M,
                AttractionImage = new AttractionImage { AttractionId = 1, Payload = fakeImage },
            };

            var newAttractionDto = new ParkAttractionDto
            {
                Id = 1,
                Name = "Test1",
                Price = 1.00M,
                Image = fakeImage,
            };

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().Update(It.IsAny<ParkAttraction>())).Returns(newAttraction);
            _unitOfWork.Setup(x => x.Repository<AttractionImage>().FindByPredicate(It.IsAny<Func<AttractionImage, bool>>()))
                .Returns(Queryable.Append(new List<AttractionImage>().AsQueryable(), newAttraction.AttractionImage));

            var expectedResultSerialized = JsonConvert.SerializeObject(expectedResult);

            // Act
            var actualResult = _sut.Update(newAttractionDto);

            // Assert
            Assert.AreEqual(expectedResultSerialized, JsonConvert.SerializeObject(actualResult));
        }

        [Test]
        public void Update_ShouldReturnNull_WhenPassingInvalidItem()
        {
            // Arrange
            var newAttractionDto = new ParkAttractionDto
            {
                Id = 1,
                Price = 1.00M,
                Image = Array.Empty<byte>(),
            };

            // Act
            var actualResult = _sut.Update(newAttractionDto);

            // Assert
            Assert.IsNull(actualResult);
        }

        [Test]
        public async Task Delete_ShouldReturnZero_WhenItemDoesNotExist()
        {
            // Arrange
            var attractionId = 1;
            var expectedResult = 0;

            _unitOfWork.Setup(x => x.Repository<ParkAttraction>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var actualResult = await _sut.DeleteAsync(attractionId);

            // Assert
            Assert.AreEqual(actualResult, expectedResult);
        }
    }
}
