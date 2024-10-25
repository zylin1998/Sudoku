using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    [Serializable]
    public class InputData
    {
        [SerializeField]
        private InputForm[] _Forms =  new InputForm[0];
        
        public InputForm[] Forms => _Forms;

        public void Reset(DefaultInput defaultInput, InputSetting setting)
        {
            if (_Forms.Length <= 0) 
            {
                _Forms = defaultInput.GetForms(setting.InputCount).ToArray();
                
                return;
            }

            if(_Forms.Length < setting.InputCount) 
            {
                _Forms = _Forms.Concat(defaultInput.GetForms(_Forms.Length - setting.InputCount)).ToArray();
            }
        }
    }

    [Serializable]
    public struct InputForm 
    {
        public InputForm(InputEntities keyboard, InputEntities joystick) 
        {
            _Keyboard = keyboard;
            _Joystick = joystick;
        }

        [SerializeField]
        private InputEntities _Keyboard;
        [SerializeField]
        private InputEntities _Joystick;

        public InputEntities Keyboard => _Keyboard;
        public InputEntities Joystick => _Joystick;
    }
}