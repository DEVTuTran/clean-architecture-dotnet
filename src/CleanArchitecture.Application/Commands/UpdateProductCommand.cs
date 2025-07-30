using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Commands;

public record UpdateProductCommand(Guid Id, UpdateProductDto Product) : IRequest<ProductDto>;