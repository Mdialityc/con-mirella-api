using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using ConMirellaApi.Endpoints.Categories.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ConMirellaApi.Endpoints.Categories;

public class CreateCategoryEndpoint : Endpoint<CreateCategoryRequest, Results<Created<Category>, Conflict, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public CreateCategoryEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/categories");
        AllowAnonymous();
    }

    public override async Task<Results<Created<Category>, Conflict, ProblemDetails>> ExecuteAsync(CreateCategoryRequest req, CancellationToken ct)
    {
        var category = new Category { Name = req.Name.Trim() };

        if (_dbContext.Categories.AsNoTracking().Any(x => x.Name == category.Name))
            return TypedResults.Conflict();

        await _dbContext.Categories.AddAsync(category, ct);
        await _dbContext.SaveChangesAsync(ct);

        return TypedResults.Created($"/categories/{category.Id}", category);
    }
}