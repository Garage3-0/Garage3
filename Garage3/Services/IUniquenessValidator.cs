namespace Garage3.Services
{
    public interface IUniquenessValidator
    {
        //Task<string> IsRegNbrUniqueAsync(string? regNbr, int? excludeVehicleId = null);
        //Task<bool> IsPnumberUniqueAsync(string pnumber, string? excludeUserId = null);

        Task<string?> IsRegNbrUniqueAsync(string? regNbr, int? excludeVehicleId = null);
        Task<bool> IsPnumberUniqueAsync(string pnumber, string? excludeUserId = null);
    }
}
