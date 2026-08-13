namespace Garage3.Services
{
    public interface IUniquenessValidator
    {
        Task<bool> IsRegNbrUniqueAsync(string regNbr, int? excludeVehicleId = null);
        Task<bool> IsPnumberUniqueAsync(string pnumber, string? excludeUserId = null);
    }
}
