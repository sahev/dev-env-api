#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Core.Settings;

public class AppSettings
{
    public string AppUrl { get; set; }
    public ApplicationDetail ApplicationDetail { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public bool UseInMemoryDatabase { get; set; }
}

public class ApplicationDetail
{
    public string ApplicationName { get; set; }
    public string Description { get; set; }
    public string ContactWebsite { get; set; }
}

public class ConnectionStrings
{
    [Required]
    public string DefaultConnection { get; set; }
}
