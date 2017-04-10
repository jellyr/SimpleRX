using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
public class ThrowSubject<T> : BaseSubject<T>
{
    private Exception exception;

    public ThrowSubject(Exception exception)
    {
        this.exception = exception;
    }

    public override void Execute()
    {
        NotifyErrorObservers(exception);   
    }
}
}
