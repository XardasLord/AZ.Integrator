using AutoMapper;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.Specifications;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;
using MediatR;

namespace AZ.Integrator.TagParcelTemplates.Application.UseCases.Commands.SaveParcelTemplate;

public class SaveParcelTemplateCommandHandler : IRequestHandler<SaveParcelTemplateCommand>
{
    private readonly IAggregateRepository<TagParcelTemplate> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentDateTime _currentDateTime;

    public SaveParcelTemplateCommandHandler(
        IAggregateRepository<TagParcelTemplate> repository,
        IMapper mapper,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUser = currentUser;
        _currentDateTime = currentDateTime;
    }
    
    public async Task<Unit> Handle(SaveParcelTemplateCommand command, CancellationToken cancellationToken)
    {
        var spec = new TagParcelTemplateByTagAndTenantSpec(command.Tag, _currentUser.TenantId);
        
        var tagParcelTemplate = await _repository.SingleOrDefaultAsync(spec, cancellationToken);

        var tagParcels = command.ParcelTemplates
            .Select(x => new TagParcel(new Dimension(x.Dimensions.Length, x.Dimensions.Width, x.Dimensions.Height), x.Weight));
        
        if (tagParcelTemplate is null)
        {
            tagParcelTemplate = TagParcelTemplate.Create(command.Tag, tagParcels, _currentUser, _currentDateTime);
            
            await _repository.AddAsync(tagParcelTemplate, cancellationToken);
        }
        else
        {
            tagParcelTemplate.SetParcels(tagParcels);
            
            await _repository.SaveChangesAsync(cancellationToken);
        }

        return new Unit();
    }
}