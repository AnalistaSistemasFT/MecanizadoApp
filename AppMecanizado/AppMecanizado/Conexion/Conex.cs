using AppMecanizado.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace AppMecanizado.Conexion
{
    public class Conex
    {
        //  <add name = "SQLSERVER5" connectionString="Server=10.10.100.26;Database=FTODO;Integrated Security=No; Uid=sa;Password=Passw0rd"
        //providerName="System.Data.SqlClient" />
        static string cadenaConexion = @"Data Source=10.10.100.26;Initial Catalog=LYBK;Persist Security Info=True;user id=sa;password=Passw0rd;Connect Timeout=160";
        //static string cadenaConexion = @"Data Source=192.168.0.200;Initial Catalog=LYBK;Persist Security Info=True;user id=sa;password=PlantaV.;Connect Timeout=160";
        static string cadenaConexion_2 = @"Data Source=192.168.0.200;Initial Catalog=Empleados;Persist Security Info=True;user id=sa;password=PlantaV.;Connect Timeout=160";
        //static string cadenaConexion_3 = @"Data Source=10.10.100.26;Initial Catalog=FTODO;Persist Security Info=True;user id=sa;password=Passw0rd;Connect Timeout=160";
        public DataTable ejecutarConsultaLogin(string sentencia)
        {
            //List<Parametro> listaEmpleados = new List<Parametro>();
            DataTable data = new DataTable();
            using (SqlConnection con = new SqlConnection(cadenaConexion_2))
            {
                con.Open();
                using (SqlCommand comando = new SqlCommand(sentencia, con))
                {
                    using (SqlDataAdapter reader = new SqlDataAdapter(comando))
                    {
                        reader.Fill(data);
                    }
                }
                con.Close();
                return data;
            }
        }
        public DataTable ejecutarConsulta(string sentencia)
        {
            //List<Parametro> listaEmpleados = new List<Parametro>();
            DataTable data = new DataTable();
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (SqlCommand comando = new SqlCommand(sentencia, con))
                {
                    using (SqlDataAdapter reader = new SqlDataAdapter(comando))
                    {
                        reader.Fill(data);
                    }
                }
                con.Close();
                return data;
            }
        }
        public DataTable ejecutarConsultaEmpleado(string sentencia)
        {
            //List<Parametro> listaEmpleados = new List<Parametro>();
            DataTable data = new DataTable();
            using (SqlConnection con = new SqlConnection(cadenaConexion_2))
            {
                con.Open();
                using (SqlCommand comando = new SqlCommand(sentencia, con))
                {
                    using (SqlDataAdapter reader = new SqlDataAdapter(comando))
                    {
                        reader.Fill(data);
                    }
                }
                con.Close();
                return data;
            }
        }
        public bool ejecutarconsultaEnviar(string sentencia)
        {
            DataTable data = new DataTable();
            bool retornar = false;
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                try
                {
                    using (SqlCommand comando = new SqlCommand(sentencia, con))
                    {
                        comando.ExecuteNonQuery();
                        retornar = true;
                    }
                }
                catch (Exception ee)
                {
                    retornar = false;
                }
                con.Close();
            }
            return retornar;
        }
        public bool response;
        public bool GuardarSolicitud(ServicioMecanizado _servicioMecanizado)
        {
            string sql = "INSERT INTO tblServicioMecanizado (p_Id_servicio_mecanizado, p_Id_solicitante, p_Fecha, p_Centro_trabajo, p_Prioridad, p_Producto, p_Descripcion, p_Cantidad, p_Muestra_img, p_Plano_img, p_Material, p_Dureza, p_Otros, p_Fecha_Finalizacion, p_Estado, p_Id_empleado, p_Cantidad_pendiente, Codigo, Correlativo) " +
                                                "values (@p_Id_servicio_mecanizado, @p_Id_solicitante, @p_Fecha, @p_Centro_trabajo, @p_Prioridad, @p_Producto ,@p_Descripcion, @p_Cantidad, @p_Muestra_img, @p_Plano_img, @p_Material, @p_Dureza, @p_Otros , @p_Fecha_Finalizacion, @p_Estado, @p_Id_empleado, @p_Cantidad_pendiente, @Codigo, @Correlativo)";
            //bool response;

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();

                using (SqlCommand comando = new SqlCommand(sql, con))
                {
                    try
                    {
                        CultureInfo ci = new CultureInfo("es-ES");
                        comando.Parameters.Add("@p_Id_servicio_mecanizado", SqlDbType.Int).Value = _servicioMecanizado.p_Id_servicio_mecanizado;
                        comando.Parameters.Add("@p_Id_solicitante", SqlDbType.Int).Value = _servicioMecanizado.p_Id_solicitante;
                        comando.Parameters.Add("@p_Fecha", SqlDbType.DateTime).Value = _servicioMecanizado.p_Fecha;
                        comando.Parameters.Add("@p_Centro_trabajo", SqlDbType.VarChar).Value = _servicioMecanizado.p_Centro_trabajo;
                        comando.Parameters.Add("@p_Prioridad", SqlDbType.Int).Value = _servicioMecanizado.p_Prioridad;
                        comando.Parameters.Add("@p_Producto", SqlDbType.Int).Value = _servicioMecanizado.p_Producto;
                        comando.Parameters.Add("@p_Descripcion", SqlDbType.VarChar).Value = _servicioMecanizado.p_Descripcion;
                        comando.Parameters.Add("@p_Cantidad", SqlDbType.Int).Value = _servicioMecanizado.p_Cantidad;
                        comando.Parameters.Add("@p_Muestra_img", SqlDbType.VarBinary).Value = _servicioMecanizado.p_Muestra_img;
                        comando.Parameters.Add("@p_Plano_img", SqlDbType.VarBinary).Value = _servicioMecanizado.p_Plano_img;
                        comando.Parameters.Add("@p_Material", SqlDbType.VarChar).Value = _servicioMecanizado.p_Material;
                        comando.Parameters.Add("@p_Dureza", SqlDbType.VarChar).Value = _servicioMecanizado.p_Dureza;
                        comando.Parameters.Add("@p_Otros", SqlDbType.VarChar).Value = _servicioMecanizado.p_Otros;
                        comando.Parameters.Add("@p_Fecha_Finalizacion", SqlDbType.DateTime).Value = _servicioMecanizado.p_Fecha_Finalizacion;
                        comando.Parameters.Add("@p_Estado", SqlDbType.Int).Value = _servicioMecanizado.p_Estado;
                        comando.Parameters.Add("@p_Id_empleado", SqlDbType.Int).Value = _servicioMecanizado.p_Id_Empleado;
                        comando.Parameters.Add("@p_Cantidad_pendiente", SqlDbType.Int).Value = _servicioMecanizado.p_Cantidad_pendiente;
                        comando.Parameters.Add("@Codigo", SqlDbType.VarChar).Value = _servicioMecanizado.Codigo;
                        comando.Parameters.Add("@Correlativo", SqlDbType.Int).Value = _servicioMecanizado.Correlativo;
                        comando.CommandType = CommandType.Text;
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("####################################= " + err.Message);
                    }
                    try
                    {
                        if (comando.ExecuteNonQuery() == 1)
                            response = true;
                        else
                            response = false;
                    }

                    catch (Exception err)
                    {
                        Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@= " + err.Message);
                        Console.WriteLine("@@@@@@@@@@@@@@@@@@@@################= " + err.ToString());
                    }
                }
                con.Close();
            }
            return response;
        }
        public bool ActualizarRespuestaMecanizado(RespuestaMecanizado _respuestaMecanizado, string _Observacion, int _idRespuestaMecanizado)
        {
            //UPDATE tblRespuestaMecanizado SET p_Observacion = " + Observacion + " WHERE p_Id_respuesta_mecanizado = " + (object)Id_respuesta_mecanizado;
            string sql = "UPDATE tblRespuestaMecanizado SET p_Observacion = " + _Observacion + " WHERE p_Id_respuesta_mecanizado = " + _idRespuestaMecanizado;
            bool response;

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (SqlCommand comando = new SqlCommand(sql, con))
                {
                    CultureInfo ci = new CultureInfo("es-ES");
                    comando.Parameters.Add("@p_Id_respuesta_mecanizado", SqlDbType.Int).Value = _respuestaMecanizado.p_Id_respuesta_mecanizado;
                    comando.Parameters.Add("@p_Id_servicio", SqlDbType.Int).Value = _respuestaMecanizado.p_Id_servicio;
                    comando.Parameters.Add("@p_Estado", SqlDbType.VarChar).Value = _respuestaMecanizado.p_Estado;
                    comando.Parameters.Add("@p_Observacion", SqlDbType.VarChar).Value = _respuestaMecanizado.p_Observacion;
                    comando.CommandType = CommandType.Text;
                    if (comando.ExecuteNonQuery() == 1)
                        response = true;
                    else
                        response = false;
                }
                con.Close();
            }
            return response;

        }
    }
}
