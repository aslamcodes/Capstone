using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Notes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduQuestTests.Notes
{
    [TestFixture]
    public class NotesControllerTests : IDisposable
    {
        private Mock<INotesService> _mockNotesService;
        private Mock<IControllerValidator> _mockValidator;
        private Mock<IMapper> _mockMapper;
        private NotesController _notesController;

        public void Dispose()
        {
            _notesController.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _mockNotesService = new Mock<INotesService>();
            _mockValidator = new Mock<IControllerValidator>();
            _mockMapper = new Mock<IMapper>();
            _notesController = new NotesController(_mockNotesService.Object, _mockValidator.Object, _mockMapper.Object);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthenticationType"));
            _notesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Test]
        public async Task CreateNote_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var noteDto = new NoteDto { ContentId = 1 };
            var createdNote = new NoteDto { ContentId = 1 };
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), noteDto.ContentId)).Returns(Task.CompletedTask);
            _mockNotesService.Setup(s => s.Add(It.IsAny<NoteDto>())).ReturnsAsync(createdNote);
            _mockMapper.Setup(m => m.Map<NoteDto>(It.IsAny<NoteDto>())).Returns(noteDto);

            // Act
            var result = await _notesController.CreateNote(noteDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(createdNote, okResult.Value);
        }

        [Test]
        public async Task CreateNote_WhenUnauthorized_ReturnsUnauthorizedResult()
        {
            // Arrange
            var noteDto = new NoteDto { ContentId = 1 };
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), noteDto.ContentId)).Throws<UnAuthorisedUserExeception>();

            // Act
            var result = await _notesController.CreateNote(noteDto);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task GetNoteById_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var noteId = 1;
            var noteDto = new NoteDto { ContentId = 1 };
            _mockNotesService.Setup(s => s.GetById(noteId)).ReturnsAsync(noteDto);
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), noteDto.ContentId)).Returns(Task.CompletedTask);

            // Act
            var result = await _notesController.GetNoteById(noteId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(noteDto, okResult.Value);
        }

        [Test]
        public async Task GetNoteById_WhenUnauthorized_ReturnsUnauthorizedResult()
        {
            // Arrange
            var noteId = 1;
            var noteDto = new NoteDto { ContentId = 1 };
            _mockNotesService.Setup(s => s.GetById(noteId)).ReturnsAsync(noteDto);
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), noteDto.ContentId)).Throws<UnAuthorisedUserExeception>();

            // Act
            var result = await _notesController.GetNoteById(noteId);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task UpdateNote_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var noteDto = new NoteDto { ContentId = 1 };
            var updatedNote = new NoteDto { ContentId = 1 };
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), noteDto.ContentId)).Returns(Task.CompletedTask);
            _mockNotesService.Setup(s => s.Update(noteDto)).ReturnsAsync(updatedNote);

            // Act
            var result = await _notesController.UpdateNote(noteDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(updatedNote, okResult.Value);
        }

        [Test]
        public async Task UpdateNote_WhenUnauthorized_ReturnsUnauthorizedResult()
        {
            // Arrange
            var noteDto = new NoteDto { ContentId = 1 };
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), noteDto.ContentId)).Throws<UnAuthorisedUserExeception>();

            // Act
            var result = await _notesController.UpdateNote(noteDto);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task DeleteNoteById_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var noteId = 1;
            var noteDto = new NoteDto { ContentId = 1 };
            _mockNotesService.Setup(s => s.GetById(noteId)).ReturnsAsync(noteDto);
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), noteDto.ContentId)).Returns(Task.CompletedTask);
            _mockNotesService.Setup(s => s.DeleteById(noteId)).ReturnsAsync(noteDto);

            // Act
            var result = await _notesController.DeleteNoteById(noteId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(noteDto, okResult.Value);
        }

        [Test]
        public async Task DeleteNoteById_WhenUnauthorized_ReturnsUnauthorizedResult()
        {
            // Arrange
            var noteId = 1;
            var noteDto = new NoteDto { ContentId = 1 };
            _mockNotesService.Setup(s => s.GetById(noteId)).ReturnsAsync(noteDto);
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), noteDto.ContentId)).Throws<UnAuthorisedUserExeception>();

            // Act
            var result = await _notesController.DeleteNoteById(noteId);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task GetNotesForContent_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var contentId = 1;
            var userId = 1;
            var notes = new List<NoteDto> { new NoteDto { ContentId = contentId } };
            _mockValidator.Setup(v => v.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(userId);
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId)).Returns(Task.CompletedTask);
            _mockNotesService.Setup(s => s.GetNotesForContent(contentId)).ReturnsAsync(_mockMapper.Object.Map<NoteDto>(notes));

            // Act
            var result = await _notesController.GetNotesForContent(contentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(notes, okResult.Value);
        }

        [Test]
        public async Task GetNotesForContent_WhenUnauthorized_ReturnsUnauthorizedResult()
        {
            // Arrange
            var contentId = 1;
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId)).Throws<UnAuthorisedUserExeception>();

            // Act
            var result = await _notesController.GetNotesForContent(contentId);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }
    }
}
