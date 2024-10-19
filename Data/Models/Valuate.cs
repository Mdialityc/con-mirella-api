namespace ConMirellaApi.Data.Models;

public class Valuate
{
    public int Id { get; set; }
    public required ValuatePunctuation Punctuation { get; set; }
    public required string? Description { get; set; }
    public required string IP { get; set; }
    public required int GroupId { get; set; }
}