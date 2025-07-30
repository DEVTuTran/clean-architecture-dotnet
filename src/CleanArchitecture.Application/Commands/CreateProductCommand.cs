using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Commands;

public record CreateProductCommand(CreateProductDto Product) : IRequest<ProductDto>;