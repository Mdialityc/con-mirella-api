using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConMirellaApi.Endpoints.Platforms;

public class GetAllPlatformsEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<Platform>>, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public GetAllPlatformsEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/platforms");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<IEnumerable<Platform>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
        var platforms = _dbContext.Platforms.OrderBy(x => x.Id).AsEnumerable();
        return TypedResults.Ok(platforms);
    }
}