namespace JobOffersRestApi.Entities;

public class JobApplication
{
    public int Id { get; set; }
    public bool IsApproved { get; set; }
    public bool IsRejected { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CvFileName { get; set; }
    public string? AdditionalInfo { get; set; }
    
    public int JobOfferId { get; set; }
    public virtual JobOffer JobOffer { get; set; }
    
    public int RecruiteeId { get; set; }
    public virtual User Recruitee { get; set; }
}