/****** Object:  StoredProcedure [dbo].[getController]    Script Date: 14/07/2013 09:27:09 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[getController](@Table varchar(max),@Solution varchar(max)) as

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

declare @CamposClave1 varchar(max)
set @CamposClave1 = ''
select @CamposClave1 += syscolumns.name + ','
FROM dbo.syscolumns 
 INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
 INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE (dbo.sysobjects.name = @Table)  and systypes.name not in ('sysname') and syscolumns.name in (select  i2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 
      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME 
      WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' and i1.TABLE_NAME = @Table)
ORDER BY  dbo.sysobjects.name, dbo.syscolumns.colorder
set @CamposClave1 = SUBSTRING(@CamposClave1,1,LEN(@CamposClave1)-1)

declare @CamposParam varchar(max)
set @CamposParam = ''
select @CamposParam += case 
when systypes.name = 'int' 
then '            int ' + (syscolumns.name) + ' = (int)' + @Table + 'Info.' + (syscolumns.name) + ';
'
when systypes.name = 'bigint' 
then '            int ' + (syscolumns.name) + ' = (int)' + @Table + 'Info.' + (syscolumns.name) + ';
'
when systypes.name = 'decimal' 
then '            decimal ' + (syscolumns.name) + ' = (decimal)' + @Table + 'Info.' + (syscolumns.name) + ';
'
when systypes.name = 'date' 
then '            DateTime ' + (syscolumns.name) + ' = (DateTime)' + @Table + 'Info.' + (syscolumns.name) + ';
'
when systypes.name = 'datetime' 
then '            DateTime ' + (syscolumns.name) + ' = (DateTime)' + @Table + 'Info.' + (syscolumns.name) + ';
'
when (systypes.name = 'nvarchar'  or systypes.name = 'varchar' or systypes.name = 'nchar' or systypes.name = 'char')
then '            string ' + (syscolumns.name) + ' = (string)' + @Table + 'Info.' + (syscolumns.name) + ';
'
else '' end
FROM dbo.syscolumns 
 INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id 
 INNER JOIN dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE (dbo.sysobjects.name = @Table)  and systypes.name not in ('sysname') and syscolumns.name in (select  i2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 
      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME 
      WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' and i1.TABLE_NAME = @Table)
ORDER BY  dbo.sysobjects.name, dbo.syscolumns.colorder

declare @Query varchar(max)        
SELECT @Query = isnull(@Query,'') + ''
set @Query = '/*
 *  ' + @Table + 'Controller.cs
 *  Controller para la entidad ' + @Table + '
 *
 *  © 2013 Rodrigo del Angel <rdelangelhmx@gmail.com>
 *  Some Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ' + @Solution + '.Controllers
{
    public class ' + @Table + 'Controller : Controller
    {

        public ActionResult Index()
        {
            int currentPage = 1;
            int pageSize = Globales.Registros;
            ViewData["currentPage"] = currentPage;
            ViewData["pageSize"] = pageSize;
            return Index(currentPage);
        }


        [HttpPost]
        public ActionResult Index(int currentPage)
        {
			Service' + @Table + '.' + @Table + ' ' + @Table + 'I = new Service' + @Table + '.' + @Table + '();
			Service' + @Table + '.' + @Table + ' ' + @Table + 'F = new Service' + @Table + '.' + @Table + '();

            int totalRows = Models.' + @Table + 'Model.Registros(' + @Table + 'I);
            int totalPages = (int)Math.Ceiling((double)totalRows / (double)Globales.Registros);

            if (totalRows == 0)
                return View(new List<Service' + @Table + '.' + @Table + '>());
            UpdatePagerViewData(totalPages, totalRows, currentPage, Globales.Registros);
            if (totalPages > 1)
                return View(Models.' + @Table + 'Model.GetRegistrosPag(currentPage, ' + @Table + 'I));
            else
                return View(Models.' + @Table + 'Model.GetRegistrosPag(1, ' + @Table + 'I));
        }


        private void UpdatePagerViewData(int totalPages, int totalRows, int currentPage, int pageSize)
        {
            ViewData["pagerStats"] = GenPagerStats(totalRows, currentPage, pageSize);
            if (totalPages == 1)
            {
                ViewData["isPagerVisible"] = false;
                return;
            }
            int lastPage = totalPages;
            ViewData["isPagerVisible"] = true;
            ViewData["currentPage"] = currentPage;
            ViewData["totalPages"] = totalPages;
            if (currentPage == 1)
            {
                ViewData["isFirstPage"] = true;
            }
            else
            {
                ViewData["isFirstPage"] = false;
                ViewData["previousPage"] = currentPage - 1;
            }
            if (currentPage == lastPage)
            {
                ViewData["isLastPage"] = true;
            }
            else
            {
                ViewData["isLastPage"] = false;
                ViewData["nextPage"] = currentPage + 1;
                ViewData["lastPage"] = lastPage;
            }
        }

        private string GenPagerStats(int totalRows, int currentPage, int pageSize)
        {
            int firstRow = (currentPage - 1) * pageSize + 1;
            int lastRow = Math.Min(firstRow + pageSize - 1, totalRows);
            string stats;
            if (firstRow == lastRow)
                stats = "Registro " + firstRow + " de " + totalRows;
            else
                stats = "Registro " + firstRow + "-" + lastRow + " de " + totalRows;
            return stats;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Service' + @Table + '.' + @Table + ' ' + @Table + 'Info)
        {
			//
			// Poner las validaciones necesarias
			//				
			/*
				if (person.Name.Trim().Length == 0)
				{
					ModelState.AddModelError("Name", "Name is required.");
				}
				if (person.Age < 1 || person.Age > 200)
				{
					ModelState.AddModelError("Age", "Age must be within range 1 to 200.");
				}
				if ((person.Zipcode.Trim().Length > 0) && (!Regex.IsMatch(person.Zipcode, @"^\d{5}$|^\d{5}-\d{4}$")))
				{
					ModelState.AddModelError("Zipcode", "Zipcode is invalid.");
				}
				if (!Regex.IsMatch(person.Phone, @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"))
				{
					ModelState.AddModelError("Phone", "Phone number is invalid.");
				}
				if (!Regex.IsMatch(person.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
				{
					ModelState.AddModelError("Email", "Email format is invalid.");
				}
				if (!ModelState.IsValid)
				{
					return View("Create", person);
				}
			 */
			//
			
            if (ModelState.IsValid)
            {
                if (Models.' + @Table + 'Model.Insert' + @Table + '(' + @Table + 'Info))
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.Error = "No se puede crear el registro";
                    return View(' + @Table + 'Info);
                }
            }
            else
            {
                ViewBag.Error = "No se puede crear el registro";
                return View(' + @Table + 'Info);
            }
        }

        public ActionResult Edit(' + @CamposClave + ')
        {
            return View(Models.' + @Table + 'Model.GetRegistro' + @Table + '(' + @CamposClave1 + '));
        }

        [HttpPost]
        public ActionResult Edit(Service' + @Table + '.' + @Table + ' ' + @Table + 'Info)
        {
			//
			// Poner las validaciones necesarias
			//				
			/*
				if (person.Name.Trim().Length == 0)
				{
					ModelState.AddModelError("Name", "Name is required.");
				}
				if (person.Age < 1 || person.Age > 200)
				{
					ModelState.AddModelError("Age", "Age must be within range 1 to 200.");
				}
				if ((person.Zipcode.Trim().Length > 0) && (!Regex.IsMatch(person.Zipcode, @"^\d{5}$|^\d{5}-\d{4}$")))
				{
					ModelState.AddModelError("Zipcode", "Zipcode is invalid.");
				}
				if (!Regex.IsMatch(person.Phone, @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"))
				{
					ModelState.AddModelError("Phone", "Phone number is invalid.");
				}
				if (!Regex.IsMatch(person.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
				{
					ModelState.AddModelError("Email", "Email format is invalid.");
				}
				if (!ModelState.IsValid)
				{
					return View("Create", person);
				}
			 */
			//
			
            if (ModelState.IsValid)
            {
                if (Models.' + @Table + 'Model.Update' + @Table + '(' + @Table + 'Info))
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.Error = "No se puede modificar el registro";
                    return View(' + @Table + 'Info);
                }
            }
            else
            {
                ViewBag.Error = "No se puede modificar el registro";
                return View(' + @Table + 'Info);
            }
        }

        public ActionResult Delete(' + @CamposClave + ')
        {
            return View(Models.' + @Table + 'Model.GetRegistro' + @Table + '(' + @CamposClave1 + '));
        }

        [HttpPost]
        public ActionResult Delete(Service' + @Table + '.' + @Table + ' ' + @Table + 'Info)
        {
' + @CamposParam + '
            if (ModelState.IsValid)
            {
                if (Models.' + @Table + 'Model.Delete' + @Table + '(' + @CamposClave1 + '))
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.Error = "No se puede eliminar el registro";
                    return View(' + @Table + 'Info);
                }
            }
            else
            {
                ViewBag.Error = "No se puede modificar el registro";
                return View(' + @Table + 'Info);
            }
        }
    }
}'

--print @Query
select @Query