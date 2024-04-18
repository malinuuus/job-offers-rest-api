using JobOffersRestApi.Models.User;

namespace JobOffersRestApi.Models.JobApplication;

public class JobApplicationDto
{
    public int Id { get; set; }
    public bool IsApproved { get; set; }
    public bool IsRejected { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public RecruiteeDto Recruitee { get; set; }
}