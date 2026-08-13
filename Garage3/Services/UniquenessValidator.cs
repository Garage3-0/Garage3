using Garage3.Data;
using Microsoft.EntityFrameworkCore;

namespace Garage3.Services
{
    public class UniquenessValidator : IUniquenessValidator
    {
        private readonly GarageContext _db;

        public UniquenessValidator(GarageContext db) => _db = db;

        public async Task<bool> IsRegNbrUniqueAsync(string regNbr, int currentVehicleId = 0)
        {
            if (string.IsNullOrWhiteSpace(regNbr)) 
                return false;
            var normalized = regNbr.Trim().ToUpper();
            return !await _db.ParkedVehicle 
                .AnyAsync(v => v.RegNbr == normalized && v.Id != currentVehicleId);
        }

        public async Task<bool> IsPnumberUniqueAsync(string pnumber, string? excludeUserId = null)
        {
            var normalized = pnumber.Trim();
            return !await _db.Users 
                .AnyAsync(u => u.PersonalIdentityNumber == normalized && u.Id != excludeUserId);
        }
    }
}
