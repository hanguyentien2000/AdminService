using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUtils
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static Response<T> Ok(T data, string? message = null)
            => new() { Success = true, Data = data, Message = message };

        public static Response<T> Fail(string message)
            => new() { Success = false, Message = message };
    }
}
