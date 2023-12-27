using MediatR;

namespace Onibi_Pro.Application.Menus.Queries.GetIngredients;
public record GetIngredientsQuery : IRequest<IReadOnlyCollection<IngredientKeyValueDto>>;