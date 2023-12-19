using MediatR;

namespace AZ.Integrator.TagParcelTemplates.Application.UseCases.Commands.SaveParcelTemplate;

public class SaveParcelTemplateCommandHandler : IRequestHandler<SaveParcelTemplateCommand>
{
    public SaveParcelTemplateCommandHandler()
    {
        
    }
    
    public Task<Unit> Handle(SaveParcelTemplateCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}