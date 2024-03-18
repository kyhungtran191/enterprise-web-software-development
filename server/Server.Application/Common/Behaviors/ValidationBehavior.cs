using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Server.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults =
            await Task.WhenAll(
                _validators.Select(
                    vr => vr.ValidateAsync(context, cancellationToken)));

        bool validationResultsAreValid = validationResults.All(vr => vr.IsValid);

        if (validationResultsAreValid)
        {
            return await next();
        }

        List<ValidationFailure> validationFailures = 
            validationResults
            .SelectMany(vr => vr.Errors)
            .Where(f => f != null)
            .ToList();

        return (dynamic)validationFailures.ConvertAll(
            validationFail => Error.Validation(
                code: validationFail.PropertyName,
                description: validationFail.ErrorMessage
            )
        );
    }
}