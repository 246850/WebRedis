using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Infrastructure
{
    public class CodeResult<T>:CodeResult
    {
        public CodeResult() : this(default(T))
        {

        }
        public CodeResult(T body):base()
        {
            Body = body;
        }
        public T Body { get; set; }
    }
}
