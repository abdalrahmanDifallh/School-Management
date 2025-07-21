

using Core.Domains;
using Microsoft.AspNetCore.Http;
using SchoolManagement.Data;
using Services.Extensions;
using Services.LoggerService;
using System.Net;
using System.Security.Claims;

namespace Services._Base
{
    public abstract class BaseService<C> where C : class
    {
        public readonly SchoolManagementContext _dbContext;
        private readonly ILoggerService<C> _logger;
        protected readonly HttpContext _httpContext;
        public BaseService(SchoolManagementContext dbContext, ILoggerService<C> logger, IHttpContextAccessor httpAccessor)
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContext = httpAccessor.HttpContext;
        }

        #region Props
        // TODO
        public string UserId
        {
            get
            {
                return _httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }
        }

        public string UserRole
        {
            get
            {
                return _httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            }
        }
        #endregion


        #region Error

        //============//============//============//============//============
        internal ResponseResult<T> Error<T>(Exception ex)
        {
            return new ResponseResult<T>
            {
                IsSuccess = false,
                Errors = new List<string> { ex.GetError() },
                StatusCode = HttpStatusCode.InternalServerError,
            };
        }

        //============//============//============//============//============
        internal ResponseResult<T> Error<T>(Exception ex, HttpStatusCode httpStatus)
        {
            _logger.LogErrorAsync("", ex);
            return new ResponseResult<T>
            {
                IsSuccess = false,
                Errors = new List<string> { ex.GetError() },
                StatusCode = httpStatus,
            };
        }

        //============//============//============//============//============
        internal ResponseResult<T> Error<T>(string errorMsg)
        {
            return new ResponseResult<T>
            {
                IsSuccess = false,
                Errors = new List<string> { errorMsg },
                StatusCode = HttpStatusCode.BadRequest,
            };
        }

        //============//============//============//============//============
        internal ResponseResult<T> Error<T>(string errorMsg, HttpStatusCode httpStatus)
        {

            return new ResponseResult<T>
            {
                IsSuccess = false,
                Errors = new List<string> { errorMsg },
                StatusCode = httpStatus,
            };
        }

        //============//============//============//============//============
        internal ResponseResult<T> Error<T>(List<string> errorMsgs)
        {
            return new ResponseResult<T>
            {
                IsSuccess = false,
                Errors = errorMsgs,
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
            };
        }

        //============//============//============//============//============
        internal ResponseResult<T> Error<T>(List<string> errorMsgs, HttpStatusCode statusCode)
        {
            return new ResponseResult<T>
            {
                IsSuccess = false,
                Errors = errorMsgs,
                StatusCode = statusCode,
            };
        }
        #endregion


        #region Success
        //============//============//============//============//============
        internal ResponseResult<T> Success<T>()
        {
            return new ResponseResult<T>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
            };
        }

        //============//============//============//============//============
        internal ResponseResult<T> Success<T>(HttpStatusCode statusCode)
        {
            return new ResponseResult<T>
            {
                IsSuccess = true,
                StatusCode = statusCode,
            };
        }

        //============//============//============//============//============
        internal ResponseResult<T> Success<T>(T data)
        {
            return new ResponseResult<T>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Data = data,
            };
        }

        //============//============//============//============//============
        internal ResponseResult<T> Success<T>(T data, HttpStatusCode statusCode)
        {
            return new ResponseResult<T>
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Data = data,
            };
        }

        #endregion

    }
}
