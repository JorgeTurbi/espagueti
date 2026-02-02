using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace campus_sbs_admin
{
    public class Conexion_BBDD
    {
        /// Creamos el objeto de conexion
        private SqlConnection Conexion;
        
        /// Variable que se usara para almacenar la sentencia de SQL
        public string SQL;
        
        /// Creamos el objeto de ejecucion de base de datos
        private SqlCommand objCmd;
        
        /// Creamos el objeto de lectura de la base de datos
        private SqlDataReader objReader;
        
        /// Creamos el objeto para iniciar una transaccion
        private SqlTransaction Transaccion;
                
        /// Creamos un los objetos necesarios para rellenar un DataSet
        
        //Constructor		
        public Conexion_BBDD(string CadenaConexion)
        {
            //
            // TODO: agregar aquí la lógica del constructor
            //
            Conexion = new SqlConnection(CadenaConexion);
            objCmd = new SqlCommand();
        }

        //Constructor		
        public Conexion_BBDD(string parSQL, string CadenaConexion)
        {
            //
            // TODO: agregar aquí la lógica del constructor
            //
            Conexion = new SqlConnection(CadenaConexion);
            objCmd = new SqlCommand();
            SQL = parSQL;
        }

        //Funcion para cargar en el objeto de lectura los datos de la sentencia
        public void CargarSelect()
        {
            //Asignamos la SQL al objeto de ejecucion
            objCmd.CommandText = SQL;
            //Asignamos la conexion al objeto de ejecucion
            objCmd.Connection = Conexion;
            //Ejecutamos la sentencia
            objReader = objCmd.ExecuteReader();
        }
        
        public void CargarSelectParametro(SqlParameter[] SQLParametro)
        {
            //Asignamos la SQL al objeto de ejecucion
            objCmd.CommandText = SQL;
            //Asignamos la conexion al objeto de ejecucion
            objCmd.Connection = Conexion;

            objCmd.Parameters.Clear();

            int i = 0;
            for (i = 0; i < SQLParametro.Length; i++)
            {
                if (SQLParametro[i] != null)
                {
                    objCmd.Parameters.Add(SQLParametro[i]);
                }
            }
            //Ejecutamos la sentencia
            objReader = objCmd.ExecuteReader();
        }

        public int EjecutarParametro(SqlParameter[] SQLParametro)
        {
            //Asignamos la SQL al objeto de ejecucion
            objCmd.CommandText = SQL;
            //Asignamos la conexion al objeto de ejecucion
            objCmd.Connection = Conexion;

            objCmd.Parameters.Clear();

            int i = 0;
            for (i = 0; i < SQLParametro.Length; i++)
            {
                if (SQLParametro[i] != null)
                {
                    objCmd.Parameters.Add(SQLParametro[i]);
                }
            }
            //Ejecutamos la sentencia
            return objCmd.ExecuteNonQuery();
        }

        //Funcion para ejecutar la sentencia de SQL
        public int Ejecutar()
        {
            //Asignamos la SQL al objeto de ejecucion
            objCmd.CommandText = SQL;
            //Asignamos la conexion al objeto de ejecucion
            objCmd.Connection = Conexion;

            //Ejecucion de la sentencia
            return objCmd.ExecuteNonQuery();
        }

        //Funcion para cargar los registros en el buffer y mirar si hay datos
        public bool Leer()
        {
            try
            {
                //Cargamos los siguientes registros y devolvemos si hay datos
                return objReader.Read();
            }
            catch
            {
                return false;
            }
        }

        //Funcion para devolver el dato de la posicion X
        public string SacarDato(int Index)
        {
            //Miramos si el dato es nulo
            if (objReader.IsDBNull(Index) == true)
            {
                //Devolvemos un vacio
                return "";
            }
            else
            {
                //Devolvemos el dato
                return objReader.GetValue(Index).ToString();
            }
        }

        //Funcion para devolver el dato de la posicion X
        public decimal SacarDecimal(int Index)
        {
            //Miramos si el dato es nulo
            if (objReader.IsDBNull(Index) == true)
            {
                //Devolvemos un vacio
                return new decimal(0);
            }
            else
            {
                //Devolvemos el dato
                return objReader.GetDecimal(Index);
            }
        }

        //Funcion para devolver el dato de la posicion X
        public byte[] SacarDatoBinario(int Index)
        {
            //Miramos si el dato es nulo
            if (objReader.IsDBNull(Index) == true)
            {
                //Devolvemos un vacio
                return null;
            }
            else
            {
                //Devolvemos el dato
                return objReader.GetSqlBinary(Index).Value;
            }
        }

        //Funcion para devolver el dato de la posicion X
        public string SacarTipoDato(int Index)
        {
            //Miramos si el dato es nulo
            if (objReader.IsDBNull(Index) == true)
            {
                //Devolvemos un vacio
                return "";
            }
            else
            {
                //Devolvemos un vacio
                return objReader.GetValue(Index).ToString();
            }
        }

        public void ComenzarTransaccion(string nombre)
        {
            Transaccion = Conexion.BeginTransaction(nombre);
            objCmd.Transaction = Transaccion;
        }

        public void Commit()
        {
            Transaccion.Commit();
        }

        public void Rollback()
        {
            Transaccion.Rollback();
        }

        public void Rollback(string nombre)
        {
            Transaccion.Rollback(nombre);
        }

        public void Save(string nombre)
        {
            Transaccion.Save(nombre);
        }

        //Funcion para vaciar y cerrar el objeto de lectura
        public void Liberar()
        {
            //Cerramos el objeto de lectura
            objReader.Close();
        }

        //Funcion para conectar con la base de datos
        public void Conectar()
        {
            //Abrimos la conexion con la base de datos
            Conexion.Open();
        }

        //Funcion para desconectar de la base de datos
        public void Desconectar()
        {
            //Cerramos la conexion con la base de datos
            Conexion.Close();
        }

        //Funcion para cargar un DataSource
        public DataSet CargarDataSource()
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(SQL, Conexion);
                DataSet ds = new DataSet();
                da.Fill(ds);

                return ds;
            }
            catch
            {
                return null;
            }
        }
    }
}