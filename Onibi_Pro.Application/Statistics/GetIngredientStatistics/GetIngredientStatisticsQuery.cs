using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Statistics.GetIngredientStatistics;
public record GetIngredientStatisticsQuery : IRequest<ErrorOr<IReadOnlyCollection<IngredientStatisticsDto>>>;