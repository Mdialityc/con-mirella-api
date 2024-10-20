using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using ConMirellaApi.Endpoints.Commons.Responses;
using ConMirellaApi.Endpoints.Valuates.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ConMirellaApi.Endpoints.Valuates;

public class SearchValuatesEndpoint : Endpoint<SearchValuatesRequest, Results<Ok<PaginatedResponse<Valuate>>, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public SearchValuatesEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/valuates");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<PaginatedResponse<Valuate>>, ProblemDetails>> ExecuteAsync(SearchValuatesRequest req, CancellationToken ct)
    {
        var query = _dbContext.Valuates.AsNoTracking().AsQueryable();
        
        // Filtering Section
        if (req.Ids?.Any() ?? false)
        {
            query = query.Where(x => req.Ids.Contains(x.Id));
        }

        if (req.GroupIds?.Any() ?? false)
        {
            query = query.Where(x => req.GroupIds.Contains(x.GroupId));
        }

        if (req.Ips?.Any() ?? false)
        {
            query = query.Where(x => req.Ips.Contains(x.IP));
        }

        if (req.Punctuation is not null)
        {
            query = query.Where(x => x.Punctuation == req.Punctuation);
        }

        var valuateList = (await query.ToListAsync(cancellationToken: ct)).AsEnumerable();
        
        // Sorting Section
        if (!string.IsNullOrEmpty(req.SortBy))
        {
            var propertyInfo = typeof(Valuate).GetProperty(req.SortBy);
            if (propertyInfo != null)
            {
                valuateList = req?.IsDescending ?? false
                    ? valuateList.OrderByDescending(s => propertyInfo.GetValue(s))
                    : valuateList.OrderBy(s => propertyInfo.GetValue(s));
            }
        }
        
        // Pagination Section
        var totalCount = valuateList.Count();
        
        var data = valuateList.Skip(((req?.Page ?? 1) - 1) * req?.PageSize ?? 10)
            .Take(req?.PageSize ?? 10);
        
        return TypedResults.Ok(new PaginatedResponse<Valuate>
        {
            Data = data,
            Page = req?.Page ?? 1,
            PageSize = req?.PageSize ?? 10,
            TotalCount = totalCount
        });
    }
}