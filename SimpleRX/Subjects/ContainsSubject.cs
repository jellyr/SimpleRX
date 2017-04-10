using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
public class ContainsSubject<T> : BaseSubject<bool>
{
    private IObservable<T> source;        
    private IDisposable subscriped;
    private BaseSubject<T> subject;
    private bool containsValue;
    private T value;

    public ContainsSubject(IObservable<T> source, T value)
    {
        this.source = source;
        this.value = value;
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
        if (this.value.Equals(value))
        {
            containsValue = true;                
        }                        
    }

    private void InnerComplete()
    {
        NotifyObservers(containsValue);
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
