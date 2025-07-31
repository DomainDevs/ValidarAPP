using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sistran.Core.Framework.UIF.Web.Helpers.Attributes
{
    public enum ValidationInputType
    {
        Alphanumeric,
        Name,
        Block
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InputValidationAttribute : ValidationAttribute
    {
        public ValidationInputType InputType { get; private set; }

        public InputValidationAttribute(ValidationInputType inputType)
        {
            this.InputType = inputType;
        }

        public override bool IsValid(object value)
        {
            // Si es nulo o vacio, no ejecuta validaci'ones
            var input = value as string;
            if (string.IsNullOrEmpty(input))
                return true;

            // Obtiene el patrón correspondiente al tipo de entrada
            var regex = new Regex(GetPattern(InputType), RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("El campo {0} contiene caracteres inválidos.", name);
        }

        // Devuelve el patrón de expresión regular según el tipo de validación solicitado
        private string GetPattern(ValidationInputType inputType)
        {
            switch (inputType)
            {
                case ValidationInputType.Name:
                    // Permite letras (con acentos), ñ, espacios, guiones, apóstrofes y puntos
                    return @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-'.]*$";

                case ValidationInputType.Alphanumeric:
                    // Permite letras, números, espacios, guiones, guion bajo y puntos
                    return @"^[a-zA-Z0-9\s\-_.]*$";

                case ValidationInputType.Block:
                    // Permite letras (de cualquier idioma), números, puntuación, espacios, saltos de línea y tabulaciones
                    return @"^[\p{L}\p{N}\p{P}\p{Zs}\r\n\t]*$";

                default:
                    // En caso de no reconocer el tipo, se lanza una excepción
                    throw new NotImplementedException("Tipo de validación no implementado: " + inputType.ToString());
            }
        }
    }

}