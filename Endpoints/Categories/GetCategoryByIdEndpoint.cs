using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using ConMirellaApi.Endpoints.Categories.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConMirellaApi.Endpoints.Categories;

public class GetCategoryByIdEndpoint : Endpoint<GetCategoryByIdRequest, Results<Ok<Category>, NotFound, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public GetCategoryByIdEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/categories/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<Category>, NotFound, ProblemDetails>> ExecuteAsync(GetCategoryByIdRequest req, CancellationToken ct)
    {
        var category = await _dbContext.Categories.FindAsync(req.Id, ct);

        if (category is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(category);
    }
}