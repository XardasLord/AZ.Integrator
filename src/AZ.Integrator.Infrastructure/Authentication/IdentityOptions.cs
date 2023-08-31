﻿namespace AZ.Integrator.Infrastructure.Authentication;

public class IdentityOptions
{
    public string PrivateKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}