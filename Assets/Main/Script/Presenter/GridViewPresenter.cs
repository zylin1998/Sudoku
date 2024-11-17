using System;
using System.Linq;
using System.Collections.Generic;
using Loyufei;
using Loyufei.MVP;
using Loyufei.ViewManagement;
using Zenject;

namespace Sudoku
{
    public class GridViewPresenter : ViewPresenter<GridView>
    {
        public override object GroupId => Declarations.Sudoku;

        [Inject]
        public Buffer Buffer { get; }
        [Inject]
        public SudokuSetting Setting { get; }

        protected override void RegisterEvents()
        {
            Register<SudokuSetup> (Setup);
            Register<ReviewNumber>(Review);
            Register<FoundSame>   (Warning);
            Register<GameOver>    (GameOver);
            Register<DisplayAll>  (Display);
            Register<FillByOffset>(Display);
            Register<SetNumber>   (Display, true);
        }

        protected override void Init() 
        {
            View.SetBinder(GetNumber);
        }

        private void Display(FillByOffset fill)
        {
            foreach (var offset in fill.Offsets) 
            {
                View.Display(offset, Buffer.Get(offset), false);
            }

            Buffer.Verified();
        }

        private void Display(DisplayAll display)
        {
            View.Display(Buffer.GetAll(true), false);
        }

        private void Display(SetNumber set) 
        {
            View.Display(set.Offset, set.Number, true);
        }

        private void Warning(FoundSame same) 
        {
            View.Warning(same.Number, same.Center);
        }

        private void Review(ReviewNumber review) 
        {
            View.Review(review.Number, review.IsOn);
        }

        private void Setup(SudokuSetup setup) 
        {
            View.RemoveLayout();

            var layout = View.Layout(Setting.Size);

            View.Display(Buffer.GetAll(false), false);

            Buffer.Verified();
        }

        private void GetNumber(IListenerAdapter adapter) 
        {
            SettleEvents(new GetNumber(adapter.To<NumberListener>().Offset));
        }

        private void GameOver(GameOver gameOver) 
        {
            View.Numbers.ForEach(n => n.Listener.interactable = false);
        }
    }
}
