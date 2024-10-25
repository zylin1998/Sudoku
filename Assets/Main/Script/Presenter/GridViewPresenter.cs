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

        protected override void RegisterEvents()
        {
            Register<ReviewNumber>    (Review);
            Register<FoundSame>       (Warning);
            Register<DisplayNumbers>  (Display);
            Register<ResponseTip>     (Display);
            Register<ResponseQueryAll>(Display);
            Register<SetNumber>       (Display, true);
            Register<SudokuSetup>     (Setup  , true);
        }

        protected override void Init() 
        {
            View.SetBinder(GetNumber);
        }

        private void Display(DisplayNumbers display) 
        {
            View.Display(Buffer.GetDisplay());
        }

        private void Display(ResponseTip tip)
        {
            View.Display(Buffer.GetDisplay());
        }

        private void Display(ResponseQueryAll query) 
        {
            View.Display(Buffer.GetDisplay());
        }

        private void Display(SetNumber set) 
        {
            View.Display(set.Offset, set.Number);
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

            View.Layout(setup.Size);
        }

        private void GetNumber(IListenerAdapter adapter) 
        {
            SettleEvents(new GetNumber(adapter.To<NumberListener>().Offset));
        }
    }
}
