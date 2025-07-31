using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using TM = System.Threading.Tasks;
namespace Sistran.Core.Application.Utilities.Utility
{
    /// <summary>
    /// Paralelismo
    /// </summary>
    public class Parallel
    {

        /// <summary>
        /// Lista en Paralelo
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="body">The body.</param>
        /// <param name="parallelOptions">The parallel options.</param>
        /// <param name="key">The key.</param>
        /// <exception cref="ArgumentNullException">source</exception>
        public static void ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body, TM.ParallelOptions parallelOptions = null, string key = "DebugParallel")
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            BusinessContext context = null;
            if (BusinessContext.Current != null)
            {
                context = BusinessContext.Current;
            }
            if (parallelOptions == null)
            {
                parallelOptions = new TM.ParallelOptions() { MaxDegreeOfParallelism = KeySettings.GetSetting(key, Environment.ProcessorCount) };
            }
            TM.Parallel.ForEach(source, parallelOptions, x =>
            {
                bool dispose = false;
                try
                {
                    if (Context.Current == null)
                    {
                        new Context();
                        dispose = true;
                    }
                    if (BusinessContext.Current == null)
                    {
                        BusinessContext.Current = context;
                    }
                    Context.Current["UserId"] = context?.UserId;
                    Context.Current["IpAddress"] = context?.IPAddress;
                    Context.Current["AccountName"] = context?.AccountName;
                    Context.Current["CultureName"] = context?.CultureName;
                    body(x);
                }
                finally
                {
                    if (dispose)
                        DataFacadeManager.Dispose();
                }
            });
        }

        /// <summary>
        /// Lista en Paralelo
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="body">The body.</param>
        /// <param name="parallelOptions">The parallel options.</param>
        /// <param name="key">The key.</param>
        /// <exception cref="ArgumentNullException">source</exception>
        public static void ForEach<TSource>(IEnumerable<TSource> source, Action<TSource, TM.ParallelLoopState> body, TM.ParallelOptions parallelOptions = null, string key = "DebugParallel")
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            BusinessContext context = null;
            if (BusinessContext.Current != null)
            {
                context = BusinessContext.Current;
            }
            if (parallelOptions == null)
            {
                parallelOptions = new TM.ParallelOptions() { MaxDegreeOfParallelism = KeySettings.GetSetting(key, Environment.ProcessorCount) };
            }
            TM.Parallel.ForEach(source, parallelOptions, (x, y) =>
            {
                bool dispose = false;
                try
                {
                    if (Context.Current == null)
                    {
                        new Context();
                        dispose = true;
                    }
                    if (BusinessContext.Current == null)
                    {
                        BusinessContext.Current = context;
                    }
                    Context.Current["UserId"] = context?.UserId;
                    Context.Current["IpAddress"] = context?.IPAddress;
                    Context.Current["AccountName"] = context?.AccountName;
                    Context.Current["CultureName"] = context?.CultureName;
                    body(x, y);
                }
                finally
                {
                    if (dispose)
                        DataFacadeManager.Dispose();
                }
            });
        }


        //
        // Resumen:
        //     Executes a foreach (For Each in Visual Basic) operation with thread-local data
        //     on an System.Collections.IEnumerable in which iterations may run in parallel,
        //     and the state of the loop can be monitored and manipulated.
        //
        // Parámetros:
        //   source:
        //     An enumerable data source.
        //
        //   localInit:
        //     The function delegate that returns the initial state of the local data for each
        //     task.
        //
        //   body:
        //     The delegate that is invoked once per iteration.
        //
        //   localFinally:
        //     The delegate that performs a final action on the local state of each task.
        //
        // Parámetros de tipo:
        //   TSource:
        //     The type of the data in the source.
        //
        //   TLocal:
        //     The type of the thread-local data.
        //
        // Devuelve:
        //     A structure that contains information about which portion of the loop completed.
        //
        // Excepciones:
        //   T:System.ArgumentNullException:
        //     The source argument is null.-or-The body argument is null.-or-The localInit argument
        //     is null.-or-The localFinally argument is null.
        //
        //   T:System.AggregateException:
        //     The exception that contains all the individual exceptions thrown on all threads.
        public static TM.ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, TM.ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally, TM.ParallelOptions parallelOptions = null, string key = "DebugParallel")
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            BusinessContext context = null;
            if (BusinessContext.Current != null)
            {
                context = BusinessContext.Current;
            }
            if (parallelOptions == null)
            {
                parallelOptions = new TM.ParallelOptions() { MaxDegreeOfParallelism = KeySettings.GetSetting(key, Environment.ProcessorCount) };
            }
            return TM.Parallel.ForEach<TSource, TLocal>
                (
                    source,
                    localInit,
                    (t1, state, t2) =>
                    {
                        bool dispose = false;
                        try
                        {
                            if (Context.Current == null)
                            {
                                new Context();
                                dispose = true;
                            }
                            if (BusinessContext.Current == null)
                            {
                                BusinessContext.Current = context;
                            }
                            Context.Current["UserId"] = context?.UserId;
                            Context.Current["IpAddress"] = context?.IPAddress;
                            Context.Current["AccountName"] = context?.AccountName;
                            Context.Current["CultureName"] = context?.CultureName;
                            return body(t1, state, t2);
                        }
                        finally
                        {
                            if (dispose)
                                DataFacadeManager.Dispose();
                        }

                    },
                    localFinally
                );
        }

        /// <summary>
        /// Fors the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="body">The body.</param>
        /// <param name="parallelOptions">The parallel options.</param>
        /// <param name="key">The key.</param>
        /// <exception cref="ArgumentNullException">source</exception>
        public static void For(int from, int to, Action<int> body, TM.ParallelOptions parallelOptions = null, string key = "DebugParallel")
        {
            if (body == null)
            {
                throw new ArgumentNullException("source");
            }
            BusinessContext context = null;
            if (BusinessContext.Current != null)
            {
                context = BusinessContext.Current;
            }
            if (parallelOptions == null)
            {
                parallelOptions = new TM.ParallelOptions() { MaxDegreeOfParallelism = KeySettings.GetSetting(key, Environment.ProcessorCount) };
            }
            TM.Parallel.For(from, to, parallelOptions, (index) =>
            {
                bool dispose = false;
                try
                {
                    if (Context.Current == null)
                    {
                        new Context();
                        dispose = true;
                    }
                    if (BusinessContext.Current == null)
                    {
                        BusinessContext.Current = context;
                    }
                    Context.Current["UserId"] = context?.UserId;
                    Context.Current["IpAddress"] = context?.IPAddress;
                    Context.Current["AccountName"] = context?.AccountName;
                    Context.Current["CultureName"] = context?.CultureName;
                    body(index);
                }
                finally
                {
                    if (dispose)
                        DataFacadeManager.Dispose();
                }
            });
        }

        /// <summary>
        /// Fors the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="body">The body.</param>
        /// <param name="parallelOptions">The parallel options.</param>
        /// <param name="key">The key.</param>
        /// <exception cref="ArgumentNullException">source</exception>
        public static void For(int from, int to, Action<int, TM.ParallelLoopState> body, TM.ParallelOptions parallelOptions = null, string key = "DebugParallel")
        {
            if (body == null)
            {
                throw new ArgumentNullException("source");
            }
            BusinessContext context = null;
            if (BusinessContext.Current != null)
            {
                context = BusinessContext.Current;
            }
            if (parallelOptions == null)
            {
                parallelOptions = new TM.ParallelOptions() { MaxDegreeOfParallelism = KeySettings.GetSetting(key, Environment.ProcessorCount) };
            }
            TM.Parallel.For(from, to, parallelOptions, (index, state) =>
            {
                bool dispose = false;
                try
                {
                    if (Context.Current == null)
                    {
                        new Context();
                        dispose = true;
                    }
                    if (BusinessContext.Current == null)
                    {
                        BusinessContext.Current = context;
                    }
                    Context.Current["UserId"] = context?.UserId;
                    Context.Current["IpAddress"] = context?.IPAddress;
                    Context.Current["AccountName"] = context?.AccountName;
                    Context.Current["CultureName"] = context?.CultureName;
                    body(index, state);
                }
                finally
                {
                    if (dispose)
                        DataFacadeManager.Dispose();
                }
            });
        }

        /// <summary>
        /// Fors the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="body">The body.</param>
        /// <param name="parallelOptions">The parallel options.</param>
        /// <param name="key">The key.</param>
        /// <exception cref="ArgumentNullException">source</exception>
        public static void For(long from, long to, Action<long> body, TM.ParallelOptions parallelOptions = null, string key = "DebugParallel")
        {
            if (body == null)
            {
                throw new ArgumentNullException("source");
            }
            BusinessContext context = null;

            if (BusinessContext.Current != null)
            {
                context = BusinessContext.Current;
            }
            if (parallelOptions == null)
            {
                parallelOptions = new TM.ParallelOptions() { MaxDegreeOfParallelism = KeySettings.GetSetting(key, Environment.ProcessorCount) };
            }
            TM.Parallel.For(from, to, parallelOptions, index =>
            {
                bool dispose = false;
                try
                {
                    if (Context.Current == null)
                    {
                        new Context();
                        dispose = true;
                    }
                    if (BusinessContext.Current == null)
                    {
                        BusinessContext.Current = context;
                    }
                    Context.Current["UserId"] = context?.UserId;
                    Context.Current["IpAddress"] = context?.IPAddress;
                    Context.Current["AccountName"] = context?.AccountName;
                    Context.Current["CultureName"] = context?.CultureName;
                    body(index);
                }
                finally
                {
                    if(dispose)
                        DataFacadeManager.Dispose();
                }
            });
        }
    }
}
