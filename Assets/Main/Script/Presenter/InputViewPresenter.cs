using System;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Zenject;

namespace Sudoku
{
    public class InputViewPresenter : Loyufei.MVP.ViewPresenter<InputView>
    {
        public override object GroupId => Declarations.Sudoku;

        private Offset2DInt _Target;

        [Inject]
        public SudokuSetting Setting { get; }

        protected override void RegisterEvents()
        {
            Register<SudokuSetup>(Setup);
            Register<GetNumber>  (GetNumber);
        }

        protected override void Init()
        {
            View.SetBinder((listener) =>
            {
                SettleEvents(new SetNumber(_Target, listener.Id));

                View.Close();
            });
        }

        public void Setup(SudokuSetup setup) 
        {
            View.RemoveLayout();

            var layout = View.Layout(Setting.Size);
        }

        private void GetNumber(GetNumber get) 
        {
            _Target = get.Offset;

            View.Open();
        }
    }
}