using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class MinSubjectSubjectInt<T> : BaseSubject<int>
    {
        private IObservable<int> source;        
        private IDisposable subscriped;
        private BaseSubject<int> subject;
        private int minValue;
        private bool valueSet;

        public MinSubjectSubjectInt(IObservable<int> source)
        {
            this.source = source;
            BaseSubject<int> subject = (BaseSubject<int>)source;
            this.subject = subject;
            var observer =
                new Observer<int>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => InnerComplete());
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(int value)
        {
            if (!valueSet)
            {
                minValue = value;
                valueSet = true;
            }
            else
            {
                if (value < minValue)
                {
                    minValue = value;
                }
            }                           
        }

        private void InnerComplete()
        {
            NotifyObservers(minValue);
            NotifyCompleteObservers();
        }

        public override void Execute()
        {
            
        }
    }
}
