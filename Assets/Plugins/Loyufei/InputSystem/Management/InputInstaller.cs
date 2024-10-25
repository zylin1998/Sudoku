using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei
{
    [CreateAssetMenu(fileName = "InputInstaller", menuName = "Loyufei/InputSystem/InputInstaller")]
    public class InputInstaller : ScriptableObjectInstaller<InputInstaller>
    {
        [SerializeField]
        private DefaultInput   _DefaultInput;
        [SerializeField]
        private InputIconAsset _InputIconAsset;
        [SerializeField]
        private InputOptions   _InputOptions;
        [SerializeField]
        private InputSetting   _Setting;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<InputManager>()
                .AsSingle();

            Container
                .Bind<DefaultInput>()
                .FromInstance(_DefaultInput)
                .AsSingle();

            Container
                .Bind<InputIconAsset>()
                .FromInstance(_InputIconAsset)
                .AsSingle();

            Container
                .Bind<InputOptions>()
                .FromInstance(_InputOptions)
                .AsSingle();

            Container
                .Bind<InputSetting>()
                .FromInstance(_Setting)
                .AsSingle();
        }
    }
}
