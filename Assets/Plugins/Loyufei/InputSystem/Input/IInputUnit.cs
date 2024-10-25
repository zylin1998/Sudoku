using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public interface IInputUnit
    {
        public object Id          { get; }
        public float  Axis        { get; }
        public float  AxisRaw     { get; }
        public bool   GetKeyDown  { get; }
        public bool   GetKey      { get; }
        public bool   GetKeyUp    { get; }

        #region Default

        public static IInputUnit Default { get; } = new DefaultInputUnit();

        private struct DefaultInputUnit : IInputUnit
        {
            public object Id          => string.Empty;
            public float  Axis        => 0;
            public float  AxisRaw     => 0;
            public bool   GetKeyDown  => false;
            public bool   GetKey      => false;
            public bool   GetKeyUp    => false;
        }

        #endregion
    }

    public class InputUnit : IInputUnit
    {
        public static InputSetting Setting { get; set; }

        public void Reset(InputEntity entity, int index) 
        {
            _Entity = entity;
            _Index  = index;
        }

        private int         _Index;
        private InputEntity _Entity;
        private float       _HoldTime;
        public object Id    => _Entity.Identity;

        public float Axis
        {
            get
            {
                var axisRaw = AxisRaw;
                
                if (Equals(axisRaw, 0f))
                {
                    _HoldTime = 0;

                    return 0;
                }
                
                if (Equals(_HoldTime, 0f)) { _HoldTime = Time.realtimeSinceStartup; }

                return (Time.realtimeSinceStartup - _HoldTime).Clamp01() * axisRaw;
            }
        }

        public float AxisRaw
        {
            get 
            {
                var positive = _Entity.Positive;
                var negative = _Entity.Negative;
                
                if (GetValue(positive) || GetValue(positive,  1)) { return  1; }
                
                if (GetValue(negative) || GetValue(negative, -1)) { return -1; }

                return 0;
            }
        }

        public bool GetKeyDown => Input.GetKeyDown(GetKeyCode(_Entity.Positive));
        public bool GetKey     => Input.GetKey    (GetKeyCode(_Entity.Positive));
        public bool GetKeyUp   => Input.GetKeyUp  (GetKeyCode(_Entity.Positive));

        private bool GetValue(EInputKey key) 
        {
            var index = (int)key;

            if (index < 600) 
            {
                return Input.GetKey((KeyCode)key);
            }

            if (index.IsClamp(600, 611)) 
            {
                var str = string.Format("Joystick{0}Button{1}", index % 10, _Index);

                return Enum.TryParse<KeyCode>(str, true, out var result);
            }

            return false;
        }

        private bool GetValue(EInputKey key, float expect) 
        {
            var index = (int)key;
            
            if (index.IsClamp(612, 617) && !index.IsClamp(614, 615))
            {
                return Setting.GetAxisRaw(key, _Index) == -expect;
            }

            if (index.IsClamp(612, 623))
            {
                return Setting.GetAxisRaw(key, _Index) ==  expect;
            }

            return false;
        }

        public static KeyCode GetKeyCode(EInputKey self)
        {
            return (int)self <= (int)KeyCode.Joystick8Button19 ? (KeyCode)self : KeyCode.None;
        }
    }
}