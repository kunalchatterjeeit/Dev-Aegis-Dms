﻿using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Api.Dms.Controllers
{
    public class ChartController : ApiController
    {
        [HttpGet]
        [JwtAuthorization(Entity.Utility.FILE)]
        public HttpResponseMessage GetWeeklyUploadChart()
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.BarChartResponse> barChartResponses = new BusinessLayer.Chart().File_GetByDateRange(
                    DateTime.Now.AddDays(-7).Date,
                    DateTime.Now.Date,
                    Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                response.ResponseData = barChartResponses;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpGet]
        [JwtAuthorization(Entity.Utility.FILE)]
        public HttpResponseMessage GetAllUploadFileSize()
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                decimal fileSize = new BusinessLayer.Chart().File_GetSize(
                    Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                response.ResponseData = (fileSize/1024).ToString("#.##"); //converting into MB
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpGet]
        [JwtAuthorization(Entity.Utility.FILE)]
        public HttpResponseMessage GetAllUploadFileCount()
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                decimal fileCount = new BusinessLayer.Chart().File_GetFileCount(
                    Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                response.ResponseData = fileCount;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }
    }
}