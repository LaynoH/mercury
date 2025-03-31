using Microsoft.EntityFrameworkCore;

class MyAPIDb : DbContext
{
    public MyAPIDb(DbContextOptions<MyAPIDb> options)
        : base(options) { }

    public DbSet<MyAPI> MyAPIs => Set<MyAPI>();
}