using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.MVP;
using Loyufei.ViewManagement;

namespace Sudoku
{
    public class SetupViewPresenter : ViewPresenter<SetupView>
    {
        public override object GroupId => Declarations.Sudoku;

        private int _Size;
        private int _Display;
        private int _Tips;

        private int MinDisplay => _Size.Pow(4) / 10;
        private int MaxDisplay => (int)(_Size.Pow(4) * 0.4f);

        private DropdownListener _SizeDrop;
        private DropdownListener _DisplayDrop;

        protected override void RegisterEvents()
        {
            Register<Setting>   (Setting);
        }

        protected override void Init() 
        {
            var layout = View.Layout();

            var dropdowns = layout
                .FindAll<DropdownListener>()
                .ToDictionary(k => k.Id);

            _SizeDrop    = dropdowns[0];
            _DisplayDrop = dropdowns[1];
            
            _Size    = Declarations.Sizes[0];
            _Display = MinDisplay;
            _Tips    = _Size;

            _SizeDrop   .Listener.options.Clear();
            _DisplayDrop.Listener.options.Clear();
            _SizeDrop   .Listener.AddOptions(Declarations.Sizes.Select(s => string.Format("{0} x {0}", s)).ToList());
            _DisplayDrop.Listener.AddOptions(Create(MinDisplay, MaxDisplay).ToList());

            _SizeDrop   .AddListener(OnSizeChanged);
            _DisplayDrop.AddListener(UpdateDisplay);

            layout.BindListener<ButtonListener>(0, Setup);
        }

        private void Setting(Setting setting) 
        {
            View.Open();
        }

        private void OnSizeChanged(IListenerAdapter listener) 
        {
            _Size    = Declarations.Sizes[_SizeDrop.Value];
            _Tips    = _Size;
            _Display = MinDisplay;
            
            _DisplayDrop.Listener.options.Clear();
            _DisplayDrop.Listener.AddOptions(Create(MinDisplay, MaxDisplay).ToList());
            _DisplayDrop.Listener.SetValueWithoutNotify(0);
        }

        private void Setup(IListenerAdapter listener) 
        {
            SettleEvents(new SudokuSetup(_Size, _Display, _Tips));
            
            View.Close();
        }

        private void UpdateDisplay(IListenerAdapter adapter) 
        {
            _Display = MinDisplay + _DisplayDrop.Value;
        }

        private IEnumerable<string> Create(int start, int end) 
        {
            for(int num = start; num <= end; num++) 
            {
                yield return num.ToString();
            }
        }
    }
}