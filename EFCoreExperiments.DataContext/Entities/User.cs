using EFCoreFiltering.Attributes;

namespace EFCoreExperiments.DataContext.Entities;

public class User
{
    [Unfilterable]
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string EmailAddress { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
