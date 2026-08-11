using Garage3.Validation;

namespace Garage3.Models
{
    public class User
    {
        private string _pnumber = string.Empty;

        [PersonalIdentityNumber]
        public required string Pnumber
        {
            get => _pnumber;
            set => _pnumber = value?.Trim().Replace(" ", "") ?? string.Empty;
        }
    }
}
