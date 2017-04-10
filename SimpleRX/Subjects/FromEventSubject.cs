using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace SimpleRX
{
    //http://badamczewski.blogspot.com/2010/04/c-dynamic-methods.html
    //http://msdn.microsoft.com/en-us/library/ms228976.aspx
    //http://msdn.microsoft.com/en-us/library/exczf7b9.aspx
    //http://msdn.microsoft.com/de-de/library/system.reflection.eventinfo.addeventhandler.aspx
    //http://msdn.microsoft.com/en-us/library/hh212048(v=vs.103).aspx
    public class FromEventSubject<T> : BaseSubject<T>
    {
        private object control;
        private string eventName;
        public T Value;

        public class EventAdd<T>
        {
            public FromEventSubject<T> subject;

            public void AddValue(object obj, T e)
            {
                subject.Value = e;
                subject.Execute();
            }
        }

        public FromEventSubject(object control, string eventName)
        {
            this.control = control;
            this.eventName = eventName;            
            var eventInfo = control.GetType().GetEvent(eventName);            
            Delegate del = CreateDelegate(eventInfo.EventHandlerType);        
            eventInfo.AddEventHandler(control, del);            
        }
        
        private Delegate CreateDelegate(Type delegateType)
        {
            Type[] methodArgs = { typeof(EventAdd<T>), typeof(object), typeof(T) };
            DynamicMethod handleEvent = new DynamicMethod(
            "HandleEventMethode",
            null,
            methodArgs,
            typeof(FromEventSubject<T>).Module);
            MethodInfo addValueInfo = typeof(EventAdd<T>).GetMethod("AddValue",
                BindingFlags.Public | BindingFlags.Instance);
            ILGenerator ilGenerator = handleEvent.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.EmitCall(OpCodes.Call, addValueInfo, null);            
            ilGenerator.Emit(OpCodes.Ret);
            EventAdd<T> eventadd = new EventAdd<T>();
            eventadd.subject = this;
            //handleEvent.Invoke(eventadd, new object[] { eventadd, new object(), new EventArgs() });
            return handleEvent.CreateDelegate(delegateType, eventadd);            
        }

        public override void Execute()
        {
            if (Value != null && !Value.Equals(default(T)))
            {
                NotifyObservers(Value);                
            }
        }
    }

    public class FromEventActionSubject<TEventArgs> : BaseSubject<TEventArgs>
    {
        private Action<Action<TEventArgs>> addHandler;
        private Action<Action<TEventArgs>> removeHandler;

        public FromEventActionSubject(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler)
        {
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
        }

        private Action<TEventArgs> action = null;

        public override void Execute()
        {
            if (action == null)
            {
                action = a =>
                    {
                        if (observers.Count == 0)
                        {
                            removeHandler(action);
                            return;
                        }
                        NotifyObservers(a);                        
                    };
            }
            addHandler(action);
        }
    }

    public class FromEventFuncSubject<TDelegate, TEventArgs> : BaseSubject<TEventArgs>
    {
        private Action<TDelegate> addHandler;
        private Action<TDelegate> removeHandler;
        private Func<Action<TEventArgs>, TDelegate> conversion;

        protected FromEventFuncSubject(Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;            
        }

        public FromEventFuncSubject(Func<Action<TEventArgs>, TDelegate> conversion, 
            Action<TDelegate> addHandler, Action<TDelegate> removeHandler) : this(addHandler, removeHandler)
        {            
            this.conversion = conversion;
        }

        private Action<TEventArgs> action = null;

        public override void Execute()
        {            
            TDelegate convert = default(TDelegate);
            if (action == null)
            {
                action = a =>
                {
                    if (observers.Count == 0)
                    {
                        removeHandler(convert);
                        return;
                    }
                    NotifyObservers(a);
                    //foreach (var observer in observers)
                    //{
                    //    observer.OnNext(a);
                    //}
                };
            }
            convert = conversion(action);
            addHandler(convert);            
        }
    }

    public class FromEventFuncGenericSubject<TDelegate, TEventArgs> : BaseSubject<TEventArgs>
    {
        private Action<TDelegate> addHandler;
        private Action<TDelegate> removeHandler;
        private Func<Action<TEventArgs>, TDelegate> conversion;

        public FromEventFuncGenericSubject(Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
        }

        private Action<TEventArgs> action = null;

        public static TResult ConvertActionToDelegate<TResult>(MulticastDelegate source)
        {
            Delegate result = null;
            foreach (Delegate sourceItem in source.GetInvocationList())
            {
                var copy = Delegate.CreateDelegate(typeof(TResult), sourceItem.Target, sourceItem.Method);
                result = Delegate.Combine(result, copy);
            }
            return (TResult)(object)result;
        }

        public static TDelegate CreateFunc(Action<TEventArgs> f)
        {            
            Action<object, TEventArgs> func = (o, e) => { f(e); };
            TDelegate tdelegate = ConvertActionToDelegate<TDelegate>(func);         
            return tdelegate;  
        }

        //private Func<Action<TEventArgs>, TDelegate> GenerateDynamicFunc()
        //{
        //    MethodInfo methodInfoCreateFunc = typeof(FromEventFuncGenericEventHandlerSubject<TDelegate, TEventArgs>).
        //        GetMethod("CreateFunc", new Type[] { typeof(Action<TEventArgs>) });   
        //    ParameterExpression parameter = Expression.Parameter(typeof(Action<TEventArgs>), "f");
        //    ParameterExpression resultVariable = Expression.Variable(typeof(TDelegate));
        //    LabelTarget returnTarget = Expression.Label();           
        //    BlockExpression blockExpression = Expression.Block(new Expression[] { 
        //        Expression.Assign(resultVariable, Expression.Call(methodInfoCreateFunc, parameter)),
        //        Expression.Return(returnTarget),
        //        Expression.Label(returnTarget),
        //        resultVariable});
        //    Expression finalExpression = Expression.Block(new[] { resultVariable },
        //                                     blockExpression);
        //    Expression<Func<Action<TEventArgs>, TDelegate>> expression =
        //        Expression.Lambda<Func<Action<TEventArgs>, TDelegate>>(finalExpression, new ParameterExpression[] { parameter });
        //    return expression.Compile();
        //}

        public override void Execute()
        {
            //Func<Action<TEventArgs>, TDelegate> compileFunc = GenerateDynamicFunc();
            TDelegate convertFunc = default(TDelegate);
            if (action == null)
            {
                action = a =>
                {
                    if (observers.Count == 0)
                    {
                        removeHandler(convertFunc);
                        return;
                    }
                    NotifyObservers(a);
                    //foreach (var observer in observers)
                    //{
                    //    observer.OnNext(a);
                    //}
                };
            }
            convertFunc = CreateFunc(action);
            //convertFunc = compileFunc(action);
            addHandler(convertFunc);            
        }
    }

    public class FromEventFuncEventHandlerSubject : BaseSubject<EventArgs>
    {
        private Action<EventHandler> addHandler;
        private Action<EventHandler> removeHandler;
        private Func<Action<EventArgs>, EventHandler> conversion;

        public FromEventFuncEventHandlerSubject(Action<EventHandler> addHandler, Action<EventHandler> removeHandler)
        {
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
        }

        private Action<EventArgs> action = null;

        public override void Execute()
        {
            if (conversion == null)
            {
                EventHandler eventHandler = null;
                conversion = f => new EventHandler((o, e) => f(e));
                if (action == null)
                {
                    action = a =>
                    {
                        if (observers.Count == 0)
                        {
                            removeHandler(eventHandler);
                            return;
                        }
                        NotifyObservers(a);
                        //foreach (var observer in observers)
                        //{
                        //    observer.OnNext(a);
                        //}
                    };
                    eventHandler = conversion(action);
                    addHandler(eventHandler);
                }
            }
        }
    }
}
