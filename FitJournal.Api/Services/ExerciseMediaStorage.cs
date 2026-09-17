using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace FitJournal.Api.Services;

public class ExerciseMediaStorage(BlobServiceClient blobServiceClient)
{
    public async Task<Uri> UploadAsync(Guid exerciseId, IFormFile file, string mediaType, CancellationToken token)
    {
        if (file.Length == 0) throw new BadHttpRequestException("The media file is empty.");
        var container = blobServiceClient.GetBlobContainerClient("exercise-media");
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: token);
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var blob = container.GetBlobClient($"{exerciseId}/{mediaType}-{Guid.NewGuid():N}{extension}");
        await using var stream = file.OpenReadStream();
        await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType }, cancellationToken: token);
        return blob.Uri;
    }
}
