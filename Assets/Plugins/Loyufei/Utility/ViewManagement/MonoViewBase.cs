using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Loyufei.ViewManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MonoViewBase : MonoBehaviour, IView
    {
        public MonoViewBase() : base () 
        {
            CreateLayoutHandler();
        }

        [SerializeField]
        private bool          _InitActive;
        [SerializeField]
        protected CanvasGroup _CanvasGroup;
        [SerializeField, Range(0f, 1f)]
        protected float       _FadeDuration = 0.5f;

        protected LayoutHandler _LayoutHandler;

        public virtual object ViewId { get; }

        #region Unity Behaviour

        protected virtual void Awake()
        {
            gameObject.SetActive(_InitActive);
        }

        #endregion

        #region Public Methods

        public virtual IObservable<long> Open()
        {
            return ChangeState(true);
        }

        public virtual IObservable<long> Close()
        {
            return ChangeState(false);
        }

        public virtual ILayout Layout() 
        {
            _LayoutHandler.Setup(GetListenerAdapter().ToArray());

            return _LayoutHandler;
        }

        public virtual void RemoveLayout() 
        {
            
        }

        #endregion

        #region Protected Methods

        protected virtual void CreateLayoutHandler() 
        {
            _LayoutHandler = new();
        }

        protected virtual IEnumerable<IListenerAdapter> GetListenerAdapter ()
        {
            foreach(IListenerAdapter listener in GetComponentsInChildren<IListenerAdapter>()) 
            {
                yield return listener;
            }
        }

        #endregion

        public virtual IObservable<long> ChangeState(bool isOn)
        {
            if (isOn) { gameObject.SetActive(true); }

            var canvasGroup = _CanvasGroup;

            canvasGroup.interactable = false;

            var (start, end, delta) = isOn ? (0f, 1f, 1f) : (1f, 0f, -1f);
            var multiplex = 1 / _FadeDuration;

            canvasGroup.alpha = start;

            var fading = Observable
                .EveryUpdate()
                .TakeWhile(l => canvasGroup.alpha != end);

            fading.Subscribe((l =>
            {
                var fade = canvasGroup.alpha;

                canvasGroup.alpha = (fade + delta * multiplex * Time.deltaTime).Clamp01();
            }));

            fading
                .Last()
                .Subscribe(l => 
                {
                    canvasGroup.interactable = isOn;

                    gameObject.SetActive(isOn);
                });

            return fading;
        }

        protected class LayoutHandler : ILayout
        {
            public List<IListenerAdapter> Listeners { get; } = new();

            public void Setup(IEnumerable<IListenerAdapter> listeners) 
            {
                Listeners.Clear();

                Listeners.AddRange(listeners);
            }

            public void BindListenerAll<TListener>(Action<IListenerAdapter> callBack) where TListener : IListenerAdapter 
            {
                foreach (var listener in Listeners)
                {
                    if (listener is TListener target)
                    {
                        target.AddListener(callBack);
                    }
                }
            }

            public void BindListener<TListener>(int id, Action<IListenerAdapter> callBack) where TListener : IListenerAdapter
            {
                foreach (var listener in Listeners)
                {
                    if (listener is TListener target && target.Id == id)
                    {
                        target.AddListener(callBack);
                    }
                }
            }

            public IEnumerable<T> FindAll<T>()
            {
                foreach (var listener in Listeners) 
                {
                    if (listener is T target) 
                    {
                        yield return target;
                    }
                }
            }

            public T Find<T>(Func<T, bool> match)
            {
                foreach (var listener in Listeners)
                {
                    if (listener is T target && match.Invoke(target))
                    {
                        return target;
                    }
                }

                return default;
            }
        }
    }
}