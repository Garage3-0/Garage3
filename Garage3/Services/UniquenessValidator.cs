using Garage3.Data;
using Microsoft.EntityFrameworkCore;

namespace Garage3.Services
{
    public class UniquenessValidator : IUniquenessValidator
    {
        private readonly GarageContext _db;

        public UniquenessValidator(GarageContext db) => _db = db;

        public async Task<bool> IsRegNbrUniqueAsync(string regNbr, int? excludeVehicleId = null)
        {
            if (string.IsNullOrWhiteSpace(regNbr)) return true;
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
