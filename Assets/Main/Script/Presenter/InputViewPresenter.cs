using System;
using System.Collections;
using System.Collections.Generic;
using Loyufei;

namespace Sudoku
{
    public class InputViewPresenter : Loyufei.MVP.ViewPresenter<InputView>
    {
        public override object GroupId => Declarations.Sudoku;

        private Offset2DInt _Target;

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

            var layout = View.Layout(setup.Size);
        }

        private void GetNumber(GetNumber get) 
        {
            _Target = get.Offset;

            View.Open();
        }
    }
}