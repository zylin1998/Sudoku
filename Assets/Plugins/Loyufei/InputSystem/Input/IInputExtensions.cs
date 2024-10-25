using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public static class IInputExtensions
    {
        public static float GetAxis(this IInput self, object id) 
        {
            return self[id].Axis;
        }

        public static float GetAxisRaw(this IInput self, object id)
        {
            return self[id].AxisRaw;
        }

        public static bool GetButtonDown(this IInput self, object id) 
        {
            return self[id].GetKeyDown;
        }

        public static bool GetButton(this IInput self, object id)
        {
            
            return self[id].GetKey;
        }

        public static bool GetButtonUp(this IInput self, object id)
        {
            return self[id].GetKeyUp;
        }
    }
}