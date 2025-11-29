namespace AZ.Integrator.Orders.Contracts.Dtos;

public class AllegroShopInfoResponseDto
{
    public string Id { get; set; } = default!;
    public string Login { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = default!;
    public BaseMarketplaceInfoResponseDto BaseMarketplace { get; set; } = default!;
    public CompanyInfoResponseDto? Company { get; set; }
    public List<string>? Features { get; set; }
}

public class BaseMarketplaceInfoResponseDto
{
    public string Id { get; set; } = default!;
}

public class CompanyInfoResponseDto
{
    public string Name { get; set; } = default!;
    public string TaxId { get; set; } = default!;
}