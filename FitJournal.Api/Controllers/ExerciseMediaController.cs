using FitJournal.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJournal.Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/v{version:apiVersion}/exercises/{exerciseId:guid}/media")]
public class ExerciseMediaController(ExerciseMediaStorage storage) : BaseController
{
    [HttpPost("{mediaType}")]
    [RequestSizeLimit(200_000_000)]
    public async Task<ActionResult<object>> UploadAsync(Guid exerciseId, string mediaType, IFormFile file, CancellationToken token)
    {
        if (mediaType is not ("thumbnail" or "video")) return BadRequest("Media type must be thumbnail or video.");
        if (mediaType == "thumbnail" && !file.ContentType.StartsWith("image/")) return BadRequest("Thumbnails must be images.");
        if (mediaType == "video" && !file.ContentType.StartsWith("video/")) return BadRequest("Videos must be video files.");
        var uri = await storage.UploadAsync(exerciseId, file, mediaType, token);
        return Created(uri, new { url = uri.ToString() });
    }
}
