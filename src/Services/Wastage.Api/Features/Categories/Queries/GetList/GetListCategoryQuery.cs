using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wastage.Api.Domain.Entities;
using Wastage.Application.Interfaces.Behaviors;
using Wastage.Core.Domain.Models.Requests;
using Wastage.Core.Domain.Models.Responses;

namespace Wastage.Api.Features.Categories.Queries.GetList;

public sealed record GetListCategoryResponse(Guid Id, string Name);
public class GetListCategoryQuery : IRequest<GetListResponse<GetListCategoryResponse>>,ILoggerableRequest
{
    public PageRequest PageRequest { get; set; }
    public string CacheKey => $"GetListCategoryQuery({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string CacheGroupKey => "Category";
}
public class GetListCategoryQueryHandler : IRequestHandler<GetListCategoryQuery, GetListResponse<GetListCategoryResponse>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;

    public GetListCategoryQueryHandler(ICategoryReadRepository categoryReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<GetListResponse<GetListCategoryResponse>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryReadRepository.GetListAsync(
                 index: request.PageRequest.PageIndex,
                 size: request.PageRequest.PageSize,
                 cancellationToken: cancellationToken);

        return category.Adapt<GetListResponse<GetListCategoryResponse>>();
    }
}

[Tags("Category")]
public sealed class GetListCategoryEndpoint : BaseController
{

    [HttpGet("Category")]
    [ProducesResponseType(typeof(GetListResponse<GetListCategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromQuery] PageRequest pageRequest)
    {
        var query = new GetListCategoryQuery() { PageRequest = pageRequest };
        var response = await Mediator.Send(query);

        return Ok(response);
    }
}
