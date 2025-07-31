using Sistran.Core.Framework.UIF.Web.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Validator
{
    public class MaxDecimalAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Gets or sets the name of the colum.
        /// </summary>
        /// <value>
        /// The name of the colum.
        /// </value>
        public string ColumName { get; set; }


        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo property = validationContext.ObjectType.GetProperty(this.ColumName);

            if (property != null && property.PropertyType == typeof(int) && value != null)
            {
                int valueNumber = (int)property.GetValue(validationContext.ObjectInstance);
                string pattern = GetPattern(valueNumber);
                if (Regex.IsMatch(value.ToString(), pattern))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        private static T GetValue<T>(object objectInstance, string propertyName)
        {
            if (objectInstance == null) throw new ArgumentNullException("objectInstance");
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException("propertyName");

            var propertyInfo = objectInstance.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
                return (T)propertyInfo.GetValue(objectInstance);
            else
                throw new ArgumentNullException(propertyName);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = ValidatorProperty.ValidateDecimal,
            };

            yield return rule;
        }

        private string GetPattern(int decimalPlaceRequired)
        {
            string pattern = "^[+]?[0-9]+";

            if (decimalPlaceRequired > 0)
                pattern += "[.,][0-9]{0," + decimalPlaceRequired + "}";

            pattern += "$";

            return pattern;
        }
    }

}