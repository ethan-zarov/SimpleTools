using System;
using System.Collections.Generic;
using UnityEngine;

namespace EthanZarov.SimpleTools.Pooling
{
    /// <summary>
    /// A tetris bag draws from a pool of objects.
    /// Once an object is drawn, it cannot be drawn again until every other object is drawn.
    /// Once every object is drawn, they are all reshuffled into the bag.
    /// </summary>
    /// <typeparam name="T">Type of items stored.</typeparam>
    public class TetrisBag<T> where T : ICloneable
    {
        private bool _debugMode;
        
        private IList<T> _currentDrawBag;
        private readonly IList<T> _currentDiscard;
        public int TotalItemCount => _currentDrawBag.Count + _currentDiscard.Count;

        #region Constructors
        /// <summary>
        /// Constructor for TetrisBag
        /// </summary>
        /// <param name="allItems">List of all items stored in a bag.</param>
        public TetrisBag(IList<T> allItems)
        {
            _currentDrawBag = new List<T>();
            _currentDiscard = new List<T>();
            
            _currentDrawBag = allItems.Clone();
            _currentDrawBag.Shuffle();

        }

        /// <summary>
        /// Constructor for TetrisBag
        /// </summary>
        /// <param name="allItems">List of all items stored in a bag.</param>
        /// <param name="enableDebugMode">If true, TetrisBag events will log to console.</param>
        public TetrisBag(IList<T> allItems, bool enableDebugMode)
        {
            _currentDrawBag = new List<T>();
            _currentDiscard = new List<T>();
            
            _currentDrawBag = allItems.Clone();
            _currentDrawBag.Shuffle();

            SetDebugMode(enableDebugMode);
            if (_debugMode) Debug.Log($"Bag created with {TotalItemCount} items of type {typeof(T).ToString()}.");
        }
        
        /// <summary>
        /// Enables or disables DebugMode,
        /// which logs events within the bag.
        /// </summary>
        public void SetDebugMode(bool enabled)
        {
            _debugMode = enabled;
        }
        #endregion
        
        #region Bag Shuffle/Reloading
        /// <summary>
        /// Reshuffles the current bag, without adding back discard pile.
        /// </summary>
        public void ReshuffleBag()
        {
            _currentDrawBag.Shuffle();
            if (_debugMode) Debug.Log("Reshuffled current bag.");
        }

        /// <summary>
        /// Returns all items from discard back to main bag.
        /// </summary>
        /// <param name="shuffleAll">If true, shuffle all cards together.
        /// If false, shuffles discard pile, and puts all its items at the very back of the draw bag.</param>
        public void ReloadBag(bool shuffleAll)
        {
            //Shuffle the discard pile that will go at the back if it won't get shuffled later.
            if (!shuffleAll) _currentDiscard.Shuffle(); 
            
            //Add discard to back of list
            foreach (var t in _currentDiscard)
            {
                _currentDrawBag.Add(t);
            }

            //If shuffleAll, shuffle them all together.
            if (shuffleAll) _currentDrawBag.Shuffle();
            
            if (_debugMode) Debug.Log("Reloaded current bag.");
        }
        
        /// <summary>
        /// Check whether the current draw is empty, and if so, reshuffle.
        /// </summary>
        private void CheckForEmptyBag()
        {
            if (_currentDrawBag.Count != 0) return;
            if (_debugMode) Debug.Log("Bag empty! Reshuffling");
            
            _currentDrawBag = _currentDiscard.Clone();
            _currentDiscard.Clear();
            _currentDrawBag.Shuffle();
        }
        #endregion

        #region Item Retrieval
        /// <summary>
        /// Draw the next element from the TetrisBag.
        /// Item drawn goes to the discard pile.
        /// </summary>
        /// <returns>Item drawn from the TetrisBag.</returns>
        public T DrawFromBag()
        {
            CheckForEmptyBag();
            T objDrawn = _currentDrawBag[0];
            _currentDrawBag.RemoveAt(0);
            _currentDiscard.Add(objDrawn);

            return objDrawn;
        }
        
        /// <summary>
        /// Look at an item coming up in the draw bag.
        /// </summary>
        /// <param name="index">Index to peek at within bag.</param>
        /// <returns>Item found at that index.</returns>
        public T PeekBag(int index)
        {
            if (index < _currentDrawBag.Count && index >= 0) return _currentDrawBag[index];
            Debug.LogWarning($"Index {index} in TetrisBag is OutOfRange!");
            return default(T);
        }
        
        /// <summary>
        /// Look at an item coming up in the discard bag.
        /// </summary>
        /// <param name="index">Index to peek at within the discard bag.</param>
        /// <returns>Item found at that index.</returns>
        public T PeekDiscard(int index)
        {
            if (index < _currentDiscard.Count && index >= 0) return _currentDiscard[index];
            Debug.LogWarning($"Index {index} in discard of TetrisBag is OutOfRange!");
            return default(T);

        }

        /// <summary>
        /// Adds a new item that shuffles around in TetrisBag.
        /// </summary>
        /// <param name="item">New item that will get shuffled through TetrisBag.</param>
        /// <param name="startInPool">If false, item will start in discard pile rather than end of current draw bag.</param>
        public void AddNewItemToBag(T item, bool startInPool)
        {
            if (startInPool) _currentDrawBag.Add(item);
            else _currentDiscard.Add(item);

            if (!_debugMode) return;
            var s = startInPool ? "draw" : "discard";
            Debug.Log($"Added {item.ToString()} to {s} pile of TetrisBag.");
        }

        /// <summary>
        /// Attempts to remove an item from the bag permanently.
        /// </summary>
        /// <param name="item">Item to permanently remove from TetrisBag.</param>
        public void RemoveItemFromBag(T item)
        {
            _currentDrawBag.Remove(item);
            _currentDiscard.Remove(item);
        }
        #endregion
    }
   
}