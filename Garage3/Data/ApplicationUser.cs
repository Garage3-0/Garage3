using Microsoft.AspNetCore.Identity;

namespace Garage3.Data;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PersonalIdentityNumber { get; set; } = string.Empty;
}