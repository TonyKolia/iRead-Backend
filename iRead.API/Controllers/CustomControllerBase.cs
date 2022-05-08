using iRead.API.Models;
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
            switch (type) 
            {
                case ResponseType.Error:
                    response = new Response(returnData, ReturnMessages.DefaultErrorMessage, StatusCodes.Status500InternalServerError);
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                    break;
                case ResponseType.BadRequest:
                    response = new Response(returnData, message, StatusCodes.Status400BadRequest);
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                    break;
                case ResponseType.Created:
                    response = new Response(returnData, ReturnMessages.DefaultCreatedMessage, StatusCodes.Status201Created);
                    return StatusCode(StatusCodes.Status201Created, response);
                case ResponseType.Data:
                    response = new Response(returnData, message, StatusCodes.Status200OK);
                    return StatusCode(StatusCodes.Status200OK, response);
                    break;
                case ResponseType.NotFound:
                    response = new Response(returnData, message, StatusCodes.Status404NotFound);
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
            Data,
            Created,
            NotFound,
            BadRequest,
            Error
        }

        internal static class ReturnMessages 
        {
            public const string DefaultErrorMessage = "An error has occured.";
            public const string DefaultCreatedMessage = "An error has occured.";
        }


    }



}
