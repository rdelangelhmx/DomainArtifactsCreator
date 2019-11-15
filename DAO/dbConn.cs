using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Utilerias;

namespace DAO
{
    public class dbConn
    {
        // Importante el el Web.config
        public static string sConn = ConfigurationSettings.AppSettings["dbConnStr"];

        // Obtener datos de la Base de Datos en un DataTable
        public static DataTable GetDataTable(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            try
            {
                DataTable oData = GetDataSet(sQuery).Tables[0];
                return oData;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message,ex.InnerException);
            }
        }
        public static DataTable GetDataTable(string sQuery, ArrayList Parametros)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            try
            {
                DataTable oData = GetDataSet(sQuery, Parametros).Tables[0];
                return oData;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }
        public static DataTable GetDataTable(string sQuery, ArrayList Parametros, int StartRecord, int RecordsMax)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            try
            {
                DataTable oData = GetDataSet(sQuery, Parametros, StartRecord, RecordsMax).Tables[0];
                return oData;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }
        public static DataTable GetDataTable(string sQuery, int StartRecord, int RecordsMax)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            try
            {
                DataTable oData = GetDataSet(sQuery, StartRecord, RecordsMax).Tables[0];
                return oData;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }

        // Obtener datos de la Base de Datos en un DataSet
        public static DataSet GetDataSet(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DataSet oDS = new DataSet();
            SqlConnection SqlConn = new SqlConnection(sConn);
            SqlCommand oCommand = new SqlCommand(sQuery, SqlConn);
            try
            {
                oCommand.CommandTimeout = 36000;
                SqlDataAdapter oReader = new SqlDataAdapter(oCommand);
                SqlConn.Open();
                oReader.Fill(oDS);
                SqlConn.Close();
                return oDS;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }
        public static DataSet GetDataSet(string sQuery, ArrayList Parametros)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DataSet oDS = new DataSet();
            SqlConnection SqlConn = new SqlConnection(sConn);
            SqlCommand oCommand = new SqlCommand(sQuery, SqlConn);
            try
            {
                oCommand.CommandTimeout = 36000;
                foreach (SqlParameter p in Parametros)
                    oCommand.Parameters.Add(p);

                SqlDataAdapter oReader = new SqlDataAdapter(oCommand);
                SqlConn.Open();
                oReader.Fill(oDS);
                SqlConn.Close();
                return oDS;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }
        public static DataSet GetDataSet(string sQuery, ArrayList Parametros, int StartRecord, int RecordsMax)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DataSet oDS = new DataSet();
            SqlConnection SqlConn = new SqlConnection(sConn);
            SqlCommand oCommand = new SqlCommand(sQuery, SqlConn);
            try
            {
                oCommand.CommandTimeout = 36000;
                foreach (SqlParameter p in Parametros)
                    oCommand.Parameters.Add(p);

                SqlDataAdapter oReader = new SqlDataAdapter(oCommand);
                SqlConn.Open();
                oReader.Fill(oDS, StartRecord, RecordsMax, "Table");
                SqlConn.Close();
                return oDS;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }
        public static DataSet GetDataSet(string sQuery, int StartRecord, int RecordsMax)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DataSet oDS = new DataSet();
            SqlConnection SqlConn = new SqlConnection(sConn);
            SqlCommand oCommand = new SqlCommand(sQuery, SqlConn);
            try
            {
                oCommand.CommandTimeout = 36000;
                SqlDataAdapter oReader = new SqlDataAdapter(oCommand);
                SqlConn.Open();
                oReader.Fill(oDS, StartRecord, RecordsMax, "Table");
                SqlConn.Close();
                return oDS;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }

        // Ejecutar Query en la Base de Datos
        public static Boolean ExeQuery(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            SqlConnection SqlConn = new SqlConnection(sConn);
            SqlCommand oCommand = new SqlCommand(sQuery, SqlConn);
            try
            {
                oCommand.CommandTimeout = 36000;
                SqlConn.Open();
                oCommand.ExecuteNonQuery();
                SqlConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }
        public static Boolean ExeQuery(string sQuery, ArrayList Parametros)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            SqlConnection SqlConn = new SqlConnection(sConn);
            SqlCommand oCommand = new SqlCommand(sQuery, SqlConn);
            try
            {
                oCommand.CommandTimeout = 36000;
                foreach (SqlParameter p in Parametros)
                    oCommand.Parameters.Add(p);
                SqlConn.Open();
                oCommand.ExecuteNonQuery();
                SqlConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }
        
        // Obtener campo de Tabla de Base de Datos
        public static string GetCampo(string sQuery)
        {
            string sCampo = "";
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            SqlConnection SqlConn = new SqlConnection(sConn);
            SqlCommand oCommand = new SqlCommand(sQuery, SqlConn);
            try
            {
                oCommand.CommandTimeout = 36000;
                SqlConn.Open();
                sCampo = oCommand.ExecuteScalar().ToString();
                SqlConn.Close();
                return sCampo;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }
        public static string GetCampo(string sQuery, ArrayList Parametros)
        {
            string sCampo = "";
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            SqlConnection SqlConn = new SqlConnection(sConn);
            SqlCommand oCommand = new SqlCommand(sQuery, SqlConn);
            try
            {
                oCommand.CommandTimeout = 36000;
                foreach (SqlParameter p in Parametros)
                    oCommand.Parameters.Add(p);
                SqlConn.Open();
                sCampo = oCommand.ExecuteScalar().ToString();
                SqlConn.Close();
                return sCampo;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO " + ex.Message, ex.InnerException);
            }
        }
    }
}
