using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using ConMirellaApi.Endpoints.Valuates.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConMirellaApi.Endpoints.Valuates;

public class GetValuateByIdEndpoint : Endpoint<GetValuateByIdRequest, Results<Ok<Valuate>, NotFound, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public GetValuateByIdEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/valuates/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<Valuate>, NotFound, ProblemDetails>> ExecuteAsync(GetValuateByIdRequest req, CancellationToken ct)
    {
        var valuate = await _dbContext.Valuates.FindAsync(req.Id, ct);

        if (valuate is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(valuate);
    }
}