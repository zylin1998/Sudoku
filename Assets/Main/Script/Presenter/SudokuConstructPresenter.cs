using Loyufei.DomainEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku
{
    public class SudokuConstructPresenter
    {
        public SudokuConstructPresenter(SudokuConstructModel model, DomainEventService service) 
        {
            Model = model;

            service.Register<ConstructMetrix>(Construct, Declarations.Sudoku);
        }

        public SudokuConstructModel Model { get; }

        public void Construct(ConstructMetrix metrix) 
        {
            Model.Construct(metrix.Size);
        }
    }

    public class ConstructMetrix : DomainEventBase 
    {
        public ConstructMetrix(int size)
        {
            Size = size;
        }

        public int Size { get; }
    }
}