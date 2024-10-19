using ConMirellaApi.Data;
using ConMirellaApi.Endpoints.Valuates.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConMirellaApi.Endpoints.Valuates;


public class DeleteValuateEndpoint : Endpoint<GetValuateByIdRequest, Results<Ok, NotFound, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public DeleteValuateEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Delete("/valuates/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(GetValuateByIdRequest req, CancellationToken ct)
    {
        var valuate = await _dbContext.Valuates.FindAsync(req.Id, ct);

        if (valuate is null)
            return TypedResults.NotFound();

        _dbContext.Valuates.Remove(valuate);
        await _dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}