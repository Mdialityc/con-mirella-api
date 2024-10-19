namespace ConMirellaApi.Data.Models;

public class SearchLog
{
    public required int Id { get; set; }
    public required string SearchQuery { get; set; }
    public required DateTimeOffset SearchDate {get; set; }
}