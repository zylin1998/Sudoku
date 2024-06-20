using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;
using Zenject;

namespace Sudoku
{
    public class InputListener : ButtonListener
    {
        [SerializeField]
        private TextMeshProUGUI _Text;

        public override int Id
        {
            get => base.Id;
            set
            { 
                base.Id = value;

                _Text.SetText(Id.ToString());
            }
        }
    }

    public class InputPool : MemoryPool<int, InputListener> 
    {
        public InputPool() : base() 
        {
            DespawnRoot = new GameObject(typeof(InputPool).Name).transform;
        }

        public Transform DespawnRoot { get; }

        protected override void Reinitialize(int id, InputListener input)
        {
            input.Id = id;

            input.gameObject.SetActive(true);
        }

        protected override void OnDespawned(InputListener input)
        {
            input.transform.SetParent(DespawnRoot);

            input.gameObject.SetActive(false);
        }
    }
}