using System;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.DomainEvents;
using UnityEngine;

namespace Sudoku
{
    public class MaskViewPresenter
    {
        public MaskViewPresenter(MaskView view, DomainEventService service) 
        {
            View = view;
            
            service.Register<InputMask>  (Mask , Declarations.Sudoku);
            service.Register<SudokuSetup>(Setup, Declarations.Sudoku);
        }

        public MaskView View { get; }

        public void Mask(InputMask input) 
        {
            View.AskNumber(input.Response);
        }

        public void Setup(SudokuSetup setup) 
        {
            var board = View.Board;

            board.Remove();

            var inputs = board.Layout(setup.Size);

            inputs.ForEach(input =>
            {
                input.AddListener((id) =>
                {
                    View.ResponseNumber((int)id);
                });
            });
        }
    }

    public class InputMask : DomainEventBase 
    {
        public InputMask(Action<int> response)
        {
            Response = response;
        }

        public Action<int> Response { get; }
    }
}