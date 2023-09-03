namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidCreationInformationCreatorException : DomainException
{
    public Guid Creator { get; }
    public override string Code => "invalid_creator";

    public InvalidCreationInformationCreatorException(Guid creator, string message) : base(message)
    {
        Creator = creator;
    }
}