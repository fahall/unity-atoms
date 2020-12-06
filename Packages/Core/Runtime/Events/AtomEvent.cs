using System;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Generic base class for Events. Inherits from `AtomEventBase`.
    /// </summary>
    /// <typeparam name="T">The type for this Event.</typeparam>
    [EditorIcon("atom-icon-cherry")]
    public class AtomEvent<T> : AtomEventBase
    {

        /// <summary>
        /// Retrieve Replay Buffer as a List. This call will allocate memory so use sparsely.
        /// </summary>
        /// <returns></returns>
        public List<T> ReplayBuffer => _replayBuffer.ToList();

        public int ReplayBufferSize { get => _replayBufferSize; set => _replayBufferSize = value; }

        [SerializeField]
        protected event Action<T> _onEvent;

        /// <summary>
        /// The event replays the specified number of old values to new subscribers. Works like a ReplaySubject in Rx.
        /// </summary>
        [SerializeField]
        [Range(0, 10)]
        [Tooltip("The number of old values (between 0-10) being replayed when someone subscribes to this Event.")]
        private int _replayBufferSize = 1;

        private Queue<T> _replayBuffer = new Queue<T>();

        private void OnDisable()
        {
            // Clear all delegates when exiting play mode
            if (_onEvent != null)
            {
                var invocationList = _onEvent.GetInvocationList();
                foreach (var d in invocationList)
                {
                    _onEvent -= (Action<T>)d;
                }
            }
        }


        /// <summary>
        /// Raise the Event.
        /// </summary>
        /// <param name="item">The value associated with the Event.</param>
        [Button] public void Raise(T item)
        {
#if !UNITY_ATOMS_GENERATE_DOCS && UNITY_EDITOR
            //StackTraces.AddStackTrace(GetInstanceID(), StackTraceEntry.Create(item));
#endif
            base.Raise();
            _onEvent?.Invoke(item);
            AddToReplayBuffer(item);
        }


        /// <summary>
        /// Register handler to be called when the Event triggers.
        /// </summary>
        /// <param name="action">The handler.</param>
        public void Register(Action<T> action)
        {
            _onEvent += action;
            ReplayBufferToSubscriber(action);
        }

        /// <summary>
        /// Unregister handler that was registered using the `Register` method.
        /// </summary>
        /// <param name="action">The handler.</param>
        public void Unregister(Action<T> action)
        {
            _onEvent -= action;
        }

        /// <summary>
        /// Unregister all handlers that were registered using the `Register` method.
        /// </summary>
        public void UnregisterAll()
        {
            _onEvent = null;
        }

        /// <summary>
        /// Register a Listener that in turn trigger all its associated handlers when the Event triggers.
        /// </summary>
        /// <param name="listener">The Listener to register.</param>
        public void RegisterListener(IAtomListener<T> listener, bool replayEventsBuffer = true)
        {
            _onEvent += listener.HandleInput;
            if (replayEventsBuffer)
            {
                ReplayBufferToSubscriber(listener.HandleInput);
            }
        }

        /// <summary>
        /// Unregister a listener that was registered using the `RegisterListener` method.
        /// </summary>
        /// <param name="listener">The Listener to unregister.</param>
        public void UnregisterListener(IAtomListener<T> listener)
        {
            _onEvent -= listener.HandleInput;
        }

        #region Observable
        /// <summary>
        /// Turn the Event into an `IObservable&lt;T&gt;`. Makes Events compatible with for example UniRx.
        /// </summary>
        /// <returns>The Event as an `IObservable&lt;T&gt;`.</returns>
        public IObservable<T> Observe()
        {
            return new ObservableEvent<T>(Register, Unregister);
        }
        #endregion // Observable

        protected void AddToReplayBuffer(T item)
        {
            if (_replayBufferSize > 0)
            {
                while (_replayBuffer.Count >= _replayBufferSize) { _replayBuffer.Dequeue(); }
                _replayBuffer.Enqueue(item);
            }
        }

        private void ReplayBufferToSubscriber(Action<T> action)
        {
            if (_replayBufferSize > 0 && _replayBuffer.Count > 0)
            {
                var enumerator = _replayBuffer.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        action(enumerator.Current);
                    }
                }
                finally
                {
                    enumerator.Dispose();
                }
            }
        }
    }
}
