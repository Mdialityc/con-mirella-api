using ConMirellaApi.Data.Models;

namespace ConMirellaApi.Endpoints.Groups.Requests;

public class SearchGroupsRequest
{
    public int[]? Ids { get; set; }
    public string[]? Names { get; set; }
    public int[]? CategoryIds { get; set; }
    public int[]? PlatformIds { get; set; }
    public string[]? Descriptions { get; set; }
    public GroupStatus? Status { get; set; }
    public bool? IsActive { get; set; }
    public double? MinimumPunctuation { get; set; }
    public double? MaximumPunctuation { get; set; }
    public DateTimeOffset? MinimumCreatedDate { get; set; }
    public DateTimeOffset? MaximumCreatedDate { get; set; }
    public DateTimeOffset? MinimumUpdatedDate { get; set; }
    public DateTimeOffset? MaximumUpdatedDate { get; set; }
    public string? SortBy { get; set; } = "Name";
    public bool? IsDescending { get; set; } = false;
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}