using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Utilerias
{
    #region Imagen
    public class dbPicture
    {
        public static string sConn = ConfigurationSettings.AppSettings["dbConexion"].ToString();

        public static Boolean SetPicture(string sTabla, string sCampo, string sFiltro, byte[] oPict)
        {
            int nRegs = 0;
            string sQuery = "";
            OdbcParameter oParameter = new OdbcParameter();
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            OdbcConnection SqlConn = new OdbcConnection(sConn);
            try
            {
                sQuery = "UPDATE " + sTabla + " set " + sCampo + " = @Imagen where " + sFiltro;
                OdbcCommand oCommand = new OdbcCommand(sQuery, SqlConn);
                //Supply the parameters
                oParameter = new OdbcParameter();
                oParameter.Direction = ParameterDirection.Input;
                oParameter.OdbcType = OdbcType.Image;
                oParameter.ParameterName = "@Imagen";
                oParameter.Value = oPict;
                oCommand.Parameters.Add(oParameter);
                //Execute the query command 
                SqlConn.Open();
                nRegs = oCommand.ExecuteNonQuery();
                if (nRegs > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                General.WriteLog(sQuery);
                General.WriteLog(ex.ToString());
                return false;
            }


        }
        public static byte[] GetPicture(string sQuery)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DataTable oData = new DataTable();
            OdbcConnection SqlConn = new OdbcConnection(sConn);
            OdbcCommand oCommand = new OdbcCommand(sQuery, SqlConn);
            try
            {
                SqlConn.Open();
                return (byte[])oCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                General.WriteLog(sQuery);
                General.WriteLog(ex.ToString());
                return null;
            }
        }
    }
    #endregion Picture
    #region Encript
    public class Encript
    {
        public static string Patron_busqueda = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz1234567890";
        public static string Patron_encripta = "0987654321zyxwvutsrqpoñnmlkjihgfedcbaZYXWVUTSRQPOÑNMLKJIHGFEDCBA";

        public static string EncryptStr(string cadena)
        {
            int idx = 0;
            string result = "";
            for (idx = 0; idx < cadena.Length; idx++)
            {
                result += EncriptarCaracter(cadena.Substring(idx, 1), cadena.Length, idx);
            }
            return result;
        }
        private static string EncriptarCaracter(string caracter, int variable, int a_indice)
        {
            int indice = 0;
            if (Patron_busqueda.IndexOf(caracter) != -1)
            {
                indice = (Patron_busqueda.IndexOf(caracter) + variable + a_indice) % Patron_busqueda.Length;
                return Patron_encripta.Substring(indice, 1);
            }
            return caracter;
        }
        public static string DecryptStr(string cadena)
        {
            int idx = 0;
            string result = "";
            for (idx = 0; idx < cadena.Length; idx++)
            {
                result += DesEncriptarCaracter(cadena.Substring(idx, 1), cadena.Length, idx);
            }
            return result;
        }
        public static string DesEncriptarCaracter(string caracter, int variable, int a_indice)
        {
            int indice = 0;
            if (Patron_encripta.IndexOf(caracter) != -1)
            {
                if ((Patron_encripta.IndexOf(caracter) - variable - a_indice) > 0)
                    indice = (Patron_encripta.IndexOf(caracter) - variable - a_indice) % Patron_encripta.Length;
                else
                    indice = (Patron_busqueda.Length) + ((Patron_encripta.IndexOf(caracter) - variable - a_indice) % Patron_encripta.Length);
                indice = indice % Patron_encripta.Length;
                return Patron_busqueda.Substring(indice, 1);
            }
            else
                return caracter;
        }

        public static int Asc(string s)
        {
            return Encoding.ASCII.GetBytes(s)[0];
        }
        public static char Chr(int c)
        {
            return Convert.ToChar(c);
        }
        public static string Mid(string s, int i, int f)
        {
            return s.Substring(i, f);
        }
        public static string Mid(string s, int i)
        {
            return s.Substring(i);
        }
        public static int Len(string s)
        {
            return s.Length;
        }
        public static bool validarEmail(string email)
        {
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                { return true; }
                else
                { return false; }
            }
            else
            { return false; }
        }
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static string GetKey()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
    }
    #endregion 
    #region General
    public class General
    {
        public static string sDirApp = Directory.GetCurrentDirectory();
        public struct MailServer
        {
            public string Server;
            public string Usuario;
            public string Password;
            public int Puerto;
        }
        //public static int nLog = Entero(string.IsNullOrEmpty(ConfigurationSettings.AppSettings["log"].ToString()) ? "0" : ConfigurationSettings.AppSettings["log"].ToString());

        public static string setFechaNull(string sFecha)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            if (sFecha == "")
                return "null";
            else
                return "'" + SetFecha(sFecha, 0) + "'";
        }

        public static string sIf(Boolean Condicion, string sYes, string sNo)
        {
            if (Condicion)
                return sYes;
            else
                return sNo;
        }
        public static int DateDiff(string howtocompare, DateTime startDate, DateTime endDate)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            int diff = 0;
            try
            {
                System.TimeSpan TS = new System.TimeSpan(startDate.Ticks - endDate.Ticks);
                switch (howtocompare.ToLower())
                {
                    case "h":
                        diff = int.Parse(Math.Round(Convert.ToDouble(TS.TotalHours),0).ToString());
                        break;
                    case "m":
                        diff = int.Parse(Math.Round(Convert.ToDouble(TS.TotalMinutes),0).ToString());
                        break;
                    case "s":
                        diff = int.Parse(Math.Round(Convert.ToDouble(TS.TotalSeconds),0).ToString());
                        break;
                    case "t":
                        diff = int.Parse(Math.Round(Convert.ToDouble(TS.Ticks),0).ToString());
                        break;
                    case "mm":
                        diff = int.Parse(Math.Round(Convert.ToDouble(TS.TotalMilliseconds),0).ToString());
                        break;
                    case "yyyy":
                        diff = int.Parse(Math.Round(Convert.ToDouble(TS.TotalDays / 365),0).ToString());
                        break;
                    case "q":
                        diff = int.Parse(Math.Round(Convert.ToDouble((TS.TotalDays / 365) / 4),0).ToString());
                        break;
                    default:
                        //d 
                        diff = int.Parse(Math.Round(Convert.ToDouble(TS.TotalDays), 0).ToString());
                        break;
                }
            }
            catch (Exception)
            {
                diff = -1;
            }
            return diff;
        }

        public static Boolean SendMail(General.MailServer oMailServer, string sFrom, string sSubject, string sBody, params string[] sTo)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            try
            {
                MailAddress oFrom = new MailAddress(sFrom);
                SmtpClient sServer = new SmtpClient();
                sServer.Host = oMailServer.Server;
                sServer.Port = oMailServer.Puerto == 0 ? 25 : oMailServer.Puerto;
                sServer.Credentials = new NetworkCredential(oMailServer.Usuario, oMailServer.Password);
                MailMessage msgMail = new MailMessage();
                for (int i = 0; i < sTo.Length; i++)
                {
                    msgMail.To.Add(sTo[i]);
                }
                msgMail.From = oFrom;
                msgMail.Subject = sSubject;
                msgMail.IsBodyHtml = true;
                msgMail.Body = sBody;
                msgMail.Priority = MailPriority.High;
                sServer.Send(msgMail);
                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }

        }

        public static string CuerpoCorreo(Int32 nCorreo)
        {
            return ConfigurationSettings.AppSettings["CuerpoCorreo"].ToString();
        }

        public static string cMonth(int index)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            string[] cMes = new string[13];
            cMes[1] = "Enero";
            cMes[2] = "Febrero";
            cMes[3] = "Marzo";
            cMes[4] = "Abril";
            cMes[5] = "Mayo";
            cMes[6] = "Junio";
            cMes[7] = "Julio";
            cMes[8] = "Agosto";
            cMes[9] = "Septiembre";
            cMes[10] = "Octubre";
            cMes[11] = "Noviembre";
            cMes[12] = "Diciembre";
            return cMes[index];
        }
        public static string cMonth(int index,int nLong)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            return cMonth(index).Substring(0, nLong);
        }

        public static string SetFecha(String sFecha, int nFormato)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DateTime dFecha;
            if (sFecha == "")
                return null;
            dFecha = DateTime.Parse(sFecha);
            sFecha = "";
            switch (nFormato)
            {
                case 1: // dd de mmmm de yyyy
                    sFecha = Ceros(dFecha.Day, 2) + " de " + cMonth(dFecha.Month) + " de " + dFecha.Year.ToString();
                    break;
                case 2: // dd/mmm/yyyy
                    sFecha = Ceros(dFecha.Day, 2) + "/" + cMonth(dFecha.Month).Substring(0, 3) + "/" + dFecha.Year.ToString();
                    break;
                case 3: // dd/mm/yyyy
                    sFecha = Ceros(dFecha.Day, 2) + "/" + Ceros(dFecha.Month, 2) + "/" + dFecha.Year.ToString();
                    break;
                case 4: // yyyy/mm/dd
                    sFecha = dFecha.Year.ToString() + "/" + Ceros(dFecha.Month, 2) + "/" + Ceros(dFecha.Day, 2);
                    break;
                case 5: // yyyy,mm,dd
                    sFecha = dFecha.Year.ToString() + "," + Ceros(dFecha.Month, 2) + "," + Ceros(dFecha.Day, 2);
                    break;
                default: // yyyy-mm-dd
                    sFecha = dFecha.Year.ToString() + "-" + Ceros(dFecha.Month, 2) + "-" + Ceros(dFecha.Day, 2);
                    break;
            }
            return sFecha;
        }

        public static string Ceros(int nNumero, int nPos)
        {
            string sCeros = "";
            for (int i = 1; i <= nPos; i++)
            {
                sCeros += "0";
            }
            sCeros += nNumero.ToString();
            return Right(sCeros, nPos);
        }

        public static string Left(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
        }

        public static string Right(string param, int length)
        {
            string result = param.Substring(param.Length - length, length);
            return result;
        }
        public static string GetFileName(string sFileName)
        {
            sFileName = sFileName.Replace(@"\", "/");
            int nIndex = sFileName.LastIndexOf("/");
            return sFileName.Substring(nIndex + 1, sFileName.Length - nIndex - 1);
        }
        public static string Replicate(string sString, int nVeces)
        {
            string sReplicate = "";
            for (int i = 1; i <= nVeces; i++)
            {
                sReplicate += sString;
            }
            return sReplicate;
        }
        public static int Residuo(int Dividendo, int Divisor)
        {
            int Residuo = 0;
            while (Dividendo >= Divisor)
            {
                Dividendo -= Divisor;
                Residuo = Dividendo;
            }
            return Residuo;
        }
        public static int Cociente(int Dividendo, int Divisor)
        {
            int Cociente = 0;
            while (Dividendo >= Divisor)
            {
                Dividendo -= Divisor;
                Cociente++;
            }
            return Cociente;
        }
        public static Boolean WriteLog(string sLog)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //string oDir = string.IsNullOrEmpty(sDirApp) ? ConfigurationSettings.AppSettings["DirLog"].ToString() : sDirApp + @"\log\";
            string oDir = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["DirLog"].ToString();
            try
            {
                StreamWriter sFileName = File.AppendText(oDir + DateTime.Now.ToString("yyyyMMdd") + ".log");
                sFileName.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                sFileName.WriteLine(sLog);
                sFileName.WriteLine(Replicate("-", 100));
                sFileName.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static int Entero(string Valor)
        {
            return int.Parse(string.IsNullOrEmpty(Valor) ? "0" : Valor);
        }

    }
    #endregion
}
