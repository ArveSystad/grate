﻿using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;
using TestCommon.TestInfrastructure;
using Testcontainers.MsSql;

namespace SqlServer.TestInfrastructure;
public class SqlServerTestContainerDatabase(
    GrateTestConfig grateTestConfig,
    ILogger<SqlServerTestContainerDatabase> logger)
    : TestContainerDatabase(logger)
{
    // Run with linux/amd86 on ARM architectures too, the docker emulation is good enough
    //public override string DockerImage => "mcr.microsoft.com/mssql/server:2019-latest";
    public override string DockerImage => grateTestConfig.DockerImage ?? "mcr.microsoft.com/mssql/server:2022-latest";
    protected override int InternalPort => 1433;

    protected override IContainer InitializeTestContainer(ILogger logger)
    {
        return new MsSqlBuilder()
            .WithImage(DockerImage)
            .WithEnvironment("DOCKER_DEFAULT_PLATFORM", "linux/amd64")
            .WithPassword(AdminPassword)
            .WithPortBinding(InternalPort, true)
            .WithEnvironment("MSSQL_COLLATION", "Danish_Norwegian_CI_AS")
            .WithLogger(logger)
            .Build();
    }
    
    public override string AdminConnectionString =>
        $"Data Source=localhost,{Port};Initial Catalog=master;User Id=sa;Password={AdminPassword};Encrypt=false;Pooling=false";

    public override string ConnectionString(string database) =>
        $"Data Source=localhost,{Port};Initial Catalog={database};User Id=sa;Password={AdminPassword};Encrypt=false;Pooling=false;Connect Timeout=2";

    public override string UserConnectionString(string database) =>
        $"Data Source=localhost,{Port};Initial Catalog={database};User Id=zorro;Password=batmanZZ4;Encrypt=false;Pooling=false;Connect Timeout=2";
    
}
