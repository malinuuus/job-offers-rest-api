namespace JobOffersRestApi.Models.JobOffer;

public enum SortDirection
{
    Asc,
    Desc
}

public class JobOfferQuery
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }
}