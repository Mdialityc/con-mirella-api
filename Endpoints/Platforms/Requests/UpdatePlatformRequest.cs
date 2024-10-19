namespace ConMirellaApi.Endpoints.Platforms.Requests;

public class UpdatePlatformRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
}