namespace JobOffersRestApi.Models.JobApplication;

public class CreateJobApplicationDto
{
    public IFormFile CvFile { get; set; }
    public string? AdditionalInfo { get; set; }
}