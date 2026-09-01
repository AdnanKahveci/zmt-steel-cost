using System.Text.Json;
using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Application.Projects;

public interface IProjectRepository
{
    Task<IReadOnlyList<Project>> GetRecentAsync(int count, CancellationToken cancellationToken = default);
    Task<Project?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(Project project, CalculationResult? result, CancellationToken cancellationToken = default);
}

public interface IProjectService
{
    Project CreateNew();
    Task<IReadOnlyList<Project>> GetRecentAsync(int count, CancellationToken cancellationToken = default);
    Task<Project?> OpenAsync(Guid id, CancellationToken cancellationToken = default);
    CalculationResult Calculate(Project project);
    Task SaveAsync(Project project, CalculationResult result, CancellationToken cancellationToken = default);
}

public sealed class ProjectService(ICalculationEngine engine, IProjectRepository repository) : IProjectService
{
    public Project CreateNew() => new()
    {
        PricingSnapshot = new PricingParameters(),
        Building = BuildingInput.CreateLegacySample()
    };

    public Task<IReadOnlyList<Project>> GetRecentAsync(int count, CancellationToken cancellationToken = default) =>
        repository.GetRecentAsync(count, cancellationToken);

    public Task<Project?> OpenAsync(Guid id, CancellationToken cancellationToken = default) =>
        repository.GetAsync(id, cancellationToken);

    public CalculationResult Calculate(Project project) => engine.Calculate(project);

    public Task SaveAsync(Project project, CalculationResult result, CancellationToken cancellationToken = default) =>
        repository.SaveAsync(project, result, cancellationToken);
}

public static class SnapshotFactory
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static CalculationSnapshot Create(Project project, CalculationResult result) => new(
        project.Id,
        result.FormulaVersion,
        project.PriceListVersionId,
        DateTime.UtcNow,
        JsonSerializer.Serialize(project.Building, Options),
        JsonSerializer.Serialize(project.PricingSnapshot, Options),
        JsonSerializer.Serialize(result, Options));
}
