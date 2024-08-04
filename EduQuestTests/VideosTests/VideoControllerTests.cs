using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Videos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace EduQuestTests.VideosTests
{
    public class VideoControllerTests : IDisposable
    {
        private Mock<IControllerValidator> _mockValidator;
        private Mock<IVideoService> _mockVideoService;
        private Mock<BlobServiceClient> _mockBlobServiceClient;
        private Mock<IMapper> _mockMapper;
        private Mock<SecretClient> _mockSecretClient;
        private VideoController _controller;

        [TearDown]
        public void Dispose()
        {
            _mockValidator.Reset();
            _mockVideoService.Reset();
            _mockBlobServiceClient.Reset();
            _mockMapper.Reset();
            _mockSecretClient.Reset();
        }

        [SetUp]
        public void Setup()
        {
            _mockValidator = new Mock<IControllerValidator>();
            _mockVideoService = new Mock<IVideoService>();
            _mockBlobServiceClient = new Mock<BlobServiceClient>();
            _mockMapper = new Mock<IMapper>();
            _mockSecretClient = new Mock<SecretClient>();

            _controller = new VideoController(_mockValidator.Object, _mockVideoService.Object, _mockBlobServiceClient.Object, _mockMapper.Object, _mockSecretClient.Object);
        }

        [Test]
        public async Task GetVideoForContent_ReturnsOkResult_WithVideo()
        {
            // Arrange
            var contentId = 1;
            var videoDto = new VideoDto { ContentId = contentId, Url = "http://example.com/video.mp4" };

            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId)).Returns(Task.CompletedTask);
            _mockVideoService.Setup(vs => vs.GetByContentId(contentId)).ReturnsAsync(videoDto);

            // Act
            var result = await _controller.GetVideoForContent(contentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(videoDto, okResult.Value);
        }

        [Test]
        public async Task GetVideoForContent_ReturnsUnauthorized_WhenUserIsNotAuthorized()
        {
            // Arrange
            var contentId = 1;
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId)).ThrowsAsync(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.GetVideoForContent(contentId);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task UploadVideoDataForContent_ReturnsOkResult_WithAddedVideo()
        {
            // Arrange
            var videoRequest = new VideoRequestDto { ContentId = 1 };
            var videoDto = new VideoDto { ContentId = 1, Url = "http://example.com/video.mp4" };

            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), videoRequest.ContentId)).Returns(Task.CompletedTask);
            _mockVideoService.Setup(vs => vs.Add(It.IsAny<VideoDto>())).ReturnsAsync(videoDto);

            // Act
            var result = await _controller.UploadVideoDataForContent(videoRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(videoDto, okResult.Value);
        }

        [Test]
        public async Task UploadVideoDataForContent_ReturnsUnauthorized_WhenUserIsNotAuthorized()
        {
            // Arrange
            var videoRequest = new VideoRequestDto { ContentId = 1 };

            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), videoRequest.ContentId)).ThrowsAsync(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.UploadVideoDataForContent(videoRequest);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task GetUploadUrl_ReturnsOkResult_WithUploadUrl()
        {
            // Arrange
            var request = new GetUploadUrlRequest { ContentId = 1, FileName = "video.mp4" };
            var expectedUrl = "http://example.com/upload?token=abc123";

            var mockContainerClient = new Mock<BlobContainerClient>();
            var mockBlobClient = new Mock<BlobClient>();

            _mockBlobServiceClient.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(mockContainerClient.Object);
            mockContainerClient.Setup(c => c.GetBlobClient(It.IsAny<string>())).Returns(mockBlobClient.Object);
            mockBlobClient.Setup(b => b.Uri).Returns(new Uri("http://example.com/upload"));
            // _mockSecretClient.Setup(s => s.GetSecret(It.IsAny<string>())).Returns(Response.FromValue(new KeyVaultSecret("name", "value"), null));

            // Act
            var result = await _controller.GetUploadUrl(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var response = okResult.Value as UploadUrlResponse;
            Assert.IsTrue(response.UploadUrl.StartsWith("http://example.com/upload"));
        }

        [Test]
        public async Task CompleteUpload_ReturnsOkResult_WithUpdatedVideo()
        {
            // Arrange
            var request = new CompleteUploadRequest { ContentId = 1, FileName = "video.mp4" };
            var videoDto = new VideoDto { ContentId = 1, Url = "http://example.com/video.mp4" };

            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), request.ContentId)).Returns(Task.CompletedTask);
            _mockVideoService.Setup(vs => vs.GetByContentId(request.ContentId)).ReturnsAsync(videoDto);
            _mockVideoService.Setup(vs => vs.Update(It.IsAny<VideoDto>())).ReturnsAsync(videoDto);

            var mockContainerClient = new Mock<BlobContainerClient>();
            var mockBlobClient = new Mock<BlobClient>();

            _mockBlobServiceClient.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(mockContainerClient.Object);
            mockContainerClient.Setup(c => c.GetBlobClient(It.IsAny<string>())).Returns(mockBlobClient.Object);
            mockBlobClient.Setup(b => b.Uri).Returns(new Uri("http://example.com/video.mp4"));
            mockBlobClient.Setup(b => b.GetPropertiesAsync(null, default)).ReturnsAsync(Response.FromValue(BlobsModelFactory.BlobProperties(), null));

            // Act
            var result = await _controller.CompleteUpload(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(videoDto, okResult.Value);
        }
    }
}