using System;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.DomainEvents;

namespace Sudoku
{
    #region Setting Event

    public struct SudokuSetup : IDomainEvent
    {

    }

    #endregion

    #region Model Event

    public struct FoundSame : IDomainEvent
    {
        public FoundSame(Offset2DInt center, int number)
        {
            Number = number;
            Center = center;
        }

        public int Number { get; }
        public Offset2DInt Center { get; }
    }

    public struct GameOver : IDomainEvent
    {

    }

    #endregion

    #region Grid Event

    public struct GetNumber : IDomainEvent
    {
        public GetNumber(Offset2DInt offset)
        {
            Offset = offset;
        }

        public Offset2DInt Offset { get; }
    }

    #endregion

    #region Input Event

    public struct SetNumber : IDomainEvent
    {
        public SetNumber(Offset2DInt offset, int number)
        {
            Offset = offset;
            Number = number;
        }

        public Offset2DInt Offset { get; }
        public int         Number { get; }
    }

    #endregion

    #region Info Event

    public struct Setting : IDomainEvent
    {

    }

    public struct DisplayAll : IDomainEvent 
    {

    }

    public struct FillByOffset : IDomainEvent 
    {
        public FillByOffset(IEnumerable<Offset2DInt> offsets) 
        {
            Offsets = offsets;
        }

        public IEnumerable<Offset2DInt> Offsets { get; }
    }

    public struct SendMessage : IDomainEvent
    {
        public SendMessage(string message) : this(message, () => { })
        {

        }

        public SendMessage(string message, Action onConfirm)
        {
            Message = message;
            OnConfirm = onConfirm;
        }

        public string Message { get; }
        public Action OnConfirm { get; }
    }

    public struct ReviewNumber : IDomainEvent
    {
        public ReviewNumber(int num, bool isOn)
        {
            IsOn   = isOn;
            Number = num;
        }

        public bool IsOn   { get; }
        public int  Number { get; }
    }

    #endregion
}