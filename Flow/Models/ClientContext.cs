using Microsoft.EntityFrameworkCore;

namespace Flow.Models;

public class ClientContext : DbContext
{
    public ClientContext(DbContextOptions<ClientContext> options)
        : base(options)
    {
    }

    public DbSet<ClientItem> ClientItems { get; set; } = null!;
}