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

        private void Awake()
        {
            _InteractText.enabled = false;

            Listener.image.color = ColorEffect.Normal;
        }

        public void SetContext(int context) 
        {
            Context = context; 
        }
    }

    public static class NumberListenerExtensions 
    {
        public static void Interact(this NumberListener self, int value)
        {
            self.SetContext(value);

            var isZero = value == 0;

            self.InteractText.enabled = isZero ? false : true;

            self.InteractText.SetText(self.Context.ToString());
        }

        public static void Display(this NumberListener self, int value)
        {
            if (self.Context != value)
            {
                self.Interact(value);
            }

            self.Listener.interactable = false;
        }

        public static void Clear(this NumberListener self)
        {
            self.SetContext(0);

            self.InteractText.enabled = false;

            self.Listener.interactable = true;
        }

        public static void Warning(this NumberListener self, bool clear = false)
        {
            var image   = self.Listener.image;
            var normal  = self.ColorEffect.Normal;
            var warning = self.ColorEffect.Warning;

            self.Listener.interactable = false;

            image
                .ChangeColor(warning, 0.2f)
                .Subscribe((l) => image.ChangeColor(normal , 0.2f)
                .Subscribe((l) => image.ChangeColor(warning, 0.2f)
                .Subscribe((l) => image.ChangeColor(normal , 0.2f)
                .Subscribe(l => { if (clear) self.Clear(); }))));
        }

        public static void Review(this NumberListener self, bool isOn)
        {
            var image    = self.Listener.image;
            var effect   = self.ColorEffect;
            var color    = isOn ? effect.Review : effect.Normal;
            var listener = self.Listener;
            
            listener.interactable = false;

            image
                .ChangeColor(color, 0.2f)
                .Subscribe(l => listener.interactable = true);
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
        public static IObservable<long> ChangeColor(this Image self, Color color, float duration) 
        {
            var changing = Observable
                .EveryUpdate()
                .TakeWhile(l => self.color != color);

            var passTime = 0f;

            changing.Subscribe(l => self.color = Color.Lerp(self.color, color, (passTime += Time.deltaTime) * (1 / duration)));

            return changing.Last();
        }
    }
}