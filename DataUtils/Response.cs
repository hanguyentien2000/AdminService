using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUtils
{
    public class Response<T>
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static Response<T> Ok(T data, string? message = null)
        {
            return new Response<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static Response<T> Fail(string message, List<string>? errors = null)
        {
            return new Response<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
