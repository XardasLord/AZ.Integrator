namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF;

public class PostgresOptions
{
    public string ConnectionStringApplication { get; set; }
    public string ConnectionStringHangfire { get; set; }
}