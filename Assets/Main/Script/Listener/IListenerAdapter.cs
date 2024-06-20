using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace Sudoku
{
    public class ListenerAdapter<TListener> : MonoBehaviour, IListenerAdapter<TListener> 
        where TListener : Selectable
    {
        [SerializeField]
        private int       _Id;
        [SerializeField]
        private TListener _Listener;

        public TListener Listener => _Listener;

        public virtual int Id { get => _Id; set => _Id = value; }

        public virtual void AddListener(Action<object> callBack) 
        {
            
        }
    }

    public interface IListenerAdapter<TListener>  : IListenerAdapter
    {
        public TListener Listener { get; }
    }

    public interface IListenerAdapter
    {
        public int Id { get; }

        public void AddListener(Action<object> callBack);
    }
}