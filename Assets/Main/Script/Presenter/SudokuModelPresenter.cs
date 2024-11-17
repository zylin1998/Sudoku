using System;
using System.Linq;
using System.Collections.Generic;
using Loyufei.MVP;

namespace Sudoku
{
    public class SudokuModelPresenter : Presenter
    {
        public SudokuModelPresenter(SudokuModel model)
        {
            Model = model;
        }

        public override object GroupId => Declarations.Sudoku;

        public SudokuModel Model { get; }

        protected override void RegisterEvents()
        {
            Register<SudokuSetup> (Start, true);            
            Register<FillByOffset>(Fill , true);            
            Register<SetNumber>   (Set);
        }

        private void Start(SudokuSetup setup) 
        {
            Model.Start();

            Model.FillRandoms();
        }

        private void Fill(FillByOffset fill) 
        {
            Model.FillRandoms(fill.Offsets);
        }

        private void Set(SetNumber number) 
        {
            var same = Model.SetNumber(number.Offset, number.Number);

            if (same > 0) 
            {
                Model.Clear(number.Offset);

                SettleEvents(new FoundSame(number.Offset, number.Number));
            }

            if (Model.GameOver()) 
            {
                SettleEvents(new GameOver());
            }
        }
    }
}
