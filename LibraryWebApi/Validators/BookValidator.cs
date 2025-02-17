using Domain.Entities;
using FluentValidation;

namespace LibraryWebApi.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(b => b.ISBN)
            .NotEmpty().WithMessage("ISBN is required.")
            .Length(13).WithMessage("ISBN must be 13 characters.");

        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title cannot exceed 255 characters.");

        RuleFor(b => b.Genre)
            .NotEmpty().WithMessage("Genre is required.")
            .MaximumLength(100).WithMessage("Genre cannot exceed 100 characters.");
    }
}
