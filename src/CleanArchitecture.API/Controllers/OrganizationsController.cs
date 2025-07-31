using Microsoft.AspNetCore.Mvc;
using MediatR;
using CleanArchitecture.Application.Commands.Organizations;
using CleanArchitecture.Application.Queries.Organizations;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrganizationDto>>> GetAll()
    {
        var query = new GetAllOrganizationsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrganizationDto>> GetById(int id)
    {
        var query = new GetOrganizationByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OrganizationDto>> Create([FromBody] CreateOrganizationCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrganizationDto>> Update(int id, [FromBody] UpdateOrganizationDto updateDto)
    {
        var command = new UpdateOrganizationCommand
        {
            Id = id,
            Name = updateDto.Name,
            StatusId = updateDto.StatusId,
            MaxPaymentUsers = updateDto.MaxPaymentUsers,
            PaymentFeeId = updateDto.PaymentFeeId,
            UseIpWhitelist = updateDto.UseIpWhitelist
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOrganizationCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }
}