/*
 * daoGenerico.cs
 * Objeto de Acceso a Datos asociada a una Entidad
 *
 *  © 2011-2012 Rodrigo del Angel <rdelangelhmx@gmail.com>
 * Some Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace DAO
{
    public class daoGenerico
    {
        ///
        /// Función que realizará una consulta a base de datos, devolviendo los registros que
        /// cumplan con los criterios establecidos en el DTO pasado como parámetro.
        /// <changelog>
        /// Rodrigo del Angel    14/08/2011    Creación
        /// Rodrigo del Angel    26/09/2012    Modificación
        /// Rodrigo del Angel    10/06/2012    Modificación
        /// </changelog>
        /// <summary>
        /// Trae los registros totales de una tabla segun la condicion
        /// </summary>
        /// <param name="datos">Estructura de Datos para la condicion</param>
        /// <returns>Cangtidad de Registros = Entero</returns>
        public int regTot(object datos)
        {
            try
            {
                StringBuilder SQLString = new StringBuilder();
                StringBuilder Campos = new StringBuilder();
                ArrayList Parametros = new ArrayList();
                if (datos == null)
                    throw new NullReferenceException("daoGenerico.select(datos)");
                Dictionary<string, object> coleccionDatos = new Dictionary<string, object>();
                coleccionDatos = ObtenerElementos(datos, MemberTypes.Property);
                if (coleccionDatos != null)
                {
                    foreach (string key in coleccionDatos.Keys)
                    {
                        if (coleccionDatos[key] != null)
                        {
                            if (coleccionDatos[key].ToString().Contains("%"))
                                Campos.Append(key).Append(" like '%").Append(coleccionDatos[key].ToString()).Append("' AND ");
                            else
                                Campos.Append(key).Append(" = @").Append(key).Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatos[key]));
                        }
                    }
                }
                SQLString.Append("SELECT Count(*) as RegTot FROM ").Append((datos.GetType()).Name);
                if (Campos.Length > 0)
                {
                    SQLString.Append(" WHERE ");
                    SQLString.Append(Campos.ToString().Substring(0, Campos.ToString().Length - 4));
                }
                return int.Parse(dbConn.GetCampo(SQLString.ToString(), Parametros));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ///
        /// Función que realizará una consulta a base de datos, devolviendo los registros que
        /// cumplan con los criterios establecidos en el DTO pasado como parámetro.
        /// <changelog>
        /// Rodrigo del Angel    14/08/2011    Creación
        /// Rodrigo del Angel    26/09/2012    Modificación
        /// </changelog>
        /// <summary>
        /// Trae los registros totales de una tabla segun la condicion
        /// </summary>
        /// <param name="datos">Estructura de Datos para la condicion</param>
        /// <returns>Cangtidad de Registros = Entero</returns>
        public int regTot(object datosIni, object datosFin)
        {
            try
            {
                StringBuilder SQLString = new StringBuilder();
                StringBuilder Campos = new StringBuilder();
                StringBuilder CamposSelect = new StringBuilder();
                ArrayList Parametros = new ArrayList();
                if (datosIni == null || datosFin == null)
                    throw new NullReferenceException("daoGenerico.select(datosIni,datosFin)");
                Dictionary<string, object> coleccionDatosIni = new Dictionary<string, object>();
                Dictionary<string, object> coleccionDatosFin = new Dictionary<string, object>();
                coleccionDatosIni = ObtenerElementos(datosIni, MemberTypes.Property);
                coleccionDatosFin = ObtenerElementos(datosFin, MemberTypes.Property);
                if (coleccionDatosIni != null)
                {
                    foreach (string key in coleccionDatosIni.Keys)
                    {
                        if (coleccionDatosIni[key] != null && coleccionDatosFin[key] != null)
                        {
                            Campos.Append(key).Append(" between @").Append(key).Append("Ini AND @").Append(key).Append("Fin ");
                            Campos.Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key + "Ini", coleccionDatosIni[key]));
                            Parametros.Add(new SqlParameter("@" + key + "Fin", coleccionDatosFin[key]));
                        }
                        if (coleccionDatosIni[key] != null && coleccionDatosFin[key] == null)
                        {
                            Campos.Append(key).Append(" like @").Append(key);
                            Campos.Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatosIni[key]));
                        }
                        CamposSelect.Append(key).Append(",");
                    }
                }
                SQLString.Append("SELECT Count(*) as RegTot FROM ").Append((datosIni.GetType()).Name);
                if (Campos.Length > 0)
                {
                    SQLString.Append(" WHERE ");
                    SQLString.Append(Campos.ToString().Substring(0, Campos.ToString().Length - 4));
                }
                return int.Parse(dbConn.GetCampo(SQLString.ToString(), Parametros));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///
        /// Función que realizará una consulta a base de datos, devolviendo los registros que
        /// cumplan con los criterios establecidos en el DTO pasado como parámetro.
        /// <changelog>
        /// Rodrigo del Angel    14/08/2011    Creación
        /// Rodrigo del Angel    26/09/2012    Modificación
        /// </changelog>
        /// <summary>
        /// Selecciona un conjunto de registros segun las condiciones enviadas
        /// </summary>
        /// <param name="datos">Estructura de Datos con las condiciones</param>
        /// <returns>Objeto de datos seleccionados</returns>
        public DataTable getData(object datos)
        {
            try
            {
                StringBuilder SQLString = new StringBuilder();
                StringBuilder Campos = new StringBuilder();
                StringBuilder CamposSelect = new StringBuilder();
                ArrayList Parametros = new ArrayList();
                if (datos == null)
                    throw new NullReferenceException("daoGenerico.select(datos)");
                Dictionary<string, object> coleccionDatos = new Dictionary<string, object>();
                coleccionDatos = ObtenerElementos(datos, MemberTypes.Property);
                if (coleccionDatos != null)
                {
                    foreach (string key in coleccionDatos.Keys)
                    {
                        if (coleccionDatos[key] != null)
                        {
                            if (coleccionDatos[key].ToString().Contains("%"))
                                Campos.Append(key).Append(" like '%").Append(coleccionDatos[key].ToString()).Append("' AND ");
                            else
                                Campos.Append(key).Append(" = @").Append(key).Append(" AND ");
                            //Campos.Append(key).Append(" = ?").Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatos[key]));
                        }
                        CamposSelect.Append(key).Append(",");
                    }
                }
                SQLString.Append("SELECT " + CamposSelect.ToString().Substring(0, CamposSelect.ToString().Length - 1) + " FROM ").Append((datos.GetType()).Name);
                if (Campos.Length > 0)
                {
                    SQLString.Append(" WHERE ");
                    SQLString.Append(Campos.ToString().Substring(0, Campos.ToString().Length - 4));
                }
                return dbConn.GetDataTable(SQLString.ToString(), Parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///
        /// Función que realizará una consulta a base de datos, devolviendo los registros que
        /// cumplan con los criterios establecidos en el DTO pasado como parámetro + paginacion.
        /// <changelog>
        /// Rodrigo del Angel    14/08/2011    Creación
        /// Rodrigo del Angel    26/09/2012    Modificación
        /// </changelog>
        /// <summary>
        /// Para Seleccionar registros paginados 
        /// </summary>
        /// <param name="datos">Estructura de Datos con las condiciones</param>
        /// <param name="Pagina">Pagina a seleccionar</param>
        /// <param name="Registros">Cantidad de registros a mostrar</param>
        /// <returns>Objeto de Datos seleccionado</returns>
        public DataTable getData(object datos, int Pagina, int Registros)
        {
            try
            {
                StringBuilder SQLString = new StringBuilder();
                StringBuilder Campos = new StringBuilder();
                StringBuilder CamposSelect = new StringBuilder();
                ArrayList Parametros = new ArrayList();
                if (datos == null)
                    throw new NullReferenceException("daoGenerico.select(datos)");
                Dictionary<string, object> coleccionDatos = new Dictionary<string, object>();
                coleccionDatos = ObtenerElementos(datos, MemberTypes.Property);
                if (coleccionDatos != null)
                {
                    foreach (string key in coleccionDatos.Keys)
                    {
                        if (coleccionDatos[key] != null)
                        {
                            if (coleccionDatos[key].ToString().Contains("%"))
                                Campos.Append(key).Append(" like '%").Append(coleccionDatos[key].ToString()).Append("' AND ");
                            else
                                Campos.Append(key).Append(" = @").Append(key).Append(" AND ");
                            //Campos.Append(key).Append(" = ?").Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatos[key]));
                        }
                        CamposSelect.Append(key).Append(",");
                    }
                }
                SQLString.Append("SELECT " + CamposSelect.ToString().Substring(0, CamposSelect.ToString().Length - 1) + " FROM ").Append((datos.GetType()).Name);
                if (Campos.Length > 0)
                {
                    SQLString.Append(" WHERE ");
                    SQLString.Append(Campos.ToString().Substring(0, Campos.ToString().Length - 4));
                }
                Pagina = Pagina == 1 ? 0 : Pagina - 1;
                return dbConn.GetDataTable(SQLString.ToString(), Parametros, Pagina * Registros, Registros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///
        /// Función que realizará una consulta a base de datos, devolviendo los registros que
        /// cumplan con los criterios establecidos en el DTO pasado como parámetro + paginacion.
        /// <changelog>
        /// Rodrigo del Angel    14/08/2011    Creación
        /// Rodrigo del Angel    26/09/2012    Modificación
        /// </changelog>
        /// <summary>
        /// Para Seleccionar registros paginados 
        /// </summary>
        /// <param name="datos">Estructura de Datos con las condiciones</param>
        /// <param name="Pagina">Pagina a seleccionar</param>
        /// <param name="Registros">Cantidad de registros a mostrar</param>
        /// <returns>Objeto de Datos seleccionado</returns>
        public DataTable getData(object datosIni, object datosFin, int Pagina, int Registros)
        {
            try
            {
                StringBuilder SQLString = new StringBuilder();
                StringBuilder Campos = new StringBuilder();
                StringBuilder CamposSelect = new StringBuilder();
                ArrayList Parametros = new ArrayList();
                if (datosIni == null || datosFin == null)
                    throw new NullReferenceException("daoGenerico.select(datosIni,datosFin)");
                Dictionary<string, object> coleccionDatosIni = new Dictionary<string, object>();
                Dictionary<string, object> coleccionDatosFin = new Dictionary<string, object>();
                coleccionDatosIni = ObtenerElementos(datosIni, MemberTypes.Property);
                coleccionDatosFin = ObtenerElementos(datosFin, MemberTypes.Property);
                if (coleccionDatosIni != null)
                {
                    foreach (string key in coleccionDatosIni.Keys)
                    {
                        if (coleccionDatosIni[key] != null && coleccionDatosFin[key] != null)
                        {
                            Campos.Append(key).Append(" between @").Append(key).Append("Ini AND @").Append(key).Append("Fin ");
                            Campos.Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key + "Ini", coleccionDatosIni[key]));
                            Parametros.Add(new SqlParameter("@" + key + "Fin", coleccionDatosFin[key]));
                        }
                        if (coleccionDatosIni[key] != null && coleccionDatosFin[key] == null)
                        {
                            Campos.Append(key).Append(" like @").Append(key);
                            Campos.Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatosIni[key]));
                        }
                        CamposSelect.Append(key).Append(",");
                    }
                }
                SQLString.Append("SELECT " + CamposSelect.ToString().Substring(0, CamposSelect.ToString().Length - 1) + " FROM ").Append((datosIni.GetType()).Name);
                if (Campos.Length > 0)
                {
                    SQLString.Append(" WHERE ");
                    SQLString.Append(Campos.ToString().Substring(0, Campos.ToString().Length - 4));
                }
                Pagina = Pagina == 1 ? 0 : Pagina - 1;
                return dbConn.GetDataTable(SQLString.ToString(), Parametros, Pagina * Registros, Registros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///
        /// Función que realizará una consulta a base de datos, devolviendo los registros que
        /// cumplan con los criterios establecidos en el DTO pasado como parámetro + paginacion.
        /// <changelog>
        /// Rodrigo del Angel    14/08/2011    Creación
        /// Rodrigo del Angel    26/09/2012    Modificación
        /// </changelog>
        /// <summary>
        /// Para Seleccionar registros paginados 
        /// </summary>
        /// <param name="datos">Estructura de Datos con las condiciones</param>
        /// <param name="Pagina">Pagina a seleccionar</param>
        /// <param name="Registros">Cantidad de registros a mostrar</param>
        /// <returns>Objeto de Datos seleccionado</returns>
        public DataTable getData(object datosIni, object datosFin)
        {
            try
            {
                StringBuilder SQLString = new StringBuilder();
                StringBuilder Campos = new StringBuilder();
                StringBuilder CamposSelect = new StringBuilder();
                ArrayList Parametros = new ArrayList();
                if (datosIni == null || datosFin == null)
                    throw new NullReferenceException("daoGenerico.select(datosIni,datosFin)");
                Dictionary<string, object> coleccionDatosIni = new Dictionary<string, object>();
                Dictionary<string, object> coleccionDatosFin = new Dictionary<string, object>();
                coleccionDatosIni = ObtenerElementos(datosIni, MemberTypes.Property);
                coleccionDatosFin = ObtenerElementos(datosFin, MemberTypes.Property);
                if (coleccionDatosIni != null)
                {
                    foreach (string key in coleccionDatosIni.Keys)
                    {
                        if (coleccionDatosIni[key] != null && coleccionDatosFin[key] != null)
                        {
                            Campos.Append(key).Append(" between @").Append(key).Append("Ini AND @").Append(key).Append("Fin ");
                            Campos.Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key + "Ini", coleccionDatosIni[key]));
                            Parametros.Add(new SqlParameter("@" + key + "Fin", coleccionDatosFin[key]));
                        }
                        if (coleccionDatosIni[key] != null && coleccionDatosFin[key] == null)
                        {
                            Campos.Append(key).Append(" like @").Append(key);
                            Campos.Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatosIni[key]));
                        }
                        CamposSelect.Append(key).Append(",");
                    }
                }
                SQLString.Append("SELECT " + CamposSelect.ToString().Substring(0, CamposSelect.ToString().Length - 1) + " FROM ").Append((datosIni.GetType()).Name);
                if (Campos.Length > 0)
                {
                    SQLString.Append(" WHERE ");
                    SQLString.Append(Campos.ToString().Substring(0, Campos.ToString().Length - 4));
                }
                return dbConn.GetDataTable(SQLString.ToString(), Parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///
        /// Función que realizará una actualizacion a base de datos, devolviendo si lo ejecuto o no
        /// y que se cumplan con los criterios establecidos en el DTO pasado como parámetros.
        /// <changelog>
        /// Rodrigo del Angel    14/08/2011    Creación
        /// </changelog>
        /// <summary>
        /// Para actualizar una tabla de la base de datos
        /// </summary>
        /// <param name="datos">Estructura de Datos con los campos</param>
        /// <param name="parametros">Estructura de Datos con los datos</param>
        /// <returns>Verdadero/Falso</returns>
        public Boolean setData(object datos, object parametros)
        {
            try
            {
                StringBuilder SQLString = new StringBuilder();
                StringBuilder Campos = new StringBuilder();
                StringBuilder CamposParam = new StringBuilder();
                ArrayList Parametros = new ArrayList();
                if (datos == null && parametros == null)
                    throw new NullReferenceException("daoGenerico.update(datos,parametros)");
                Dictionary<string, object> coleccionDatos = new Dictionary<string, object>();
                coleccionDatos = ObtenerElementos(datos, MemberTypes.Property);
                if (coleccionDatos != null)
                {
                    foreach (string key in coleccionDatos.Keys)
                    {
                        if (coleccionDatos[key] != null)
                        {
                            Campos.Append(key).Append(" = @").Append(key).Append(", ");
                            //Campos.Append(key).Append(" = ?").Append(", ");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatos[key]));
                        }
                    }
                }
                coleccionDatos = ObtenerElementos(parametros, MemberTypes.Property);
                if (coleccionDatos != null)
                {
                    foreach (string key in coleccionDatos.Keys)
                    {
                        if (coleccionDatos[key] != null)
                        {
                            if (coleccionDatos[key].ToString().Contains("%"))
                                Campos.Append(key).Append(" like '%").Append(coleccionDatos[key].ToString()).Append("' AND ");
                            else
                                CamposParam.Append(key).Append(" = @").Append(key).Append(" AND ");
                            //CamposParam.Append(key).Append(" = ?").Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatos[key]));
                        }
                    }
                }
                SQLString.Append("UPDATE ").Append((datos.GetType()).Name).Append(" SET ");
                SQLString.Append(Campos.ToString().Substring(0, Campos.ToString().Length - 2));
                if (CamposParam.Length > 0)
                {
                    SQLString.Append(" WHERE ");
                    SQLString.Append(CamposParam.ToString().Substring(0, CamposParam.ToString().Length - 4));
                }
                return dbConn.ExeQuery(SQLString.ToString(), Parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///
        /// Función que realizará una inserción a base de datos, devolviendo si lo ejecuto o no
        /// con los criterios establecidos como valores en el DTO pasado como parámetros.
        /// <changelog>
        /// Rodrigo del Angel    14/08/2011    Creación
        /// </changelog>
        /// <summary>
        /// Inserta registros en una tabla
        /// </summary>
        /// <param name="datos">Estructura de datos con los campos</param>
        /// <param name="parametros">Estructura de datos con los valores</param>
        /// <returns>Verdadero/Falso</returns>
        public Boolean insData(object datos, object parametros)
        {
            try
            {
                StringBuilder SQLString = new StringBuilder();
                StringBuilder Campos = new StringBuilder();
                StringBuilder CamposParam = new StringBuilder();
                ArrayList Parametros = new ArrayList();
                if (datos == null && parametros == null)
                    throw new NullReferenceException("daoGenerico.insert(datos,parametros)");
                Dictionary<string, object> coleccionDatos = new Dictionary<string, object>();
                coleccionDatos = ObtenerElementos(parametros, MemberTypes.Property);
                if (coleccionDatos != null)
                {
                    foreach (string key in coleccionDatos.Keys)
                    {
                        if (coleccionDatos[key] != null)
                        {
                            Campos.Append(key).Append(",");
                        }
                    }
                }
                coleccionDatos = ObtenerElementos(parametros, MemberTypes.Property);
                if (coleccionDatos != null)
                {
                    foreach (string key in coleccionDatos.Keys)
                    {
                        if (coleccionDatos[key] != null)
                        {
                            //if (coleccionDatos[key].ToString().Contains("%"))
                            //    Campos.Append(key).Append(" like '%").Append(coleccionDatos[key].ToString()).Append("' AND ");
                            //else
                            CamposParam.Append("@").Append(key).Append(",");
                            //CamposParam.Append("?").Append(",");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatos[key]));
                        }
                    }
                }
                SQLString.Append("INSERT INTO ").Append((parametros.GetType()).Name);
                SQLString.Append(" (").Append(Campos.ToString().Substring(0, Campos.ToString().Length - 1)).Append(")");
                SQLString.Append(" VALUES (").Append(CamposParam.ToString().Substring(0, CamposParam.ToString().Length - 1)).Append(")");
                return dbConn.ExeQuery(SQLString.ToString(), Parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///
        /// Función que realizará una inserción a base de datos, devolviendo si lo ejecuto o no
        /// con los criterios establecidos como valores en el DTO pasado como parámetros.
        /// <changelog>
        /// Rodrigo del Angel    14/08/2011    Creación
        /// </changelog>
        /// <summary>
        /// Inserta registros en una tabla
        /// </summary>
        /// <param name="datos">Estructura de datos con los campos</param>
        /// <param name="parametros">Estructura de datos con los valores</param>
        /// <returns>Verdadero/Falso</returns>
        public Boolean insData(object parametros)
        {
            try
            {
                StringBuilder SQLString = new StringBuilder();
                StringBuilder Campos = new StringBuilder();
                StringBuilder CamposParam = new StringBuilder();
                ArrayList Parametros = new ArrayList();
                if (parametros == null)
                    throw new NullReferenceException("daoGenerico.insert(datos,parametros)");
                Dictionary<string, object> coleccionDatos = new Dictionary<string, object>();
                coleccionDatos = ObtenerElementos(parametros, MemberTypes.Property);
                if (coleccionDatos != null)
                {
                    foreach (string key in coleccionDatos.Keys)
                    {
                        if (coleccionDatos[key] != null)
                        {
                            Campos.Append(key).Append(",");
                        }
                    }
                }
                coleccionDatos = ObtenerElementos(parametros, MemberTypes.Property);
                if (coleccionDatos != null)
                {
                    foreach (string key in coleccionDatos.Keys)
                    {
                        if (coleccionDatos[key] != null)
                        {
                            CamposParam.Append("@").Append(key).Append(",");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatos[key]));
                        }
                    }
                }
                SQLString.Append("INSERT INTO ").Append((parametros.GetType()).Name);
                SQLString.Append(" (").Append(Campos.ToString().Substring(0, Campos.ToString().Length - 1)).Append(")");
                SQLString.Append(" VALUES (").Append(CamposParam.ToString().Substring(0, CamposParam.ToString().Length - 1)).Append(")");
                return dbConn.ExeQuery(SQLString.ToString(), Parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///
        /// Función que realizará una eliminación a base de datos, devolviendo si lo ejecuto o no
        /// y que se cumplan con los criterios establecidos en el DTO pasado como parámetros.
        /// <changelog>
        /// Rodrigo del Angel    07/06/2013    Creación
        /// </changelog>
        /// <summary>
        /// Para eliminar registro de una tabla de la base de datos
        /// </summary>
        /// <param name="parametros">Estructura de Datos con los datos</param>
        /// <returns>Verdadero/Falso</returns>
        public Boolean delData(object parametros)
        {
            try
            {
                StringBuilder SQLString = new StringBuilder();
                StringBuilder Campos = new StringBuilder();
                StringBuilder CamposParam = new StringBuilder();
                ArrayList Parametros = new ArrayList();
                if (parametros == null)
                    throw new NullReferenceException("daoGenerico.delete(parametros)");
                Dictionary<string, object> coleccionDatos = new Dictionary<string, object>();
                coleccionDatos = ObtenerElementos(parametros, MemberTypes.Property);
                if (coleccionDatos != null)
                {
                    foreach (string key in coleccionDatos.Keys)
                    {
                        if (coleccionDatos[key] != null)
                        {
                            if (coleccionDatos[key].ToString().Contains("%"))
                                Campos.Append(key).Append(" like '%").Append(coleccionDatos[key].ToString()).Append("' AND ");
                            else
                                CamposParam.Append(key).Append(" = @").Append(key).Append(" AND ");
                            //CamposParam.Append(key).Append(" = ?").Append(" AND ");
                            Parametros.Add(new SqlParameter("@" + key, coleccionDatos[key]));
                        }
                    }
                }
                SQLString.Append("DELETE FROM ").Append((parametros.GetType()).Name);
                if (CamposParam.Length > 0)
                {
                    SQLString.Append(" WHERE ");
                    SQLString.Append(CamposParam.ToString().Substring(0, CamposParam.ToString().Length - 4));
                }
                return dbConn.ExeQuery(SQLString.ToString(), Parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///
        /// Función encargada de devolver un tipo de elemento en concreto de un objeto en un conjunto
        /// de pares clave - valor.
        /// <changelog>
        /// Rodrigo del Angel    14/08/2011    Creación
        /// </changelog>
        /// <summary>
        /// Obtener la estructura de datos de un objeto
        /// </summary>
        /// <param name="objeto">Estructura de Datos</param>
        /// <param name="TipoElemento">Propiedades de la Estructura de Datos</param>
        /// <returns>Diccionaro de la Estructura de Datos</returns>
        private Dictionary<string, object> ObtenerElementos(object objeto, MemberTypes TipoElemento)
        {
            try
            {
                // Declaramos un Diccionario que contendra el nombre de los elementos del objeto y el
                //contenido de cada elemento.
                Dictionary<string, object> Elementos = new Dictionary<string, object>();

                // Se recorren los miembros del objeto
                foreach (MemberInfo infoMiembro in objeto.GetType().GetMembers())
                {
                    // Si el tipo del objeto es del tipo que buscamos, se añade al diccionario
                    if (infoMiembro.MemberType == TipoElemento)
                    {

                        if ((PropertyInfo)infoMiembro != null)
                            Elementos.Add(((PropertyInfo)infoMiembro).Name, ((PropertyInfo)infoMiembro).GetValue(objeto, null));
                    }
                }
                return Elementos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
