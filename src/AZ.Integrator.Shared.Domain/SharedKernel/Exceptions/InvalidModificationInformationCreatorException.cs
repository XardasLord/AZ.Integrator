namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidModificationInformationCreatorException : DomainException
{
    public Guid Creator { get; }
    public string CreatorName { get; }
    public override string Code => "invalid_creator_of_modification";

    public InvalidModificationInformationCreatorException(Guid creator, string message) : base(message)
    {
        Creator = creator;
    }
}