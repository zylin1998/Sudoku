using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using Loyufei.ViewManagement;

namespace Sudoku
{
    public class ReviewListener : ToggleListener
    {
        [SerializeField]
        private TextMeshProUGUI _Label;

        public override int Id 
        {
            get => base.Id;

            set
            {
                base.Id = value;

                _Label.SetText(Id.ToString());
            }
        }
    }

    public class  ReviewPool : MemoryPool<int, ReviewListener> 
    {
        public ReviewPool() : base() 
        {
            DespawnRoot = new GameObject("ReviewListener").transform;
        }

        public Action<IListenerAdapter> Binder { get; set; }

        public Transform DespawnRoot { get; }

        protected override void Reinitialize(int id, ReviewListener listener)
        {
            listener.Id = id;

            listener.gameObject.SetActive(true);
        }

        protected override void OnDespawned(ReviewListener listener)
        {
            listener.gameObject.SetActive(false);

            listener.transform.SetParent(DespawnRoot);
        }

        protected override void OnCreated(ReviewListener listener)
        {
            listener.AddListener(Binder);
        }
    }
}