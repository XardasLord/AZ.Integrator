﻿using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.TagParcelTemplates.Application.UseCases.Commands.SaveParcelTemplate;

public record SaveParcelTemplateCommand(string Tag, IEnumerable<ParcelTemplateModel> ParcelTemplates) : HeaderRequest, IRequest;