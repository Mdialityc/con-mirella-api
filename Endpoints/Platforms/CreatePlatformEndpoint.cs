using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using ConMirellaApi.Endpoints.Categories.Requests;
using ConMirellaApi.Endpoints.Platforms.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ConMirellaApi.Endpoints.Platforms;

public class CreatePlatformEndpoint : Endpoint<CreatePlatformRequest, Results<Created<Platform>, Conflict, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public CreatePlatformEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/platforms");
        AllowAnonymous();
    }

    public override async Task<Results<Created<Platform>, Conflict, ProblemDetails>> ExecuteAsync(CreatePlatformRequest req, CancellationToken ct)
    {
        var platform = new Platform { Name = req.Name.Trim() };

        if (_dbContext.Platforms.AsNoTracking().Any(x => x.Name == platform.Name))
            return TypedResults.Conflict();

        await _dbContext.Platforms.AddAsync(platform, ct);
        await _dbContext.SaveChangesAsync(ct);

        return TypedResults.Created($"/platforms/{platform.Id}", platform);
    }
}