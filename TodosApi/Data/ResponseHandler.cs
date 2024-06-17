
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;

namespace TodosApi.Data
{
    public class ResponseHandler : ControllerBase
    {
        public IActionResult ApiReponseHandler(Object data)
        {
            return Ok(new ApiResponse
            {
                Code = (int)HttpStatusCode.OK,
                Message = "OK",
                Data = data
            });
        }

        public IActionResult ApiReponseException(Exception ex)
        {
            return StatusCode(500, new ApiResponse
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = "InternalServerError",
                Data = ex.Message
            });
        }
        public IActionResult ApiReponseBadRequest(string ex)
        {
            return BadRequest(new ApiResponse
            {
                Code = (int)HttpStatusCode.BadRequest,
                Message = "BadRequest",
                Data = ex
            });
        }
        public IActionResult ApiReponseNotFound(string ex)
        {
            return NotFound(new ApiResponse
            {
                Code = (int)HttpStatusCode.NotFound,
                Message = "NotFound",
                Data = ex
            });
        }
    }
}
