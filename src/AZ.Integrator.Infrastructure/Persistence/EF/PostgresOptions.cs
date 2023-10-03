namespace AZ.Integrator.Infrastructure.Persistence.EF;

internal class PostgresOptions
{
    public string ConnectionStringApplication { get; set; }
    public string ConnectionStringHangfire { get; set; }
}