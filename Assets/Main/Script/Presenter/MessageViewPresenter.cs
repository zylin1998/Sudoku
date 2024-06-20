using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;
using UnityEngine;

namespace Sudoku
{
    public class MessageViewPresenter
    {
        public MessageViewPresenter(MessageView view, DomainEventService service) 
        {
            View = view;

            service.Register<InitScene>  (Init, Declarations.Sudoku);
            service.Register<SendMessage>(Send, Declarations.Sudoku);
        }

        public MessageView View { get; }

        private Action _OnConfirm = () => { };

        public void Init(InitScene init) 
        {
            var confirm = View.OfType<ButtonListener>().First();

            confirm.AddListener((id) => Confirm());
        }

        public void Send(SendMessage send) 
        {
            View.SetText(send.Message);

            _OnConfirm = send.OnConfirm;

            View.Open();
        }

        private void Confirm() 
        {
            _OnConfirm?.Invoke();

            View.Close();
        }
    }

    public class SendMessage : DomainEventBase 
    {
        public SendMessage(string message) : this(message, () => { })
        {
            
        }

        public SendMessage(string message, Action onConfirm)
        {
            Message   = message;
            OnConfirm = onConfirm;
        }

        public string Message   { get; }
        public Action OnConfirm { get; }
    }
}