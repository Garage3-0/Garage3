using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Garage3.Validation
{
    public class PersonalIdentityNumberAttribute : ValidationAttribute
    {
        private static readonly Regex FormatRegex = new(@"^\d{8}-\d{4}$", RegexOptions.Compiled);

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value is not string input || string.IsNullOrWhiteSpace(input))
                return new ValidationResult("Personal Identity Number required.");

            if (!FormatRegex.IsMatch(input))
                return new ValidationResult("Personal Identity Number must have format YYYYMMDD-XXXX.");

            bool validDate = DateTime.TryParseExact(
                input[..8], "yyyyMMdd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _);

            return validDate
                ? ValidationResult.Success
                : new ValidationResult("The date is not valid in the Personal Identity Number.");
        }
    }
}
