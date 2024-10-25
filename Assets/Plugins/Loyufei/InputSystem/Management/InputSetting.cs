using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    [Serializable]
    public class InputSetting
    {
        [SerializeField]
        private int    _InputCount;
        [SerializeField]
        private string _LSHorizontal;
        [SerializeField]
        private string _LSVertical;
        [SerializeField]
        private string _RSHorizontal;
        [SerializeField]
        private string _RSVertical;
        [SerializeField]
        private string _DPADHorizontal;
        [SerializeField]
        private string _DPADVertical;

        public int    InputCount     => _InputCount;
        public string LSHorizontal   => _LSHorizontal;
        public string LSVertical     => _LSVertical;
        public string RSHorizontal   => _RSHorizontal;
        public string RSVertical     => _RSVertical;
        public string DPADHorizontal => _DPADHorizontal;
        public string DPADVertical   => _DPADVertical;

        public float GetAxisRaw(EInputKey input, int index) 
        {
            var i = (int)input;

            if (!i.IsClamp(612, 623)) { return 0; }

            if (i.IsClamp(612, 613)) { return Input.GetAxisRaw(LSVertical     + index); }
            if (i.IsClamp(614, 615)) { return Input.GetAxisRaw(LSHorizontal   + index); }
            if (i.IsClamp(616, 617)) { return Input.GetAxisRaw(LSVertical     + index); }
            if (i.IsClamp(618, 619)) { return Input.GetAxisRaw(LSHorizontal   + index); }
            if (i.IsClamp(620, 621)) { return Input.GetAxisRaw(DPADVertical   + index); }
            if (i.IsClamp(622, 623)) { return Input.GetAxisRaw(DPADHorizontal + index); }

            return 0;
        }
    }
}
