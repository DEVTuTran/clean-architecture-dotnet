using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;