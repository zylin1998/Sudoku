using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using Zenject;
using Loyufei;
using Loyufei.ViewManagement;

namespace Sudoku
{
    public class NumberListener : ButtonListener
    {
        [SerializeField]
        private TextMeshProUGUI _InteractText;
        [SerializeField, Inject]
        private ColorEffect     _ColorEffect;
        [SerializeField]
        private Vector2Int _Offset;

        public int Context { get; private set; }
        
        public Offset2DInt Offset { get => new(_Offset.x, _Offset.y); set => _Offset = new(value.X, value.Y); }

        public ColorEffect     ColorEffect  => _ColorEffect;
        public TextMeshProUGUI InteractText => _InteractText;

        public IObservable<long> Current { get; private set; }

        private void Awake()
        {
            _InteractText.enabled = false;

            Listener.image.color = ColorEffect.Normal;
        }

        public void SetContext(int context) 
        {
            Context = context; 
        }

        public void SetObservable(IObservable<long> stopable) 
        {
            Current.To<StopableObservable>()?.Stop();
            
            Current = stopable;

            Current.To<StopableObservable>()?.Start();
        }
    }

    public static class NumberListenerExtensions 
    {
        public static void Interact(this NumberListener self, int value, bool interactable)
        {
            self.SetContext(value);

            var isZero = value == 0;

            self.InteractText.enabled = isZero ? false : true;

            self.InteractText.SetText(self.Context.ToString());

            self.Listener.interactable = interactable;
        }

        public static void Clear(this NumberListener self)
        {
            self.SetContext(0);

            self.InteractText.enabled = false;

            self.Listener.interactable = true;
        }

        public static void Warning(this NumberListener self, bool clear = false)
        {
            var interactable = self.Listener.interactable;
            var image        = self.Listener.image;
            var current      = image.color;
            var warning      = self.ColorEffect.Warning;

            self.Listener.interactable = false;

            var loop = 0;
            var passtime = 0f;

            var stopable = new StopableObservable()
                .StopWhen(() =>
                {
                    var color = loop % 2 == 0 ? warning : current;

                    if (image.ChangeColor(color, 0.2f, ref passtime)) 
                    {
                        passtime = 0f;

                        loop++;
                    }

                    return loop >= 4;
                });

            stopable
                .Subscribe((l) => { }, () =>
                {
                    if (clear)
                    {
                        self.Clear();
                        
                        if (current == self.ColorEffect.Review) { self.Review(false); }
                    }

                    else self.Listener.interactable = interactable;
                });

            self.SetObservable(stopable);
        }

        public static void Review(this NumberListener self, bool isOn)
        {
            var interactable = self.Listener.interactable;
            var image        = self.Listener.image;
            var effect       = self.ColorEffect;
            var targetColor  = isOn ? effect.Review : effect.Normal;
            var listener     = self.Listener;
            
            listener.interactable = false;
            
            var passtime = 0f;

            var stopable = new StopableObservable()
                .StopWhen(() => image.ChangeColor(targetColor, 0.2f, ref passtime));

            stopable
                .Subscribe((l) => { }, () =>
                {
                    image.color = targetColor;

                    listener.interactable = interactable;
                });
            
            self.SetObservable(stopable);
        }
    }

    public class NumberPool : MemoryPool<Offset2DInt, NumberListener> 
    {
        public NumberPool() : base() 
        {
            DespawnRoot = new GameObject(typeof(NumberPool).Name).transform;
        }

        public Transform DespawnRoot { get; }

        public Action<IListenerAdapter> Binder { get; set; }

        protected override void Reinitialize(Offset2DInt offset, NumberListener number)
        {
            number.gameObject.SetActive(false);

            number.Offset = offset;

            number.Listener.interactable = false;
        }

        protected override void OnDespawned(NumberListener number)
        {
            number.Clear();

            number.gameObject.SetActive(false);
            
            number.transform.SetParent(DespawnRoot);
        }

        protected override void OnCreated(NumberListener listener)
        {
            listener.AddListener(Binder);
        }
    }

    public static class ImageExtension 
    {
        public static bool ChangeColor(this Image self, Color color, float duration, ref float passTime) 
        {
            if (self.color == color) { return true; }
            
            passTime =  passTime += Time.deltaTime * (1 / duration);

            self.color = Color.Lerp(self.color, color, passTime);
            
            return self.color == color;
        }
    }

    public class StopableObservable : IObservable<long>
    {
        private IObservable<long> source;

        public Subject<long> Subject { get; } = new();

        public bool Stopped { get; set; } = false;

        public Func<bool> ShouldStop { get; set; } = () => false;

        public IObservable<long> Start()
        {
            source = Observable
                .EveryUpdate()
                .TakeWhile((l) => !ShouldStop.Invoke() && !Stopped);

            source.Subscribe(Subject.OnNext, Subject.OnError, Subject.OnCompleted);
            
            return this;
        }

        public void Stop() 
        {
            Stopped = true;
        }

        public IObservable<long> StopWhen(Func<bool> predicate) 
        {
            ShouldStop = predicate;

            return this;
        }

        public IDisposable Subscribe(IObserver<long> observer)
        {
            return Subject?.Subscribe(observer);
        }
    }
}