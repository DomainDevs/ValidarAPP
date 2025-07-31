using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Sistran.Core.Application.CommonService.Helper
{
    public class ThreadManager
    {
        private List<ManualResetEvent> ManualResetEvents;

        public ThreadManager()
        {
            this.ManualResetEvents = new List<ManualResetEvent>();
        }

        public void Run<T>(string methodName, object[] parameters = null)
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            MethodInfo methodInfo = typeof(T).GetMethod(methodName);
            object instanceMethod = Activator.CreateInstance<T>();

            ManualResetEvents.Add(manualResetEvent);

            ThreadPool.QueueUserWorkItem(
            delegate
            {
                try
                {
                    methodInfo.Invoke(instanceMethod, parameters);
                }
                catch (Exception)
                {
                    ManualResetEvents.Remove(manualResetEvent);
                };
                manualResetEvent.Set();
            }, manualResetEvent);
        }

        public void WaitAll()
        {
            if (ManualResetEvents != null && ManualResetEvents.Count > 0)
            {
                WaitHandle.WaitAll(ManualResetEvents.ToArray(), -1);
            }
        }
    }
}