using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Util
{
    public class SqlServerUtil
    {
        private static string _connStringSqlServer;

        private static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

            return configuration.GetConnectionString("DevConnection");
        }

        /// <summary>
        /// Crea un parametro de SqlServer
        /// </summary>
        /// <param name="nombre">nombre del parametro en el query (le agrega @)</param>
        /// <param name="tipo">tipo de dato</param>
        /// <param name="valor">valor del parametro</param>
        /// <returns>SqlParameter creado</returns>
        public static SqlParameter CreateParameter(string nombre, SqlDbType tipo, object valor)
        {
            var parameter = new SqlParameter();

            parameter.ParameterName = "@" + nombre;
            parameter.SqlDbType = tipo;
            parameter.Value = valor;

            return parameter;
        }

        /// <summary>
        /// Ejecuta una consulta sobre la base de datos
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parametros"></param>
        /// <returns>DataSet con la estructura de tablas devuelta por la consulta</returns>
        public static DataRowCollection ExecuteQueryDataSet(string query, params SqlParameter[] parametros)
        {
            try
            {
                var result = new DataSet();

                _connStringSqlServer = GetConnectionString();

                using (SqlConnection conn = new SqlConnection(_connStringSqlServer))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = query;
                            foreach (var item in parametros)
                            {
                                cmd.Parameters.Add(item);
                            }

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                            adapter.Fill(result);

                            conn.Close();
                        }
                        catch (Exception)
                        {
                            conn.Close();
                            throw;
                        }
                    }
                }
                return result.Tables[0].Rows;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Ejecuta una consulta sobre la base de datos
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parametros"></param>
        /// <returns>Núnero de filas afectadas</returns>
        public static int ExecuteNonQuery(string query, params SqlParameter[] parametros)
        {
            int affRows = 0;

            try
            {
                _connStringSqlServer = GetConnectionString();

                using (SqlConnection conn = new SqlConnection(_connStringSqlServer))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = query;
                            foreach (var item in parametros)
                            {
                                cmd.Parameters.Add(item);
                            }

                            affRows = cmd.ExecuteNonQuery();

                            conn.Close();
                        }
                        catch (Exception)
                        {
                            conn.Close();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return affRows;
        }
    }
}
