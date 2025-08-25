﻿using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;

namespace AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;

public class TagParcelTemplate : Entity, IAggregateRoot
{
    private Tag _tag;
    private CreationInformation _creationInformation;
    private List<TagParcel> _parcels;

    public Tag Tag => _tag;
    public CreationInformation CreationInformation => _creationInformation;
    public IReadOnlyCollection<TagParcel> Parcels => _parcels;

    private TagParcelTemplate()
    {
        _parcels = [];
    }

    private TagParcelTemplate(Tag tag, IEnumerable<TagParcel> parcels, ICurrentUser currentUser, ICurrentDateTime currentDateTime) 
        : this()
    {
        _tag = tag;
        _creationInformation = new CreationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
        _parcels = parcels.ToList();
    }

    public static TagParcelTemplate Create(Tag tag, IEnumerable<TagParcel> parcels, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var template = new TagParcelTemplate(tag, parcels, currentUser, currentDateTime);
        
        return template;
    }

    public void SetParcels(IEnumerable<TagParcel> parcels)
    {
        _parcels = parcels.ToList();
    }
}