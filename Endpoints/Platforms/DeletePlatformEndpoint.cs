using ConMirellaApi.Data;
using ConMirellaApi.Endpoints.Categories.Requests;
using ConMirellaApi.Endpoints.Platforms.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConMirellaApi.Endpoints.Platforms;

public class DeletePlatformEndpoint : Endpoint<GetPlatformByIdRequest, Results<Ok, NotFound, ProblemDetails>>
{
    private readonly AppDbContext dbContext;

    public DeletePlatformEndpoint(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Delete("/platforms/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(GetPlatformByIdRequest req, CancellationToken ct)
    {
        var platform = await dbContext.Platforms.FindAsync(req.Id, ct);

        if (platform is null)
            return TypedResults.NotFound();

        dbContext.Platforms.Remove(platform);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}
