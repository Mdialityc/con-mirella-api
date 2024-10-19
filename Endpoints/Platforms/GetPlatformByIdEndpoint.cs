using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using ConMirellaApi.Endpoints.Categories.Requests;
using ConMirellaApi.Endpoints.Platforms.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConMirellaApi.Endpoints.Platforms;

public class GetPlatformByIdEndpoint : Endpoint<GetPlatformByIdRequest, Results<Ok<Platform>, NotFound, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public GetPlatformByIdEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/platforms/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<Platform>, NotFound, ProblemDetails>> ExecuteAsync(GetPlatformByIdRequest req, CancellationToken ct)
    {
        var platform = await _dbContext.Platforms.FindAsync(req.Id, ct);

        if (platform is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(platform);
    }
}