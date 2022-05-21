using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace ANI.Arquitetura
{
    public class DataBase
    {
        private const string Host = "tccani.database.windows.net,1433";
        private const string Base = "BD_ANI";
        private const string Usuario = "invoice";
        private const string Senha = "tccANI2022";
        private SqlConnection sqlConnection;

        public SqlConnection GetConnection()
        {
            if (sqlConnection == null)
                SetConnection();
            return sqlConnection;
        }

        private void SetConnection()
        {
            sqlConnection = new SqlConnection($@"Data Source={Host};Initial Catalog={Base};User Id={Usuario};Password={Senha};");
        }

        private void Open()
        {
            if (GetConnection().State == ConnectionState.Open)
                GetConnection().Close();
            GetConnection().Open();
        }

        public void Close()
        {
            GetConnection().Close();
        }
        
        public T ExecuteScalar<T>(string pCommand)
        {
            var valor = default(T);
            Open();
            SqlCommand sqlCommand = new SqlCommand(pCommand, GetConnection());
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader[0] != DBNull.Value)
                        valor = (T)Convert.ChangeType(reader[0], typeof(T));
                }
            }
            Close();
            return valor;
        }

        public T ExecuteQuery<T>(string pCommand) where T : new()
        {
            var entidade = default(T);
            Open();
            SqlCommand sqlCommand = new SqlCommand(pCommand, GetConnection());
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                entidade = MapeiaRetornoPropriedade<T>(reader);
            }
            Close();
            return entidade;
        }

        public List<T> ExecuteQueryList<T>(string pCommand) where T : new()
        {
            var lista = new List<T>();
            Open();
            SqlCommand sqlCommand = new SqlCommand(pCommand, GetConnection());
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                lista = MapeiaRetornoPropriedadeLista<T>(reader);
            }
            Close();
            return lista;
        }

        public void ExecuteCommand<T>(string pCommand, T pRegistro)
        {
            Open();
            SqlCommand sqlCommand = new SqlCommand(pCommand, GetConnection());

            foreach (var variavel in pRegistro.GetType().GetProperties().ToList())
            {
                if (pCommand.Contains("@" + variavel.Name) && !pCommand.Contains("@" + variavel.Name + "BD"))
                {
                    SqlParameter sqlParametro = sqlCommand.CreateParameter();
                    sqlParametro.Value = variavel.GetValue(pRegistro) ?? DBNull.Value;
                    sqlParametro.ParameterName = variavel.Name;
                    sqlCommand.Parameters.Add(sqlParametro);
                }
            }
            sqlCommand.ExecuteNonQuery();
            Close();
        }

        private T MapeiaRetornoPropriedade<T>(SqlDataReader data) where T : new()
        {

            if (!data.Read())
                return default(T);

            var entidade = new T();
            var propiedades = typeof(T).GetProperties().ToList();

            foreach (var propriedade in propiedades)
            {
                object saida = null;
                object valor = null;
                try
                {
                    valor = data[propriedade.Name];
                    Type tipo = propriedade.PropertyType.GetTypeInfo();
                    MapeiaTipo(valor, tipo, out saida);
                    propriedade.SetValue(entidade, saida);
                }
                catch (IndexOutOfRangeException e)
                {

                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return entidade;
        }

        private List<T> MapeiaRetornoPropriedadeLista<T>(SqlDataReader data) where T : new()
        {

            var lista = new List<T>();
            var propiedades = typeof(T).GetProperties().ToList();

            while(data.Read())
            {
                var entidade = new T();
                foreach (var propriedade in propiedades)
                {
                    object saida = null;
                    object valor = null;
                    try
                    {
                        valor = data[propriedade.Name];
                        Type tipo = propriedade.PropertyType.GetTypeInfo();
                        MapeiaTipo(valor, tipo, out saida);
                        propriedade.SetValue(entidade, saida);
                    }
                    catch (IndexOutOfRangeException e)
                    {

                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                lista.Add(entidade);
            }           
            return lista;
        }

        private void MapeiaTipo(object valor, Type tipo, out object saida)
        {
            //Null
            if (valor is null || valor == DBNull.Value || valor.ToString() == "")
            {
                saida = null;
            }
            //String
            else if (tipo == typeof(string))
            {
                saida = Convert.ToString(valor);
            }
            //Long
            else if (tipo == typeof(long) || tipo == typeof(long?))
            {
                saida = Convert.ToInt64(valor);
            }
            //Int
            else if (tipo == typeof(int) || tipo == typeof(int?) || tipo.IsEnum)
            {
                saida = Convert.ToInt32(valor);
            }
            //DateTime
            else if (tipo == typeof(DateTime) || tipo == typeof(DateTime?))
            {
                saida = Convert.ToDateTime(valor);
            }
            //Decimal
            else if (tipo == typeof(decimal) || tipo == typeof(decimal?))
            {
                var valorConvertido = Convert.ToString(valor);

                if (valorConvertido.Contains('E'))
                    saida = decimal.Parse(valorConvertido, NumberStyles.Any);
                else
                    saida = Convert.ToDecimal(valor);
            }
            //Char
            else if (tipo == typeof(char) || tipo == typeof(char?))
            {
                saida = Convert.ToChar(valor);
            }
            //Bool
            else if (tipo == typeof(bool))
            {
                saida = Convert.ToBoolean(valor);
            }
            //Double
            else if (tipo == typeof(double) || tipo == typeof(double?))
            {
                saida = Convert.ToDouble(valor);
            }
            //Byte[]
            else if (tipo == typeof(byte[]))
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, valor);
                    saida = ms.ToArray();
                }
            }
            else
            {
                saida = valor;
            }           
        }
    }
}
