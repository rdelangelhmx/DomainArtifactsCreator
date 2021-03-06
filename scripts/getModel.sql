/****** Object:  StoredProcedure [dbo].[getModel]    Script Date: 17/07/2013 06:19:26 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[getModel](@Table varchar(max),@Solution varchar(max)) as

declare @Query varchar(max)        

SELECT @Query = isnull(@Query,'') + ''

declare @CamposClave varchar(max)
set @CamposClave = ''
select @CamposClave += case 
when systypes.name = 'int' 
then 'int ' + (syscolumns.name) + ','
when systypes.name = 'bigint' 
then 'int ' + (syscolumns.name) + ','
when systypes.name = 'decimal' 
then 'decimal ' + (syscolumns.name) + ','
when systypes.name = 'date' 
then 'DateTime ' + (syscolumns.name) + ','
when systypes.name = 'datetime' 
then 'DateTime ' + (syscolumns.name) + ','
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then 'string ' + (syscolumns.name) + ','
else '' end
FROM dbo.syscolumns 
 INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
 INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE (dbo.sysobjects.name = @Table)  and systypes.name not in ('sysname') and syscolumns.name in (select  i2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 
      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME 
      WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' and i1.TABLE_NAME = @Table)
ORDER BY  dbo.sysobjects.name, dbo.syscolumns.colorder

set @CamposClave = SUBSTRING(@CamposClave,1,LEN(@CamposClave)-1)

declare @CamposParam varchar(max)
set @CamposParam = ''
select @CamposParam += '			' + @Table + 'Param.' + syscolumns.name + ' = ' + syscolumns.name + ';'
FROM dbo.syscolumns 
 INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
 INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE (dbo.sysobjects.name = @Table)  and systypes.name not in ('sysname') and syscolumns.name in (select  i2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 
      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME 
      WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' and i1.TABLE_NAME = @Table)
ORDER BY  dbo.sysobjects.name, dbo.syscolumns.colorder

declare @CamposParam1 varchar(max)
set @CamposParam1 = ''
select @CamposParam1 += '			' + @Table + 'Param.' + syscolumns.name + ' = ' + syscolumns.name + ';
			' + @Table + 'Info.' + syscolumns.name + ' = null;'
FROM dbo.syscolumns 
 INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
 INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE (dbo.sysobjects.name = @Table)  and systypes.name not in ('sysname') and syscolumns.name in (select  i2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 
      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME 
      WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' and i1.TABLE_NAME = @Table)
ORDER BY  dbo.sysobjects.name, dbo.syscolumns.colorder

declare @CamposInfo varchar(max)
set @CamposInfo = ''
select @CamposInfo += '			' + @Table + 'Param.' + syscolumns.name + ' = ' + @Table + 'Info.' + syscolumns.name + ';
			' + @Table + 'Info.' + syscolumns.name + ' = null;'
FROM dbo.syscolumns 
 INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
 INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE (dbo.sysobjects.name = @Table)  and systypes.name not in ('sysname') and syscolumns.name in (select  i2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 
      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME 
      WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' and i1.TABLE_NAME = @Table)
ORDER BY  dbo.sysobjects.name, dbo.syscolumns.colorder

set @Query = '/*
 *  ' + @Table + 'Model.cs
 *  Model para la entidad ' + @Table + '
 *
 *  © 2013 Rodrigo del Angel <rdelangelhmx@gmail.com>
 *  Some Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ' + @Solution + '.Service' + @Table + ';

namespace ' + @Solution + '.Models
{    
    
    public class ' + @Table + 'Model
    {
        public static int Registros(' + @Table + ' ' + @Table + 'Param)
        {
            return new I' + @Table + 'Client().RegistrosTotales(' + @Table + 'Param);
        }

        public static int RegistrosRange(' + @Table + ' ' + @Table + 'Ini, ' + @Table + ' ' + @Table + 'Fin)
        {
            return new I' + @Table + 'Client().RegistrosTotalesRange(' + @Table + 'Ini, ' + @Table + 'Fin);
        }

        public static List<' + @Table + '> GetRegistrosPag(int Pagina, ' + @Table + ' ' + @Table + 'Param)
        {
            return new I' + @Table + 'Client().GetRegistrosPag(' + @Table + 'Param, Globales.Registros, Pagina);
        }

        public static List<' + @Table + '> GetRegistrosPagRange(int Pagina, ' + @Table + ' ' + @Table + 'Ini, ' + @Table + ' ' + @Table + 'Fin)
        {
            return new I' + @Table + 'Client().GetRegistrosPagRange(' + @Table + 'Ini, ' + @Table + 'Fin, Globales.Registros, Pagina);
        }

        public static List<' + @Table + '> Get' + @Table + '()
        {
            ' + @Table + ' ' + @Table + 'Param = new ' + @Table + '();
            return new I' + @Table + 'Client().Get' + @Table + '(' + @Table + 'Param);
        }

        public static ' + @Table + ' GetRegistro' + @Table + '(' + @CamposClave + ')
        {
            ' + @Table + ' ' + @Table + 'Param = new ' + @Table + '();
' + @CamposParam + '
            return new I' + @Table + 'Client().GetRegistro(' + @Table + 'Param);
        }

        public static Boolean Insert' + @Table + '(' + @Table + ' ' + @Table + 'Info)
        {
            ' + @Table + 'Info.Estatus = 1;
            return new I' + @Table + 'Client().Insert' + @Table + '(' + @Table + 'Info, ' + @Table + 'Info);
        }

        public static Boolean Delete' + @Table + '(' + @CamposClave + ')
        {
            ' + @Table + ' ' + @Table + 'Info = new ' + @Table + '();
            ' + @Table + ' ' + @Table + 'Param = new ' + @Table + '();
' + @CamposParam1 + '
            ' + @Table + 'Info.Estatus = 0;
            return new I' + @Table + 'Client().Delete' + @Table + '(' + @Table + 'Info, ' + @Table + 'Param);
        }

        public static Boolean Borrar' + @Table + '(' + @CamposClave + ')
        {
            ' + @Table + ' ' + @Table + 'Param = new ' + @Table + '();
' + @CamposParam1 + '
            return new I' + @Table + 'Client().Borra' + @Table + '(' + @Table + 'Param);
        }

        public static Boolean Update' + @Table + '(' + @Table + ' ' + @Table + 'Info)
        {
            ' + @Table + ' ' + @Table + 'Param = new ' + @Table + '();
' + @CamposInfo + '
            return new I' + @Table + 'Client().Update' + @Table + '(' + @Table + 'Info, ' + @Table + 'Param);
        }

    }
}'
--print @Query
select @Query