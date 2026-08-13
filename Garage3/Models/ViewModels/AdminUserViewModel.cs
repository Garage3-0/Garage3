using Garage3.Data;

namespace Garage3.Models.ViewModels;

public class AdminUserViewModel
{
    public ApplicationUser User { get; set; } = default!;

    public IList<string> Roles { get; set; } = new List<string>();

    public IList<string> AvailableRoles { get; set; } = new List<string>();
}
