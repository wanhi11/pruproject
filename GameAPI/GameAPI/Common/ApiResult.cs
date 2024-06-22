﻿using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Common;

public record ApiResult<T>
{
    public bool Success { get; set; }
    public T? Result { get; set; }
    
    public static ApiResult<T> Succeed(T? result)
    {
        return new ApiResult<T> { Success = true, Result = result };
    }

    public static ApiResult<T> Error(T? result)
    {
        return new ApiResult<T> { Success = false, Result = result };
    }

    public static ApiResult<object> Fail(Exception ex)
    {
        return new ApiResult<object>
        {
            Success = false,
            Result = new
            {
                ex.Message,
            }
        };
    }
}