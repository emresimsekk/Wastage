using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wastage.Api.Domain.Entities;
using Wastage.Application.Interfaces.Behaviors;

namespace Wastage.Api.Features.Categories.Commands.Create;
public sealed record CreateCategoryRequest(string Name);
public sealed record CreateCategoryResponse(Guid Id, string Name);

public class CreateCategoryCommand : IRequest<CreateCategoryResponse>, ICacheRemoverRequest//, ILoggerableRequest
{
    public required string Name { get; set; }
    public string CacheKey => "";

    public bool ByPassCache => false;

    public string CacheGroupKey => "Category";
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    private readonly ICategoryWriteRepository _categoryWriteRepository;

    public CreateCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository)
    {
        _categoryWriteRepository = categoryWriteRepository;
    }

    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = request.Adapt<Category>();
        var response = await _categoryWriteRepository.AddAsync(category);
        return response.Adapt<CreateCategoryResponse>();

    }
}
public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{

    public CreateCategoryValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Kategori Ad alanı boş geçilemez !")
            .MinimumLength(5)
            .WithMessage("Kategori Ad alanı minumum 5 karakter olmalı !")
            .MaximumLength(50)
            .WithMessage("Kategori Ad alanı maksimum 200 karakter olmalı !");

    }

}
[Tags("Category")]
public sealed class CreateCategoryEndpoint : BaseController
{

    [HttpPost("Category")]
    [ProducesResponseType(typeof(CreateCategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreateCategoryResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Post([FromBody] CreateCategoryRequest request)
    {
        var command = request.Adapt<CreateCategoryCommand>();

        var response = await Mediator.Send(command);
        return Ok(response);
    }
}
