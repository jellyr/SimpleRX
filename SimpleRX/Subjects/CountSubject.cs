using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
public class CountSubject<T> : BaseSubject<int>
{
    private IObservable<T> source;        
    private IDisposable subscriped;
    private BaseSubject<T> subject;
    private bool containsValue;
    private int counter;

    public CountSubject(IObservable<T> source)
    {
        this.source = source;
        BaseSubject<T> subject = (BaseSubject<T>)source;
        this.subject = subject;
        var observer =
            new Observer<T>(
                localvalue => InnerExecute(localvalue),
                ex => NotifyErrorObservers(ex),
                () => InnerComplete());
        this.subscriped = subject.ColdSubscribe(observer);
    }

    private void InnerExecute(T value)
    {
        counter++;        
    }

    private void InnerComplete()
    {
        NotifyObservers(counter);
        NotifyCompleteObservers();
    }

    public override void Execute()
    {
        try
        {
            subject.Execute();
        }
        catch (Exception exception)
        {
            NotifyErrorObservers(exception);
        }           
    }
}
}
