using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine.EventSystems;

namespace Loyufei
{
    public class InputManager : IInitializable
    {
        private class Access 
        {
            public Access(InputForm form, int index)
            {
                Index = index;
                Form  = form;
                Input = new InputBase(Form, Index);

                Reset(EInputType.Keyboard);
            }

            public int        Index     { get; }
            public IInput     Input     { get; }
            public bool       Accessed  { get; set; }
            public InputForm  Form      { get; }
            public EInputType InputMode { get; private set; } = EInputType.Keyboard;

            public void Reset(EInputType mode) 
            {
                InputMode = mode;

                Input.To<InputBase>().Reset(mode);
            }

            public void Default(DefaultInput defaultInput, EInputType mode) 
            {
                Input.To<InputBase>().Default(defaultInput, mode);
            }
        }

        public InputManager(InputData inputData, InputSetting setting, DefaultInput defaultInput, InputIconAsset inputIcon, InputOptions options) 
        {
            InputData    = inputData;
            InputIcon    = inputIcon;
            InputOptions = options;
            Setting      = setting;
            DefaultInput = defaultInput;
            Changer      = new(Setting, InputIcon);

            InputUnit.Setting = Setting;

            Init();
        }

        public InputSetting   Setting      { get; }
        public DefaultInput   DefaultInput { get; }
        public InputData      InputData    { get; }  
        public InputIconAsset InputIcon    { get; }
        public InputOptions   InputOptions { get; }

        public int InputCount => Setting.InputCount;

        public IInput Main { get; protected set; }

        private KeyCodeChanger Changer { get; }

        private Dictionary<int, Access> _Accesses;

        public void Initialize() 
        {
            EventSystem.current.gameObject.AddComponent<KeyUnityInput>();
        }

        public IInput Fetch(int index)
        {
            var accessed = _Accesses.TryGetValue(index, out var access);

            if (!accessed || access.Accessed) { return default; }

            return access.Input;
        }

        public bool Release(int index) 
        {
            var accessed = _Accesses.TryGetValue(index, out var access);

            if (!accessed) { return false; }

            access.Accessed = false;

            return true;
        }

        public void SetMain(int index) 
        {
            var accessed = _Accesses.TryGetValue(index, out var access);

            if (!accessed) { return; }

            Main = access.Input;
        }

        public InputForm GetForm(int index) 
        {
            var accessed = _Accesses.TryGetValue(index, out var access);

            if (!accessed) { return default; }

            return access.Form;
        }

        public IEnumerable<OptionInfo> GetOptions(int index, EInputType inputMode) 
        {
            var accessed = _Accesses.TryGetValue(index, out var access);

            if (!accessed) { yield break; }
            
            var form = inputMode == EInputType.Keyboard ? access.Form.Keyboard : access.Form.Joystick;
            
            foreach (var entity in InputOptions) 
            {
                var option   = entity.Data;
                var data     = form[option.TargetId].Data;
                var inputKey = option.Positive == EPositive.Positive ? data.Positive : data.Negative;
                
                yield return new(option, InputIcon[inputKey].Data);
            }
        }

        public IEnumerable<OptionInfo> Reset(int index, EInputType inputMode)
        {
            var accessed = _Accesses.TryGetValue(index, out var access);

            if (!accessed) { return new OptionInfo[0]; }

            access.Reset(inputMode);

            return GetOptions(index, inputMode);
        }

        public IEnumerable<OptionInfo> Default(int index, EInputType inputMode) 
        {
            var accessed = _Accesses.TryGetValue(index, out var access);

            if (!accessed) { return new OptionInfo[0]; }

            access.Default(DefaultInput, inputMode);

            return GetOptions(index, inputMode);
        }

        public IAction<InputIconAsset.InputIcon> ChangeInput(int index, object id, EInputType inputMode = EInputType.Keyboard, EPositive positive = EPositive.Positive) 
        {
            var accessed = _Accesses.TryGetValue(index, out var access);

            if (!accessed) { return default; }

            var entities = inputMode == EInputType.Keyboard ? access.Form.Keyboard : access.Form.Joystick;

            return Changer.ChangeInput(entities[id].Data, index, positive);
        }

        private void Init() 
        {
            InputData.Reset(DefaultInput, Setting);
            
            _Accesses = new();

            var index = 0;
            foreach (var forms in InputData.Forms) 
            {
                _Accesses.Add(index, new(forms, index));

                index++;
            }

            Main = _Accesses.Values.FirstOrDefault()?.Input;
        }

        public struct OptionInfo 
        {
            public OptionInfo(InputOptions.Option option, InputIconAsset.InputIcon icon)
                => (Option, Icon) = (option, icon);

            public InputOptions.Option      Option { get; }
            public InputIconAsset.InputIcon Icon   { get; }
        }
    }
}
