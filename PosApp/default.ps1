task build {
    exec { msbuild ".\posApp.sln" }
    $connectionString = 'Data Source=.\SQLExpress;Initial Catalog=pos;Integrated Security=True'
    $migrationTarget = ".\src\Pos.Migration\bin\Debug\Pos.Migration.dll"
    iex ".\packages\FluentMigrator.1.6.2\tools\Migrate.exe /connection '$connectionString' /db SqlServer2014 /target '$migrationTarget'"
}