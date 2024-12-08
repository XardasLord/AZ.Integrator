using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;
using AZ.Integrator.Domain.SharedKernel.Exceptions;

namespace AZ.Integrator.Domain.Extensions;

public static class GuardExtensions
{
    public static string Email(this IGuardClause guardClause, string input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        var emailAttr = new EmailAddressAttribute();

        if (string.IsNullOrWhiteSpace(input))
            throw new InvalidEmailException(input, "Email cannot be empty");

        if (!emailAttr.IsValid(input))
            throw new InvalidEmailException(input, "Email format is invalid");

        return input;
    }

    public static DateTime CreationInformationDate(this IGuardClause guardClause, DateTime input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (input == default)
            throw new InvalidCreationInformationDateException(input, "Creation date cannot be empty");

        if (input > DateTime.UtcNow)
            throw new InvalidCreationInformationDateException(input, "Creation date cannot be in the future");

        return input;
    }

    public static Guid CreationInformationCreator(this IGuardClause guardClause, Guid input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        // if (input == default)
        //     throw new InvalidCreationInformationCreatorException(input, "Creator cannot be empty");

        // if (input == Guid.Empty)
        //     throw new InvalidCreationInformationCreatorException(input, "Creator cannot be unknown");

        return input;
    }

    public static DateTime ModificationInformationDate(this IGuardClause guardClause, DateTime input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (input == default)
            throw new InvalidModificationInformationDateException(input, "Modification date cannot be empty");

        if (input > DateTime.UtcNow)
            throw new InvalidModificationInformationDateException(input, "Modification date cannot be in the future");

        return input;
    }

    public static Guid ModificationInformationCreator(this IGuardClause guardClause, Guid input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (input == default)
            throw new InvalidModificationInformationCreatorException(input, "Creator of modification cannot be empty");

        if (input == Guid.Empty)
            throw new InvalidModificationInformationCreatorException(input, "Creator of modification cannot be unknown");

        return input;
    }

    

    public static DateTime DeletionInformationDate(this IGuardClause guardClause, DateTime input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (input == default)
            throw new InvalidDeletionInformationDateException(input, "Deletion date cannot be empty");

        if (input > DateTime.UtcNow)
            throw new InvalidDeletionInformationDateException(input, "Deletion date cannot be in the future");

        return input;
    }

    public static Guid DeletionInformationPerson(this IGuardClause guardClause, Guid input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (input == default)
            throw new InvalidDeletionInformationPersonException(input, "Deletor cannot be empty");

        if (input == Guid.Empty)
            throw new InvalidDeletionInformationPersonException(input, "Deletor cannot be unknown");

        return input;
    }
    
    public static string AllegroOrderNumber(this IGuardClause guardClause, string input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new InvalidAllegroOrderNumberException(input, "Allegro order number cannot be empty");

        return input;
    }

    public static string TenantId(this IGuardClause guardClause, string input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        return input;
    }
}