using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Sistran.Core.Application.CommonService.Helper
{
    public class TaskManager
    {

         private ConcurrentBag<Task> tasks;

         public TaskManager()
        {
            this.tasks = new ConcurrentBag<Task>();
        }

        public void Run<T>(string methodName, object[] parameters = null)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(methodName);
            object instanceMethod = Activator.CreateInstance<T>();
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            tasks.Add(Task.Run(() => methodInfo.Invoke(instanceMethod, parameters), token));
        }

        public void WaitAll()
        {
            if (tasks != null && tasks.Count > 0)
            {
                Task.WaitAll(tasks.ToArray());
            }
        }
    }
}
