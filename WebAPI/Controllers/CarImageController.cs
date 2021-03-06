﻿using Business.Abstract;
using Core.Utilities.Helpers;
using Entities.Concrete.ImageEntity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarImageController : ControllerBase
    {

        public static IWebHostEnvironment _webHostEnviroment;
        ICarImageService _carImageService;

        string path = "C:\\Users\\ErenYaylaci\\Desktop\\software camping\\ReCapProject\\Images";
        string defaultpath = "C:\\Users\\ErenYaylaci\\Desktop\\software camping\\ReCapProject\\Images\\Logo.jpg";

        public CarImageController(IWebHostEnvironment webHostEnviroment, ICarImageService carImageService)
        {
            _webHostEnviroment = webHostEnviroment;
            _carImageService = carImageService;
        }
        
        [HttpPost("add")]
        public IActionResult ImageAdd([FromForm] CarImage carImage)
        {
            if (carImage.ImageFile != null)
            {
                if (carImage.ImageFile.Length > 0)
                {
                    //_webHostEnviroment.WebRootPath + "\\CarImages\\";
                    //Path.Combine(_webHostEnviroment.WebRootPath, "CarImages");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string extension = Path.GetExtension(carImage.ImageFile.FileName);
                    string guidFileName = Guid.NewGuid().ToString() + extension;

                    using (var fileStream = new FileStream(Path.Combine(path, guidFileName), FileMode.Create))
                    {
                        carImage.ImageFile.CopyToAsync(fileStream);
                    }

                    carImage.ImageName = guidFileName;

                }
            }
                
           var result = _carImageService.Add(carImage);
            if (result.Success)
            {
                return Ok();
            }
            return BadRequest();
           
        }
        [HttpGet("delete")]
        public IActionResult Delete(int id)
        {

            var carImage = _carImageService.GetById(id).Data;
            if (carImage.ImageName != null)
            {
                FileHelper.Delete(path + "\\" + carImage.ImageName);
            }
            var result = _carImageService.Delete(carImage);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _carImageService.GetById(id);
            var imageData = result.Data;
            string newPath;
            if (imageData == null)
            {
                newPath = Path.Combine(path, "Logo.jpg");
            }
            else
            {
                newPath = path + "\\" + result.Data.ImageName;
            }
            
            
            if (result.Success)
            {
                byte[] file = System.IO.File.ReadAllBytes(newPath);
                return File(file, "image/jpeg");
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getbycarid")]
        public IActionResult GetbyCarId(int id)
        {
            var result = _carImageService.GetByCarId(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message); 
            
        }
        [HttpPost("update")]
        public IActionResult Update(CarImage carImage)
        {
            return Ok();
        }
        [HttpGet("getbycarhomeid")]
        public IActionResult GetByCarHomeId(int id)
        {
            string newPath;
            var result = _carImageService.GetByCarId(id);
           
            
             newPath = Path.Combine(path, "Logo.jpg");
               
           
              byte[] file = System.IO.File.ReadAllBytes(newPath);
               return File(file, "image/jpeg");
            
           
        }

    }
}
/*
 https://www.c-sharpcorner.com/UploadFile/mahesh/create-a-file-using-fileinfo-in-C-Sharp/
https://docs.microsoft.com/tr-tr/dotnet/api/system.io.file?view=net-5.0
https://docs.microsoft.com/tr-tr/dotnet/api/system.guid.newguid?view=net-5.0
https://docs.microsoft.com/tr-tr/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-5.0/
//Eksik olan yer
CREATE TABLE [dbo].[UserOperationClaims] (
    [Id]               INT IDENTITY (1, 1) NOT NULL,
    [UserId]           INT NOT NULL,
    [OperationClaimId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


 */