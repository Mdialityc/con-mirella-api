namespace ConMirellaApi.Endpoints.Categories.Requests;

public class UpdateCategoryRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
}