using ConMirellaApi.Data;
using ConMirellaApi.Endpoints.Categories.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConMirellaApi.Endpoints.Categories;

public class UpdateCategoryEndpoint : Endpoint<UpdateCategoryRequest, Results<Ok, NotFound, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public UpdateCategoryEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Put("/categories/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(UpdateCategoryRequest req, CancellationToken ct)
    {
        var category = await _dbContext.Categories.FindAsync(req.Id, ct);

        if (category is null)
            return TypedResults.NotFound();

        category.Name = req.Name;
        await _dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}