using CashFlow.Exception;
using DocumentFormat.OpenXml.InkML;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace CashFlow.Application.UseCases.Users
{
    public partial class PasswordValidator<T> : PropertyValidator<T, string>
    {
        public const string ERROR_MESSAGE_KEY = "ErrorMessage";
        public override string Name => "PasswordValidator";

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return $"{{{ERROR_MESSAGE_KEY}}}";
        }

        public override bool IsValid(ValidationContext<T> context, string password)
        {
            if (
                string.IsNullOrEmpty(password) || 
                password.Length < 8 || 
                !AtLeastOneUpperCaseLetter().IsMatch(password) || 
                !AtLeastOneLowerCaseLetter().IsMatch(password) || 
                !AtLeastOneNumber().IsMatch(password) || 
                !AtLeastOneSpecialCharacter().IsMatch(password))
            {
                context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
                return false;
            }

            return true;
        }

        [GeneratedRegex(@"[A-Z]+")]
        private static partial Regex AtLeastOneUpperCaseLetter();

        [GeneratedRegex(@"[a-z]+")]
        private static partial Regex AtLeastOneLowerCaseLetter();

        [GeneratedRegex(@"[0-9]+")]
        private static partial Regex AtLeastOneNumber();

        [GeneratedRegex(@"[\!\?\*\.\@\#\$\%\&\(\)\,\~\]\[\´\`\/\;\:\>\<\-\+\=\']+")]
        private static partial Regex AtLeastOneSpecialCharacter();
    }
}