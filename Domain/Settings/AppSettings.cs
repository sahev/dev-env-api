#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Domain.Settings;

public class AppSettings
{
    public string AppUrl { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public bool UseInMemoryDatabase { get; set; }
}

public class ConnectionStrings
{
    [Required]
    public string DefaultConnection { get; set; }
}
