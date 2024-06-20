using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sudoku
{
    public class ReviewListener : ListenerAdapter<Toggle>
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

        public override void AddListener(Action<object> callBack)
        {
            Listener.onValueChanged.AddListener(isOn => callBack?.Invoke(isOn));
        }
    }

    public class  ReviewPool : MemoryPool<int, ReviewListener> 
    {
        protected override void Reinitialize(int id, ReviewListener listener)
        {
            listener.Id = id;
        }
    }
}