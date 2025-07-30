using MediatR;

namespace CleanArchitecture.Application.Commands;

public record DeleteProductCommand(Guid Id) : IRequest;