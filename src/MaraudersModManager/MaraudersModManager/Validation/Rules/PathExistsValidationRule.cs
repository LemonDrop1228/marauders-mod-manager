using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace MaraudersModManager.Validation.Rules;

public class PathExistsValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is string path)
        {
            if (Directory.Exists(path) || File.Exists(path))
            {
                return ValidationResult.ValidResult;
            }
        }
        return new ValidationResult(false, "Path does not exist");
    }
}