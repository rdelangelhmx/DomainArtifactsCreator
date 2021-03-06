/****** Object:  StoredProcedure [dbo].[getDTO]    Script Date: 14/07/2013 09:27:29 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[getDTO](@Table varchar(max)) as

declare @Query varchar(max)        

set @Query = '/*
 *  ' + @Table + '.cs
 *  Objeto de Transferencia de Datos asociada a una Entidad
 *
 *  © 2011-2012 Rodrigo del Angel <rdelangelhmx@gmail.com>
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
        #region Constructores

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

        public DataTable getDatos(object ' + @Table + 'Ini,object ' + @Table + 'Fin, int Registros, int Pagina)
        {
            DataTable Datos = new DataTable();
            try
            {
                Datos = new DAO.daoGenerico().getData(' + @Table + 'Ini,' + @Table + 'Fin, Pagina, Registros);
            }
            catch (Exception ex)
            {
                General.WriteLog("ERROR -- Class: getRegistros Documentos");
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
Select @Query
--print @Query