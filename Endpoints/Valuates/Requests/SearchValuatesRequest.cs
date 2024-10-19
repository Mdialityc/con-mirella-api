using ConMirellaApi.Data.Models;

namespace ConMirellaApi.Endpoints.Valuates.Requests;

public class SearchValuatesRequest
{
    public int[]? Ids { get; set; }
    public ValuatePunctuation? Punctuation { get; set; }
    public string[]? Ips { get; set; }
    public int[]? GroupIds { get; set; }
    public string? SortBy { get; set; } = "Punctuation";
    public bool? IsDescending { get; set; } = false;
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}