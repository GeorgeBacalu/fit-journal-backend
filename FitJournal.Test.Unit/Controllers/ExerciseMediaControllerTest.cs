using Azure;
using Azure.Storage.Blobs;
using FitJournal.Api.Controllers;
using FitJournal.Api.Services;
using FitJournal.Core.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FitJournal.Test.Unit.Controllers;

public class ExerciseMediaControllerTest
{
    private readonly ExerciseMediaController _controller = new(
        new ExerciseMediaStorage(new BlobServiceClient(
            new Uri("https://test.blob.core.windows.net"),
            new AzureSasCredential("sig=fake"))),
        new Mock<IUnitOfWork>().Object);

    [Theory]
    [InlineData("thumbnail", "image/png", 0, "The media file is empty.")]
    [InlineData("thumbnail", "video/mp4", 1, "Thumbnails must be images.")]
    [InlineData("video", "image/png", 1, "Videos must be video files.")]
    [InlineData("thumbnail", "image/png", 10_000_001, "Thumbnails cannot exceed 10 MB.")]
    [InlineData("video", "video/mp4", 200_000_001, "Videos cannot exceed 200 MB.")]
    public async Task UploadAsync_RejectsInvalidMedia(string mediaType, string contentType, long length, string expectedMessage)
    {
        var file = new Mock<IFormFile>();
        file.SetupGet(candidate => candidate.ContentType).Returns(contentType);
        file.SetupGet(candidate => candidate.Length).Returns(length);

        var result = await _controller.UploadAsync(Guid.NewGuid(), mediaType, file.Object, default);

        result.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(expectedMessage);
    }
}
