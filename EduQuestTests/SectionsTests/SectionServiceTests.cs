using NUnit.Framework;
using Moq;
using EduQuest.Entities;
using EduQuest.Features.Sections;
using AutoMapper;

namespace EduQuestTests.SectionsTests
{
    [TestFixture]
    public class SectionServiceTests
    {
        private Mock<ISectionRepo> _mockSectionRepo;
        private IMapper _mapper;
        private SectionService _sectionService;

        [SetUp]
        public void Setup()
        {
            _mockSectionRepo = new Mock<ISectionRepo>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Section, SectionDto>();
                cfg.CreateMap<SectionDto, Section>();
            });
            _mapper = config.CreateMapper();

            _sectionService = new SectionService(_mockSectionRepo.Object, _mapper);
        }

        [Test]
        public async Task DeleteSectionsForCourse_ShouldReturnDeletedSections()
        {
            // Arrange
            int courseId = 1;
            var sections = new List<Section>
            {
                new Section { Id = 1, CourseId = courseId },
                new Section { Id = 2, CourseId = courseId }
            };

            _mockSectionRepo.Setup(repo => repo.DeleteByCourse(courseId)).ReturnsAsync(sections);

            // Act
            var result = await _sectionService.DeleteSectionsForCourse(courseId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(courseId, result[0].CourseId);
            Assert.AreEqual(courseId, result[1].CourseId);
        }

        [Test]
        public async Task GetSectionForCourse_ShouldReturnSectionsForGivenCourse()
        {
            // Arrange
            int courseId = 1;
            var sections = new List<Section>
            {
                new Section { Id = 1, CourseId = courseId, OrderId = 2 },
                new Section { Id = 2, CourseId = courseId, OrderId = 1 },
                new Section { Id = 3, CourseId = 2, OrderId = 1 }
            };

            _mockSectionRepo.Setup(repo => repo.GetAll()).ReturnsAsync(sections);

            // Act
            var result = await _sectionService.GetSectionForCourse(courseId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].OrderId);
            Assert.AreEqual(2, result[1].OrderId);
            Assert.AreEqual(courseId, result[0].CourseId);
            Assert.AreEqual(courseId, result[1].CourseId);
        }
    }
}
