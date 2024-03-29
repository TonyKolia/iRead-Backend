﻿using iRead.API.Models;
using iRead.API.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        protected readonly ILogger _logger;

        public CustomControllerBase(ILogger<CustomControllerBase> logger)
        {
            _logger = logger;
        }

        protected ActionResult ReturnResponse(ResponseType type, string message = "", object returnData = null)
        {
            Response response = null;
            var success = type == ResponseType.Created || type == ResponseType.Data || type == ResponseType.Updated || type == ResponseType.Token || type == ResponseType.Deleted;

            switch (type) 
            {
                case ResponseType.Error:
                    response = new Response(returnData, ReturnMessages.DefaultErrorMessage, StatusCodes.Status500InternalServerError, success);
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                    break;
                case ResponseType.BadRequest:
                    response = new Response(returnData, message, StatusCodes.Status400BadRequest, success);
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                    break;
                case ResponseType.Created:
                    response = new Response(returnData, ReturnMessages.DefaultCreatedMessage, StatusCodes.Status201Created, success);
                    return StatusCode(StatusCodes.Status201Created, response);
                case ResponseType.Data:
                case ResponseType.Updated:
                case ResponseType.Token:
                case ResponseType.Deleted:
                    response = new Response(returnData, message, StatusCodes.Status200OK, success);
                    return StatusCode(StatusCodes.Status200OK, response);
                    break;
                case ResponseType.NotFound:
                    response = new Response(returnData, message, StatusCodes.Status404NotFound, success);
                    return StatusCode(StatusCodes.Status404NotFound, response);
                    break;
                default:
                    break;
            }

            return null;
        }

        protected ActionResult ReturnIfNotEmpty<T>(IEnumerable<T> data, string notFoundMessage = "", bool performMapping = true)
        {
            if(data.Count() > 0)
            {
                return ReturnResponse(ResponseType.Data, "", performMapping ? data.MapResponse() : data);
            }
            else
            {
                return ReturnResponse(ResponseType.NotFound, notFoundMessage);
            }

            //return data.Count() > 0 ? Ok(performMapping ? data.MapResponse() : data) : NotFound(notFoundMessage);
        }

        protected ActionResult ReturnIfNotEmpty<T> (T data, string notFoundMessage = "", bool performMapping = true)
        {
            if (data != null)
            {
                return ReturnResponse(ResponseType.Data, "", performMapping ? data.MapResponse() : data);
            }
            else
            {
                return ReturnResponse(ResponseType.NotFound, notFoundMessage);
            }
            //return data != null ? Ok(performMapping ? data.MapResponse() : data) : NotFound(notFoundMessage);
        }

       

        protected enum ResponseType
        {
            Deleted,
            Token,
            Data,
            Created,
            Updated,
            NotFound,
            BadRequest,
            Error
        }

        internal static class ReturnMessages 
        {
            public const string DefaultErrorMessage = "An error has occured.";
            public const string DefaultCreatedMessage = "Created successfully.";
        }


    }



}
