using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing;
using System.Data.OracleClient;

namespace dbUtilOracle
{
    /// <summary>
    /// **************************************************************************
    /// *** Nombre: dbUtil
    /// *** Creador: Rodrigo del Angel Hurtado
    /// *** Fecha Creación: 29/Sep/2009
    /// **************************************************************************
    /// *** Control de Versiones
    /// **************************************************************************
    /// *** ver. 1.0.0  29/Sep/2009     Creación
    /// *** ver. 1.1.0  02/Dic/2014     Modificación para Agregar Oracle
    /// **************************************************************************
    /// </summary>

    #region OracleDataBase
    public class dbData
    {
        public static string sConn = "";

        // Obtener los datos de Base de Datos
        public static DataTable GetData(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DataTable oData = new DataTable();
            OracleConnection SqlConn = new OracleConnection(sConn);
            OracleCommand oCommand = new OracleCommand(sQuery, SqlConn);
            try
            {
                SqlConn.Open();
                OracleDataReader oReader = oCommand.ExecuteReader();
                oData.Load(oReader);
                return oData;
            }
            catch (Exception ex)
            {
                //Globales.WriteLog(sQuery);
                //Globales.WriteLog(ex.ToString());
                return null;
            }
            finally
            {
                SqlConn.Dispose();
                oCommand.Dispose();
            }
        }
        // Obtener datos de la Base de Datos en un DataSet
        public static DataSet GetDataSet(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DataSet oDS = new DataSet();
            OracleConnection SqlConn = new OracleConnection(sConn);
            OracleCommand oCommand = new OracleCommand(sQuery, SqlConn);
            try
            {
                OracleDataAdapter oReader = new OracleDataAdapter(oCommand);
                SqlConn.Open();
                oReader.Fill(oDS);
                return oDS;
            }
            catch (Exception ex)
            {
                //Globales.WriteLog(sQuery);
                //Globales.WriteLog(ex.ToString());
                return null;
            }
            finally
            {
                SqlConn.Dispose();
                oCommand.Dispose();
            }
        }

        // Obtener campo de Tabla de Base de Datos
        public static string GetCampo(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DataTable oData = new DataTable();
            OracleConnection SqlConn = new OracleConnection(sConn);
            OracleCommand oCommand = new OracleCommand(sQuery, SqlConn);
            try
            {
                SqlConn.Open();
                string sValor = oCommand.ExecuteScalar().ToString();
                SqlConn.Close();
                return sValor;
            }
            catch (Exception ex)
            {
                //Globales.WriteLog(sQuery);
                //Globales.WriteLog(ex.ToString());
                return null;
            }
            finally
            {
                SqlConn.Dispose();
                oCommand.Dispose();
            }
        }
        // Ejecutar Query en la Base de Datos
        public static Boolean ExeQuery(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            OracleConnection SqlConn = new OracleConnection(sConn);
            OracleCommand oCommand = new OracleCommand(sQuery, SqlConn);
            try
            {
                SqlConn.Open();
                oCommand.ExecuteNonQuery();
                SqlConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                //Globales.WriteLog(sQuery);
                //Globales.WriteLog(ex.ToString());
                return false;
            }
            finally
            {
                SqlConn.Dispose();
                oCommand.Dispose();
            }

        }

        public static Image ConvertByteToImage(byte[] myByteArray)
        {
            try
            {
                System.Drawing.Image newImage;
                MemoryStream ms = new MemoryStream();
                ms.Write(myByteArray, 0, myByteArray.Length);
                newImage = System.Drawing.Image.FromStream(ms, true);
                return newImage;
            }
            catch (Exception ex)
            {
                //Globales.WriteLog(ex.ToString());
                return null;
            }
        }
    }
    #endregion
}