using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public interface IGroupedObservable<out T, out TKey> : IObservable<T>
    {
        TKey Key { get; }
        //IDisposable Subscribe(IObserver<T> observer);
    }
}
