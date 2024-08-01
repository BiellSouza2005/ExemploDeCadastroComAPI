using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class DatabBaseContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }

    public string ConnectionString { get; }

    public DatabBaseContext()
    {
        //SQL Server
        var serverName = "ITLNB074\\MSSQLSERVER2";
        var dbName = "Clientes";
        var userName = "FormAPIUser";
        var password = "FormAPIUser";

        ConnectionString = $"Data Source={serverName};TrustServerCertificate=True; Initial Catalog={dbName}; User Id={userName}; Password={password}";
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(ConnectionString);
}

