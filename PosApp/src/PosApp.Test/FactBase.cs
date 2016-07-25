using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Autofac;

namespace PosApp.Test
{
    public class FactBase : IDisposable
    {
        readonly IContainer m_container;

        public FactBase(Action<ContainerBuilder> customRegistration)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new PosAppModule());
            if (customRegistration != null)
            {
                customRegistration(containerBuilder);
            }

            m_container = containerBuilder.Build();
            ResetDatabase();
        }

        static void ResetDatabase()
        {
            const string cleanupSql =
                "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';" +
                "EXEC sp_MSForEachTable 'DELETE FROM ?';" +
                "EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'";

            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                ExecuteSql(connection, cleanupSql);
            }
        }

        static void ExecuteSql(SqlConnection connection, string sql)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        protected IContainer GetContainer()
        {
            return m_container;
        }

        public void Dispose()
        {
            m_container.Dispose();
        }
    }
}