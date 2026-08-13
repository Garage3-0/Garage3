using Garage3.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Garage3.Services
{
    public class UniquenessValidator : IUniquenessValidator
    {
        private readonly GarageContext _db;

        private static readonly Regex RegNbrRegex = new(@"^[A-Z]{3}\s?[0-9]{3}$", RegexOptions.Compiled);

        public UniquenessValidator(GarageContext db) => _db = db;

        public async Task<string?> IsRegNbrUniqueAsync(string? regNbr, int? excludeVehicleId = null)
        {
            if (string.IsNullOrWhiteSpace(regNbr)) 
                return "Registration number is required";
            var normalized = regNbr.Trim().ToUpperInvariant();

            if (!RegNbrRegex.IsMatch(normalized))
                return "Invalid registration number format. Must be 3 letters followed by 3 numbers (e.g. ABC123).";

            bool exists = await _db.ParkedVehicle
                .AnyAsync(v => v.RegNbr == normalized && (excludeVehicleId == null || v.Id != excludeVehicleId));

            if (exists)
                return "This registration number already exists in the garage.";

            return null; 
        }

        public async Task<bool> IsPnumberUniqueAsync(string pnumber, string? excludeUserId = null)
        {
            var normalized = pnumber.Trim();
            return !await _db.Users 
                .AnyAsync(u => u.PersonalIdentityNumber == normalized && u.Id != excludeUserId);
        }
    }
}
