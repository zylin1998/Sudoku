using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public interface IInput
    {
        public IInputUnit this[object id] { get; }

        public static IInput Default { get; }

        private struct DefaultInput : IInput
        {
            public IInputUnit this[object id] => IInputUnit.Default;
        }
    }

    public class InputBase : IInput
    {
        public InputBase(InputForm form, int index) 
        {
            Form  = form;
            Index = index;
        }

        public InputForm Form  { get; }
        public int       Index { get; }

        public IInputUnit this[object id] 
            => Units.GetorReturn(id, () => IInputUnit.Default);

        protected Dictionary<object, IInputUnit> Units { get; } = new();

        public void Reset(EInputType mode) 
        {
            var entities = mode == EInputType.Keyboard ? Form.Keyboard : Form.Joystick;
            
            foreach (var entity in entities)
            {
                var unit = Units.GetorAdd(entity.Identity, () => new InputUnit());

                ((InputUnit)unit).Reset(entity.Data, Index);
            }
        }

        public void Default(DefaultInput defaultInput, EInputType inputMode) 
        {
            if (inputMode == EInputType.Keyboard)
            { 
                Form.Keyboard.Default(defaultInput.GetKeyboard()); 
            }

            if (inputMode == EInputType.Joystick)
            {
                Form.Joystick.Default(defaultInput.GetJoystick());
            }
        }
    }
}