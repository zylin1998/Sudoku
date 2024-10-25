using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loyufei
{
    [CreateAssetMenu(fileName = "InputIconAsset", menuName = "Loyufei/InputSystem/InputIconAsset", order = 1)]
    public class InputIconAsset : EntityFormAsset<InputIconAsset.InputIcon, InputIconAsset.InputIcon>
    {
        [Serializable]
        public class InputIcon : IEntity<InputIcon>
        {
            public InputIcon() : this(EInputKey.None, default, string.Empty) 
            {

            }

            public InputIcon(EInputKey input) : this(input, default, input.ToString()) 
            {

            }

            public InputIcon(EInputKey input, Sprite icon, string name) 
            {
                _Input = input;
                _Icon  = icon;
                _Name  = name;
            }

            [SerializeField]
            private string     _Name;
            [SerializeField]
            private EInputKey _Input;
            [SerializeField]
            private Sprite     _Icon;

            public object    Identity => _Input;
            public InputIcon Data     => this;
            public string    Name     => _Name;
            public Sprite    Icon     => _Icon;
        }

        public void Reset()
        {
            _Entities = new List<InputIcon>();
            
            foreach(EInputKey input in Enum.GetValues(typeof(EInputKey))) 
            {
                _Entities.Add(new(input));
            }
        }
    }
}