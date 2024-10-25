using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    [CreateAssetMenu(fileName = "InputOptions", menuName = "Loyufei/InputSystem/InputOptions", order = 1)]
    public class InputOptions : EntityFormAsset<InputOptions.Option, InputOptions.Option>
    {
        [Serializable]
        public class Option : IEntity<Option>
        {
            [SerializeField]
            private string    _OptionName;
            [SerializeField]
            private string    _TargetId;
            [SerializeField]
            private EPositive _Positive = EPositive.Positive;

            public object    Identity => _OptionName;
            public Option    Data     => this;
            public string    TargetId => _TargetId;
            public EPositive Positive => _Positive;
        }
    }
}