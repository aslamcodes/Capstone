using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Notes;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.Notes;

public class NotesRepoTests
{
    private EduQuestContext _context;
    private NotesRepo _repo;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _context = new EduQuestContext(options);
        _repo = new NotesRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Add_ShouldAddNoteToDatabase()
    {
        // Arrange
        var note = new Note { Id = 1, UserId = 1, ContentId = 1, NoteContent = "Test Note" };

        // Act
        var result = await _repo.Add(note);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.UserId, Is.EqualTo(1));
        Assert.That(result.ContentId, Is.EqualTo(1));
        Assert.That(result.NoteContent, Is.EqualTo("Test Note"));
        Assert.That(await _context.Notes.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ShouldReturnCorrectNote()
    {
        // Arrange
        var note = new Note { Id = 1, UserId = 1, ContentId = 1, NoteContent = "Test Note" };
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByKey(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.UserId, Is.EqualTo(1));
        Assert.That(result.ContentId, Is.EqualTo(1));
        Assert.That(result.NoteContent, Is.EqualTo("Test Note"));
    }

    [Test]
    public async Task GetAll_ShouldReturnAllNotes()
    {
        // Arrange
        var notes = new List<Note>
        {
            new() { Id = 1, UserId = 1, ContentId = 1, NoteContent = "Note 1" },
            new() { Id = 2, UserId = 1, ContentId = 2, NoteContent = "Note 2" },
            new() { Id = 3, UserId = 2, ContentId = 1, NoteContent = "Note 3" }
        };
        await _context.Notes.AddRangeAsync(notes);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAll();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.Select(n => n.NoteContent), Is.EquivalentTo(new[] { "Note 1", "Note 2", "Note 3" }));
    }

    [Test]
    public async Task Update_ShouldUpdateExistingNote()
    {
        // Arrange
        var note = new Note { Id = 1, UserId = 1, ContentId = 1, NoteContent = "Original Note" };
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
        note.NoteContent = "Updated Note";

        // Act
        var result = await _repo.Update(note);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.NoteContent, Is.EqualTo("Updated Note"));
        var updatedNote = await _context.Notes.FindAsync(1);
        Assert.That(updatedNote.NoteContent, Is.EqualTo("Updated Note"));
    }

    [Test]
    public async Task Delete_ShouldRemoveNoteFromDatabase()
    {
        // Arrange
        var note = new Note { Id = 1, UserId = 1, ContentId = 1, NoteContent = "Test Note" };
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.Delete(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(await _context.Notes.CountAsync(), Is.EqualTo(0));
    }

    [Test]
    public void GetByKey_ShouldThrowExceptionWhenNoteNotFound()
    {
        // Act & Assert
        Assert.ThrowsAsync<EntityNotFoundException>(async () => await _repo.GetByKey(1));
    }
}