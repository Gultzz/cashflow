using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}