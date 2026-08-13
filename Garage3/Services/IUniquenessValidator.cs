namespace Garage3.Services
{
    public interface IUniquenessValidator
    {
        Task<bool> IsRegNbrUniqueAsync(string regNbr, int currentVehicleId = 0);
        Task<bool> IsPnumberUniqueAsync(string pnumber, string? excludeUserId = null);
    }
}
