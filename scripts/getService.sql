/****** Object:  StoredProcedure [dbo].[getService]    Script Date: 14/07/2013 09:28:44 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[getService](@Table varchar(max)) as

declare @Query varchar(max)        

set @Query = N'/*
*  s' + @Table + '.svc.cs
*  Servicios relacionados a un Modelo de Datos
*
*  © 2013 Rodrigo del Angel <rdelangelhmx@gmail.com>
*  Some Rights Reserved.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;

namespace WCF
{
	/// <summary>	
	/// ServiceContract de la entidad ' + @Table + '
	/// </summary>
	/// <changelog>
	/// Rodrigo del Angel    15 Feb 2013 23:42:14    Creación
	/// Rodrigo del Angel    ' + CONVERT(varchar(20),getdate(),113) + '    Modificación
	/// </changelog>

	public class s' + @Table + ' : I' + @Table + '
	{
		public int RegistrosTotales(' + @Table + ' ' + @Table + 'Params)
		{
			return new DTO.' + @Table + '().getRegistros(' + @Table + 'Params);
		}		

        public int RegistrosTotalesRange(' + @Table + ' ' + @Table + 'Ini, ' + @Table + ' ' + @Table + 'Fin)
        {
            return new DTO.' + @Table + '().getRegistros(' + @Table + 'Ini, ' + @Table + 'Fin);
        }
		
		public List<' + @Table + '> GetRegistrosPag(' + @Table + ' ' + @Table + 'Params, int Registros, int Pagina)
		{
			List<' + @Table + '> ' + @Table + 'Data = new List<' + @Table + '>();
			try
			{
				DataTable dt = new DTO.' + @Table + '().getDatos(' + @Table + 'Params, Registros, Pagina);            
				foreach(DataRow dtRow in dt.Rows)
				{
					' + @Table + ' ' + @Table + 'Info = new ' + @Table + '();
'
Select @Query = @Query + 
case 
when systypes.name = 'int' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = int.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'bigint' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = int.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'decimal' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = decimal.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'date' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = DateTime.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'datetime' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = DateTime.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then '					' + @Table + 'Info.'+ dbo.syscolumns.name + ' = dtRow["' + dbo.syscolumns.name + '"].ToString();
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
 
select @Query = @Query + '					' + @Table + 'Data.Add(' + @Table + 'Info);
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
				DataTable dt = new DTO.' + @Table + '().getDatos(' + @Table + 'Ini,' + @Table + 'Fin, Registros, Pagina);            
				foreach(DataRow dtRow in dt.Rows)
				{
					' + @Table + ' ' + @Table + 'Info = new ' + @Table + '();
'
Select @Query = @Query + 
case 
when systypes.name = 'int' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = int.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'bigint' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = int.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'decimal' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = decimal.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'date' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = DateTime.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'datetime' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = DateTime.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then '					' + @Table + 'Info.'+ dbo.syscolumns.name + ' = dtRow["' + dbo.syscolumns.name + '"].ToString();
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
 
select @Query = @Query + '					' + @Table + 'Data.Add(' + @Table + 'Info);
	            }
            }
            catch
			{
				return new List<' + @Table + '>();
			}
			return ' + @Table + 'Data;
		}

		public List<' + @Table + '> Get' + @Table + '(' + @Table + ' ' + @Table + 'Params)
		{
			List<' + @Table + '> ' + @Table + 'Data = new List<' + @Table + '>();
			try
			{
				DataTable dt = new DTO.' + @Table + '().getDatos(' + @Table + 'Params);            
				foreach(DataRow dtRow in dt.Rows)
				{
					' + @Table + ' ' + @Table + 'Info = new ' + @Table + '();
'
Select @Query = @Query + 
case 
when systypes.name = 'int' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = int.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'bigint' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = int.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'decimal' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = decimal.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'date' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = DateTime.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'datetime' 
then '				if (dtRow["'+ dbo.syscolumns.name + '"].ToString() != "")
						' + @Table + 'Info.'+ dbo.syscolumns.name + ' = DateTime.Parse(dtRow["' + dbo.syscolumns.name + '"].ToString());
'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then '					' + @Table + 'Info.'+ dbo.syscolumns.name + ' = dtRow["' + dbo.syscolumns.name + '"].ToString();
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
 
select @Query = @Query + '					' + @Table + 'Data.Add(' + @Table + 'Info);
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
				DataRow dr = new DTO.' + @Table + '().getDato(' + @Table + 'Params);            
'
Select @Query = @Query + 
case 
when systypes.name = 'int' 
then '				if (dr["'+ dbo.syscolumns.name + '"].ToString() != "")
					' + @Table + 'Info.'+ dbo.syscolumns.name + ' = int.Parse(dr["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'bigint' 
then '				if (dr["'+ dbo.syscolumns.name + '"].ToString() != "")
					' + @Table + 'Info.'+ dbo.syscolumns.name + ' = int.Parse(dr["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'decimal' 
then '				if (dr["'+ dbo.syscolumns.name + '"].ToString() != "")
					' + @Table + 'Info.'+ dbo.syscolumns.name + ' = decimal.Parse(dr["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'date' 
then '				if (dr["'+ dbo.syscolumns.name + '"].ToString() != "")
					' + @Table + 'Info.'+ dbo.syscolumns.name + ' = DateTime.Parse(dr["' + dbo.syscolumns.name + '"].ToString());
'
when systypes.name = 'datetime' 
then '				if (dr["'+ dbo.syscolumns.name + '"].ToString() != "")
					' + @Table + 'Info.'+ dbo.syscolumns.name + ' = DateTime.Parse(dr["' + dbo.syscolumns.name + '"].ToString());
'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then '				' + @Table + 'Info.'+ dbo.syscolumns.name + ' = dr["' + dbo.syscolumns.name + '"].ToString();
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
 
select @Query = @Query + '            }
            catch
			{
				return new ' + @Table + '();
			}
			return ' + @Table + 'Info;
		}

		public Boolean Insert' + @Table + '(' + @Table + ' ' + @Table + 'Info, ' + @Table + ' ' + @Table + 'Param)
		{
			return new DTO.' + @Table + '().setRegistro(' + @Table + 'Info, ' + @Table + 'Param, 1);
		}

		public Boolean Update' + @Table + '(' + @Table + ' ' + @Table + 'Info, ' + @Table + ' ' + @Table + 'Param)
		{
			return new DTO.' + @Table + '().setRegistro(' + @Table + 'Info, ' + @Table + 'Param, 2);
		}

		public Boolean Delete' + @Table + '(' + @Table + ' ' + @Table + 'Info, ' + @Table + ' ' + @Table + 'Param)
		{
			return new DTO.' + @Table + '().setRegistro(' + @Table + 'Info, ' + @Table + 'Param, 3);
		}

		public Boolean Borra' + @Table + '(' + @Table + ' ' + @Table + 'Param)
		{
			return new DTO.' + @Table + '().setRegistro(' + @Table + 'Param, ' + @Table + 'Param, 4);
		}
	}
}'

Select @Query
--print @Query




