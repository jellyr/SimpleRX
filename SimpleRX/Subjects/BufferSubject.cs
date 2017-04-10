using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleRX
{
    public class BufferSubject<T, T1> : BaseSubject<T>
    {        
        private int count;        
        private int actualCounter;
        private IDisposable subscriped;
        private BaseSubject<T1> subject;
        private IList<T1> buffer;

        public BufferSubject(int count)
        {
            Type type = typeof(T);            
            Type itemType = type.GetGenericArguments()[0];
            Type listType = typeof(List<>);
            Type constructed = listType.MakeGenericType(itemType);
            buffer = (List<T1>) Activator.CreateInstance(constructed);                                       
            this.count = count;            
        }

        public void Start(IObservable<T1> source)
        {
            BaseSubject<T1> subject = (BaseSubject<T1>)source;
            this.subject = subject;
            var observer =
                new Observer<T1>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => { });
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(T1 value)
        {
            if(actualCounter < count)
            {
                MethodInfo methodInfo = buffer.GetType().GetMethod("Add");
                methodInfo.Invoke(buffer, new object[] { value });
            }
            actualCounter++;
        }

        public override void Execute()
        {
            try
            {
                subject.Execute();                
                NotifyObservers((T)buffer);
                NotifyCompleteObservers();
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }                     
        }
    }
}
