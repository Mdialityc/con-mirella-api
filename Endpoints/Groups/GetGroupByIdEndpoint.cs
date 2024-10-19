using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using ConMirellaApi.Endpoints.Groups.Requests;
using ConMirellaApi.Endpoints.Groups.Responses;
using ConMirellaApi.Endpoints.Platforms.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Group = FastEndpoints.Group;

namespace ConMirellaApi.Endpoints.Groups;

public class
    GetGroupByIdEndpoint : Endpoint<GetGroupByIdRequest, Results<Ok<GroupResponse>, NotFound, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public GetGroupByIdEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/groups/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<GroupResponse>, NotFound, ProblemDetails>> ExecuteAsync(
        GetGroupByIdRequest req, CancellationToken ct)
    {
        var group = await _dbContext.Groups.FindAsync(req.Id, ct);

        if (group is null)
            return TypedResults.NotFound();

        var valuates = _dbContext.Valuates.Where(y => y.GroupId == y.Id).ToList();
        return TypedResults.Ok(new GroupResponse
        {
            Id = group.Id,
            CreatedDate = group.CreatedDate,
            UpdatedDate = group.UpdatedDate,
            IsActive = group.IsActive,
            Status = group.Status,
            Name = group.Name,
            Description = group.Description,
            Link = group.Link,
            ImageOG = group.ImageOG,
            Category = _dbContext.Categories.AsNoTracking().FirstOrDefault(y => y.Id == group.CategoryId)?.Name ??
                       string.Empty,
            Platform = _dbContext.Platforms.AsNoTracking().FirstOrDefault(y => y.Id == group.PlatformId)?.Name ??
                       string.Empty,
            FiveStarsAmount =
                valuates.Count(y => y.Punctuation == ValuatePunctuation.FIVE_STARS),
            FourStarsAmount =
                valuates.Count(y => y.Punctuation == ValuatePunctuation.FOUR_STARS),
            ThreeStarsAmount =
                valuates.Count(y => y.Punctuation == ValuatePunctuation.THREE_STARS),
            TwoStarsAmount =
                valuates.Count(y => y.Punctuation == ValuatePunctuation.TWO_STARS),
            OneStarsAmount = valuates.Count(y => y.Punctuation == ValuatePunctuation.ONE_STARS)
        });
    }
}