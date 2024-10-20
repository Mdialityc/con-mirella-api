using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using ConMirellaApi.Endpoints.Commons.Responses;
using ConMirellaApi.Endpoints.Groups.Requests;
using ConMirellaApi.Endpoints.Groups.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ConMirellaApi.Endpoints.Groups;

public class SearchGroupsEndpoint : Endpoint<SearchGroupsRequest,
    Results<Ok<PaginatedResponse<GroupResponse>>, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public SearchGroupsEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/groups");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<PaginatedResponse<GroupResponse>>, ProblemDetails>>
        ExecuteAsync(SearchGroupsRequest req, CancellationToken ct)
    {
        var query = _dbContext.Groups
            .Include(group => group.Category)
            .Include(group => group.Platform)
            .AsNoTracking().AsQueryable();

        // Filtering Section
        if (req.Ids?.Any() ?? false)
        {
            query = query.Where(x => req.Ids.Contains(x.Id));
        }

        if (req.Names?.Any() ?? false)
        {
            query = query.Where(x => req.Names.Any(y => x.Name.ToLower().Contains(y.ToLower().Trim())));
        }

        if (req.CategoryIds?.Any() ?? false)
        {
            query = query.Where(x => req.CategoryIds.Contains(x.Category.Id));
        }

        if (req.PlatformIds?.Any() ?? false)
        {
            query = query.Where(x => req.PlatformIds.Contains(x.Platform.Id));
        }

        if (req.Descriptions?.Any() ?? false)
        {
            query = query.Where(x => req.Descriptions.Contains(x.Description));
        }

        if (req.Status is not null)
        {
            query = query.Where(x => x.Status == req.Status);
        }

        if (req.IsActive is not null)
        {
            query = query.Where(x => x.IsActive == req.IsActive);
        }

        if (req.MinimumCreatedDate is not null)
        {
            req.MinimumCreatedDate = req.MinimumCreatedDate.Value.ToUniversalTime();
            query = query.Where(x => x.CreatedDate >= req.MinimumCreatedDate);
        }

        if (req.MaximumCreatedDate is not null)
        {
            req.MaximumCreatedDate = req.MaximumCreatedDate.Value.ToUniversalTime();
            query = query.Where(x => x.CreatedDate <= req.MaximumCreatedDate);
        }

        if (req.MinimumUpdatedDate is not null)
        {
            req.MinimumUpdatedDate = req.MinimumUpdatedDate.Value.ToUniversalTime();
            query = query.Where(x => x.UpdatedDate >= req.MinimumUpdatedDate);
        }

        if (req.MaximumUpdatedDate is not null)
        {
            req.MaximumUpdatedDate = req.MaximumUpdatedDate.Value.ToUniversalTime();
            query = query.Where(x => x.UpdatedDate <= req.MaximumUpdatedDate);
        }

        var groupList = await query.ToListAsync(ct);
        
        var valuatesList = await _dbContext.Valuates
            .Where(x => groupList.Select(y => y.Id).Contains(x.GroupId))
            .ToListAsync(ct);

        var groupData = groupList.Select(x =>
        {
            var valuates = valuatesList.Where(y => y.GroupId == x.Id);
            return new GroupResponse
            {
                Id = x.Id,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
                IsActive = x.IsActive,
                Status = x.Status,
                Name = x.Name,
                Description = x.Description,
                Link = x.Link,
                ImageOG = x.ImageOG,
                Category = x.Category.Name,
                Platform = x.Platform.Name,
                FiveStarsAmount =
                    valuates.Count(y => y.Punctuation == ValuatePunctuation.FIVE_STARS),
                FourStarsAmount =
                    valuates.Count(y => y.Punctuation == ValuatePunctuation.FOUR_STARS),
                ThreeStarsAmount =
                    valuates.Count(y => y.Punctuation == ValuatePunctuation.THREE_STARS),
                TwoStarsAmount =
                    valuates.Count(y => y.Punctuation == ValuatePunctuation.TWO_STARS),
                OneStarsAmount = valuates.Count(y => y.Punctuation == ValuatePunctuation.ONE_STARS)
            };
        });

        if (req.MinimumPunctuation is not null)
        {
            groupData = groupData.Where(x => x.AvgPunctuation >= req.MinimumPunctuation);
        }

        if (req.MaximumPunctuation is not null)
        {
            groupData = groupData.Where(x => x.AvgPunctuation <= req.MaximumPunctuation);
        }

        // Sorting Section
        if (!string.IsNullOrEmpty(req.SortBy))
        {
            var propertyInfo = typeof(GroupResponse).GetProperty(req.SortBy);
            if (propertyInfo != null)
            {
                groupData = req?.IsDescending ?? false
                    ? groupData.OrderByDescending(s => propertyInfo.GetValue(s))
                    : groupData.OrderBy(s => propertyInfo.GetValue(s));
            }
        }

        // Pagination Section
        var totalCount = groupData.Count();
        var data = groupData.Skip(((req?.Page ?? 1) - 1) * req?.PageSize ?? 10)
            .Take(req?.PageSize ?? 10);

        return TypedResults.Ok(new PaginatedResponse<GroupResponse>
        {
            Data = data,
            Page = req?.Page ?? 1,
            PageSize = req?.PageSize ?? 10,
            TotalCount = totalCount
        });
    }
}