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
            
            return !await _db.Vehicles 
                .AnyAsync(v => v.RegNbr == normalized && v.Id != excludeVehicleId);
        }

        public async Task<bool> IsPnumberUniqueAsync(string pnumber, string? excludeUserId = null)
        {
            var normalized = pnumber.Trim();
            return !await _db.Users 
                .AnyAsync(u => u.PersonalIdentityNumber == normalized && u.Id != excludeUserId);
        }
    }
}
