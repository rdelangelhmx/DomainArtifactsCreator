
alter procedure [dbo].[getDTOCompleto](@Table varchar(max)) as

declare @Campos VARCHAR(MAX)
declare @Campos1 VARCHAR(MAX)
declare @Query varchar(max)        
SET @Query = ''
SET @Campos = ''
SET @Campos1 = ''

Select @Campos = @Campos + '
' +
case 
when systypes.name = 'int' 
then 
'		public int?  ' + syscolumns.name + ' {get;set;}'
when systypes.name = 'bigint' 
then 
'		public int? ' + syscolumns.name + '  {get;set;}'
when systypes.name = 'decimal' 
then 
'		public decimal? ' + syscolumns.name + '  {get;set;}'
when systypes.name = 'date' 
then 
'		public DateTime? ' + syscolumns.name + '  {get;set;}'
when systypes.name = 'datetime' 
then 
'		public DateTime? ' + syscolumns.name + '  {get;set;}'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then 
'		public string ' + syscolumns.name + '  {get;set;}'
when (systypes.name = 'uniqueidentifier')
then 
'		public string ' + syscolumns.name + '  {get;set;}'
else '' end
FROM dbo.syscolumns 
 INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
 INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE (dbo.sysobjects.name = @Table)  and systypes.name not in ('sysname')
ORDER BY  dbo.sysobjects.name, dbo.syscolumns.colorder

Select @Campos1 = @Campos1 + 
case 
when systypes.name = 'int' 
then '					if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
							' + @Table + 'Info.'+ dbo.syscolumns.name + ' = int.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'bigint' 
then '					if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
							' + @Table + 'Info.'+ dbo.syscolumns.name + ' = int.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'decimal' 
then '					if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
							' + @Table + 'Info.'+ dbo.syscolumns.name + ' = decimal.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'date' 
then '					if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
							' + @Table + 'Info.'+ dbo.syscolumns.name + ' = DateTime.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'datetime' 
then '					if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
							' + @Table + 'Info.'+ dbo.syscolumns.name + ' = DateTime.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then '						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = dtRow["' + dbo.syscolumns.name + '"].ToString();
'	
when (systypes.name = 'uniqueidentifier')
then '						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = dtRow["' + dbo.syscolumns.name + '"].ToString();
'
else '' end
FROM         
 dbo.syscolumns INNER JOIN
 dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id INNER JOIN
 dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE     
 (dbo.sysobjects.name = @Table) and systypes.name <> 'sysname'
ORDER BY 
 dbo.sysobjects.name, 
 dbo.syscolumns.colorder

set @Query = '/*
 *  ' + @Table + '.cs
 *  Objeto de Transferencia de Datos asociada a una Entidad
 *
 *  © 2011-2013 Rodrigo del Angel <rdelangelhmx@gmail.com>
 *  Some Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Utilerias;

namespace DTO
{
    /// <summary>	
    /// Clase que representa a la entidad ' + @Table + ', almacenada en una fuente de datos
    /// </summary>
    /// <changelog>
    /// Rodrigo del Angel    11 Nov 2011 14:29:14    Creación
    /// Rodrigo del Angel    31 Jul 2012 21:50:18    Modificación
    /// Rodrigo del Angel    ' + CONVERT(varchar(20),getdate(),113) + '    Modificación
    /// </changelog>
    public class ' + @Table + '
    {
		#region DataMember 
		' + @Campos + '
		#endregion

		#region Metodos
		public int RegistrosTotales(' + @Table + ' ' + @Table + 'Params)
		{
			return new ' + @Table + '().getRegistros(' + @Table + 'Params);
		}		

        public int RegistrosTotalesRange(' + @Table + ' ' + @Table + 'Ini, ' + @Table + ' ' + @Table + 'Fin)
        {
            return new ' + @Table + '().getRegistros(' + @Table + 'Ini, ' + @Table + 'Fin);
        }
		
		public List<' + @Table + '> GetRegistrosPag(' + @Table + ' ' + @Table + 'Params, int Registros, int Pagina)
		{
			List<' + @Table + '> ' + @Table + 'Data = new List<' + @Table + '>();
			try
			{
				DataTable dt = new ' + @Table + '().getDatos(' + @Table + 'Params, Registros, Pagina);            
				foreach(DataRow dtRow in dt.Rows)
				{
					' + @Table + ' ' + @Table + 'Info = new ' + @Table + '();
' + @Campos1 + '
					' + @Table + 'Data.Add(' + @Table + 'Info);
	            }
            }
            catch
			{
				return new List<' + @Table + '>();
			}
			return ' + @Table + 'Data;
		}

		public List<' + @Table + '> GetRegistrosRange(' + @Table + ' ' + @Table + 'Ini, ' + @Table + ' ' + @Table + 'Fin)
		{
			List<' + @Table + '> ' + @Table + 'Data = new List<' + @Table + '>();
			try
			{
				DataTable dt = new ' + @Table + '().getDatos(' + @Table + 'Ini,' + @Table + 'Fin);            
				foreach(DataRow dtRow in dt.Rows)
				{
					' + @Table + ' ' + @Table + 'Info = new ' + @Table + '();
' + @Campos1 + '
					' + @Table + 'Data.Add(' + @Table + 'Info);
	            }
            }
            catch
			{
				return new List<' + @Table + '>();
			}
			return ' + @Table + 'Data;
		}

		public List<' + @Table + '> GetRegistrosPagRange(' + @Table + ' ' + @Table + 'Ini, ' + @Table + ' ' + @Table + 'Fin, int Registros, int Pagina)
		{
			List<' + @Table + '> ' + @Table + 'Data = new List<' + @Table + '>();
			try
			{
				DataTable dt = new ' + @Table + '().getDatos(' + @Table + 'Ini,' + @Table + 'Fin, Registros, Pagina);            
				foreach(DataRow dtRow in dt.Rows)
				{
					' + @Table + ' ' + @Table + 'Info = new ' + @Table + '();
' + @Campos1 + '
					' + @Table + 'Data.Add(' + @Table + 'Info);
	            }
            }
            catch
			{
				return new List<' + @Table + '>();
			}
			return ' + @Table + 'Data;
		}

		public List<' + @Table + '> GetRegistros(' + @Table + ' ' + @Table + 'Params)
		{
			List<' + @Table + '> ' + @Table + 'Data = new List<' + @Table + '>();
			try
			{
				DataTable dt = new ' + @Table + '().getDatos(' + @Table + 'Params);            
				foreach(DataRow dtRow in dt.Rows)
				{
					' + @Table + ' ' + @Table + 'Info = new ' + @Table + '();
' + @Campos1 + '
					' + @Table + 'Data.Add(' + @Table + 'Info);
	            }
            }
            catch
			{
				return new List<' + @Table + '>();
			}
			return ' + @Table + 'Data;
		}

		public ' + @Table + ' GetRegistro(' + @Table + ' ' + @Table + 'Params)
		{
			' + @Table + ' ' + @Table + 'Info = new ' + @Table + '();
			try
			{
				DataRow dtRow = new ' + @Table + '().getDato(' + @Table + 'Params);            
' + @Campos1 + '
            }
            catch
			{
				return new ' + @Table + '();
			}
			return ' + @Table + 'Info;
		}

		public Boolean Insert(' + @Table + ' ' + @Table + 'Info, ' + @Table + ' ' + @Table + 'Param)
		{
			return new ' + @Table + '().setRegistro(' + @Table + 'Info, ' + @Table + 'Param, 1);
		}

		public Boolean Update(' + @Table + ' ' + @Table + 'Info, ' + @Table + ' ' + @Table + 'Param)
		{
			return new ' + @Table + '().setRegistro(' + @Table + 'Info, ' + @Table + 'Param, 2);
		}

		public Boolean Delete(' + @Table + ' ' + @Table + 'Info, ' + @Table + ' ' + @Table + 'Param) // Borrado Logico
		{
			return new ' + @Table + '().setRegistro(' + @Table + 'Info, ' + @Table + 'Param, 3);
		}

		public Boolean Borra(' + @Table + ' ' + @Table + 'Param) // Borrado Fisico
		{
			return new ' + @Table + '().setRegistro(' + @Table + 'Param, ' + @Table + 'Param, 4);
		}
		#endregion

        #region DataObjects
        public int getRegistros(object oData)
        {
			int Registros = 0;
			try
			{
				Registros = new DAO.daoGenerico().regTot(oData);
			}
			catch(Exception ex)
			{
				General.WriteLog("ERROR -- Class: getRegistros ' + @Table + '");
				General.WriteLog(ex.ToString());
				return 0;
			}
			return Registros;
        }

        public int getRegistros(object ' + @Table + 'Ini, object ' + @Table + 'Fin)
        {
            int Registros = 0;
            try
            {
                Registros = new DAO.daoGenerico().regTot(' + @Table + 'Ini,' + @Table + 'Fin);
            }
            catch (Exception ex)
            {
                General.WriteLog("ERROR -- Class: getRegistros ' + @Table + '");
                General.WriteLog(ex.ToString());
                return 0;
            }
            return Registros;
        }

        public DataTable getDatos(object ' + @Table + 'Params, int Registros, int Pagina)
        {
			DataTable Datos = new DataTable();
			try
			{
				Datos = new DAO.daoGenerico().getData(' + @Table + 'Params, Pagina, Registros);
			}
			catch(Exception ex)
			{
				General.WriteLog("ERROR -- Class: getRegistros ' + @Table + '");
				General.WriteLog(ex.ToString());
				return null;
			}    
            return Datos;        
        }

        public DataTable getDatos(object ' + @Table + 'Ini,object ' + @Table + 'Fin)
        {
            DataTable Datos = new DataTable();
            try
            {
                Datos = new DAO.daoGenerico().getData(' + @Table + 'Ini,' + @Table + 'Fin);
            }
            catch (Exception ex)
            {
                General.WriteLog("ERROR -- Class: getRegistros ' + @Table + '");
                General.WriteLog(ex.ToString());
                return null;
            }
            return Datos;
        }

        public DataTable getDatos(object ' + @Table + 'Ini,object ' + @Table + 'Fin, int Registros, int Pagina)
        {
            DataTable Datos = new DataTable();
            try
            {
                Datos = new DAO.daoGenerico().getData(' + @Table + 'Ini,' + @Table + 'Fin, Pagina, Registros);
            }
            catch (Exception ex)
            {
                General.WriteLog("ERROR -- Class: getRegistros ' + @Table + '");
                General.WriteLog(ex.ToString());
                return null;
            }
            return Datos;
        }

        public DataTable getDatos(object ' + @Table + 'Params)
        {
			DataTable Datos = new DataTable();
			try
			{
				Datos = new DAO.daoGenerico().getData(' + @Table + 'Params);
			}
			catch(Exception ex)
			{
				General.WriteLog("ERROR -- Class: getRegistros ' + @Table + '");
				General.WriteLog(ex.ToString());
				return null;
			}    
            return Datos;        
        }

        public DataRow getDato(object ' + @Table + 'Params)
        {
			DataTable Datos = new DataTable();
			DataRow Registro;
			try
			{
				Datos = new DAO.daoGenerico().getData(' + @Table + 'Params);
				Registro = Datos.Rows[0];
			}
			catch(Exception ex)
			{
				General.WriteLog("ERROR -- Class: getRegistros ' + @Table + '");
				General.WriteLog(ex.ToString());
				return null;
			}    
            return Registro;        
        }

        public Boolean setRegistro(object ' + @Table + 'Info, object ' + @Table + 'Param, int Tipo)
        {
			try
			{
				switch (Tipo)
				{
					case 1: // Insert
						return new DAO.daoGenerico().insData(' + @Table + 'Info, ' + @Table + 'Param);
					case 2: // Update
						return new DAO.daoGenerico().setData(' + @Table + 'Info, ' + @Table + 'Param);
					case 3: // Delete Logico
						return new DAO.daoGenerico().setData(' + @Table + 'Info, ' + @Table + 'Param);
					case 4: // Delete Fisico
						return new DAO.daoGenerico().delData(' + @Table + 'Param);
					default:
						return false;
				}
			}
			catch(Exception ex)
			{
				General.WriteLog("ERROR -- Class: ' + @Table + '");
				General.WriteLog(ex.ToString());
				return false;
			}
        }        
		#endregion
    }
}'
--Select @Query
print @Query
