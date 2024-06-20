using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Zenject;

namespace Sudoku
{
    public class NumberListener : ListenerAdapter<Button>
    {
        [SerializeField]
        private TextMeshProUGUI _InteractText;
        [SerializeField, Inject]
        private ColorEffect     _ColorEffect;

        public int Context { get; private set; }
        public int AreaId  { get; private set; }
        
        public ColorEffect     ColorEffect  => _ColorEffect;
        public TextMeshProUGUI InteractText => _InteractText;

        private void Awake()
        {
            _InteractText.enabled = false;

            Listener.interactable = false;

            Listener.image.color = ColorEffect.Normal;
        }

        public override void AddListener(Action<object> callBack)
        {
            Listener.onClick.AddListener(() => callBack.Invoke(Id));
        }

        public void SetContext(int context) 
        {
            Context = context; 
        }

        public void SetId(int id, int area, int size) 
        {
            var length = (int)Mathf.Pow(size, 2);
            var x1 = id   % size;
            var x2 = area % size * size;
            var y1 = id   / size;
            var y2 = area / size * size;
            
            Id     = x1 + x2 + length * (y1 + y2);
            AreaId = area;
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
                .DOColor(warning, 0.2f).OnComplete(() => image
                .DOColor(normal , 0.2f).OnComplete(() => image
                .DOColor(warning, 0.2f).OnComplete(() => image
                .DOColor(normal , 0.2f).OnComplete(() => { if (clear) self.Clear(); }))));
        }

        public static void Review(this NumberListener self, bool isOn)
        {
            var image    = self.Listener.image;
            var effect   = self.ColorEffect;
            var color    = isOn ? effect.Review : effect.Normal;
            var listener = self.Listener;
            
            listener.interactable = false;

            image.DOColor(color, 0.5f).OnComplete(() => listener.interactable = true);
        }
    }

    public class NumberPool : MemoryPool<int, int, int, NumberListener> 
    {
        public NumberPool() : base() 
        {
            DespawnRoot = new GameObject(typeof(NumberPool).Name).transform;
        }

        public Transform DespawnRoot { get; }

        protected override void Reinitialize(int id, int area, int size, NumberListener number)
        {
            number.gameObject.SetActive(false);

            number.SetId(id, area, size);
        }

        protected override void OnDespawned(NumberListener number)
        {
            number.gameObject.SetActive(false);
            
            number.transform.SetParent(DespawnRoot);
        }
    }
}