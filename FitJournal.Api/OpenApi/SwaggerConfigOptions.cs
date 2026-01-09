using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FitJournal.Api.OpenApi;

public class SwaggerConfigOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName, new()
            {
                Title = "FitJournal API",
                Description = "Health platform with real-time activity insights for accessible fitness progress",
                Version = description.ApiVersion.ToString()
            });
    }
}
