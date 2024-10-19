using ConMirellaApi.Data.Models;

namespace ConMirellaApi.Endpoints.Groups.Responses;

public class GroupResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
    public string? Description { get; set; }
    public string Platform { get; set; }
    public string ImageOG { get; set; }
    public GroupStatus Status { get; set; }
    public string Category { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
    public bool IsActive { get; set; }

    public int FiveStarsAmount { get; set; }
    public int FourStarsAmount { get; set; }
    public int ThreeStarsAmount { get; set; }
    public int TwoStarsAmount { get; set; }
    public int OneStarsAmount { get; set; }

    public double AvgPunctuation
    {
        get
        {
            int totalVotes = FiveStarsAmount + FourStarsAmount + ThreeStarsAmount + TwoStarsAmount + OneStarsAmount;
            if (totalVotes == 0) return 0;
            
            double totalScore = (FiveStarsAmount * 5) +
                                (FourStarsAmount * 4) +
                                (ThreeStarsAmount * 3) +
                                (TwoStarsAmount * 2) +
                                (OneStarsAmount * 1);
            
            return totalScore / totalVotes;
        }
    }
}