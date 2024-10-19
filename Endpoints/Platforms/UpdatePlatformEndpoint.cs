using ConMirellaApi.Data;
using ConMirellaApi.Endpoints.Categories.Requests;
using ConMirellaApi.Endpoints.Platforms.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConMirellaApi.Endpoints.Platforms;

public class UpdatePlatformEndpoint : Endpoint<UpdatePlatformRequest, Results<Ok, NotFound, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public UpdatePlatformEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Put("/platforms/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(UpdatePlatformRequest req, CancellationToken ct)
    {
        var platform = await _dbContext.Platforms.FindAsync(req.Id, ct);

        if (platform is null)
            return TypedResults.NotFound();

        platform.Name = req.Name;
        await _dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}