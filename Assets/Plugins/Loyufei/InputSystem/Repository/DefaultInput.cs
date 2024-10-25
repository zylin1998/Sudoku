using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Loyufei 
{
    [CreateAssetMenu(fileName = "DefaultInput", menuName = "Loyufei/InputSystem/DefaultInput", order = 1)]
    public class DefaultInput : ScriptableObject
    {
        [SerializeField]
        private InputEntities _Keyboard;
        [SerializeField]
        private InputEntities _Joystick;
        
        public IEnumerable<InputForm> GetForms(int count) 
        {
            for (var i = 0; i < count; i++) 
            {
                yield return new(GetKeyboard(), GetJoystick());
            }
        }

        public InputEntities GetKeyboard() 
        {
            return new(_Keyboard.OfType<InputEntity>().Select(e => new InputEntity(e)));
        }

        public InputEntities GetJoystick()
        {
            return new(_Joystick.OfType<InputEntity>().Select(e => new InputEntity(e)));
        }
    }
}
