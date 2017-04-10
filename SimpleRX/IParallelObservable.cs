using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRX
{
    public interface IParallelObservable<T> : IObservable<T>
    {
        //IDisposable Subscribe(IObserver<T> observer);
    }
}
