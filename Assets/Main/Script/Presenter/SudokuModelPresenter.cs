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
            Register<SudokuSetup>(Start);            
            Register<AskTip>     (Tip);            
            Register<SetNumber>  (Set);            
            Register<AskQueryAll>(Query);           
        }

        private void Start(SudokuSetup setup) 
        {
            Model.Start(setup.Size);

            Model.GetRandom(setup.Display);

            SettleEvents(new DisplayNumbers());
        }

        private void Tip(AskTip ask) 
        {
            Model.GetRandom();
            
            SettleEvents(new ResponseTip());
        }

        private void Set(SetNumber number) 
        {
            var same = Model.SetNumber(number.Offset, number.Number);

            if (same > 0) 
            {
                Model.Clear(number.Offset);

                SettleEvents(new FoundSame(number.Number, number.Offset));
            }

            if (Model.GameOver()) 
            {
                SettleEvents(new GameOver());
            }
        }

        private void Query(AskQueryAll query)
        {
            Model.QueryAll();

            SettleEvents(new ResponseQueryAll(), new GameOver());
        }
    }
}
