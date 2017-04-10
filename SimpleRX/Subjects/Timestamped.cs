using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public struct Timestamped<T>
    {
        public T Value { get; set; }
        public DateTimeOffset Timestamp { get; set; }        
    }
}
