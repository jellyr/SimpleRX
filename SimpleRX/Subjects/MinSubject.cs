using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class MinSubjectInt : BaseSubject<int>
    {
        private IObservable<int> source;        
        private IDisposable subscriped;
        private BaseSubject<int> subject;
        private int minValue;
        private bool valueSet;

        public MinSubjectInt(IObservable<int> source)
        {
            this.source = source;
            BaseSubject<int> subject = (BaseSubject<int>)source;
            this.subject = subject;
            var observer =
                new Observer<int>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
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

        public override void Execute()
        {
            try
            {
                subject.Execute();
                NotifyObservers(minValue);
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }      
        }
    }

    public class MinSubjectDouble : BaseSubject<double>
    {
        private IObservable<double> source;
        private IDisposable subscriped;
        private BaseSubject<double> subject;
        private double minValue;
        private bool valueSet;

        public MinSubjectDouble(IObservable<double> source)
        {
            this.source = source;
            BaseSubject<double> subject = (BaseSubject<double>)source;
            this.subject = subject;
            var observer =
                new Observer<double>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(double value)
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

        public override void Execute()
        {
            try
            {
                subject.Execute();
                NotifyObservers(minValue);   
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }      
        }
    }
}
