using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyReact2.Abstract;
using MyReact2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MyReact2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MyController : ControllerBase
    {
        ICity service;
        public MyController(ICity service) 
        { 
            this.service = service;
        }
        [HttpGet, Route("GetCity")]
        public ActionResult GetCity()
        {
            var model = new List<City>()
            {
                new City() { id = 1, name="Almaty"},
                new City() { id = 2, name="Aktobe"},
                new City() { id = 3, name="Astana"}
            };
            return Ok(model);
        }

        [HttpPost, Route("AddOrEdit")]
        public ActionResult AddOrEdit(City model)
        { 
            return Ok(service.AddOrEdit(model));
        }

        [HttpGet, Route("GetCityAll/(id)")]
        public ActionResult GetCityAll(string id)
        {
            return Ok(service.GetCityAll(id));
        }
    }
}


//CREATE Table City
//( 
//    id INT PRIMARY KEY IDENTITY,

//    [name] NVARCHAR(100)
//)

//CREATE PROC pCity --INSUPD
//@id INT,
//@name NVARCHAR(100)
//AS
//IF @id=0
//  BEGIN
//      INSERT INTO City (name)
//      VALUES (@name)
//      SELECT 'ok'
//  END
//ELSE
//  BEGIN
//     UPDATE City
//	 SET [name] = @name

//     WHERE id = @id
//	 SELECT 'ok'
//  END 

//CREATE PROC pCity; 2--Get
//@id VARCHAR(100)
//AS
//IF @id = 'all'
//  SELECT *
//  FROM City
//ELSE
//  SELECT *
//  FROM City
//WHERE (id=@id)

//CREATE PROC pCity; 3--Del
//@id INT
//AS
//DELETE
//FROM City
//WHERE id=@id