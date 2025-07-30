using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Queries;

public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;