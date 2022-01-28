using System;
using System.Collections.Generic;

namespace EthanZarov.SimpleTools.Pooling
{
    public class ObjectPool<T> where T : ICloneable
    {
        private T _baseItem;
        private Queue<T> _pool;
        
        #region Constructors
        public ObjectPool(T baseItem)
        {
            _baseItem = baseItem;
            _pool = new Queue<T>();
        }

        /// <summary>
        /// Creates the base ObjectPool.
        /// </summary>
        /// <param name="baseItem">Base item that's duplicated within the ObjectPool.</param>
        /// <param name="objCount"></param>
        public ObjectPool(T baseItem, int objCount)
        {
            _baseItem = baseItem;
            _pool = new Queue<T>();
            InitializePoolCount(objCount);
        }
        
            
        /// <summary>
        /// Set a starting pool count.
        /// </summary>
        /// <param name="numberOfObjects">Number of baseItems to put into pool.</param>
        public void InitializePoolCount(int numberOfObjects)
        {
            var c = numberOfObjects - _pool.Count;
            if (c <= 0) return;
            
            AddObjects(c);
        }

        #endregion

        #region Add/Remove to Pool

        

        /// <summary>
        /// Add the base object to ObjectPool.
        /// </summary>
        public void AddObject()
        {
            if (_baseItem == null) return;
            _pool.Enqueue((T)_baseItem.Clone());
        }

        /// <summary>
        /// Add an object to the ObjectPool that isn't the base object.
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(T obj)
        {
            _pool.Enqueue(obj);
        }

        /// <summary>
        /// Add x base objects to pool.
        /// </summary>
        /// <param name="count">Total amount of objects to add to pool.</param>
        public void AddObjects(int count)
        {
            while (count > 0)
            {
                AddObject();
                count--;
            }
        }

        #endregion
        
        
        /// <summary>
        /// Get an object from the ObjectPool.
        /// </summary>
        /// <returns>Object taken out of the pool.</returns>
        public T Get()
        {
            var obj = _pool.Dequeue();
            return obj;
        }

        /// <summary>
        /// Return an object to the ObjectPool.
        /// </summary>
        /// <param name="obj">Object to return.</param>
        public void Return(T obj)
        {
            _pool.Enqueue(obj);
        }
    }
}

