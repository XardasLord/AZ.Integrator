namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;

public sealed record Dimensions(int LengthMm, int WidthMm, int ThicknessMm)
{
    public static Dimensions Create(int lengthMm, int widthMm, int thicknessMm)
    {
        if (lengthMm <= 0)
            throw new ArgumentException("Length must be greater than 0", nameof(lengthMm));
        if (widthMm <= 0)
            throw new ArgumentException("Width must be greater than 0", nameof(widthMm));
        if (thicknessMm <= 0)
            throw new ArgumentException("Thickness must be greater than 0", nameof(thicknessMm));

        return new Dimensions(lengthMm, widthMm, thicknessMm);
    }
}
