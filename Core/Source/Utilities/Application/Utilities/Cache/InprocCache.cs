using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
namespace Sistran.Core.Application.Utilities.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InProcCache
    {
        private static object syncRoot = new Object();
        private static ConcurrentDictionary<string, ConcurrentDictionary<string, object>> singleTypes;
        private static int currentVersion;
        private static volatile InProcCache instance;
        public static ConcurrentDictionary<string, object> StateCache = new ConcurrentDictionary<string, object>();
        public static ConcurrentDictionary<string, ConcurrentDictionary<string, object>> StateCacheCurrent = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();
        private Hashtable items;
        public static InProcCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new InProcCache();
                        }
                    }
                }

                return instance;
            }
        }
        #region version anterior
        private InProcCache()
        {
            items = Hashtable.Synchronized(new Hashtable());
        }

        [Obsolete("usar InsertCurrent")]
        public void Insert(string key, object item)
        {
            lock (instance.items.SyncRoot)
            {
                instance.items[key] = item;
            }
        }
        [Obsolete("usar GetCurrent")]
        public object Get(string key)
        {
            return instance.items[key];
        }
        public bool ContainsValue(object value)
        {
            return instance.items.ContainsValue(value);
        }
        public bool Containskey(string key)
        {
            return instance.items.ContainsKey(key);
        }
        #endregion

        /// <summary>
        /// Flushes the specified class name.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Flush(string className)
        {
            throw new System.NotImplementedException();
        }

        #region diccionario
        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public object GetCurrent<T>(string key, Func<T, bool> predicate)
        {
            if (StateCache != null && StateCache.ContainsKey(key))
            {
                var t = ((List<T>)StateCache[key]).Cast<T>().ToList().Where(predicate).FirstOrDefault();
                return t;
            }
            {
                return null;
            }

        }

        /// <summary>
        /// Gets the current list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Llave de busqueda</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public object GetCurrentList<T>(string key, Func<T, bool> predicate = null)
        {
            if (StateCache.ContainsKey(key))
            {
                List<T> s = null;
                if (predicate != null)
                {
                    s = ((List<T>)StateCache[key]).Cast<T>().Where(predicate).ToList();
                }
                else
                {
                    s = ((List<T>)StateCache[key]).Cast<T>().ToList();
                }

                return s;
            }
            return null;
        }

        /// <summary>
        /// Gets the current dic.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="keyData">The key data.</param>
        /// <returns></returns>
        public object GetCurrentDic(string key, string keyData)
        {
            if (StateCacheCurrent != null && StateCacheCurrent.ContainsKey(key) && StateCacheCurrent[key].ContainsKey(keyData))
            {
                var t = StateCacheCurrent[key][keyData];
                return t;
            }
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Objeto de un diccionario
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="keyData">The key data.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public object GetCurrentDic<T>(string key, string keyData, Func<T, bool> predicate)
        {
            if (StateCacheCurrent != null && StateCacheCurrent.ContainsKey(key) && StateCacheCurrent[key].ContainsKey(keyData))
            {
                var t = StateCacheCurrent[key][keyData];
                return t;
            }
            {
                return null;
            }
        }

        /// <summary>
        /// Inserts the current.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        public void InsertCurrent<T>(string key, object item)
        {
            lock (syncRoot)
            {
                if (!StateCache.ContainsKey(key))
                {
                    Type type = item.GetType();
                    if (type.Name != typeof(List<>).Name)
                    {
                        List<T> items = new List<T>();
                        items.Add((T)item);
                        InProcCache.StateCache.TryAdd(key, items);
                    }
                    else
                    {
                        InProcCache.StateCache.TryAdd(key, item);
                    }
                }
                else
                {
                    ((List<T>)InProcCache.StateCache[key]).Add((T)item);
                }
            }
        }
        /// <summary>
        /// Inserts the current dic.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="keyHash">The key hash.</param>
        /// <param name="item">The item.</param>
        public void InsertCurrentDic(string key, string keyHash, object item)
        {
            lock (syncRoot)
            {
                if (!InProcCache.StateCacheCurrent.ContainsKey(key))
                {
                    ConcurrentDictionary<string, object> concurrentDictionary = new ConcurrentDictionary<string, object>();
                    concurrentDictionary.TryAdd(keyHash, item);
                    InProcCache.StateCacheCurrent.TryAdd(key, concurrentDictionary);
                }
                else
                {
                    InProcCache.StateCacheCurrent[key].TryAdd(keyHash, item);
                }
            }
        }
        public void RemoveCurrent(string key, object keyHash)
        {
            StateCache.TryRemove(key, out keyHash);
        }
        #endregion

        #region Masivos
        public static void Add(string type, string key, object item)
        {
            lock (syncRoot)
            {
                if (StateCacheCurrent.ContainsKey(type))
                {
                    object oldItem;
                    StateCacheCurrent[type].TryRemove(key, out oldItem);
                    StateCacheCurrent[type].TryAdd(key, item);
                }
                else
                {
                    ConcurrentDictionary<string, object> singleType = new ConcurrentDictionary<string, object>();
                    singleType.TryAdd(key, item);
                    StateCacheCurrent.TryAdd(type, singleType);
                }
            }
        }

        public static object Get(string type, string key)
        {
            if (StateCacheCurrent.ContainsKey(type))
            {
                object item;
                StateCacheCurrent.First(x => x.Key == type).Value.TryGetValue(key, out item);

                return item;
            }
            else
            {
                return null;
            }
        }

        public static void Remove(string type)
        {
            lock (syncRoot)
            {
                if (StateCacheCurrent.ContainsKey(type))
                {
                    ConcurrentDictionary<string, object> singleType;
                    StateCacheCurrent.TryRemove(type, out singleType);

                    GC.Collect();
                }
            }
        }
        #endregion
        public static void Flush()
        {
            lock (syncRoot)
            {
                singleTypes = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();
                GC.Collect();
            }
        }

        public static int GetCurrentVersion()
        {
            return currentVersion;
        }

        public static void SetCurrentVersion(int version)
        {
            lock (syncRoot)
            {
                currentVersion = version;
            }
        }
    }
}