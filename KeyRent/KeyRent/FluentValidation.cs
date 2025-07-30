using FluentValidation;
using KeyRent.ViewModels;

public class UserViewModelValidator : AbstractValidator<UserViewModel>
{
    public UserViewModelValidator()
    {
        // Dodaj zaawansowaną walidację dla FirstName
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imię jest wymagane.")
            .Length(2, 50).WithMessage("Imię musi mieć od 2 do 50 znaków.")
            .Matches(@"^[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\s-]+$").WithMessage("Imię może zawierać tylko litery.");

        // Podobnie dla LastName
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .Length(2, 50).WithMessage("Nazwisko musi mieć od 2 do 50 znaków.")
            .Matches(@"^[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\s-]+$").WithMessage("Nazwisko może zawierać tylko litery.");

        // Walidacja dla Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Adres e-mail jest wymagany.")
            .EmailAddress().WithMessage("Nieprawidłowy format adresu e-mail.");

        // Walidacja dla PhoneNumber
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Numer telefonu jest wymagany.")
            .Matches(@"^\d{9}$").WithMessage("Numer telefonu musi zawierać dokładnie 9 cyfr.");
    }
}
