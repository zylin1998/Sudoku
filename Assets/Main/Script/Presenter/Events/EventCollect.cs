using System;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.DomainEvents;

namespace Sudoku
{
    public struct SudokuSetup : IDomainEvent
    {
        public SudokuSetup(int size, int display, int tips)
        {
            Size    = size;
            Display = display;
            Tips    = tips;
        }

        public int Size    { get; }
        public int Display { get; }
        public int Tips    { get; }
    }

    public struct SendMessage : IDomainEvent
    {
        public SendMessage(string message) : this(message, () => { })
        {

        }

        public SendMessage(string message, Action onConfirm)
        {
            Message   = message;
            OnConfirm = onConfirm;
        }

        public string Message { get; }
        public Action OnConfirm { get; }
    }

    public struct Setting : IDomainEvent
    {

    }

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

    public struct GetNumber : IDomainEvent
    {
        public GetNumber(Offset2DInt offset)
        {
            Offset = offset;
        }

        public Offset2DInt Offset { get; }
    }

    public struct FoundSame : IDomainEvent
    {
        public FoundSame(int number, Offset2DInt center)
        {
            Number = number;
            Center = center;
        }

        public int         Number { get; }
        public Offset2DInt Center { get; }
    }

    public struct GameOver : IDomainEvent
    {

    }

    public struct DisplayNumbers : IDomainEvent
    {

    }

    public struct AskTip : IDomainEvent
    {

    }

    public struct ResponseTip : IDomainEvent
    {

    }

    public struct AskQueryAll : IDomainEvent
    {

    }

    public struct ResponseQueryAll : IDomainEvent
    {

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
}