using AutoMapper;
using EduQuest.Entities;
using EduQuest.Features.Notes;
using Moq;

namespace EduQuestTests.Notes;

[TestFixture]
public class NotesServiceTests
{
    private Mock<INotesRepo> _mockNotesRepo;
    private Mock<IMapper> _mockMapper;
    private NotesService _notesService;

    [SetUp]
    public void Setup()
    {
        _mockNotesRepo = new Mock<INotesRepo>();
        _mockMapper = new Mock<IMapper>();
        _notesService = new NotesService(_mockNotesRepo.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetNotesForContent_ReturnsNote_WhenNoteExists()
    {
        // Arrange
        int contentId = 1;
        var notes = new List<Note>
        {
            new Note { Id = 1, ContentId = 1, NoteContent = "Test note 1" },
            new Note { Id = 2, ContentId = 2, NoteContent= "Test note 2" }
        };
        var expectedNoteDto = new NoteDto { Id = 1, ContentId = 1, NoteContent = "Test note 1" };

        _mockNotesRepo.Setup(repo => repo.GetAll()).ReturnsAsync(notes);
        _mockMapper.Setup(mapper => mapper.Map<NoteDto>(It.IsAny<Note>())).Returns(expectedNoteDto);

        // Act
        var result = await _notesService.GetNotesForContent(contentId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(expectedNoteDto.Id));
        Assert.That(result.ContentId, Is.EqualTo(expectedNoteDto.ContentId));
        Assert.That(result.NoteContent, Is.EqualTo(expectedNoteDto.NoteContent));
    }

    [Test]
    public async Task GetNotesForContent_ReturnsNull_WhenNoteDoesNotExist()
    {
        // Arrange
        int contentId = 3;
        var notes = new List<Note>
        {
            new Note { Id = 1, ContentId = 1, NoteContent = "Test note 1" },
            new Note { Id = 2, ContentId = 2, NoteContent = "Test note 2" }
        };

        _mockNotesRepo.Setup(repo => repo.GetAll()).ReturnsAsync(notes);

        // Act
        var result = await _notesService.GetNotesForContent(contentId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetNotesForContent_CallsGetAll_OnlyOnce()
    {
        // Arrange
        int contentId = 1;
        var notes = new List<Note>
        {
            new Note { Id = 1, ContentId = 1, NoteContent = "Test note 1" }
        };

        _mockNotesRepo.Setup(repo => repo.GetAll()).ReturnsAsync(notes);
        _mockMapper.Setup(mapper => mapper.Map<NoteDto>(It.IsAny<Note>())).Returns(new NoteDto());

        // Act
        await _notesService.GetNotesForContent(contentId);

        // Assert
        _mockNotesRepo.Verify(repo => repo.GetAll(), Times.Once);
    }
}