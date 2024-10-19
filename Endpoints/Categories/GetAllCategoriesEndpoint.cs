using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConMirellaApi.Endpoints.Categories;

public class GetAllCategoriesEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<Category>>, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public GetAllCategoriesEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/categories");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<IEnumerable<Category>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
        var categories = _dbContext.Categories.OrderBy(x => x.Id).AsEnumerable();
        return TypedResults.Ok(categories);
    }
}