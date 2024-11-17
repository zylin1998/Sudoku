using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.MVP;
using Loyufei.ViewManagement;
using UnityEngine;

namespace Sudoku
{
    public class SetupViewPresenter : ViewPresenter<SetupView>
    {
        public SetupViewPresenter(SudokuSetting setting) 
        {
            Setting = setting;
        }

        public override object GroupId => Declarations.Sudoku;

        public SudokuSetting Setting { get; private set; }

        protected override void RegisterEvents()
        {
            Register<Setting>(Open);
        }

        protected override void Init() 
        {
            var layout = View.Layout();

            View.SetSizeOptions(Declarations.Sizes);
            View.SetDisplayOptions(Enumerable.Range(Setting.MinDisplay, Setting.MaxDisplay - Setting.MinDisplay + 1));

            layout.BindListener<DropdownListener>(0, OnSizeChanged);
            layout.BindListener<DropdownListener>(1, OnDisplayChanged);
            
            layout.BindListener<ButtonListener>(0, Setup);
        }

        private void Open(Setting setting) 
        {
            View.Open();
        }

        private void OnSizeChanged(IListenerAdapter listener) 
        {
            var size = Declarations.Sizes[listener.To<DropdownListener>().Value];

            Setting.Size = size;
            
            View.SetDisplayOptions(Enumerable.Range(Setting.MinDisplay, Setting.MaxDisplay - Setting.MinDisplay + 1));
        }

        private void Setup(IListenerAdapter listener) 
        {
            SettleEvents(new SudokuSetup());
            
            View.Close();
        }

        private void OnDisplayChanged(IListenerAdapter listener) 
        {
            Setting.Display = Setting.MinDisplay + listener.To<DropdownListener>().Value;
        }
    }
}