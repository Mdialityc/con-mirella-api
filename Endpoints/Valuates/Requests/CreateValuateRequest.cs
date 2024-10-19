using ConMirellaApi.Data.Models;

namespace ConMirellaApi.Endpoints.Valuates.Requests;

public class CreateValuateRequest
{
    public required ValuatePunctuation Punctuation { get; set; }
    public string? Description { get; set; }
    public required string IP { get; set; }
    public required int GroupId { get; set; }
}