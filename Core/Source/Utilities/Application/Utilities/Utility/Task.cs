using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using System;
using System.Threading;
using TM = System.Threading.Tasks;
namespace Sistran.Core.Application.Utilities.Utility
{
    /// <summary>
    /// Manejo Hilos Encapsulado
    /// </summary>
    public class Task
    {
        #region Hilos
        /// <summary>
        /// Ejecucion Por Hilos
        /// </summary>
        /// <param name="action">Accion a ejecutar.</param>
        /// <returns></returns>
        public static TM.Task Run(Action action)
        {
            CancellationToken ct = new CancellationToken();
            var context = BusinessContext.Current;
            return TM.Task.Run(() =>
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
                    action();
                }
                finally
                {
                    BusinessContext.Current = null;
                    if (dispose)
                        DataFacadeManager.Dispose();
                }
            }, ct);
        }

        /// <summary>
        /// Runs the specified function.
        /// </summary>
        /// <typeparam name="Tsource">The type of the source.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public static TM.Task<Tsource> Run<Tsource>(Func<Tsource> function)
        {
            var context = BusinessContext.Current;
            return TM.Task.Run<Tsource>(() =>
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
                    return function();
                }
                finally
                {
                    BusinessContext.Current = null;
                    if (dispose)
                        DataFacadeManager.Dispose();
                }
            });
        }

        /// <summary>
        /// Runs the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="ct">The ct.</param>
        /// <returns></returns>
        public static TM.Task Run(Action action, CancellationToken ct)
        {
            var context = BusinessContext.Current;
            return TM.Task.Run(() =>
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

                    action();
                }
                finally
                {
                    BusinessContext.Current = null;
                    if (dispose)
                        DataFacadeManager.Dispose();
                }
            }, ct);

        }
        #endregion hilos
    }
}
