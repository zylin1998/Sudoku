using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Loyufei
{
    public class KeyCodeChanger : IAction<InputIconAsset.InputIcon>
    {
        public KeyCodeChanger(InputSetting setting, InputIconAsset iconAsset)
        {
            Setting   = setting;
            IconAsset = iconAsset;
        }

        public InputSetting   Setting   { get; }
        public InputIconAsset IconAsset { get; }

        public Action<InputIconAsset.InputIcon> _OnStart    = (icon) => { };
        public Action<InputIconAsset.InputIcon> _OnPeriod   = (icon) => { };
        public Action<InputIconAsset.InputIcon> _OnComplete = (icon) => { };

        private IObservable<long> _Observable;

        public IAction<InputIconAsset.InputIcon> ChangeInput(InputEntity entity, int index, EPositive positive = EPositive.Positive) 
        {
            var originInput = positive == EPositive.Positive ? entity.Positive : entity.Negative;
            var originIcon  = GetIcon(entity, positive);
            var keyCodes    = Enum.GetValues(typeof(KeyCode));

            _Observable = Observable
                .EveryUpdate()
                .TakeWhile((l) =>
                {
                    if (!Input.anyKeyDown && !((int)originInput).IsClamp(612, 623)) { return true; }

                    foreach (KeyCode key in keyCodes)
                    {
                        if (Input.GetKeyDown(key)) 
                        {
                            if (positive == EPositive.Positive) { entity.Positive = GetKey(key); }
                            if (positive == EPositive.Negative) { entity.Negative = GetKey(key); }

                            return false; 
                        }
                    }

                    var inputs = new EInputKey[]
                    {
                        GetJoystickAxis(Setting.LSHorizontal  , index, EInputKey.LSRight  , EInputKey.LSLeft),
                        GetJoystickAxis(Setting.LSVertical    , index, EInputKey.LSDown   , EInputKey.LSUp),
                        GetJoystickAxis(Setting.RSHorizontal  , index, EInputKey.RSRight  , EInputKey.RSLeft),
                        GetJoystickAxis(Setting.RSVertical    , index, EInputKey.RSDown   , EInputKey.RSUp),
                        GetJoystickAxis(Setting.DPADHorizontal, index, EInputKey.DPADRight, EInputKey.DPADLeft),
                        GetJoystickAxis(Setting.DPADVertical  , index, EInputKey.DPADUp   , EInputKey.DPADDown),
                    };
                    

                    if (!inputs.Any(i => ((int)i).IsClamp(612, 623))) { return true; }
                    
                    if (positive == EPositive.Positive)
                    {
                        entity.Positive = inputs.First(i => ((int)i).IsClamp(612, 623));

                        return false;
                    }

                    if (positive == EPositive.Negative)
                    {
                        entity.Negative = inputs.First(i => ((int)i).IsClamp(612, 623));

                        return false;
                    }

                    return true;
                });

            _Observable.First().Subscribe((l) => _OnStart?.Invoke(originIcon));
            _Observable.Subscribe        ((l) => _OnPeriod?.Invoke(originIcon));
            _Observable.Last().Subscribe ((l) => _OnComplete?.Invoke(GetIcon(entity, positive)));
            
            return this;
        }

        public IAction<InputIconAsset.InputIcon> OnComplete(Action<InputIconAsset.InputIcon> onComplete)
        {
            _OnComplete = onComplete;

            return this;
        }

        public IAction<InputIconAsset.InputIcon> OnPeriod(Action<InputIconAsset.InputIcon> onPeriod)
        {
            _OnPeriod = onPeriod;

            return this;
        }

        public IAction<InputIconAsset.InputIcon> OnStart(Action<InputIconAsset.InputIcon> onStart)
        {
            _OnStart = onStart;

            return this;
        }

        private EInputKey GetKey(KeyCode keyCode) 
        {
            var index = (int)keyCode;

            if (index < 330) { return (EInputKey)index; }

            return (index % 20).IsClamp(0, 11) ? (EInputKey)(600 + index % 20) : EInputKey.None;
        }

        private EInputKey GetJoystickAxis(string axisName, int index, EInputKey positive, EInputKey negative) 
        {
            var axisRaw = Input.GetAxisRaw(axisName + index);

            if (axisRaw >=  1) { return positive; }

            if (axisRaw <= -1) { return negative; }

            return EInputKey.None;
        }

        private InputIconAsset.InputIcon GetIcon(InputEntity entity, EPositive positive) 
        {
            var input = positive == EPositive.Positive ? entity.Positive : entity.Negative;
            
            return IconAsset[input].Data;
        }
    }
}