namespace ConMirellaApi.Data.Models;

public class Group
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Link { get; set; }
    public required string? Description { get; set; }
    public required string ImageOG { get; set; }
    public required GroupStatus Status { get; set; }
    public required DateTimeOffset CreatedDate { get; set; }
    public required DateTimeOffset UpdatedDate { get; set; }
    public required bool IsActive { get; set; }
    
    // Navigational Relationships
    public Category Category { get; set; }
    public Platform Platform { get; set; }
}