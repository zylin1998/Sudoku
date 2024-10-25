using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Loyufei.ViewManagement;

namespace Sudoku
{
    public class InputView : MonoViewBase
    {
        [Inject]
        private InputBoard _Board;

        public override object ViewId => Declarations.Input;

        public ILayout Layout(int size) 
        {
            _Board.Layout(size);

            return Layout();
        }

        public override void RemoveLayout()
        {
            _Board.RemoveLayout();
        }

        public void SetBinder(Action<IListenerAdapter> binder) 
        {
            _Board.InputPool.Binder = binder;
        }
    }
}