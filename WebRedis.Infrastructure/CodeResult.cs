using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Infrastructure
{
    public class CodeResult
    {
        public CodeResult():this(0, "success")
        {

        }
        public CodeResult(int code, string msg)
        {
            Code = code;
            Msg = msg;
        }
        public int Code { get; set; }
        public string Msg { get; set; }
    }
}
