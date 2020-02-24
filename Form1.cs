using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WCF_MVC_Creator
{
    public partial class Form1 : Form
    {
        public string sPath = AppDomain.CurrentDomain.BaseDirectory;
        public string sTablasEx = ConfigurationSettings.AppSettings["tablas"].ToString();
        public string NameSpace = ConfigurationSettings.AppSettings["NameSpace"].ToString();
        public string ServiceName = "orcl";
        public string[] dbSQL = new string[6];
        public string[] dbMySQL = new string[6];
        public string[] dbOracle = new string[6];
        public string Creado = string.Format("{0:dd-MMM-yyyy HH:mm}", DateTime.Now).Replace(".","");
        public string Anio = DateTime.Now.Year.ToString();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pbProgreso.Visible = false;
            lProgreso.Visible = false;
            textBox1.Text = "";
            textBox3.Text = sPath;
            tNameSpace.Text = NameSpace;
            tService.Text = ServiceName;
            try
            {
                tService.Visible = false;
                label9.Visible = false;

                textBox3.Text = ConfigurationSettings.AppSettings["Path"];

                tSchema.Text = ConfigurationSettings.AppSettings["Schema"];

                tAddition.Text = ConfigurationSettings.AppSettings["Addition"].ToString();
                tDatos.Text = ConfigurationSettings.AppSettings["Datos"].ToString().Replace("[","<").Replace("]", ">");

                dbSQL = ConfigurationSettings.AppSettings["dbSQL"].Split(';');
                dbMySQL = ConfigurationSettings.AppSettings["dbMySQL"].Split(';');
                dbOracle = ConfigurationSettings.AppSettings["dbOracle"].Split(';');

                tDrivers.Text = dbSQL[0];
                tServidor.Text = dbSQL[1];
                tDataBase.Text = dbSQL[2];
                tUser.Text = dbSQL[3];
                tPassword.Text = dbSQL[4];
                tPuerto.Text = dbSQL[5];
                tService.Text = dbSQL[6];
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.DoEvents(); 
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Porporcione la Ruta destino");
                return;
            }
            NameSpace = tNameSpace.Text;
            ServiceName = tService.Text;
            if (tDrivers.Text == "{Microsoft ODBC Driver for Oracle}")
                textBox1.Text = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + tService.Text + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
            else
                textBox1.Text = "Driver=" + tDrivers.Text + ";Server=" + tServidor.Text + ";uid=" + tUser.Text + ";password=" + tPassword.Text + ";Database=" + tDataBase.Text + (string.IsNullOrEmpty(tPuerto.Text) ? "" : ";Port=" + tPuerto.Text) + ";";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string sQuery = "";
                string sModel = "";
                string sInterfase = "";
                string sRepositoryKeys = "";
                string sRepository = "";
                string sOrder = "";
                string sValores = "";
                string sWhere = "";
                string sParams = "";
                string sStored = "";
                string sStoredA = "";
                string sStoredB = "";
                string sStoredC = "";
                string sStoredD = "";
                string sStoredE = "";
                string sStoredF = "";
                string sSelect = "";
                string sCamposParam = "";
                string sValoresParam = "";
                string sParamsParams = "";
                string sCamposHabilitar = "";
                string Esquema = "";
                string Tipo = "";
                string Tabla = "";
                string FileTabla = "";
                string Model = "";
                string Interfase = "";
                string Repository = "";
                string Order = "";
                string Valores = "";
                string Where = "";
                string Params = "";
                string Select = "";
                string CamposParam = "";
                string ValoresParam = "";
                string ParamsParams = "";
                switch (tDrivers.Text)
                {
                    #region {SQL Server}
                    case "{SQL Server}":
                        if(string.IsNullOrEmpty(tTabla.Text))
                            sQuery = "select o.object_id IdTabla,case when type='U' then 'Tabla' when type='V' then 'Vista' else ''end Tipo,O.name as Tabla,S.schema_id as IdEsquema,S.name as Esquema from sys.objects O inner join sys.schemas S on O.schema_id=S.schema_id where s.name = 'dbo' and o.type in('V','U') order by 3";
                        else if(tTabla.Text.Contains(";"))
                            sQuery = "select o.object_id IdTabla,case when type='U' then 'Tabla' when type='V' then 'Vista' else ''end Tipo,O.name as Tabla,S.schema_id as IdEsquema,S.name as Esquema from sys.objects O inner join sys.schemas S on O.schema_id=S.schema_id where s.name = 'dbo' and o.name in ('" + string.Join("','", tTabla.Text.Split(';')) + "') and o.type in('V','U') order by 3";
                        else
                            sQuery = "select o.object_id IdTabla,case when type='U' then 'Tabla' when type='V' then 'Vista' else ''end Tipo,O.name as Tabla,S.schema_id as IdEsquema,S.name as Esquema from sys.objects O inner join sys.schemas S on O.schema_id=S.schema_id where s.name = 'dbo' and o.name = '" + tTabla.Text + "' and o.type in('V','U') order by 3";
                        sModel = @"
select
case when charindex('-',syscolumns.name) > 0
then '		[Column(""' + syscolumns.name + '""' + 
    case when
    (systypes.name = 'date' or systypes.name = 'datetime' or systypes.name = 'smalldatetime' or systypes.name = 'time' or systypes.name = 'money' or systypes.name = 'smallint' or systypes.name = 'real' or systypes.name = 'bigint' or systypes.name = 'time' or systypes.name = 'float')
	then ', TypeName = ""' + systypes.name + '""' else '' end + ')]
' else '' end +
case
when (systypes.name = 'int' or systypes.name = 'smallint' or systypes.name = 'tinyint') and CONSTRAINT_NAME is not null
then '		[Key]
		public int? ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when (systypes.name = 'int' or systypes.name = 'smallint' or systypes.name = 'tinyint') and CONSTRAINT_NAME is null
then '		public int? ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when systypes.name = 'bigint' and CONSTRAINT_NAME is not null
then '		[Key]
		public long? ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when systypes.name = 'bigint' and CONSTRAINT_NAME is null
then '		public long? ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when systypes.name = 'decimal' or systypes.name = 'money'
then '		public decimal? ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when systypes.name = 'real' 
then '		public double? ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when systypes.name = 'float' 
then '		public float? ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when systypes.name = 'date' or systypes.name = 'datetime' or systypes.name = 'smalldatetime' or systypes.name = 'time'
then '		public DateTime? ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char') and CONSTRAINT_NAME is not null
then '		[Key]
' + (case when syscolumns.length > 0 then '		[StringLength(' + convert(varchar,syscolumns.length) + ')]
' else '' end) + 
'		public string ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char') and CONSTRAINT_NAME is null
then (case when syscolumns.length > 0 then 
'		[StringLength(' + convert(varchar,syscolumns.length) + ')]
' else '' end) + 
'		public string ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when systypes.name = 'uniqueidentifier' and CONSTRAINT_NAME is not null
then '		[Key]
		public Guid? ' + replace(syscolumns.name,'-','') + ' { get; set; }'   
when systypes.name = 'uniqueidentifier' and CONSTRAINT_NAME is null
then '		public Guid? ' + replace(syscolumns.name,'-','') + ' { get; set; }'
when (systypes.name = 'bit')
then '		public bool? ' + replace(syscolumns.name,'-','') + ' { get; set; }'
else '		public ' + systypes.name + ' ' + replace(syscolumns.name,'-','') + ' { get; set; }'  end
FROM dbo.syscolumns 
    INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
    INNER JOIN (select o.object_id,O.name as object,S.schema_id,S.name as [schema]
			from sys.objects O 
			inner join sys.schemas S on O.schema_id=S.schema_id 
			where S.name = '{1}' ) schemas on sysobjects.id = schemas.object_id
    INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
    LEFT JOIN (SELECT DISTINCT ORDINAL_POSITION, CONSTRAINT_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
    and TABLE_NAME  = '{0}' and CONSTRAINT_SCHEMA = '{1}') Keys on dbo.syscolumns.colid = Keys.ORDINAL_POSITION
where  dbo.sysobjects.name = '{0}' and systypes.name not in ('sysname') and [schema] = '{1}'
order by schemas.schema_id, dbo.syscolumns.colid";
                        sInterfase = @"
Select
case 
when (systypes.name = 'int' or systypes.name = 'smallint') 
then '		Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(int? _' + replace(syscolumns.name,'-','') + ');
'
when systypes.name = 'bigint' 
then '		Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(long? _' + replace(syscolumns.name,'-','') + ');
'
when systypes.name = 'decimal' or systypes.name = 'money'
then '		Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(decimal? _' + replace(syscolumns.name,'-','') + ');
'
when systypes.name = 'real' 
then '		Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(double? _' + replace(syscolumns.name,'-','') + ');
'
when systypes.name = 'float' 
then '		Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(float? _' + replace(syscolumns.name,'-','') + ');
'
when systypes.name = 'date' or systypes.name = 'datetime' 
then '		Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(DateTime? _' + replace(syscolumns.name,'-','') + ');
'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then '		Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(string _' + replace(syscolumns.name,'-','') + ');
'
when systypes.name = 'uniqueidentifier' 
then '		Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(Guid? _' + replace(syscolumns.name,'-','') + ');
'
when (systypes.name = 'bit')
then '		Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(bool? _' + replace(syscolumns.name,'-','') + ');
'
else '		Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(' + systypes.name + ' _' + replace(syscolumns.name,'-','') + ');
'  end
FROM dbo.syscolumns 
    INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
    INNER JOIN (select o.object_id,O.name as object,S.schema_id,S.name as [schema]
			from sys.objects O 
			inner join sys.schemas S on O.schema_id=S.schema_id 
			where S.name = '{1}') schemas on sysobjects.id = schemas.object_id
    INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
    LEFT JOIN (SELECT DISTINCT ORDINAL_POSITION, CONSTRAINT_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
    and TABLE_NAME  = '{0}' and CONSTRAINT_SCHEMA = '{1}') Keys on dbo.syscolumns.colid = Keys.ORDINAL_POSITION
where  dbo.sysobjects.name = '{0}' and systypes.name not in ('sysname') and CONSTRAINT_NAME is not null and [schema] = '{1}'
order by schemas.schema_id, dbo.syscolumns.colid";
                        sRepositoryKeys = @"
select distinct syscolumns.name keys
FROM dbo.syscolumns 
    INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
    INNER JOIN (select o.object_id,O.name as object,S.schema_id,S.name as [schema]
			from sys.objects O 
			inner join sys.schemas S on O.schema_id=S.schema_id 
			where S.name = '{1}' ) schemas on sysobjects.id = schemas.object_id
    INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
    LEFT JOIN (SELECT DISTINCT ORDINAL_POSITION, CONSTRAINT_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
    and TABLE_NAME = '{0}' and CONSTRAINT_SCHEMA = '{1}') Keys on dbo.syscolumns.colid = Keys.ORDINAL_POSITION
where  dbo.sysobjects.name = '{0}' and systypes.name not in ('sysname') and CONSTRAINT_NAME is not null and [schema] = '{1}'
order by 1";
                        sRepository = @"
Select
case 
when (systypes.name = 'int' or systypes.name = 'smallint') 
then '		public async Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(int? _' + replace(syscolumns.name,'-','') + ')
'
when systypes.name = 'bigint' 
then '		public async Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(long? _' + replace(syscolumns.name,'-','') + ')
'
when systypes.name = 'decimal' or systypes.name = 'money'
then '		public async Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(decimal? _' + replace(syscolumns.name,'-','') + ')
'
when systypes.name = 'real' 
then '		public async Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(double? _' + replace(syscolumns.name,'-','') + ')
'
when systypes.name = 'float' 
then '		public async Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(float? _' + replace(syscolumns.name,'-','') + ')
'
when systypes.name = 'date' or systypes.name = 'datetime' 
then '		public async Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(DateTime? _' + replace(syscolumns.name,'-','') + ')
'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then '		public async Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(string _' + replace(syscolumns.name,'-','') + ')
'
when systypes.name = 'uniqueidentifier' 
then '		public async Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(Guid? _' + replace(syscolumns.name,'-','') + ')
'
when (systypes.name = 'bit')
then '		public async Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(bool? _' + replace(syscolumns.name,'-','') + ')
'
else '		public async Task<{0}> GetBy' + replace(replace(syscolumns.name,'-',''),'_','') + '(' + systypes.name + ' _' + replace(syscolumns.name,'-','') + ')
'  end + 
'		{
            try
            {
                {0} item = new {0}() { ' + replace(syscolumns.name,'-','') + ' = _' + replace(syscolumns.name,'-','') + '};
                var parameters = (object)DataHelper.MappingParams(item);
                var entityName = DataHelper.GetItemName(item);
                var storedName = $""{config.DBschema()}.spGetRecords{entityName}"";
                string where = """";
                IEnumerable<{0}> data;
                // Implementar la carga por Cache
                if (aEntitiesCache.Contains(entityName))
                {
                    try
                    {
                        where = string.Join("" and "", DataHelper.MappingWhere(item));
                        // Validar que exista la clave
                        if (!cacheData.ExistsKey(entityName))
                        {
                            // Obtener datos de la BD
                            using (IDbConnection conn = dbConnection)
                            {
                                conn.Open();
                                data = parameters != null ?
                                    // Con Parametros
                                    await conn.QueryAsync<{0}>(storedName, param: parameters, commandType: CommandType.StoredProcedure) :
                                    // Sin Parametros
                                    await conn.QueryAsync<{0}>(storedName, commandType: CommandType.StoredProcedure);
                                conn.Close();
                                // Poner los datos en Cache
                                cacheData.PutData(entityName, data);
                            }
                        }
                        else
                            data = cacheData.GetRecords<{0}>(entityName);
                        if (string.IsNullOrEmpty(where))
                            return data.FirstOrDefault();
                        else
                            return data.AsQueryable().Where(where).FirstOrDefault();

                    }
                    catch (Exception ex)
                    {
                        appLogger.LogError(ex);
                        using (IDbConnection conn = dbConnection)
                        {
                            conn.Open();
                            var res = parameters != null ?
                                // Con Parametros
                                await conn.QueryAsync<{0}>(storedName, param: parameters, commandType: CommandType.StoredProcedure) :
                                // Sin Parametros
                                await conn.QueryAsync<{0}>(storedName, commandType: CommandType.StoredProcedure);
                            conn.Close();
                            return res.FirstOrDefault();
                        }
                    }
                }
                else
                {
                    using (IDbConnection conn = dbConnection)
                    {
                        conn.Open();
                        var res = parameters != null ?
                            // Con Parametros
                            await conn.QueryAsync<{0}>(storedName, param: parameters, commandType: CommandType.StoredProcedure) :
                            // Sin Parametros
                            await conn.QueryAsync<{0}>(storedName, commandType: CommandType.StoredProcedure);
                        conn.Close();
                        return res.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                appLogger.LogError(ex);
            }
            return null;
        }
'
FROM dbo.syscolumns 
    INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
    INNER JOIN (select o.object_id,O.name as object,S.schema_id,S.name as [schema]
			from sys.objects O 
			inner join sys.schemas S on O.schema_id=S.schema_id 
			where S.name = '{1}' ) schemas on sysobjects.id = schemas.object_id
    INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
    LEFT JOIN (SELECT DISTINCT ORDINAL_POSITION, CONSTRAINT_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
    and TABLE_NAME = '{0}' and CONSTRAINT_SCHEMA = '{1}') Keys on dbo.syscolumns.colid = Keys.ORDINAL_POSITION
where  dbo.sysobjects.name = '{0}' and systypes.name not in ('sysname') and CONSTRAINT_NAME is not null and [schema] = '{1}'
order by schemas.schema_id, dbo.syscolumns.colid";
                        sOrder = @"
SELECT DISTINCT '[' + COLUMN_NAME + ']'
FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE
WHERE TABLE_NAME = '{0}' AND CONSTRAINT_SCHEMA = '{1}'";
                        sValores = @"
SELECT '	@' + REPLACE(COLUMN_NAME,'-','_')
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '{1}'
ORDER BY COLUMN_NAME";
                        sWhere = @"
SELECT '(@' + REPLACE(COLUMN_NAME,'-','_') + ' IS NULL OR [' + COLUMN_NAME + '] LIKE ' + 
CASE WHEN (DATA_TYPE = 'nvarchar'  or DATA_TYPE = 'varchar' or DATA_TYPE = 'nchar' or DATA_TYPE = 'char') THEN ' + ''%'' + ' ELSE '' END
+ ' @' + REPLACE(COLUMN_NAME,'-','_') + 
CASE WHEN (DATA_TYPE = 'nvarchar'  or DATA_TYPE = 'varchar' or DATA_TYPE = 'nchar' or DATA_TYPE = 'char') THEN ' + ''%''' ELSE '' END
+ ')'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '{1}'
ORDER BY ORDINAL_POSITION";
                        sParams = @"
SELECT CASE WHEN CHARACTER_MAXIMUM_LENGTH IS NULL
THEN '@' + REPLACE(COLUMN_NAME,'-','_') + ' ' + DATA_TYPE + ' = NULL'
ELSE '@' + REPLACE(COLUMN_NAME,'-','_') + ' ' + DATA_TYPE + '('+ (CASE WHEN CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR(max)) <=0 THEN 'MAX' ELSE CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR(max)) END) +') = NULL'
END 
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '{1}'
ORDER BY ORDINAL_POSITION";
                        sSelect = @"
SELECT '[' + COLUMN_NAME + ']'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '{1}'
ORDER BY ORDINAL_POSITION";
                        sCamposParam = @"
SELECT '--				@' + COLUMN_NAME + ' = <value or null>'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '{1}'
ORDER BY ORDINAL_POSITION";
                        sValoresParam = @"
SELECT '@' + COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '{1}'
ORDER BY ORDINAL_POSITION";
                        sParamsParams = @"
SELECT '			@' + COLUMN_NAME + ' = @' + COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '{1}'
ORDER BY ORDINAL_POSITION";
                        sStored = @"use {0}
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

/*
* Elimina el STORED para traer el Index 
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spGetIndex{1}')
	DROP PROCEDURE spGetIndex{1}
GO
/* 
* Crea el STORED para traer el Index 
*/

CREATE PROCEDURE spGetIndex{1}(
/*
*  Stored procedure para obtener:
*       + registros paginados (INDEX)
*   
*  Arhivo: spGetIndex{1}.sql
*  Creado: {2}
*  Autor: {3}
*  
*/
{4},
@PageNo int = 1, 
@RecordsPerPage int = 10) 
AS 
SELECT {5} 
FROM {1}
WHERE {6}
ORDER BY {7}
OFFSET ( @PageNo - 1 ) * @RecordsPerPage ROWS 
FETCH NEXT @RecordsPerPage ROWS ONLY;
GO

/* 
* Elimina el STORED para traer el Count 
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spGetCount{1}')
	DROP PROCEDURE spGetCount{1}
GO
/* 
* Crea el STORED para traer el Count 
*/
CREATE PROCEDURE spGetCount{1}(
/*
*  Stored procedure para obtener:
*       + registros totales (COUNT)
*   
*  Arhivo: spGetCount{1}.sql
*  Creado: {2}
*  Autor: {3}
*  
*/
{4}
) 
AS 	
SELECT count(*) Registros
FROM {1}
WHERE {6};
GO

/* 
* Elimina el STORED para traer el(los) Record(s) 
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spGetRecords{1}')
	DROP PROCEDURE spGetRecords{1}
GO
/* 
* Crea el STORED para traer el(los) Record(s) 
*/
CREATE PROCEDURE spGetRecords{1}(
/*
*  Stored procedure para obtener:
*       + registros no paginados (RECORD)
*   
*  Arhivo: spGetRecords{1}.sql
*  Creado: {2}
*  Autor: {3}
*  
*/
{4}
) 
AS 
SELECT {5} 
FROM {1} 
WHERE {6}
ORDER BY {7};
GO

/* 
* Elimina el STORED para insertar un registro
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spInsRecord{1}')
	DROP PROCEDURE spInsRecord{1}
GO
/* 
* Crea el STORED para insertar un registro 
*/
CREATE PROCEDURE spInsRecord{1}(
/*
*  Stored procedure para insertar:
*       + registro en la tabla
*		+ manejo de errores
*   
*  Arhivo: spInsRecord{1}.sql
*  Creado: {2}
*  Autor: {3}
*  
*/
@Campos varchar(max) = '',
@Valores varchar(max) = ''
) 
AS 
DECLARE @Query varchar(max) = '';
IF @Campos is not null and @Valores is not null
BEGIN
	BEGIN try
		BEGIN TRANSACTION;
			SET @Query = 'INSERT INTO {1} (
' + @Campos + ') 
VALUES(
' + @Valores + ');
{10}'
			EXEC(@Query)
			RETURN 1;
		COMMIT TRANSACTION; 
	END try
	BEGIN CATCH
		SELECT ERROR_NUMBER() AS ErrorNumber,
			ERROR_SEVERITY() AS ErrorSeverity,
			ERROR_MESSAGE() AS ErrorMessage;
		ROLLBACK TRANSACTION;
	END CATCH;
END
ELSE 
	RETURN 0;
GO

/* 
* Elimina el STORED para actualizar uno o varios registros
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spUpdRecord{1}')
	DROP PROCEDURE spUpdRecord{1}
GO
/* 
* Crea el STORED para insertar un registro 
*/
CREATE PROCEDURE spUpdRecord{1}(
/*
*  Stored procedure para actualizar:
*       + registro en la tabla
*		+ manejo de transacción para no afectar datos
*		+ manejo de errores
*   
*  Arhivo: spUpdRecord{1}.sql
*  Creado: {2}
*  Autor: {3}
*  
*/
{4},
@Valores varchar(max) = NULL
) 
AS 
DECLARE @Query varchar(max) = '';
IF @Valores is not null
BEGIN
	BEGIN try
		BEGIN TRANSACTION;
			SET @Query = 'UPDATE {1} SET 
' + @Valores + ' 
WHERE 
{9};'
			EXEC(@Query)
			RETURN 1;
		COMMIT TRANSACTION; 
	END try
	BEGIN CATCH
		SELECT ERROR_NUMBER() AS ErrorNumber,
			ERROR_SEVERITY() AS ErrorSeverity,
			ERROR_MESSAGE() AS ErrorMessage;
		ROLLBACK TRANSACTION;
	END CATCH;
END
ELSE 
	RETURN 0;
GO";
                        sStoredA = @"use {0}
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

/*
* Elimina el STORED para traer el Index 
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spGetIndex{1}' and schema_id = (select schema_id from sys.schemas where name = '{10}'))
	DROP PROCEDURE {10}.spGetIndex{1}
GO
/* 
* Crea el STORED para traer el Index 
*/

CREATE PROCEDURE {10}.spGetIndex{1}(
-- ============================================================================================================================
-- Author:		{3}
-- Create date: {2}
-- Description:	Obtiene registros paginados para la tabla {1}
-- ============================================================================================================================
-- Llamada: 
--		EXEC {10}.spGetIndex{1} 
{11}
-- ============================================================================================================================
-- History:                      
-- Date             Author              Description                      
-- -------------------------------------------------------------------------------                      
-- ============================================================================================================================
{4},
@PageNo int = 1, 
@RecordsPerPage int = 10) 
AS 
SELECT {5} 
FROM {1}
WHERE {6}
ORDER BY {7}
OFFSET ( @PageNo - 1 ) * @RecordsPerPage ROWS 
FETCH NEXT @RecordsPerPage ROWS ONLY;
GO";
                        sStoredB = @"use {0}
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
* Elimina el STORED para traer el Count 
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spGetCount{1}' and schema_id = (select schema_id from sys.schemas where name = '{10}'))
	DROP PROCEDURE {10}.spGetCount{1}
GO
/* 
* Crea el STORED para traer el Count 
*/
CREATE PROCEDURE {10}.spGetCount{1}(
-- ============================================================================================================================
-- Author:		{3}
-- Create date: {2}
-- Description:	Obtiene registros totales para la tabla {1}
-- ============================================================================================================================
-- Llamada: 
--		EXEC {10}.spGetCount{1} 
{11}
-- ============================================================================================================================
-- History:                      
-- Date             Author              Description                      
-- -------------------------------------------------------------------------------                      
-- ============================================================================================================================
{4}
) 
AS 	
SELECT count(*) Registros
FROM {1}
WHERE {6};
GO";
                        sStoredC = @"use {0}
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
* Elimina el STORED para traer el(los) Record(s) 
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spGetRecords{1}' and schema_id = (select schema_id from sys.schemas where name = '{10}'))
	DROP PROCEDURE {10}.spGetRecords{1}
GO
/* 
* Crea el STORED para traer el(los) Record(s) 
*/
CREATE PROCEDURE {10}.spGetRecords{1}(
-- ============================================================================================================================
-- Author:		{3}
-- Create date: {2}
-- Description:	Obtiene registros paginados para la tabla {1}
-- ============================================================================================================================
-- Llamada: 
--		    EXEC {10}.spGetRecords{1}
{11}
-- ============================================================================================================================
-- History:                      
-- Date             Author              Description                      
-- -------------------------------------------------------------------------------                      
-- ============================================================================================================================
{4}
) 
AS 
SELECT {5} 
FROM {1} 
WHERE {6}
ORDER BY {7};
GO";
                        sStoredD = @"use {0}
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
* Elimina el STORED para insertar un registro
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spInsRecord{1}' and schema_id = (select schema_id from sys.schemas where name = '{10}'))
	DROP PROCEDURE {10}.spInsRecord{1}
GO
/* 
* Crea el STORED para insertar un registro 
*/
CREATE PROCEDURE {10}.spInsRecord{1}(
-- ============================================================================================================================
-- Author:		{3}
-- Create date: {2}
-- Description:	Inserta un registro para la tabla {1}
-- ============================================================================================================================
-- Llamada: 
--		EXEC {10}.spInsRecord{1} 
---			EXEC {10}.spInsRecordAsociados_Operaciones 
{11}
--				@Campos = '{5}',
--				@Valores = '{12}'
-- ============================================================================================================================
-- History:                      
-- Date             Author              Description                      
-- -------------------------------------------------------------------------------                      
-- ============================================================================================================================
@Campos nvarchar(max) = '',
@Valores nvarchar(max) = '',
{4}
) 
AS 
DECLARE @Query nvarchar(max) = '',
@Parametros nvarchar(max) = '
{4}';
IF @Campos is not null and @Valores is not null
BEGIN
	BEGIN try
		SET @Query = 'INSERT INTO {1} (' + @Campos + ') VALUES(' + @Valores + ');'
		EXEC sp_Executesql 
			@Query, 
			@Parametros, 
{13}
		SELECT 1;
	END try
	BEGIN CATCH
		SELECT ERROR_NUMBER() AS ErrorNumber,
			ERROR_SEVERITY() AS ErrorSeverity,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH;
END
ELSE 
	SELECT 0;
GO";
                        sStoredE = @"use {0}
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
* Elimina el STORED para actualizar uno o varios registros
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spUpdRecord{1}' and schema_id = (select schema_id from sys.schemas where name = '{10}'))
	DROP PROCEDURE {10}.spUpdRecord{1}
GO
/* 
* Crea el STORED para actualizar uno o varios registros
*/
CREATE PROCEDURE {10}.spUpdRecord{1}(
-- ============================================================================================================================
-- Author:		{3}
-- Create date: {2}
-- Description:	Actualiza registros para la tabla {1}
-- ============================================================================================================================
-- Llamada: 
--		    EXEC {10}.spUpdRecord{1} 
{11}
--				@Valores = '(list of values for any field format: @Field = <value>)'
-- ============================================================================================================================
-- History:                      
-- Date             Author              Description                      
-- -------------------------------------------------------------------------------                      
-- ============================================================================================================================
@Valores nvarchar(max) = NULL,
{4}
) 
AS 
DECLARE @Query nvarchar(max) = '',
@Parametros nvarchar(max) = '
{4}';
IF @Valores is not null 
BEGIN
	BEGIN try
		SET @Query = 'UPDATE {1} SET ' + @Valores + ' 
            WHERE {6} ;'
		EXEC sp_Executesql 
			@Query, 
			@Parametros, 
{13}
		SELECT 1;
	END try
	BEGIN CATCH
		SELECT ERROR_NUMBER() AS ErrorNumber,
			ERROR_SEVERITY() AS ErrorSeverity,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH;
END
ELSE 
	SELECT 0;
GO";
                        sStoredF = @"use {0}
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
* Elimina el STORED para eliminar uno o varios registros
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spDelRecord{1}' and schema_id = (select schema_id from sys.schemas where name = '{10}'))
	DROP PROCEDURE {10}.spDelRecord{1}
GO
/* 
* Crea el STORED para eliminar uno o varios registros
*/
CREATE PROCEDURE {10}.spDelRecord{1}(
-- ============================================================================================================================
-- Author:		{3}
-- Create date: {2}
-- Description:	Elimina registros para la tabla {1}
-- ============================================================================================================================
-- Llamada: 
--		    EXEC {10}.spDelRecord{1} 
{11}
-- ============================================================================================================================
-- History:                      
-- Date             Author              Description                      
-- -------------------------------------------------------------------------------                      
-- ============================================================================================================================
{4}
) 
AS 
BEGIN try
	DELETE FROM {1} 
        WHERE {6} ;
	SELECT 1;
END try
BEGIN CATCH
	SELECT ERROR_NUMBER() AS ErrorNumber,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_MESSAGE() AS ErrorMessage;
END CATCH;
GO";
                        sCamposHabilitar = @"
SELECT replace(COLUMN_NAME,'-','_') Campo,  
case 
when DATA_TYPE = 'int' or DATA_TYPE = 'smallint' or DATA_TYPE = 'bigint' or DATA_TYPE = 'decimal' or 
	DATA_TYPE = 'money' or DATA_TYPE = 'real' or DATA_TYPE = 'float' or DATA_TYPE = 'nvarchar'  or 
	DATA_TYPE = 'varchar' or DATA_TYPE = 'nchar' or DATA_TYPE = 'char' or DATA_TYPE = 'uniqueidentifier' or
	DATA_TYPE = 'date' or DATA_TYPE = 'datetime'
then '1'
when (DATA_TYPE = 'bit')
then '2'
else '1'  end Tipo
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '{1}' and (COLUMN_DEFAULT = '('''')' or COLUMN_DEFAULT = '((0))' or COLUMN_DEFAULT = '(''A'')' or COLUMN_DEFAULT IS NULL)
ORDER BY ORDINAL_POSITION";
                        break;
                    #endregion
                    #region {MySQL ODBC 8.0 ANSI Driver}
                    case "{MySQL ODBC 8.0 ANSI Driver}":
                        if (string.IsNullOrEmpty(tTabla.Text))
                            sQuery = "select Table_Comment Tabla, case when Table_Type ='BASE TABLE' then 'Tabla' else 'Vista' end Tipo FROM INFORMATION_SCHEMA.Tables where Table_Schema = '" + tDataBase.Text + "' order by Table_Name";
                        else if (tTabla.Text.Contains(";"))
                            sQuery = "select Table_Comment Tabla, case when Table_Type ='BASE TABLE' then 'Tabla' else 'Vista' end Tipo FROM INFORMATION_SCHEMA.Tables where Table_Name in ('" + string.Join("','", tTabla.Text.Split(';')) + "') and Table_Schema = '" + tDataBase.Text + "' order by Table_Name";
                        else
                            sQuery = "select Table_Comment Tabla, case when Table_Type ='BASE TABLE' then 'Tabla' else 'Vista' end Tipo FROM INFORMATION_SCHEMA.Tables where Table_Schema = '" + tDataBase.Text + "' and Table_Name = '" + tTabla.Text + "' order by Table_Name";
                        sModel = @"
select concat(
case when INSTR(column_name, '-') > 0
then Concat('[[Column(""',column_name,'"" ',
    case when data_type = 'date' or data_type = 'datetime' or data_type = 'smalldatetime' or data_type = 'time' or data_type = 'money' or data_type = 'smallint' or data_type = 'real'
    then concat(', TypeName = ""',data_type,'""') else '' end, 
')]]\n') else '' end,
case
when COLUMN_KEY <> '' then '		[Key]\n' else '' END,
case
when data_type = 'int' or data_type = 'smallint'
then CONCAT('		public int?  ' , replace(column_name, '-', '') , ' {get;set;}\n')
when(data_type = 'int' or data_type = 'smallint') and trim(COLUMN_KEY) = ''
then CONCAT('		public int?  ' , replace(column_name, '-', '') , ' {get;set;}\n')
when data_type = 'bigint'
then CONCAT('		public long? ' , replace(column_name, '-', '') , ' {get;set;}\n')
when data_type = 'decimal'
then CONCAT('		public decimal? ' , replace(column_name, '-', '') , ' {get;set;}\n')
when data_type = 'double'
then CONCAT('		public double? ' , replace(column_name, '-', '') , ' {get;set;}\n')
when data_type = 'float'
then CONCAT('		public float? ' , replace(column_name, '-', '') , ' {get;set;}\n')
when data_type = 'bit'
then CONCAT('		public bool? ' , replace(column_name, '-', '') , ' {get;set;}\n')
when data_type = 'date' or data_type = 'datetime' or data_type = 'time'
then CONCAT('		public DateTime? ' , replace(column_name, '-', '') , ' {get;set;}\n')
when data_type = 'varchar'  or data_type = 'nvarchar' or data_type = 'char' or data_type = 'longtext' or data_type = 'text' or data_type = 'longblob'
then CONCAT('		public string ' , replace(column_name, '-', '') , ' {get;set;}\n')
else CONCAT('		public ', data_type, ' ', replace(column_name, '-', ''), ' {get;set;}\n') end) LineaCodigo
    FROM INFORMATION_SCHEMA.COLUMNS Cols
WHERE table_schema = '{0}' and table_name = '{1}'
ORDER BY  ordinal_position; ";
                        sInterfase = @"
select 
case 
when data_type = 'int' or data_type = 'smallint'
then CONCAT('		Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(int?  ' , replace(column_name,'-','') , ');\n')
when data_type = 'bigint'   
then CONCAT('		Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(long? ' , replace(column_name,'-','') , ');\n')
when data_type = 'decimal'  
then CONCAT('		Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(decimal? ' , replace(column_name,'-','') , ');\n')
when data_type = 'double'  
then CONCAT('		Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(double? ' , replace(column_name,'-','') , ');\n')
when data_type = 'float'  
then CONCAT('		Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(float? ' , replace(column_name,'-','') , ');\n')
when data_type = 'bit'  
then CONCAT('		Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(bool? ' , replace(column_name,'-','') , ');\n')
when data_type = 'date' or data_type = 'datetime' or data_type = 'time'
then CONCAT('		Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(DateTime? ' , replace(column_name,'-','') , ');\n')
when data_type = 'varchar'  or data_type = 'nvarchar' or data_type = 'char' or data_type = 'longtext' or data_type = 'text' or data_type = 'longblob'
then CONCAT('		Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(string ' , replace(column_name,'-','') , ');\n')
else CONCAT('		Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(',data_type,' ' , replace(column_name,'-','') , ');\n') end LineaCodigo
FROM INFORMATION_SCHEMA.COLUMNS Cols 
WHERE table_schema = '{0}' and table_name = '{1}' and COLUMN_KEY <> ''
ORDER BY  ordinal_position;";
                        sRepository = @"select 
CONCAT(
case 
when data_type = 'int' or data_type = 'smallint'
then CONCAT('		public async Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(int? _' , replace(column_name,'-','') , ')\n')
when data_type = 'bigint'   
then CONCAT('		public async Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(long? _' , replace(column_name,'-','') , ')\n')
when data_type = 'bit'   
then CONCAT('		public async Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(bool? _' , replace(column_name,'-','') , ')\n')
when data_type = 'decimal'  
then CONCAT('		public async Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(decimal? _' , replace(column_name,'-','') , ')\n')
when data_type = 'double'  
then CONCAT('		public async Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(double? _' , replace(column_name,'-','') , ')\n')
when data_type = 'float'  
then CONCAT('		public async Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(float? _' , replace(column_name,'-','') , ')\n')
when data_type = 'date' or data_type = 'datetime' or data_type = 'time'
then CONCAT('		public async Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(DateTime? _' , replace(column_name,'-','') , ')\n')
when data_type = 'varchar'  or data_type = 'nvarchar' or data_type = 'char' or data_type = 'longtext' or data_type = 'text' or data_type = 'longblob'
then CONCAT('		public async Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(string _' , replace(column_name,'-','') , ')\n')
else CONCAT('		public async Task<{1}> GetBy',replace(replace(column_name,'-',''),'_',''),'(',data_type,' _', replace(column_name,'-','') , ')\n') end,
'		{\n',
'            try\n',
'            {\n',
CONCAT('                {1} item = new {1}() { ',replace(column_name,'-',''),' = _',replace(column_name,'-',''),'};\n'),
'                var parameters = (object)DataHelpers.MappingParams(item);\n',
'                var storedName = ""spGetRecords"" + DataHelpers.GetItemName(item);\n',
'\n',
'                // Implementar la carga por REDIS\n',
'\n',
'                using (IDbConnection conn = dbConnection)\n',
'                {\n',
'                    conn.Open();\n',
CONCAT('                    var res = await conn.QueryFirstOrDefaultAsync<{1}>(storedName, param: parameters, commandType: CommandType.StoredProcedure);\n'),
'                    conn.Close();\n',
'                    return res;\n',
'                }\n',
'            }\n',
'            catch (Exception ex)\n',
'            {\n',
'                appLogger.LogError(ex);\n',
'            }\n',
'            return null;\n',
'		}\n') AS LineaCodigo
FROM INFORMATION_SCHEMA.COLUMNS Cols 
WHERE TABLE_NAME = '{1}' AND TABLE_SCHEMA = '{0}' and COLUMN_KEY <> ''
ORDER BY  ordinal_position;";
                        sOrder = @"
SELECT DISTINCT CONCAT('`',REPLACE(COLUMN_NAME,'-','_'),'`')
FROM INFORMATION_SCHEMA.COLUMNS Cols
WHERE table_schema = '{0}' and table_name = '{1}' and column_key <> ''";
                        sValores = @"
SELECT CONCAT('`@',column_name,'`')
FROM INFORMATION_SCHEMA.COLUMNS Cols
WHERE table_schema = '{0}' and table_name = '{1}'
ORDER BY ordinal_position";
                        sWhere = @"
SELECT CONCAT('(`@',REPLACE(COLUMN_NAME,'-','_'),'` IS NULL OR `',COLUMN_NAME,'` LIKE',
CASE WHEN (DATA_TYPE = 'nvarchar'  or DATA_TYPE = 'varchar' or DATA_TYPE = 'nchar' or DATA_TYPE = 'char') THEN '''%'' + ' ELSE '' END,
' `@',REPLACE(COLUMN_NAME,'-','_'),'`',
CASE WHEN (DATA_TYPE = 'nvarchar'  or DATA_TYPE = 'varchar' or DATA_TYPE = 'nchar' or DATA_TYPE = 'char') THEN ' + ''%''' ELSE '' END,
')')
FROM INFORMATION_SCHEMA.COLUMNS
WHERE table_schema = '{0}' and table_name = '{1}'
ORDER BY ordinal_position; ";
                        sParams = @"
SELECT 
CASE WHEN CHARACTER_MAXIMUM_LENGTH IS NULL
THEN CONCAT('IN `@',REPLACE(COLUMN_NAME,'-','_'),'` ',DATA_TYPE)
ELSE CONCAT('IN `@',REPLACE(COLUMN_NAME,'-','_'),'` ',DATA_TYPE,'(',CASE WHEN CHARACTER_MAXIMUM_LENGTH <=0 THEN 'MAX' ELSE CHARACTER_MAXIMUM_LENGTH END,')')
END 
FROM INFORMATION_SCHEMA.COLUMNS
WHERE table_schema = '{0}' and table_name = '{1}'
ORDER BY ordinal_position";
                        sSelect = @"
SELECT CONCAT('`',COLUMN_NAME,'`')
FROM INFORMATION_SCHEMA.COLUMNS
WHERE table_schema = '{0}' and table_name = '{1}'
ORDER BY  ordinal_position";
                        sStored = @"DELIMITER //
/*
* Elimina el STORED para traer el Index 
*/
DROP PROCEDURE IF EXISTS spGetIndex{0};//
/* 
* Crea el STORED para traer el Index 
*/
CREATE PROCEDURE spGetIndex{0} 
/*
*  Stored procedure para obtener:
*       + registros paginados (INDEX)
*   
*  Arhivo: spGetIndex{0}.sql
*  Creado: {1}
*  Autor: {2}
*  
*/
({3},
IN `@PageNo` int, 
IN `@RecordsPerPage` int) 
BEGIN
	Declare `@pagNo` int;
	Declare `@registros` int;

	set `@pagNo` = 0;
	set `@registros` = 0;

	set `@pagNo` = `@PageNo` - 1;
	set `@registros` = `@pagNo` * `@RecordsPerPage`;
	SELECT {5}
	FROM {0}
	WHERE {4}
    ORDER BY {6}
	LIMIT `@registros`, `@RecordsPerPage`;
END//

/* 
* Elimina el STORED para traer el Count 
*/
DROP PROCEDURE IF EXISTS spGetCount{0};//
/* 
* Crea el STORED para traer el Count 
*/
CREATE PROCEDURE spGetCount{0} 
/*
*  Stored procedure para obtener:
*       + registros totales (COUNT)
*   
*  Arhivo: spGetCount{0}.sql
*  Creado: {1}
*  Autor: {2}
*  
*/
({3})
BEGIN
	SELECT Count(*)
	FROM {0}
	WHERE {4};
END//

/* 
* Elimina el STORED para traer el(los) Registro(s)
*/
DROP PROCEDURE IF EXISTS spGetRecords{0};//
/* 
* Crea el STORED para traer el(los) Registro(s) 
*/
CREATE PROCEDURE spGetRecords{0} 
/*
*  Stored procedure para obtener:
*       + registros sin paginar (RECORDS)
*   
*  Arhivo: spGetRecords{0}.sql
*  Creado: {1}
*  Autor: {2}
*  
*/
({3})
BEGIN
	SELECT {5}
	FROM {0}
	WHERE {4}
    ORDER BY {6};
END//

/* 
* Elimina el STORED para insertar un registro
*/
DROP PROCEDURE IF EXISTS spInsRecord{0};//
/* 
* Crea el STORED para insertar un registro 
*/
CREATE PROCEDURE spInsRecord{0}(
/*
*  Stored procedure para insertar:
*       + registro en la tabla
*		+ manejo de errores
*   
*  Arhivo: spInsRecord{0}.sql
*  Creado: {1}
*  Autor: {2}
*  
*/
IN `@Campos` longtext,
IN `@Valores` longtext
) 
BEGIN
    DECLARE exit handler for SQLEXCEPTION
    BEGIN
      GET DIAGNOSTICS CONDITION 1 @ErrorSeverity = RETURNED_SQLSTATE, 
		    @ErrorNumber = MYSQL_ERRNO, 
		    @ErrorMessage = MESSAGE_TEXT;
	    SELECT @ErrorNumber,
		    @ErrorSeverity,
		    @ErrorMessage;
	    ROLLBACK;
        SELECT 0;
    END;

    START TRANSACTION;
    BEGIN
	    DECLARE `@Query` longtext;
	    SET `@Query` = CONCAT('INSERT INTO {0} (\n',`@Campos`,') \n VALUES(\n',`@Valores`,');');
	    PREPARE b FROM @Query;
	    execute b;
        COMMIT; 
        SELECT 1;
    END;
END//

/* 
* Elimina el STORED para actualizar uno o varios registros
*/
DROP PROCEDURE IF EXISTS spUpdRecord{0};//
/* 
* Crea el STORED para actualizar uno o varios registros
*/
CREATE PROCEDURE spUpdRecord{0}(
/*
*  Stored procedure para actualizar:
*       + registro en la tabla
*		+ manejo de transacción para no afectar datos
*		+ manejo de errores
*   
*  Arhivo: spUpdRecord{0}.sql
*  Creado: {1}
*  Autor: {2}
*  
*/
{3},
IN `@Valores` longtext
) 
BEGIN
    DECLARE exit handler for SQLEXCEPTION
    BEGIN
      GET DIAGNOSTICS CONDITION 1 @ErrorSeverity = RETURNED_SQLSTATE, 
		    @ErrorNumber = MYSQL_ERRNO, 
		    @ErrorMessage = MESSAGE_TEXT;
	    SELECT @ErrorNumber,
		    @ErrorSeverity,
		    @ErrorMessage;
	    ROLLBACK;
        SELECT 0;
    END;

    START TRANSACTION;
    BEGIN
	    DECLARE `@Query` longtext;
	    SET `@Query` = CONCAT('UPDATE {0} \n SET \n',`@Valores`,'\n WHERE \n{8};');
	    PREPARE b FROM @Query;
	    execute b;
        COMMIT; 
        SELECT 1;
    END;
END//

DELIMITER ;";
                        sStoredA = @"DELIMITER //
/*
* Elimina el STORED para traer el Index 
*/
DROP PROCEDURE IF EXISTS spGetIndex{0};//
/* 
* Crea el STORED para traer el Index 
*/
CREATE PROCEDURE spGetIndex{0} 
-- ============================================================================================================================
-- Author:		{2}
-- Create date: {1}
-- Description:	Obtiene registros paginados
-- ============================================================================================================================
-- Llamada: 
--		EXEC spGetIndex{0} @Afiliado = 'Test1001', @IdDestino = 1
-- ============================================================================================================================
-- History:                      
-- Date             Author              Description                      
-- -------------------------------------------------------------------------------                      
-- ============================================================================================================================
/*
*  Stored procedure para obtener:
*       + registros paginados (INDEX)
*   
*  Arhivo: spGetIndex{0}.sql
*  Creado: {1}
*  Autor: {2}
*  
*/
({3},
IN `@PageNo` int, 
IN `@RecordsPerPage` int) 
BEGIN
	Declare `@pagNo` int;
	Declare `@registros` int;

	set `@pagNo` = 0;
	set `@registros` = 0;

	set `@pagNo` = `@PageNo` - 1;
	set `@registros` = `@pagNo` * `@RecordsPerPage`;
	SELECT {5}
	FROM {0}
	WHERE {4}
    ORDER BY {6}
	LIMIT `@registros`, `@RecordsPerPage`;
END//

DELIMITER ;";
                        sStoredB = @"DELIMITER //
/* 
* Elimina el STORED para traer el Count 
*/
DROP PROCEDURE IF EXISTS spGetCount{0};//
/* 
* Crea el STORED para traer el Count 
*/
CREATE PROCEDURE spGetCount{0} 
/*
*  Stored procedure para obtener:
*       + registros totales (COUNT)
*   
*  Arhivo: spGetCount{0}.sql
*  Creado: {1}
*  Autor: {2}
*  
*/
({3})
BEGIN
	SELECT Count(*)
	FROM {0}
	WHERE {4};
END//

DELIMITER ;";
                        sStoredC = @"DELIMITER //
/* 
* Elimina el STORED para traer el(los) Registro(s)
*/
DROP PROCEDURE IF EXISTS spGetRecords{0};//
/* 
* Crea el STORED para traer el(los) Registro(s) 
*/
CREATE PROCEDURE spGetRecords{0} 
/*
*  Stored procedure para obtener:
*       + registros sin paginar (RECORDS)
*   
*  Arhivo: spGetRecords{0}.sql
*  Creado: {1}
*  Autor: {2}
*  
*/
({3})
BEGIN
	SELECT {5}
	FROM {0}
	WHERE {4}
    ORDER BY {6};
END//

DELIMITER ;";
                        sStoredD = @"DELIMITER //
/* 
* Elimina el STORED para insertar un registro
*/
DROP PROCEDURE IF EXISTS spInsRecord{0};//
/* 
* Crea el STORED para insertar un registro 
*/
CREATE PROCEDURE spInsRecord{0}(
/*
*  Stored procedure para insertar:
*       + registro en la tabla
*		+ manejo de errores
*   
*  Arhivo: spInsRecord{0}.sql
*  Creado: {1}
*  Autor: {2}
*  
*/
IN `@Campos` longtext,
IN `@Valores` longtext
) 
BEGIN
    DECLARE exit handler for SQLEXCEPTION
    BEGIN
      GET DIAGNOSTICS CONDITION 1 @ErrorSeverity = RETURNED_SQLSTATE, 
		    @ErrorNumber = MYSQL_ERRNO, 
		    @ErrorMessage = MESSAGE_TEXT;
	    SELECT @ErrorNumber,
		    @ErrorSeverity,
		    @ErrorMessage;
	    ROLLBACK;
        SELECT 0;
    END;

    START TRANSACTION;
    BEGIN
	    DECLARE `@Query` longtext;
	    SET `@Query` = CONCAT('INSERT INTO {0} (\n',`@Campos`,') \n VALUES(\n',`@Valores`,');');
	    PREPARE b FROM @Query;
	    execute b;
        COMMIT; 
        SELECT 1;
    END;
END//

DELIMITER ;";
                        sStoredE = @"DELIMITER //
/* 
* Elimina el STORED para actualizar uno o varios registros
*/
DROP PROCEDURE IF EXISTS spUpdRecord{0};//
/* 
* Crea el STORED para actualizar uno o varios registros
*/
CREATE PROCEDURE spUpdRecord{0}(
/*
*  Stored procedure para actualizar:
*       + registro en la tabla
*		+ manejo de transacción para no afectar datos
*		+ manejo de errores
*   
*  Arhivo: spUpdRecord{0}.sql
*  Creado: {1}
*  Autor: {2}
*  
*/
{3},
IN `@Valores` longtext
) 
BEGIN
    DECLARE exit handler for SQLEXCEPTION
    BEGIN
      GET DIAGNOSTICS CONDITION 1 @ErrorSeverity = RETURNED_SQLSTATE, 
		    @ErrorNumber = MYSQL_ERRNO, 
		    @ErrorMessage = MESSAGE_TEXT;
	    SELECT @ErrorNumber,
		    @ErrorSeverity,
		    @ErrorMessage;
	    ROLLBACK;
        SELECT 0;
    END;

    START TRANSACTION;
    BEGIN
	    DECLARE `@Query` longtext;
	    SET `@Query` = CONCAT('UPDATE {0} \n SET \n',`@Valores`,'\n WHERE \n',`@Valores`,';');
	    PREPARE b FROM @Query;
	    execute b;
        COMMIT; 
        SELECT 1;
    END;
END//

DELIMITER ;";
                        break;
                    #endregion
                    #region {Microsoft ODBC Driver for Oracle}
                    case "{Microsoft ODBC Driver for Oracle}":
                        if (string.IsNullOrEmpty(tTabla.Text))
                            sQuery = "select DISTINCT table_name Tabla, case when global_stats ='NO' then 'Vista' else 'Table' end Tipo from ALL_TAB_COLUMNS where owner = '" + tDataBase.Text + "' ORDER BY 1, 2";
                        else
                            sQuery = "select DISTINCT table_name Tabla, case when global_stats ='NO' then 'Vista' else 'Table' end Tipo from ALL_TAB_COLUMNS where owner = '" + tDataBase.Text + "' and table_name = '" + tTabla.Text + "' ORDER BY 1, 2";
                        sModel = @"
                                SELECT 
                                case 
                                when data_type = 'NUMBER' and (data_scale = 0 or data_scale is null) then '	    public int? ' || column_name || '  {get;set;}" + Environment.NewLine + @"'
                                when data_type = 'NUMBER' and (data_scale <> 0 or data_scale is not null) then '	    public decimal? ' || column_name || ' {get;set;}" + Environment.NewLine + @"'
                                when data_type = 'DATE' then '	    public DateTime? ' || column_name || ' {get;set;}" + Environment.NewLine + @"'
                                when data_type = 'TIMESTAMP' then '	    public DateTime? ' || column_name || ' {get;set;}" + Environment.NewLine + @"'
                                when data_type = 'VARCHAR2' then '	    public string ' || column_name || ' {get;set;}" + Environment.NewLine + @"'
                                else '' end
                                from ALL_TAB_COLUMNS where owner = '{0}' and table_name = '{1}'";
                        sInterfase = @"
                            select 
                            case 
                                when data_type = 'NUMBER' and (data_scale = 0 or data_scale is null) then '	[DataMember]		public int?  ' ||  column_name  ||  ' {get;set;}'" + Environment.NewLine + @"
                                when data_type = 'NUMBER' and (data_scale <> 0 or data_scale is not null) then '	[DataMember]		public decimal?  ' ||  column_name  ||  ' {get;set;}'" + Environment.NewLine + @"
                                when data_type = 'DATE' then '	[DataMember]		public DateTime?  ' ||  column_name  ||  ' {get;set;}'" + Environment.NewLine + @"
                                when data_type = 'TIMESTAMP' then '	[DataMember]		public DateTime?  ' ||  column_name  ||  ' {get;set;}'" + Environment.NewLine + @"
                                when data_type = 'VARCHAR2'then '	[DataMember]		public string  ' ||  column_name  ||  ' {get;set;}'" + Environment.NewLine + @"
                            else '' end
                            from ALL_TAB_COLUMNS where owner = '{0}' and table_name = '{1}'";
                        sRepository = @"
                            select 
                            case 
                                when data_type = 'NUMBER' and (data_scale = 0 or data_scale is null) then '	[DataMember]		public int?  ' ||  column_name  ||  ' {get;set;}'" + Environment.NewLine + @"
                                when data_type = 'NUMBER' and (data_scale <> 0 or data_scale is not null) then '	[DataMember]		public decimal?  ' ||  column_name  ||  ' {get;set;}'" + Environment.NewLine + @"
                                when data_type = 'DATE' then '	[DataMember]		public DateTime?  ' ||  column_name  ||  ' {get;set;}'" + Environment.NewLine + @"
                                when data_type = 'TIMESTAMP' then '	[DataMember]		public DateTime?  ' ||  column_name  ||  ' {get;set;}'" + Environment.NewLine + @"
                                when data_type = 'VARCHAR2'then '	[DataMember]		public string  ' ||  column_name  ||  ' {get;set;}'" + Environment.NewLine + @"
                            else '' end
                            from ALL_TAB_COLUMNS where owner = '{0}' and table_name = '{1}'";
                        sWhere = "";
                        sParams = "";
                        sSelect = "";
                        break;
                    #endregion
                }
                // Obtener las Tablas
                dbUtil.dbData.sConn = textBox1.Text;
                System.Data.Odbc.OdbcConnectionStringBuilder strConn = new System.Data.Odbc.OdbcConnectionStringBuilder();
                    strConn.Driver = tDrivers.Text;
                    strConn["Server"] = tServidor.Text;
                    strConn.Add("Uid", tUser.Text);
                    strConn.Add("Pwd", tPassword.Text);
                    strConn.Add("DataBase", tDataBase.Text);
                if (!string.IsNullOrEmpty(tPuerto.Text)) strConn["Port"] = tPuerto.Text;
                textBox1.Text = strConn.ConnectionString;
                DataTable oObjetos = new DataTable();
                if (tDrivers.Text == "{Microsoft ODBC Driver for Oracle}")
                {
                    sQuery = "select DISTINCT table_name Tabla, case when global_stats ='NO' then 'Vista' else 'Table' end Tipo from ALL_TAB_COLUMNS where owner = '" + tDataBase.Text + "' and table_name = '" + tTabla.Text + "' ORDER BY 1, 2";
                    textBox1.Text = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + ServiceName + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
                    dbUtilOracle.dbData.sConn = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + ServiceName + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
                    oObjetos = dbUtilOracle.dbData.GetData(sQuery);
                }
                else
                {
                    dbUtil.dbData.sConn = strConn.ConnectionString;
                    oObjetos = dbUtil.dbData.GetData(sQuery);
                }
                if (oObjetos != null)
                {
                    pbProgreso.Visible = true;
                    pbProgreso.Minimum = 0;
                    pbProgreso.Maximum = 100;
                    lProgreso.Visible = true;
                    lProgreso.Text = "Iniciando proceso";
                    Application.DoEvents();
                    // recorrer las tablas que se seleccionaron
                    foreach (DataRow oObjeto in oObjetos.Rows)
                    {
                        pbProgreso.Value = 0;
                        Tipo = oObjeto["Tipo"].ToString();
                        if (tDrivers.Text == "{SQL Server}")
                            Esquema = oObjeto["Esquema"].ToString();
                        else
                            Esquema = tDataBase.Text;
                        Tabla = oObjeto["Tabla"].ToString();
                        FileTabla = oObjeto["Tabla"].ToString();
                        Model = "";
                        Interfase = "";
                        Repository = "";
                        if (!sTablasEx.Contains(Tabla))
                        {
                            #region Carga Campos
                            // Obtener la lista de Model
                            DataTable oModel = new DataTable();
                            // Obtener la lista de Interfase
                            DataTable oInterfases = new DataTable();
                            // Obtener la lista de Repository
                            DataTable oRepositoriesKeys = new DataTable();
                            DataTable oRepositories = new DataTable();
                            // Obtener la lista de Order
                            DataTable oOrder = new DataTable();
                            // Obtener la lista de Valores
                            DataTable oValores = new DataTable();
                            // Obtener la lista de Where
                            DataTable oWhere = new DataTable();
                            // Obtener la lista de Params
                            DataTable oParams = new DataTable();
                            // Obtener la lista de Select
                            DataTable oSelect = new DataTable();
                            // Obtener la lista de Select
                            DataTable oCamposParam = new DataTable();
                            // Obtener la lista de Select
                            DataTable oValoresParam = new DataTable();
                            // Obtener la lista de Select
                            DataTable oParamsParams = new DataTable();
                            // Obtener los campos para habitar JS
                            DataTable oCamposHabilitar = new DataTable();
                            switch (tDrivers.Text)
                            {
                                case "{Microsoft ODBC Driver for Oracle}":
                                    oModel = dbUtilOracle.dbData.GetData(sModel.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oInterfases = dbUtilOracle.dbData.GetData(sInterfase.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oRepositories = dbUtilOracle.dbData.GetData(sRepository.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oOrder = dbUtilOracle.dbData.GetData(sOrder.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oValores = dbUtilOracle.dbData.GetData(sValores.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oWhere = dbUtilOracle.dbData.GetData(sWhere.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oParams = dbUtilOracle.dbData.GetData(sParams.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oSelect = dbUtilOracle.dbData.GetData(sSelect.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    break;
                                case "{MySQL ODBC 8.0 ANSI Driver}":
                                    oModel = dbUtil.dbData.GetData(sModel.Replace("{1}", Tabla).Replace("{0}", Esquema));
                                    oInterfases = dbUtil.dbData.GetData(sInterfase.Replace("{1}", Tabla).Replace("{0}", Esquema));
                                    oRepositories = dbUtil.dbData.GetData(sRepository.Replace("{1}", Tabla).Replace("{0}", Esquema));
                                    oOrder = dbUtil.dbData.GetData(sOrder.Replace("{1}", Tabla).Replace("{0}", Esquema));
                                    oValores = dbUtil.dbData.GetData(sValores.Replace("{1}", Tabla).Replace("{0}", Esquema));
                                    oWhere = dbUtil.dbData.GetData(sWhere.Replace("{1}", Tabla).Replace("{0}", Esquema));
                                    oParams = dbUtil.dbData.GetData(sParams.Replace("{1}", Tabla).Replace("{0}", Esquema));
                                    oSelect = dbUtil.dbData.GetData(sSelect.Replace("{1}", Tabla).Replace("{0}", Esquema));
                                    break;
                                default: /// {SQL Server} 
                                    oModel = dbUtil.dbData.GetData(sModel.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oInterfases = dbUtil.dbData.GetData(sInterfase.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oRepositoriesKeys = dbUtil.dbData.GetData(sRepositoryKeys.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oOrder = dbUtil.dbData.GetData(sOrder.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oValores = dbUtil.dbData.GetData(sValores.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oWhere = dbUtil.dbData.GetData(sWhere.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oParams = dbUtil.dbData.GetData(sParams.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oSelect = dbUtil.dbData.GetData(sSelect.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oCamposParam = dbUtil.dbData.GetData(sCamposParam.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oValoresParam = dbUtil.dbData.GetData(sValoresParam.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oParamsParams = dbUtil.dbData.GetData(sParamsParams.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    oCamposHabilitar = dbUtil.dbData.GetData(sCamposHabilitar.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                    break;
                            }
                            pbProgreso.Value = 35;
                            lProgreso.Text = "Leer base de datos";
                            Application.DoEvents();
                            #endregion
                            // Creando Archivos de Clases
                            try
                            {
                                #region Leyendo Campos
                                Model = string.Join("" + Environment.NewLine, oModel.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                pbProgreso.Value += 5;
                                lProgreso.Text = "Leyendo Modelo";
                                Application.DoEvents();
                                Interfase = string.Join("" + Environment.NewLine, oInterfases.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                oRepositoriesKeys = dbUtil.dbData.GetData(sRepository.Replace("{0}", Tabla).Replace("{1}", Esquema));
                                pbProgreso.Value += 5;
                                lProgreso.Text = "Leyendo Interface";
                                Application.DoEvents();
                                // Repository of Keys
                                foreach (DataRow key in oRepositoriesKeys.Rows)
                                {
                                    //Repository += key[0].ToString();
                                    oRepositories = dbUtil.dbData.GetData(sRepository.Replace("{0}", Tabla).Replace("{1}", Esquema).Replace("{2}", key["keys"].ToString()));
                                    Repository += string.Join(Environment.NewLine + Environment.NewLine, oRepositories.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                }
                                pbProgreso.Value += 5;
                                lProgreso.Text = "Leyendo Repositorio";
                                Application.DoEvents();
                                Where = string.Join(" AND " + Environment.NewLine, oWhere.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                pbProgreso.Value += 5;
                                lProgreso.Text = "Leyendo Order";
                                Application.DoEvents();
                                Order = string.Join(", " + Environment.NewLine, oOrder.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                pbProgreso.Value += 5;
                                lProgreso.Text = "Leyendo Valores";
                                Application.DoEvents();
                                Valores = string.Join(", " + Environment.NewLine, oValores.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                lProgreso.Text = "Leyendo Filtros";
                                Application.DoEvents();
                                Params = string.Join(", " + Environment.NewLine, oParams.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                pbProgreso.Value += 5;
                                lProgreso.Text = "Leyendo Parametros";
                                Application.DoEvents();
                                Select = string.Join(", ", oSelect.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                CamposParam = string.Join(", " + Environment.NewLine, oCamposParam.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                ValoresParam = string.Join(", ", oValoresParam.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                ParamsParams = string.Join(", " + Environment.NewLine, oParamsParams.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                                pbProgreso.Value += 5;
                                lProgreso.Text = "Leyendo Campos";
                                Application.DoEvents();
                                #endregion
                                #region Armado de Archivos
                                // Model
                                var path = Path.Combine(textBox3.Text, "Model");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                var fileName = Path.Combine(path, FileTabla + ".cs");
                                File.WriteAllText(fileName, GetModel(FileTabla, Model));
                                pbProgreso.Value += 5;
                                lProgreso.Text = "Creando Clase Modelo";
                                Application.DoEvents();
                                // Interface
                                path = Path.Combine(textBox3.Text, "Services");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                fileName = Path.Combine(path, "I" + FileTabla + "Service.cs");
                                File.WriteAllText(fileName, GetInterface(FileTabla, Interfase));
                                pbProgreso.Value += 5;
                                lProgreso.Text = "Creando Clase Interface";
                                Application.DoEvents();
                                // Repository
                                path = Path.Combine(textBox3.Text, "Repository");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                fileName = Path.Combine(path, FileTabla + "Repository.cs");
                                File.WriteAllText(fileName, GetRepository(FileTabla, Repository, tAddition.Text));
                                pbProgreso.Value += 5;
                                lProgreso.Text = "Creando Clase Repository";
                                Application.DoEvents();
                                #region Stored Procedures
                                // Stored Procedure
                                path = Path.Combine(textBox3.Text, "Querys");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);


                                fileName = Path.Combine(path, "Query_GetIndex" + FileTabla + ".sql");
                                if (tDrivers.Text == "{SQL Server}")
                                    File.WriteAllText(fileName, sStoredA
                                        .Replace("{0}", tDataBase.Text)
                                        .Replace("{1}", Tabla)
                                        .Replace("{2}", Creado)
                                        .Replace("{3}", tDatos.Text)
                                        .Replace("{4}", Params)
                                        .Replace("{5}", Select)
                                        .Replace("{6}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{7}", Order)
                                        .Replace("{8}", Valores)
                                        .Replace("{9}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{10}", tSchema.Text)
                                        .Replace("{11}", CamposParam));
                                if (tDrivers.Text == "{MySQL ODBC 8.0 ANSI Driver}")
                                    File.WriteAllText(fileName, sStoredA.Replace("{0}", Tabla).Replace("{1}", Creado).Replace("{2}", tDatos.Text).Replace("{3}", Params).Replace("{4}", Where).Replace("{5}", Select).Replace("{6}", Order).Replace("{7}", Valores).Replace("{8}", Where.Replace("'%'", "''%''")));


                                fileName = Path.Combine(path, "Query_GetCount" + FileTabla + ".sql");
                                if (tDrivers.Text == "{SQL Server}")
                                    File.WriteAllText(fileName, sStoredB
                                        .Replace("{0}", tDataBase.Text)
                                        .Replace("{1}", Tabla)
                                        .Replace("{2}", Creado)
                                        .Replace("{3}", tDatos.Text)
                                        .Replace("{4}", Params)
                                        .Replace("{5}", Select)
                                        .Replace("{6}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{7}", Order)
                                        .Replace("{8}", Valores)
                                        .Replace("{9}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{10}", tSchema.Text)
                                        .Replace("{11}", CamposParam));
                                if (tDrivers.Text == "{MySQL ODBC 8.0 ANSI Driver}")
                                    File.WriteAllText(fileName, sStoredB.Replace("{0}", Tabla).Replace("{1}", Creado).Replace("{2}", tDatos.Text).Replace("{3}", Params).Replace("{4}", Where).Replace("{5}", Select).Replace("{6}", Order).Replace("{7}", Valores).Replace("{8}", Where.Replace("'%'", "''%''")));


                                fileName = Path.Combine(path, "Query_GetRecords" + FileTabla + ".sql");
                                if (tDrivers.Text == "{SQL Server}")
                                    File.WriteAllText(fileName, sStoredC
                                        .Replace("{0}", tDataBase.Text)
                                        .Replace("{1}", Tabla)
                                        .Replace("{2}", Creado)
                                        .Replace("{3}", tDatos.Text)
                                        .Replace("{4}", Params)
                                        .Replace("{5}", Select)
                                        .Replace("{6}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{7}", Order)
                                        .Replace("{8}", Valores)
                                        .Replace("{9}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{10}", tSchema.Text)
                                        .Replace("{11}", CamposParam));
                                if (tDrivers.Text == "{MySQL ODBC 8.0 ANSI Driver}")
                                    File.WriteAllText(fileName, sStoredC.Replace("{0}", Tabla).Replace("{1}", Creado).Replace("{2}", tDatos.Text).Replace("{3}", Params).Replace("{4}", Where).Replace("{5}", Select).Replace("{6}", Order).Replace("{7}", Valores).Replace("{8}", Where.Replace("'%'", "''%''")));


                                fileName = Path.Combine(path, "Query_InsRecord" + FileTabla + ".sql");
                                if (tDrivers.Text == "{SQL Server}")
                                    File.WriteAllText(fileName, sStoredD
                                        .Replace("{0}", tDataBase.Text)
                                        .Replace("{1}", Tabla)
                                        .Replace("{2}", Creado)
                                        .Replace("{3}", tDatos.Text)
                                        .Replace("{4}", Params)
                                        .Replace("{5}", Select)
                                        .Replace("{6}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{7}", Order)
                                        .Replace("{8}", Valores)
                                        .Replace("{9}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{10}", tSchema.Text)
                                        .Replace("{11}", CamposParam.Replace("-", "_").Replace("__", "--"))
                                        .Replace("{12}", ValoresParam.Replace("-", "_").Replace("__", "--"))
                                        .Replace("{13}", ParamsParams.Replace("-", "_").Replace("__", "--")));
                                if (tDrivers.Text == "{MySQL ODBC 8.0 ANSI Driver}")
                                    File.WriteAllText(fileName, sStoredD.Replace("{0}", Tabla).Replace("{1}", Creado).Replace("{2}", tDatos.Text).Replace("{3}", Params).Replace("{4}", Where).Replace("{5}", Select).Replace("{6}", Order).Replace("{7}", Valores).Replace("{8}", Where.Replace("'%'", "''%''")));

                                fileName = Path.Combine(path, "Query_UpdRecord" + FileTabla + ".sql");
                                if (tDrivers.Text == "{SQL Server}")
                                    File.WriteAllText(fileName, sStoredE
                                        .Replace("{0}", tDataBase.Text)
                                        .Replace("{1}", Tabla)
                                        .Replace("{2}", Creado)
                                        .Replace("{3}", tDatos.Text)
                                        .Replace("{4}", Params)
                                        .Replace("{5}", Select)
                                        .Replace("{6}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{7}", Order)
                                        .Replace("{8}", Valores)
                                        .Replace("{9}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{10}", tSchema.Text)
                                        .Replace("{11}", CamposParam.Replace("-", "_").Replace("__", "--"))
                                        .Replace("{12}", ValoresParam.Replace("-", "_").Replace("__", "--"))
                                        .Replace("{13}", ParamsParams.Replace("-", "_").Replace("__", "--")));
                                if (tDrivers.Text == "{MySQL ODBC 8.0 ANSI Driver}")
                                    File.WriteAllText(fileName, sStoredE.Replace("{0}", Tabla).Replace("{1}", Creado).Replace("{2}", tDatos.Text).Replace("{3}", Params).Replace("{4}", Where).Replace("{5}", Select).Replace("{6}", Order).Replace("{7}", Valores).Replace("{8}", Where.Replace("'%'", "''%''")));

                                fileName = Path.Combine(path, "Query_DelRecord" + FileTabla + ".sql");
                                if (tDrivers.Text == "{SQL Server}")
                                    File.WriteAllText(fileName, sStoredF
                                        .Replace("{0}", tDataBase.Text)
                                        .Replace("{1}", Tabla)
                                        .Replace("{2}", Creado)
                                        .Replace("{3}", tDatos.Text)
                                        .Replace("{4}", Params)
                                        .Replace("{5}", Select)
                                        .Replace("{6}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{7}", Order)
                                        .Replace("{8}", Valores)
                                        .Replace("{9}", Where.Replace(" + '%' + ", "").Replace(" + '%'", ""))
                                        .Replace("{10}", tSchema.Text)
                                        .Replace("{11}", CamposParam.Replace("-", "_").Replace("__", "--")));
                                if (tDrivers.Text == "{MySQL ODBC 8.0 ANSI Driver}")
                                    File.WriteAllText(fileName, sStoredF.Replace("{0}", Tabla).Replace("{1}", Creado).Replace("{2}", tDatos.Text).Replace("{3}", Params).Replace("{4}", Where).Replace("{5}", Select).Replace("{6}", Order).Replace("{7}", Valores).Replace("{8}", Where.Replace("'%'", "''%''")));

                                pbProgreso.Value += 5;
                                lProgreso.Text = "Creando Stored Procedure";
                                Application.DoEvents();
                                #endregion
                                #region Armar MVC
                                // Armando MVC
                                path = Path.Combine(textBox3.Text, "JavaScript");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);

                                FileTabla = Tabla.Replace("_", "");
                                fileName = Path.Combine(path, FileTabla + ".js");
                                File.WriteAllText(fileName, GetJavaScript(Tabla, oRepositoriesKeys, oCamposHabilitar));
                                lProgreso.Text = "Creando JavaScript";
                                Application.DoEvents();
                                path = Path.Combine(textBox3.Text, "Views", FileTabla);
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                fileName = Path.Combine(path, "_Lista.cshtml");
                                File.WriteAllText(fileName, GetViewList(Tabla, oCamposHabilitar));
                                lProgreso.Text = "Creando _Lista";
                                Application.DoEvents();
                                path = Path.Combine(textBox3.Text, "Views", FileTabla);
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                fileName = Path.Combine(path, "_Loading.cshtml");
                                File.WriteAllText(fileName, GetViewLoading(Tabla, oCamposHabilitar));
                                lProgreso.Text = "Creando _Loading";
                                Application.DoEvents();
                                path = Path.Combine(textBox3.Text, "Views", FileTabla);
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                fileName = Path.Combine(path, "Index.cshtml");
                                File.WriteAllText(fileName, GetViewIndex(Tabla, oRepositoriesKeys));
                                lProgreso.Text = "Creando Index";
                                Application.DoEvents();
                                path = Path.Combine(textBox3.Text, "Controllers");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                fileName = Path.Combine(path, FileTabla + "Controller.cs");
                                File.WriteAllText(fileName, GetController(Tabla, oRepositoriesKeys));
                                pbProgreso.Value = 95;
                                lProgreso.Text = "Creando Controller";
                                Application.DoEvents();
                                #endregion
                                // Transient
                                fileName = Path.Combine(textBox3.Text, "SetupTransient.txt");
                                File.AppendAllText(fileName, "services.AddTransient<I" + Tabla + "Service, " + Tabla + "Repository>();" + Environment.NewLine);
                                // Query CMD
                                fileName = Path.Combine(textBox3.Text, "ExeQuery.bat");
                                File.AppendAllText(fileName, $@"sqlcmd -S {tServidor.Text} -U {tUser.Text} -d {tDataBase.Text} -P ""{tPassword.Text}"" -i .\Querys\Query_GetIndex{FileTabla}.sql{Environment.NewLine}");
                                File.AppendAllText(fileName, $@"sqlcmd -S {tServidor.Text} -U {tUser.Text} -d {tDataBase.Text} -P ""{tPassword.Text}"" -i .\Querys\Query_GetCount{FileTabla}.sql{Environment.NewLine}");
                                File.AppendAllText(fileName, $@"sqlcmd -S {tServidor.Text} -U {tUser.Text} -d {tDataBase.Text} -P ""{tPassword.Text}"" -i .\Querys\Query_GetRecords{FileTabla}.sql{Environment.NewLine}");
                                File.AppendAllText(fileName, $@"sqlcmd -S {tServidor.Text} -U {tUser.Text} -d {tDataBase.Text} -P ""{tPassword.Text}"" -i .\Querys\Query_InsRecord{FileTabla}.sql{Environment.NewLine}");
                                File.AppendAllText(fileName, $@"sqlcmd -S {tServidor.Text} -U {tUser.Text} -d {tDataBase.Text} -P ""{tPassword.Text}"" -i .\Querys\Query_UpdRecord{FileTabla}.sql{Environment.NewLine}");
                                File.AppendAllText(fileName, $@"sqlcmd -S {tServidor.Text} -U {tUser.Text} -d {tDataBase.Text} -P ""{tPassword.Text}"" -i .\Querys\Query_DelRecord{FileTabla}.sql{Environment.NewLine}");
                                pbProgreso.Value = 100;
                                lProgreso.Text = "Proceso terminado";
                                Application.DoEvents();
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    this.Cursor = Cursors.Default;
                    lProgreso.Text = "Completado 100%";
                    Application.DoEvents();
                    MessageBox.Show("Proceso Terminado");
                    pbProgreso.Visible = false;
                    lProgreso.Visible = false;
                    Application.Exit();
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.ToString());
            }
        }

        private string GetModel(string Tabla, string Model)
        {
            return @"/*
*  Modelo de datos para una Tabla
*
*  Arhivo: " + Tabla + @".cs
*  Creado: " + Creado + @"
*  Autor: " + tDatos.Text + @" 
*  
*/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace " + NameSpace + @".Entities.Models
{
    public partial class " + Tabla + @"
    {
" + Model + @"
    }   
}";            
        }

        private string GetInterface(string Tabla, string Interface)
        {
            return @"/*
*  Interface de servicios para una Tabla
*
*  Arhivo: I" + Tabla + @"Service.cs
*  Creado: " + Creado + @"
*  Autor: " + tDatos.Text + @" 
*  
*/

using " + NameSpace + @".Domain.Interfaces;
using " + NameSpace + @".Entities.Models;
using System.Threading.Tasks;

namespace " + NameSpace + @".Domain.Services
{
    public interface I" + Tabla + @"Service : IAsyncRepository<" + Tabla + @">
    {
" + Interface + @"
	}
}";
        }

        private string GetRepository(string Tabla, string Repository, string addition = "")
        {
            return @"/*
*  Repositorio de datos para las llaves e index de la tabla " + Tabla.ToUpper() + @"
*
*  Arhivo: " + Tabla + @"Repository.cs
*  Creado: " + Creado + @"
*  Autor: " + tDatos.Text + @" 
*  
*/

using " + NameSpace + @".Domain.Interfaces;
using " + NameSpace + @".Domain.Services;
using " + NameSpace + @".Entities.Models;
using " + NameSpace + @".Helpers;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace " + NameSpace + @".DataAccess.Repositories
{
    public class " + Tabla + @"Repository : DataRepository" + addition + @"<" + Tabla + @">, I" + Tabla + @"Service
    {
        private readonly IAppConfig config;
        private readonly IAppLogger<" + Tabla + @"> appLogger;
        private readonly ICacheData<" + Tabla + @"> cacheData;
        private readonly string[] aEntitiesCache;

        public " + Tabla + @"Repository(
            IAppConfig _config, 
            IAppLogger<" + Tabla + @"> _logger, 
            ICacheData<" + Tabla + @"> _cacheData) : base(_config, _logger, _cacheData)
        {
            config = _config;
            appLogger = _logger;
            cacheData = _cacheData;
            aEntitiesCache = config.CacheEntities().Split('|');
        }

" + Repository + @"
    }
}";
        }

#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable S2479 // Whitespace and control characters in string literals should be explicit
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
        private string GetJavaScript(string Tabla, DataTable RepositoriesKeys, DataTable CamposHabilitar)
        {
            // Armar las llaves de la tabla
            var keys = RepositoriesKeys.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
            // Validar para el plural de la tabla 
            string CampoPlural;
            string Campo = Tabla.Split('_')[Tabla.Split('_').Count() - 1];
            var aVocales = new string[] { "a", "e", "i", "o", "u" };
            if (aVocales.Contains(Campo.ToLower().Substring(Campo.Length - 1, 1)))
                CampoPlural = $"{Campo}s";
            else if (Campo.ToLower().Substring(Campo.Length - 1, 1) == "s")
                CampoPlural = Campo;
            else
                CampoPlural = $"{Campo}es";
            // Armar el filtro
            var keysFilter = "";
            var paramFilter = "";
            var blockFilters = "";
            var enableFilters = "";
            var formDataDelete = "";
            foreach (string key in keys)
            {
                keysFilter += $"$('#{key.Split('_')[key.Split('_').Count() - 1]}').val() === '' && ";
                paramFilter += $@"                        {key}: $('#{key.Split('_')[key.Split('_').Count() - 1]}').val() === '' ? null : $('#{key.Split('_')[key.Split('_').Count() - 1]}').val(),
";
                blockFilters += $@"
            $('#{key}').prop('disabled', true);
            $('#{key}').css('cursor', 'no-drop');
";
                enableFilters += $@"
            $('#{key}').prop('disabled', false);
            $('#{key}').css('cursor', 'default');
";
                formDataDelete += $@"                        formData.append(""{key}"", $('#{key.Split('_')[key.Split('_').Count() - 1]}').val());
";
            }
            var showCampos = "";
            var hideCampos = "";
            var newCampos = "";
            var validCampos = "";
            var formDataCampos = "";
            foreach (DataRow key in CamposHabilitar.Rows)
            {
                if (key["Tipo"].ToString() == "1")
                {
                    showCampos += $@"
                $(`#Label_{key["Campo"].ToString()}_${"{"}{CampoPlural}{"}"}`).hide();
                $(`#Input_{key["Campo"].ToString()}_${"{"}{CampoPlural}{"}"}`).show();
";
                    hideCampos += $@"
                $(`#Label_{key["Campo"].ToString()}_${"{"}{CampoPlural}{"}"}`).show();
                $(`#Input_{key["Campo"].ToString()}_${"{"}{CampoPlural}{"}"}`).hide();
";
                    newCampos += $@"
                <td scope=""row"" column=""{key["Campo"].ToString()}"" >
                    <div id=""Label_{key["Campo"].ToString()}_New"" class=""hide""></div>
                    <input id=""Input_{key["Campo"].ToString()}_New"" type=""text"" class=""form-control form-control-sm"" placeholder=""${"{"}SharedLocalizer.{key["Campo"].ToString()}{"}"}"" required>
                </td>
";
                    validCampos += $@"
            $(`#Input_{key["Campo"]}_${"{"}id{Campo}{"}"}`).removeClass('is-invalid');
            if ($(`#Input_{key["Campo"]}_${"{"}id{Campo}{"}"}`).val() === '') {"{"}
                $(`#Input_{key["Campo"]}_${"{"}id{Campo}{"}"}`).addClass('is-invalid');
                valid = false;
            {"}"}
";
                    formDataCampos += $@"            formData.append('{key["Campo"]}', $(`#Input_{key["Campo"]}_${"{"}id{Campo}{"}"}`).val());
";
                }
                else
                {
                    showCampos += $@"                $(`#{key["Campo"].ToString()}_${"{"}{CampoPlural}{"}"}`).prop('disabled', false);
";
                    hideCampos += $@"                $(`#{key["Campo"].ToString()}_${"{"}{CampoPlural}{"}"}`).prop('disabled', true);
";
                    newCampos += $@"
                <td scope=""row"" column=""{key["Campo"].ToString()}"" >
                    <div class=""custom-control custom-switch"">
                        <input id=""{key["Campo"].ToString()}_New"" name=""Es_Binario_New"" type=""checkbox"" class=""custom-control-input"">
                        <label for=""{key["Campo"].ToString()}_New"" class=""custom-control-label""></label>
                    </div>
                </td>
";
                    formDataCampos += $@"            formData.append('{key["Campo"].ToString()}', $(`#{key["Campo"].ToString()}_${"{"}id{Campo}{"}"}`).is(':checked'));
";
                }
            }
            return $@"/*
*  Javascript Controller para un ABC de la tabla {Tabla}
*
*  Arhivo: {Tabla}.js
*  Creado: {Creado}
*  Autor: {tDatos.Text} 
*  
*/

var pl = {"{"}
    ctrl: {"{"}{"}"}
{"}"},
    goFilter = null,
    LoadingPage = '',
    idEdit = '';

$(function () {"{"}
    pl.ctrl.{CampoPlural} = {"{"}
        init: function () {"{"}
            goFilter = null;
            LoadingPage = $('#LoadingList').html();
            $('.BtnFiltro').on('click', function () {"{"}
                pl.ctrl.{CampoPlural}.Filtrar();
            {"}"});
            this.GotoPage(1);
        {"}"},
        GotoPage: function (goPage) {"{"}
            var sendData = {"{"}
                page: goPage === null ? 1 : goPage,
                filtro{CampoPlural}: goFilter === null ? null : JSON.stringify(goFilter)
            {"}"};
            RequestView({"{"}
                url: '/{CampoPlural}/GetLista',
                dataSend: sendData,
                fnBeforeSend: function () {"{"} // BeforeSend
                    $('#{CampoPlural}List').html(LoadingPage);
                {"}"},
                fnAfterSend: function () {"{"} // AfterSend
                    $('#{CampoPlural}List').html(LoadingPage);
                {"}"},
                fnSuccess: function (response) {"{"} // Success
                    if (response) {"{"}
                        $('#{CampoPlural}List').html(response);
                        $('#currPage').val(sendData.page);
                        pagination.init();
                        pl.ctrl.{CampoPlural}.initBotones();
                        window.parent.postMessage($('html').height(), ""*"");
                        // Quitar o poner el filtro
                        if ($('#TotalRecords').val() <= 10) {"{"}
                            $('#Filtro').addClass('hide');
                            goFilter = {"{"}{"}"};
                        {"}"} else {"{"}
                            $('#Filtro').removeClass('hide');
                        {"}"}
                    {"}"} else {"{"}
                        Alert(SharedLocalizer.msgErr0001, severidad.danger); // Error no trajo la lista
                    {"}"}
                {"}"},
                fnError: function (response) {"{"} // Error de base de datos
                    if(response)
                        console.error(response);
                    Alert(SharedLocalizer.msgErr0003, severidad.danger);
                {"}"}
            {"}"});
        {"}"},
        Filtrar: function () {"{"}
            if ({keysFilter} goFilter === null) {"{"}
                Alert(SharedLocalizer.msgErr0002, severidad.warning);
            {"}"}
            else {"{"}
                if ({keysFilter} goFilter !== null) {"{"}
                    goFilter = null;
                {"}"} else {"{"}
                    goFilter = {"{"}
{paramFilter}
                    {"}"};
                {"}"}
                pl.ctrl.{CampoPlural}.GotoPage(1);
            {"}"}
        {"}"},
        initBotones: function () {"{"}
            $('.BtnEdit').on(""click"", function () {"{"}
                var {Campo} = $(this).data(""info"");    
                if ({Campo} !== idEdit) {"{"}
                    $(`#GroupSave_${"{"}idEdit{"}"} .BtnCancel`).click();                        
                {"}"}
                idEdit = {Campo};

                pl.ctrl.{CampoPlural}.blockButtons();
{showCampos}
                $(`#GroupEdit_${"{"}{Campo}{"}"}`).hide();
                $(`#GroupSave_${"{"}{Campo}{"}"}`).show();

                $(`#GroupSave_${"{"}{Campo}{"}"} .BtnSave`).on(""click"", function () {"{"}
                    var {Campo} = $(this).data(""info"");
                    Confirm({"{"} Question: `${"{"}SharedLocalizer.AlertMessage05{"}"}<br>`, Title: `${"{"}SharedLocalizer.Edit{"}"} ${"{"}SharedLocalizer.Operation{"}"}`, Severity: severidad.warning {"}"})
                        .then(function (answer) {"{"}
                            if (answer) {"{"}
                                pl.ctrl.{CampoPlural}.SaveData(2, {Campo});
                            {"}"} else {"{"}
                                $(`#GroupSave_${"{"}{Campo}{"}"} .BtnCancel`).click();
                            {"}"}
                        {"}"});
                {"}"});
            {"}"});

            $('.BtnCancel').on(""click"", function (e) {"{"}
                e.preventDefault;
                var {Campo} = $(this).data(""info"");

                pl.ctrl.{CampoPlural}.enableButtons();

{hideCampos}

                // Quitar la funcionalidad del boton Guardar
                $(`#GroupSave_${"{"}{Campo}{"}"} .BtnSave`).unbind('click');
                $(`#GroupEdit_${"{"}{Campo}{"}"}`).show();
                $(`#GroupSave_${"{"}{Campo}{"}"}`).hide();
            {"}"});

            $('.BtnAgregar').on(""click"", function (e) {"{"}
                e.preventDefault;
                $('table#Tabla{CampoPlural} tr#NewRow').remove();
                pl.ctrl.{CampoPlural}.Create();
            {"}"});

            $('.BtnDelete').on(""click"", function (e) {"{"}
                e.preventDefault;
                var {Campo} = $(this).data(""info"");
                if ({Campo} !== idEdit) {"{"}
                    $(`#GroupSave_${"{"}idEdit{"}"} .BtnCancel`).click();
                {"}"}
                idEdit = """";
                pl.ctrl.{CampoPlural}.Delete({Campo});
            {"}"});
        {"}"},
        blockButtons: function () {"{"}
            $('.BtnAgregar').prop('disabled', true);
            $('.BtnAgregar').css('cursor', 'no-drop');

            $('.BtnFiltro').prop('disabled', true);
            $('.BtnFiltro').css('cursor', 'no-drop');

            $('.BtnFiltro').prop('disabled', true);
            $('.BtnFiltro').css('cursor', 'no-drop');

            $('#{Campo}').prop('disabled', true);
            $('#{Campo}').css('cursor', 'no-drop');

            // Bloquea campos filtro
{blockFilters}
        {"}"},
        enableButtons: function () {"{"}
            $('.BtnAgregar').prop('disabled', false);
            $('.BtnAgregar').css('cursor', 'default');

            $('.BtnFiltro').prop('disabled', false);
            $('.BtnFiltro').css('cursor', 'default');

            $('.BtnFiltro').prop('disabled', false);
            $('.BtnFiltro').css('cursor', 'default');

            $('#{Campo}').prop('disabled', false);
            $('#{Campo}').css('cursor', 'default');

            // Habilita campos filtro
{enableFilters}
        {"}"},
        Create: function () {"{"}
            if (idEdit !== 'New') {"{"}
                $(`#GroupSave_${"{"}idEdit{"}"} .BtnCancel`).click();                        
            {"}"}
            $('#Tabla{CampoPlural} > tbody').prepend(`
            <tr id=""NewRow"">
{newCampos}
                <td scope=""row"">
                    <div id=""GroupEdit_New"" class=""btn-group btn-group-sm"" style=""display:none"">
                        <button type=""button"" data-info=""New"" class=""BtnEdit btn btn-primary"">${"{"}SharedLocalizer.Edit{"}"}</button>
                        <button type=""button"" data-info=""New"" class=""BtnDelete btn btn-danger"" disabled style=""cursor: no-drop; display: none;"">${"{"}SharedLocalizer.Delete{"}"}</button>
                    </div>
                    <div id=""GroupSave_New"" class=""btn-group btn-group-sm"">
                        <button type=""button"" data-info=""New"" class=""BtnSave btn btn-primary"">${"{"}SharedLocalizer.Save{"}"}</button>
                        <button type=""button"" data-info=""New"" class=""BtnCancel btn btn-danger"">${"{"}SharedLocalizer.Cancel{"}"}</button>
                    </div>
                    <button id=""BtnSaveLoading_New"" type=""button"" data-info=""New"" class=""btnLoading btn btn-primary btn-sm"" style=""display:none"">
                        <span class=""spinner-border spinner-border-sm"" role=""status"" aria-hidden=""true""></span>
                        ${"{"}SharedLocalizer.Saving{"}"}...
                    </button>
                </td>
            </tr>`);
            pl.ctrl.{CampoPlural}.blockButtons();
            $('#GroupSave_New .BtnSave').on(""click"", function () {"{"}
                Confirm({"{"} Question: `${"{"}SharedLocalizer.AlertMessage04{"}"}<br>`, Title: `${"{"}SharedLocalizer.Add{"}"} ${"{"}SharedLocalizer.Operation{"}"}`, Severity: severidad.warning {"}"})
                    .then(function (answer) {"{"}
                        if (answer) {"{"}
                            pl.ctrl.{CampoPlural}.SaveData(1, 'New');
                        {"}"} else {"{"}
                            $('#GroupSave_New .BtnCancel').click();
                        {"}"}
                    {"}"});
            {"}"});
            $('#GroupSave_New .BtnCancel').on(""click"", function () {"{"}
                $('table#Tabla{CampoPlural} tr#NewRow').remove();
                pl.ctrl.{CampoPlural}.enableButtons();
            {"}"});
            idEdit = 'New';
        {"}"},
        Delete: function (Clav_{Campo}) {"{"}
            Confirm({"{"} Question: `${"{"}SharedLocalizer.AlertMessage03{"}"}<br>`, Title: SharedLocalizer.Operation, Severity: severidad.danger {"}"})
                .then(function (answer) {"{"}
                    if (answer) {"{"}
                        var formData = new FormData();
{formDataDelete}
                        AjaxArgs = {"{"}
                            url: '/{CampoPlural}/Delete',
                            dataSend: formData,
                            fnBeforeSend: function () {"{"} // BeforeSend
                                $('#{CampoPlural}List').html(LoadingPage);
                            {"}"},
                            fnAfterSend: function () {"{"} // AfterSend
                                $('#{CampoPlural}List').html(LoadingPage);
                            {"}"},
                            fnSuccess: function (response) {"{"} // Success
                                if (response) {"{"}
                                    Alert(response.mensaje, severidad.success);
                                {"}"}
                                pl.ctrl.GotoPage($('#currPage').val());
                            {"}"},
                            fnError: function (response) {"{"} // Error de base de datos
                                if (response)
                                    console.error(response);
                                Alert(SharedLocalizer.msgErr0003, severidad.danger);
                            {"}"}
                        {"}"};
                        SendForm(AjaxArgs).done(function (response) {"{"} // Success
                            if (response.exito) {"{"}
                                site.Alert(response.mensaje, severidad.success);
                                setTimeout(
                                    function () {"{"}
                                        pl.ctrl.{CampoPlural}.GotoPage($('#currPage').val());
                                    {"}"}, 5500);
                            {"}"}
                            else {"{"}
                                site.Alert(response.mensaje, severidad.warning);
                            {"}"}
                        {"}"});
                    {"}"}
                    return false;
                {"}"});
        {"}"},
        SaveData: function (Action, id{Campo}) {"{"} 
            // Validar que los campos contengan información
            var valid = true;
{validCampos}
            if (!valid) {"{"}
                Alert(SharedLocalizer.AlertMessage01, severidad.danger);
                pl.ctrl.{CampoPlural}.BtnLoading(Action, id{Campo});
                return;
            {"}"}
            var formData = new FormData();
{formDataCampos}
            if (Action !== 1) {"{"}
                formData.append('Clav_{Campo}_old', $(`#Input_Clave_{Campo}_Old_${"{"}id{Campo}{"}"}`).val());
            {"}"}
            SendForm({"{"}
                url: Action === 1 ? '/{CampoPlural}/Guardar' : '/{CampoPlural}/Actualizar',
                dataSend: formData,
                fnBeforeSend: function () {"{"} // BeforeSend
                    if (Action === 1) {"{"}
                        $(`#BtnSaveLoading_New`).show();
                        $(`#GroupSave_New`).hide();
                    {"}"} else {"{"}
                        $(`#BtnSaveLoading_${"{"}id{Campo}{"}"}`).show();
                        $(`#GroupSave_${"{"}id{Campo}{"}"}`).hide();
                    {"}"}
                {"}"},
                fnAfterSend: function () {"{"} // AfterSend
                {"}"},
                fnSuccess: function (response) {"{"} // Success
                    if (response.exito) {"{"}
                        Alert(response.mensaje, severidad.success); // Exito se creo el registro
                    {"}"} else {"{"}
                        Alert(response.mensaje, severidad.danger); // Error no se creo el registro
                    {"}"}
                    pl.ctrl.{CampoPlural}.enableButtons();
                    pl.ctrl.{CampoPlural}.BtnLoading(Action, id{Campo});
                    pl.ctrl.{CampoPlural}.GotoPage($('#currPage').val());
                {"}"},
                fnError: function (response) {"{"} // Error de base de datos
                    pl.ctrl.{CampoPlural}.BtnLoading(Action, id{Campo});
                    Alert(SharedLocalizer.msgErr0003, severidad.danger);
                {"}"}
            {"}"});
            return false;
        {"}"},
        BtnLoading: function (Action, id{Campo}) {"{"}
            if (Action === '1') {"{"}
                $(`#BtnSaveLoading_New`).hide();
                $(`#GroupSave_New`).show();
            {"}"} else {"{"}
                $(`#BtnSaveLoading_${"{"}id{Campo}{"}"}`).hide();
                $(`#GroupSave_${"{"}id{Campo}{"}"}`).show();
            {"}"}
        {"}"}
    {"}"};
    pl.ctrl.{CampoPlural}.init();
{"}"});";
        }

        private string GetController(string Tabla, DataTable RepositoriesKeys)
        {
            // Armar las llaves de la tabla
            var keys = RepositoriesKeys.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
            // Validar para el plural de la tabla 
            string Campo = keys[0];
            // Armar el filtro
            var keysFilter = "";
            foreach (string key in keys)
            {
                keysFilter += $@"
                    {key} = DataHelpers.GetLikeAll(filter.{key}),";
            }
            return $@"/*
*  Arhivo: {Tabla}Controller.cs
*  Creado: {Creado}
*  Autor: {tDatos.Text}
*/
using BDTG.SelfService.Domain.Interfaces;
using BDTG.SelfService.Domain.Services;
using BDTG.SelfService.Entities.Models;
using BDTG.SelfService.Entities.Others;
using BDTG.SelfService.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BDTG.SelfService.WebApp.Controllers
{"{"}
    public class {Tabla}Controller : Controller
    {"{"}
        private readonly I{Tabla}Service {Tabla}Repo;
        private readonly IAsociados_Usuarios_AfiliadosService UsuariosRepo;
        private readonly IAppLogger<{Tabla}Controller> appLogger;
        private readonly IStringLocalizer<SharedResources> SharedLocalizer;

        public {Tabla}Controller(I{Tabla}Service _{Tabla}Repo,
            IAsociados_Usuarios_AfiliadosService _UsuariosRepo,
            IAppLogger<{Tabla}Controller> _appLogger,
            IStringLocalizer<SharedResources> _sharedLocalizer)
        {"{"}
            {Tabla}Repo = _{Tabla}Repo;
            UsuariosRepo = _UsuariosRepo;
            appLogger = _appLogger;
            SharedLocalizer = _sharedLocalizer;
        {"}"}

        // GET: {Tabla}
        public ActionResult Index()
        {"{"}
            ViewData[""TitMenu""] = ""Catálogo de {Tabla}"";
            var userIdentity = User.Identity as ClaimsIdentity;
            ViewData[""DomainOrigin""] = EncriptHelper.Decode64Binary(userIdentity.FindFirst(""Origin"").ToString().Replace(""Origin: "", """"));
            return View();
        {"}"}

        // GET: {Tabla}/GetLista
        public async Task<IActionResult> GetLista(int? page, string filtro{Tabla})
        {"{"}
            try
            {"{"}
                var filter = filtro{Tabla} == null ? new {Tabla}() : JsonConvert.DeserializeObject<{Tabla}>(filtro{Tabla});
                var pagNo = page ?? 1;
                var RecordsPage = 10;
                int Registros = 0;

                ViewData[""Action""] = ""1"";
                ViewData[""TitMenu""] = ""{Tabla}"";
                var userIdentity = User.Identity as ClaimsIdentity;
                ViewData[""DomainOrigin""] = EncriptHelper.Decode64Binary(userIdentity.FindFirst(""Origin"").ToString().Replace(""Origin: "", """"));
                filter = new {Tabla}()
                {"{"}
{keysFilter}
                {"}"};
                Registros = await {Tabla}Repo.GetAllRecords(filter);
                // Datos para el paginador
                ViewData[""Registros""] = string.Format(""{"{"}0:#,#0{"}"}"", Registros) + SharedLocalizer[""TotalRecords""];
                ViewData[""Paginator""] = new Paginator()
                {"{"}
                    currentPage = pagNo,
                    totalItems = Registros,
                    pageSize = RecordsPage,
                    fnScript = ""pl.ctrl.{Tabla}.GotoPage""
                {"}"};
                return PartialView(""_Lista"", await {Tabla}Repo.GetAllPaging(filter, pagNo, RecordsPage));
            {"}"}
            catch (Exception ex)
            {"{"}
                appLogger.LogError(ex);
                return View(""/Home/Error"");
            {"}"}
        {"}"}

        // POST: {Tabla}/Create
        [HttpPost]
        [Produces(""application/json"")]
        public async Task<IActionResult> Guardar({Tabla} Registro)
        {"{"}
            Respuesta response = new Respuesta();
            try
            {"{"}
                if (ModelState.IsValid)
                {"{"}
                    if (Registro != null)
                    {"{"}
                        if (await {Tabla}Repo.SetDataAsync(Registro, null, Entities.Enums.TipoQueryEnum.TipoQuery.Insert))
                        {"{"}
                            Registro.fAlta = DateTime.Now;
                            Registro.IdUsuarioAlta = Usuario;
                            response.Mensaje = SharedLocalizer[""CreateRecord""];
                            response.Exito = true;
                        {"}"}
                        else
                            response.Mensaje = SharedLocalizer[""CannotCreateRecord""];
                    {"}"}
                    else
                        response.Mensaje = SharedLocalizer[""NoInformationReceived""];
                {"}"}
                else
                    response.Mensaje = SharedLocalizer[""AlertMessage01""];
            {"}"}
            catch (Exception ex)
            {"{"}
                appLogger.LogError(ex);
                response.Mensaje = SharedLocalizer[""CannotCreateRecord""];
            {"}"}
            return Json(response);
        {"}"}

        // POST: {Tabla}/Edit/5
        [HttpPost]
        [Produces(""application/json"")]
        public async Task<IActionResult> Actualizar({Tabla} Registro, string Clav_{Campo}_Old)
        {"{"}
            Respuesta response = new Respuesta();
            try
            {"{"}
                if (ModelState.IsValid)
                {"{"}
                    if (Registro != null)
                    {"{"}
                        if ((await {Tabla}Repo.SetDataAsync(Registro, new {Tabla}() {"{"} Clav_{Campo} = Clav_{Campo}_Old {"}"}, Entities.Enums.TipoQueryEnum.TipoQuery.Update)))
                        {"{"}
                            response.Mensaje = SharedLocalizer[""RegistrationUpdated""];
                            response.Exito = true;
                        {"}"}
                        else
                            response.Mensaje = SharedLocalizer[""UnableToUpdateTheRecord""];
                    {"}"}
                    else
                        response.Mensaje = SharedLocalizer[""NoInformationReceived""];

                {"}"}
                else
                    response.Mensaje = SharedLocalizer[""AlertMessage01""];
            {"}"}
            catch (Exception ex)
            {"{"}
                response.Mensaje = SharedLocalizer[""ErrorValidateData""];
                appLogger.LogError(ex);
            {"}"}
            return Json(response);
        {"}"}

        // POST: {Tabla}/Delete/5
        [HttpPost]
        [Produces(""application/json"")]
        public async Task<IActionResult> Delete({Tabla} Registro)
        {"{"}
            Respuesta response = new Respuesta();
            try
            {"{"}
                if (ModelState.IsValid)
                {"{"}
                    if (Registro != null)
                    {"{"}
                        if (await {Tabla}Repo.SetDataAsync(Registro, null, Entities.Enums.TipoQueryEnum.TipoQuery.Delete))
                        {"{"}
                            response.Mensaje = SharedLocalizer[""RecordDelete""];
                            response.Exito = true;
                        {"}"}
                        else
                            response.Mensaje = SharedLocalizer[""UnableToDeleteTheRecord""];
                    {"}"}
                    else
                        response.Mensaje = SharedLocalizer[""NoInformationReceived""];
                {"}"}
                else
                    response.Mensaje = SharedLocalizer[""AlertMessage01""];
            { "}"}
            catch(Exception ex)
            {"{"}
                response.Mensaje = SharedLocalizer[""UnableToDeleteTheRecord""];
                appLogger.LogError(ex);
            {"}"}
            return Json(response);
        {"}"}
    {"}"}
{"}"}";
        }

        private string GetViewList(string Tabla, DataTable CamposHabilitar)
        {
            // Validar para el plural de la tabla 
            string CampoPlural;
            string Campo = Tabla.Split('_')[Tabla.Split('_').Count() - 1];
            var aVocales = new string[] { "a", "e", "i", "o", "u" };
            if (aVocales.Contains(Campo.ToLower().Substring(Campo.Length - 1, 1)))
                CampoPlural = $"{Campo}s";
            else if (Campo.ToLower().Substring(Campo.Length - 1, 1) == "s")
                CampoPlural = Campo; //$"{Campo.Substring(0, Campo.Length - 1)}";
            else
                CampoPlural = $"{Campo}es";
            var wCampos = 90 / CamposHabilitar.Rows.Count;
            var hCampos = "";
            var sCampos = "";
            var index = 0;
            foreach (DataRow key in CamposHabilitar.Rows)
            {
                hCampos += $@"
                <th style=""width: {wCampos}%"" scope=""col"">@SharedLocalizer[""{key["Campo"].ToString()}""]</th>";
            }
            foreach (DataRow key in CamposHabilitar.Rows)
            {
                if (key["Tipo"].ToString() == "1")
                {
                    sCampos += $@"
                <td scope=""row"" column=""{key["Campo"].ToString()}"" >
                    <div id=""Label_{key["Campo"].ToString()}_@item.Clav_{Campo}"">@item.{key["Campo"].ToString()}</div>
                    <input id=""Input_{key["Campo"].ToString()}_@item.Clav_{Campo}"" type=""text"" class=""form-control form-control-sm hidden"" placeholder=""@SharedLocalizer[""{key["Campo"].ToString()}""]"" required />
                </td>";
                }
                else
                {
                    sCampos += $@"
                <td scope=""row"" column=""{key["Campo"].ToString()}"" >
                    <div class=""custom-control custom-switch"">
                        <input id=""{key["Campo"].ToString()}_@item.Clav_{Campo}"" name=""{key["Campo"].ToString()}_@item.Clav_{Campo}"" type=""checkbox"" class=""custom-control-input"" />
                        <label for=""{key["Campo"].ToString()}_@item.Clav_{Campo}"" class=""custom-control-label""></label>
                    </div>
                </td>";
                }
            }

            return $@"
@model IEnumerable<{Tabla}>
<div class=""table-responsive-sm"">
        <input type=""hidden"" id=""Action"" />      
    <table id=""Tabla{CampoPlural}"" class=""table table-sm table-hover"">
        <thead>
            <tr>{hCampos}
                <th style=""width: 10%"" scope=""col"">@SharedLocalizer[""Actions""]</th>
            </tr>
        </thead>
        <tbody>
            @foreach(var item in Model)
            {"{"}
            <tr>{sCampos}
                <td scope=""row"">
                    <div id=""GroupEdit_@item.Clav_{Campo}"" class=""btn-group btn-group-sm"">
                        <button type=""button"" data-info=""@item.Clav_{Campo}"" class=""BtnEdit btn btn-primary"">@SharedLocalizer[""Edit""]</button>
                        <button type=""button"" data-info=""@item.Clav_{Campo}"" class=""BtnDelete btn btn-danger hidden"" disabled> @SharedLocalizer[""Delete""] </button> 
                    </div> 
                    <div id=""GroupSave_@item.Clav_{Campo}"" class=""btn-group btn-group-sm hidden"">
                        <button type=""button"" data-info=""@item.Clav_{Campo}"" class=""BtnSave btn btn-primary"">@SharedLocalizer[""Save""]</button>
                        <button type=""button"" data-info=""@item.Clav_{Campo}"" class=""BtnCancel btn btn-danger"">@SharedLocalizer[""Cancel""]</button>
                    </div>
                    <button id=""BtnSaveLoading_@item.Clav_{Campo}"" type=""button"" data-info=""@item.Clav_{Campo}"" class=""btnLoading btn btn-primary btn-sm hidden"">
                        <span class=""spinner-border spinner-border-sm"" role=""status"" aria-hidden=""true""></span>
                        @SharedLocalizer[""Saving""]...
                    </button>
                </td>
            </tr>
            {"}"}
        </tbody>
        <tfoot>
            <tr>
                <td colspan=""{CamposHabilitar.Rows.Count}"">
                    <partial name=""~/Views/Shared/_Paginator.cshtml"" model=""@ViewData[""Paginator""]"" />
                </td>
            </tr>
        </tfoot>
    </table>
</div>";
        }

        private string GetViewLoading(string Tabla, DataTable CamposHabilitar)
        {
            // Validar para el plural de la tabla 
            string CampoPlural;
            string Campo = Tabla.Split('_')[Tabla.Split('_').Count() - 1];
            var aVocales = new string[] { "a", "e", "i", "o", "u" };
            if (aVocales.Contains(Campo.ToLower().Substring(Campo.Length - 1, 1)))
                CampoPlural = $"{Campo}s";
            else if (Campo.ToLower().Substring(Campo.Length - 1, 1) == "s")
                CampoPlural = Campo; //$"{Campo.Substring(0, Campo.Length - 1)}";
            else
                CampoPlural = $"{Campo}es";
            var wCampos = 90 / CamposHabilitar.Rows.Count;
            var hCampos = "";
            var sCampos = "";
            foreach (DataRow key in CamposHabilitar.Rows)
            {
                hCampos += $@"
                <th style=""width: {wCampos}%"" scope=""col"">@SharedLocalizer[""{key["Campo"].ToString()}""]</th>";
            }
            foreach (DataRow key in CamposHabilitar.Rows)
            {
                sCampos += $@"
                <td scope=""row"" column=""{key["Campo"].ToString()}""><div class=""animated-background""></div></td>";
            }

            return $@"
@model IEnumerable<{Tabla}>
<div class=""table-responsive-sm"">
    <table id=""Tabla{CampoPlural}"" class=""table table-sm table-hover"">
        <thead>
            <tr>{hCampos}
                <th style=""width: 10%"" scope=""col"">@SharedLocalizer[""Actions""]</th>
            </tr>
        </thead>
        <tbody>
            @for (int x = 0; x < 10; x++)
            {"{"}
            <tr>{sCampos}
                <td scope=""row"">
                    <div class=""btn-group btn-group-sm"">
                        <button type=""button"" class=""btn btn-primary"" disabled>Edit</button>
                        <button type=""button"" class=""btn btn-danger"" disabled>Delete</button>
                    </div>
                </td>
            </tr>
            {"}"}
        </tbody>
        <tfoot>
            <tr>
                <td colspan=""{CamposHabilitar.Rows.Count}""><div class=""animated-background""></div></td>
            </tr>
        </tfoot>
    </table>
</div>";
        }

        private string GetViewIndex(string Tabla, DataTable RepositoriesKeys)
        {
            // Armar las llaves de la tabla
            var keys = RepositoriesKeys.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
            // Validar para el plural de la tabla 
            string CampoPlural;
            string Campo = Tabla.Split('_')[Tabla.Split('_').Count() - 1];
            var aVocales = new string[] { "a", "e", "i", "o", "u" };
            if (aVocales.Contains(Campo.ToLower().Substring(Campo.Length - 1, 1)))
                CampoPlural = $"{Campo}s";
            else if (Campo.ToLower().Substring(Campo.Length - 1, 1) == "s")
                CampoPlural = Campo; //$"{Campo.Substring(0, Campo.Length - 1)}";
            else
                CampoPlural = $"{Campo}es";
            // Armar el filtro
            var keysFilter = "";
            foreach (string key in keys)
            {
                keysFilter += $@"
        <div class=""form-group m-1"">
            <label>@SharedLocalizer[""{key}""]:</label>
            <input type=""text"" class=""form-control form-control-sm"" id=""{key}"" placeholder=""@SharedLocalizer[""{key}""]"">
        </div>";
            }
            return $@"
<div id=""MenuPrincipal"">
    <partial name=""_MenuPrincipal"" />
</div>
<!--General container-->
<div class=""m-3"">
    <!--Filters-->
    <div class=""m-3 form-row"" id=""Filtro"">{keysFilter}
        <div class=""form-group m-1"">
            <label></label>
            <button type=""button"" class=""BtnFiltro btn btn-primary btn-sm form-control m-1"">@SharedLocalizer[""Filter""]</button>
        </div>
    </div>
    <div class=""row"">
        <button type=""button"" class=""BtnAgregar btn btn-primary btn-sm ml-3 "">@SharedLocalizer[""Add2""] @SharedLocalizer[""{Campo}""] <i class=""fas fa-plus""></i></button>
    </div>
    <div id=""{CampoPlural}List""></div>
    <div id=""LoadingList"" class=""hidden"">
        <partial name=""_Loading"" />
    </div>
</div>
@section Scripts {"{"}
    <partial name=""_ValidationScriptsPartial"" />
    <script src=""~/js/Controllers/{CampoPlural}.js""></script>
    <script src=""~/js/Controllers/Pagination.min.js""></script>
{"}"}";
        }
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
#pragma warning restore S2479 // Whitespace and control characters in string literals should be explicit
#pragma warning restore S1144 // Unused private types or members should be removed

        private void bFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                sPath = folderBrowserDialog1.SelectedPath;
                textBox3.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void tDrivers_SelectedIndexChanged(object sender, EventArgs e)
        {
            NameSpace = tNameSpace.Text;
            ServiceName = tService.Text;

            tService.Visible = false;
            label9.Visible = false;
            tTabla.Items.Clear();
            tDataBase.Items.Clear();
            switch (tDrivers.Text)
            {
                case "{SQL Server}":
                    textBox1.Text = "Driver=" + tDrivers.Text + ";Server=" + tServidor.Text + ";uid=" + tUser.Text + ";password=" + tPassword.Text + ";Database=" + tDataBase.Text + (string.IsNullOrEmpty(tPuerto.Text) ? "" : ";Port=" + tPuerto.Text) + ";";
                    tDrivers.Text = dbSQL[0];
                    tServidor.Text = dbSQL[1];
                    tDataBase.Text = dbSQL[2];
                    tUser.Text = dbSQL[3];
                    tPassword.Text = dbSQL[4];
                    tPuerto.Text = dbSQL[5];
                    tService.Text = dbSQL[6];
                    break;
                case "{MySQL ODBC 8.0 ANSI Driver}":
                    textBox1.Text = "Driver=" + tDrivers.Text + ";Server=" + tServidor.Text + ";uid=" + tUser.Text + ";password=" + tPassword.Text + ";Database=" + tDataBase.Text + (string.IsNullOrEmpty(tPuerto.Text) ? "" : ";Port=" + tPuerto.Text) + ";";
                    tDrivers.Text = dbMySQL[0];
                    tServidor.Text = dbMySQL[1];
                    tDataBase.Text = dbMySQL[2];
                    tUser.Text = dbMySQL[3];
                    tPassword.Text = dbMySQL[4];
                    tPuerto.Text = dbMySQL[5];
                    tService.Text = dbMySQL[6];
                    break;
                case "{Microsoft ODBC Driver for Oracle}":
                    textBox1.Text = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + ServiceName + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
                    tDrivers.Text = dbOracle[0];
                    tServidor.Text = dbOracle[1];
                    tDataBase.Text = dbOracle[2];
                    tUser.Text = dbOracle[3];
                    tPassword.Text = dbOracle[4];
                    tPuerto.Text = dbOracle[5];
                    tService.Text = dbOracle[6];
                    tService.Visible = true;
                    label9.Visible = true;
                    break;
            }

        }

        private void tServidor_TextChanged(object sender, EventArgs e)
        {
            NameSpace = tNameSpace.Text;
            ServiceName = tService.Text;
            if (tDrivers.Text == "{Microsoft ODBC Driver for Oracle}")
                textBox1.Text = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + ServiceName + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
            else
                textBox1.Text = "Driver=" + tDrivers.Text + ";Server=" + tServidor.Text + ";uid=" + tUser.Text + ";password=" + tPassword.Text + ";Database=" + tDataBase.Text + (string.IsNullOrEmpty(tPuerto.Text) ? "" : ";Port=" + tPuerto.Text) + ";";
        }

        private void tPuerto_TextChanged(object sender, EventArgs e)
        {
            NameSpace = tNameSpace.Text;
            ServiceName = tService.Text;
            if (tDrivers.Text == "{Microsoft ODBC Driver for Oracle}")
                textBox1.Text = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + ServiceName + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
            else
                textBox1.Text = "Driver=" + tDrivers.Text + ";Server=" + tServidor.Text + ";uid=" + tUser.Text + ";password=" + tPassword.Text + ";Database=" + tDataBase.Text + (string.IsNullOrEmpty(tPuerto.Text) ? "" : ";Port=" + tPuerto.Text) + ";";
        }

        private void tUser_TextChanged(object sender, EventArgs e)
        {
            NameSpace = tNameSpace.Text;
            ServiceName = tService.Text;
            if (tDrivers.Text == "{Microsoft ODBC Driver for Oracle}")
                textBox1.Text = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + ServiceName + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
            else
                textBox1.Text = "Driver=" + tDrivers.Text + ";Server=" + tServidor.Text + ";uid=" + tUser.Text + ";password=" + tPassword.Text + ";Database=" + tDataBase.Text + (string.IsNullOrEmpty(tPuerto.Text) ? "" : ";Port=" + tPuerto.Text) + ";";
        }

        private void tPassword_TextChanged(object sender, EventArgs e)
        {
            NameSpace = tNameSpace.Text;
            ServiceName = tService.Text;
            if (tDrivers.Text == "{Microsoft ODBC Driver for Oracle}")
                textBox1.Text = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + ServiceName + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
            else
                textBox1.Text = "Driver=" + tDrivers.Text + ";Server=" + tServidor.Text + ";uid=" + tUser.Text + ";password=" + tPassword.Text + ";Database=" + tDataBase.Text + (string.IsNullOrEmpty(tPuerto.Text) ? "" : ";Port=" + tPuerto.Text) + ";";
        }

        private void tDataBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            NameSpace = tNameSpace.Text;
            ServiceName = tService.Text;
            if (tDrivers.Text == "{Microsoft ODBC Driver for Oracle}")
                textBox1.Text = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + tService.Text + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
            else
                textBox1.Text = "Driver=" + tDrivers.Text + ";Server=" + tServidor.Text + ";uid=" + tUser.Text + ";password=" + tPassword.Text + ";Database=" + tDataBase.Text + (string.IsNullOrEmpty(tPuerto.Text) ? "" : ";Port=" + tPuerto.Text) + ";";
        }

        private void tDataBase_DropDown(object sender, System.EventArgs e)
        {
            // Cargar Databases
            if (tServidor.Text == "")
            {
                MessageBox.Show("Seleccione un Servidor");
                return;
            }
            if (tUser.Text == "")
            {
                MessageBox.Show("Seleccione un Servidor");
                return;
            }
            if (tPassword.Text == "")
            {
                MessageBox.Show("Seleccione un Servidor");
                return;
            }
            string sQuery = "";
            switch (tDrivers.Text)
            {
                case "{SQL Server}":
                    sQuery = "Select name databasename FROM sys.databases";
                    break;
                case "{MySQL ODBC 8.0 ANSI Driver}":
                    sQuery = "select SCHEMA_NAME databasename FROM INFORMATION_SCHEMA.SCHEMATA";
                    break;
                case "{Microsoft ODBC Driver for Oracle}":
                    sQuery = "Select name FROM sys.databases";
                    break;
            }
            dbUtil.dbData.sConn = textBox1.Text;
            System.Data.Odbc.OdbcConnectionStringBuilder strConn = new System.Data.Odbc.OdbcConnectionStringBuilder();
            strConn.Driver = tDrivers.Text;
            strConn.Add("Server", tServidor.Text);
            if(!string.IsNullOrEmpty(tPuerto.Text)) strConn["Port"] = tPuerto.Text;
            strConn.Add("Uid", tUser.Text);
            strConn.Add("Pwd", tPassword.Text);
            textBox1.Text = strConn.ConnectionString;
            DataTable oDataBases = new DataTable();
            if (tDrivers.Text == "{Microsoft ODBC Driver for Oracle}")
            {
                textBox1.Text = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + ServiceName + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
                dbUtilOracle.dbData.sConn = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + ServiceName + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
                oDataBases = dbUtilOracle.dbData.GetData(sQuery);
            }
            else
            {
                dbUtil.dbData.sConn = strConn.ConnectionString;
                this.Cursor = Cursors.WaitCursor;
                oDataBases = dbUtil.dbData.GetData(sQuery);
            }
            tDataBase.Items.Clear();
            if (oDataBases != null)
            {
                tTabla.Items.Clear();
                foreach (DataRow oDataBase in oDataBases.Rows)
                {
                    tDataBase.Items.Add(oDataBase[0].ToString());
                }
                tDataBase.SelectedIndex = 0;
            }
            this.Cursor = Cursors.Default;
        }

        private void tTabla_DropDown(object sender, EventArgs e)
        {
            // Cargar Tablas
            if (tServidor.Text == "")
            {
                MessageBox.Show("Seleccione un Servidor");
                return;
            }
            if (tUser.Text == "")
            {
                MessageBox.Show("Seleccione un Servidor");
                return;
            }
            if (tPassword.Text == "")
            {
                MessageBox.Show("Seleccione un Servidor");
                return;
            }
            string sQuery = "";
            switch (tDrivers.Text)
            {
                case "{SQL Server}":
                    sQuery = "select O.name as Tabla,o.object_id IdTabla,case when type='U' then 'Tabla' else 'Vista' end Tipo,S.schema_id as IdEsquema,S.name as Esquema from sys.objects O inner join sys.schemas S on O.schema_id=S.schema_id where type='U' and s.name = 'dbo' order by 1";
                    break;
                case "{MySQL ODBC 8.0 ANSI Driver}":
                    sQuery = "select Table_Comment Tabla, case when Table_Type ='BASE TABLE' then 'Tabla' else 'Vista' end Tipo FROM INFORMATION_SCHEMA.Tables where table_schema = '" + tDataBase.Text + "' order by Table_Name";
                    break;
                case "{Microsoft ODBC Driver for Oracle}":
                    sQuery = "select DISTINCT table_name Tabla, case when global_stats ='NO' then 'Vista' else 'Table' end Tipo from ALL_TAB_COLUMNS ORDER BY 1, 2";
                    break;
            }
            // Obtener las Tablas
            dbUtil.dbData.sConn = textBox1.Text;
            System.Data.Odbc.OdbcConnectionStringBuilder strConn = new System.Data.Odbc.OdbcConnectionStringBuilder();
            strConn.Driver = tDrivers.Text;
            strConn["Server"] = tServidor.Text;
            strConn.Add("Uid", tUser.Text);
            strConn.Add("Pwd", tPassword.Text);
            strConn.Add("DataBase", tDataBase.Text);
            if (!string.IsNullOrEmpty(tPuerto.Text)) strConn["Port"] = tPuerto.Text;
            textBox1.Text = strConn.ConnectionString;
            DataSet oObjetos = new DataSet();
            if (tDrivers.Text == "{Microsoft ODBC Driver for Oracle}")
            {
                textBox1.Text = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + tService.Text + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
                dbUtilOracle.dbData.sConn = "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + tServidor.Text + ")(PORT=" + tPuerto.Text + "))(CONNECT_DATA=(SERVICE_NAME=" + tService.Text + ")));User Id=" + tUser.Text + ";Password=" + tPassword.Text + ";";
                oObjetos = dbUtilOracle.dbData.GetDataSet(sQuery);
            }
            else
            {
                dbUtil.dbData.sConn = strConn.ConnectionString;
                oObjetos = dbUtil.dbData.GetDataSet(sQuery);
            }
            if (oObjetos != null)
            {
                tTabla.DataSource = oObjetos.Tables[0].DefaultView;
                tTabla.ValueMember = "Tabla";
                //foreach (DataRow oTabla in oObjetos.Rows)
                //{
                //    tTabla.Items.Add(oTabla[0].ToString());
                //}
                tTabla.SelectedIndex = 0;
            } else
                MessageBox.Show("No existen datos para su seleccion");
            this.Cursor = Cursors.Default;
        }
    }
}
