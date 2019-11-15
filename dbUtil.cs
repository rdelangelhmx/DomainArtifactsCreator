using System;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Globalization;
using System.IO;


namespace dbUtil
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
    /// **************************************************************************
    /// </summary>

    #region SQLServer
    public class dbData
    {
        public static string sConn { get; set; }

        // Obtener la fecha del servidor
        public static DateTime GetFecha()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            OdbcConnection SqlConn = new OdbcConnection(sConn);
            OdbcCommand oCommand = new OdbcCommand("select getdate()", SqlConn);
            try
            {
                SqlConn.Open();
                DateTime sValor = Convert.ToDateTime(oCommand.ExecuteScalar());
                SqlConn.Close();
                return sValor;
            }
            catch
            {
                return DateTime.Now;
            }
        }
        // Obtener los datos de Base de Datos
        public static DataTable GetData(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DataTable oData = new DataTable();
            try
            {
                OdbcConnection SqlConn = new OdbcConnection(sConn);
                OdbcCommand oCommand = new OdbcCommand(sQuery, SqlConn);
                SqlConn.Open();
                OdbcDataReader oReader = oCommand.ExecuteReader();
                oData.Load(oReader);
                return oData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(sQuery);
                Console.WriteLine(ex.ToString());
                return null;
            }

        }
        // Obtener datos de la Base de Datos en un DataSet
        public static DataSet GetDataSet(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DataSet oDS = new DataSet();
            OdbcConnection SqlConn = new OdbcConnection(sConn);
            OdbcCommand oCommand = new OdbcCommand(sQuery, SqlConn);
            try
            {
                OdbcDataAdapter oReader = new OdbcDataAdapter(oCommand);
                SqlConn.Open();
                oReader.Fill(oDS);
                return oDS;
            }
            catch (Exception ex)
            {
                Console.WriteLine(sQuery);
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        // Obtener campo de Tabla de Base de Datos
        public static string GetCampo(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            OdbcConnection SqlConn = new OdbcConnection(sConn);
            OdbcCommand oCommand = new OdbcCommand(sQuery, SqlConn);
            try
            {
                SqlConn.Open();
                string sValor = oCommand.ExecuteScalar().ToString();
                SqlConn.Close();
                return sValor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(sQuery);
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        // Ejecutar Query en la Base de Datos
        public static Boolean ExeQuery(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            OdbcConnection SqlConn = new OdbcConnection(sConn);
            OdbcCommand oCommand = new OdbcCommand(sQuery, SqlConn);
            try
            {
                SqlConn.Open();
                oCommand.ExecuteNonQuery();
                SqlConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(sQuery);
                Console.WriteLine(ex.ToString());
                return false;
            }

        }
        // Pone en un dataset una hoja de Excel
        public static DataSet GetExcel(string sFileImport, string sSheet)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            string sConnE = "Driver={Microsoft Excel Driver (*.xls)};DriverId=790;Dbq=" + sFileImport + ";";

            DataSet oDS = new DataSet();
            OdbcConnection SqlConn = new OdbcConnection(sConnE);
            OdbcCommand oCommand = new OdbcCommand("Select * from [" + sSheet + "$]", SqlConn);
            try
            {
                OdbcDataAdapter oReader = new OdbcDataAdapter(oCommand);
                SqlConn.Open();
                oReader.Fill(oDS);
                SqlConn.Close();
                return oDS;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Select * from [" + sSheet + "$]");
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static string GetId(string sTabla, string sCampo)
        {
            int nId = 0;
            nId = Convert.ToInt32(GetCampo("select ifnull(" + sCampo + ",0) from " + sTabla + " order by " + sCampo + " desc limit 1")) + 1;
            return nId.ToString();
        }
        public static string GetParametro(string Parametro)
        {
            return GetCampo("select valor from Parametros where Parametro='" + Parametro + "'");
        }
        public static Image ConvertByteToImage(byte[] myByteArray)
        {
            System.Drawing.Image newImage;
            MemoryStream ms = new MemoryStream();
            ms.Write(myByteArray, 0, myByteArray.Length);
            newImage = System.Drawing.Image.FromStream(ms, true);
            return newImage;
        }
    }
    #endregion SQLServer
}