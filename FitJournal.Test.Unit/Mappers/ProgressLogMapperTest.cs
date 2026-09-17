using AutoMapper;
using FitJournal.Core.Dtos.Responses.ProgressLogs;
using FitJournal.Core.Mappers;
using FitJournal.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitJournal.Test.Unit.Mappers;

public class ProgressLogMapperTest
{
    [Fact]
    public void ShortResponse_ShouldIncludeDashboardMeasurementSeries()
    {
        var configuration = new MapperConfiguration(
            config => config.AddProfile<ProgressLogMapper>(),
            NullLoggerFactory.Instance);
        var mapper = configuration.CreateMapper();
        var progress = new ProgressLog
        {
            Date = new DateOnly(2026, 9, 17),
            Weight = 78.5m,
            BodyFat = 16.2m,
            WaistCm = 82,
            ChestCm = 101,
            ArmsCm = 36.5m
        };

        var response = mapper.Map<ShortProgressLogResponse>(progress);

        response.Should().BeEquivalentTo(new
        {
            progress.Date,
            progress.Weight,
            progress.BodyFat,
            progress.WaistCm,
            progress.ChestCm,
            progress.ArmsCm
        });
    }
}
