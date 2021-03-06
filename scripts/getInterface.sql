/****** Object:  StoredProcedure [dbo].[getInterface]    Script Date: 14/07/2013 09:27:49 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[getInterface](@Table varchar(max)) as

declare @Query varchar(max)        

SELECT @Query = isnull(@Query,'') + ''

set @Query = '/*
 *  I' + @Table + '.cs
 *  DataContract y ServiceContract para la entidad ' + @Table + '
 *
 *  © 2013 Rodrigo del Angel <rdelangelhmx@gmail.com>
 *  Some Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCF
{
	/// <summary>	
	/// DataContract e Interfaz de la entidad ' + @Table + '
	/// </summary>
	/// <changelog>
	/// Rodrigo del Angel    15 Feb 2013 23:42:14    Creación
	/// Rodrigo del Angel    ' + CONVERT(varchar(20),getdate(),113) + '    Modificación
	/// </changelog>
	[ServiceContract]
	public interface I' + @Table + '
	{
		[OperationContract]
		int RegistrosTotales(' + @Table + ' ' + @Table + 'Params);

        [OperationContract]
        int RegistrosTotalesRange(' + @Table + ' ' + @Table + 'Ini, ' + @Table + ' ' + @Table + 'Fin);

		[OperationContract]
		List<' + @Table + '> GetRegistrosPag(' + @Table + ' ' + @Table + 'Params, int Registros, int Pagina);

        [OperationContract]
        List<' + @Table + '> GetRegistrosPagRange(' + @Table + ' ' + @Table + 'Ini, ' + @Table + ' ' + @Table + 'Fin, int Registros, int Pagina);

		[OperationContract]
		List<' + @Table + '> Get' + @Table + '(' + @Table + ' ' + @Table + 'Params);

		[OperationContract]
		' + @Table + ' GetRegistro(' + @Table + ' ' + @Table + 'Params);

		[OperationContract]
		Boolean Insert' + @Table + '(' + @Table + ' ' + @Table + 'Info, ' + @Table + ' ' + @Table + 'Param);

		[OperationContract]
		Boolean Update' + @Table + '(' + @Table + ' ' + @Table + 'Info, ' + @Table + ' ' + @Table + 'Param);

		[OperationContract]
		Boolean Delete' + @Table + '(' + @Table + ' ' + @Table + 'Info, ' + @Table + ' ' + @Table + 'Param);

		[OperationContract]
		Boolean Borra' + @Table + '(' + @Table + ' ' + @Table + 'Param);
	}

	[DataContract]
	public class ' + @Table + '
	{
'
select @Query = @Query + 
case 
when systypes.name = 'int' 
then '	    int? ' + lower(syscolumns.name) + ' = null;
'
when systypes.name = 'bigint' 
then '	    int? ' + lower(syscolumns.name) + ' = null;
'
when systypes.name = 'decimal' 
then '	    decimal? ' + lower(syscolumns.name) + ' = null;
'
when systypes.name = 'date' 
then '	    DateTime? ' + lower(syscolumns.name) + ' = null;
'
when systypes.name = 'datetime' 
then '	    DateTime? ' + lower(syscolumns.name) + ' = null;
'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then '	    string ' + lower(syscolumns.name) + ' = string.Empty;
'
else '' end
FROM dbo.syscolumns 
 INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
 INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE (dbo.sysobjects.name = @Table)  and systypes.name not in ('sysname')
ORDER BY  dbo.sysobjects.name, dbo.syscolumns.colorder

select @Query = @Query + '
' +
case 
when systypes.name = 'int' 
then 
'	  [DataMember]
	  public int? ' + syscolumns.name + '
	  {
	       get { return ' + LOWER(syscolumns.name) + '; }
	       set { ' + LOWER(syscolumns.name) + ' = value; }
	   }

'
when systypes.name = 'bigint' 
then 
'	   [DataMember]
	   public int? ' + syscolumns.name + '
	   {
	      get { return ' + LOWER(syscolumns.name) + '; }
	       set { ' + LOWER(syscolumns.name) + ' = value; }
	   }

'
when systypes.name = 'decimal' 
then 
'	  [DataMember]
	  public decimal? ' + syscolumns.name + '
	  {
	      get { return ' + LOWER(syscolumns.name) + '; }
	      set { ' + LOWER(syscolumns.name) + ' = value; }
	  }

'
when systypes.name = 'date' 
then 
'	  [DataMember]
	  public DateTime? ' + syscolumns.name + '
	  {
	      get { return ' + LOWER(syscolumns.name) + '; }
	      set { ' + LOWER(syscolumns.name) + ' = value; }
	   }

'
when systypes.name = 'datetime' 
then 
'	  [DataMember]
	  public DateTime? ' + syscolumns.name + '
	  {
	      get { return ' + LOWER(syscolumns.name) + '; }
	      set { ' + LOWER(syscolumns.name) + ' = value; }
	  }

'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then 
'	 [DataMember]
	 public string ' + syscolumns.name + '
	 {
	      get { return ' + LOWER(syscolumns.name) + '; }
	      set { ' + LOWER(syscolumns.name) + ' = value; }
	  }

'
else '' end
FROM dbo.syscolumns 
 INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
 INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE (dbo.sysobjects.name = @Table)  and systypes.name not in ('sysname')
ORDER BY  dbo.sysobjects.name, dbo.syscolumns.colorder

Select @Query = @Query + '	}
}'
Select @Query
--print @Query
