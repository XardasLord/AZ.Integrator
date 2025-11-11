namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF;

public class PostgresOptions
{
    public static string SectionName = "Infrastructure:Postgres";
    
    public string ConnectionStringApplication { get; set; }
    public string ConnectionStringHangfire { get; set; }
}