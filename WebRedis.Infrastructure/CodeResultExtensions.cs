using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Infrastructure
{
    public static class CodeResultExtensions
    {
        public static CodeResult<T> ToResult<T>(this T source)
        {
            return new CodeResult<T>(source);
        }

        public static CodeResult<T> Success<T>(this T source)
        {
            return new CodeResult<T>(source);
        }
    }
}
