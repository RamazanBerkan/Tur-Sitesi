using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Helpers.Attributes
{
    public class ValueControllAttribute : ValidationAttribute
    {
        private readonly bool _param;
        public ValueControllAttribute(bool param)
        {
            _param = param;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (_param == (bool)value)
            {
                return ValidationResult.Success;
            }

            var errorMessage = FormatErrorMessage((validationContext.DisplayName));
            return new ValidationResult(errorMessage);
        }
    }

    public class ValueControllIntAttribute : ValidationAttribute
    {
        private readonly int _param;
        public ValueControllIntAttribute(int param)
        {
            _param = param;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (_param != (int)value)
            {
                return ValidationResult.Success;
            }

            var errorMessage = FormatErrorMessage((validationContext.DisplayName));
            return new ValidationResult(errorMessage);
        }
    }
}
