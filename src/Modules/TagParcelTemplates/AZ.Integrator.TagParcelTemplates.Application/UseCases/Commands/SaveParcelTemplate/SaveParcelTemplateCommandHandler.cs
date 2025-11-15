using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.Specifications;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;
using Mediator;

namespace AZ.Integrator.TagParcelTemplates.Application.UseCases.Commands.SaveParcelTemplate;

public class SaveParcelTemplateCommandHandler(
    IAggregateRepository<TagParcelTemplate> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : IRequestHandler<SaveParcelTemplateCommand>
{
    public async ValueTask<Unit> Handle(SaveParcelTemplateCommand command, CancellationToken cancellationToken)
    {
        var spec = new TagParcelTemplateByTagSpec(command.Tag, currentUser.TenantId);
        
        var tagParcelTemplate = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        var tagParcels = command.ParcelTemplates
            .Select(x => new TagParcel(
                new Dimension(x.Dimensions.Length, x.Dimensions.Width, x.Dimensions.Height), 
                x.Weight, 
                currentUser.TenantId));
        
        if (tagParcelTemplate is null)
        {
            tagParcelTemplate = TagParcelTemplate.Create(command.Tag, tagParcels, currentUser, currentDateTime);
            
            await repository.AddAsync(tagParcelTemplate, cancellationToken);
        }
        else
        {
            tagParcelTemplate.SetParcels(tagParcels);
            
            await repository.SaveChangesAsync(cancellationToken);
        }
        
        return Unit.Value;
    }
}