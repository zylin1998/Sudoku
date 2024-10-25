using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.MVP;
using Loyufei.ViewManagement;

namespace Sudoku
{
    public class ConfirmViewPresenter : ViewPresenter<ConfirmView>
    {
        public override object GroupId => Declarations.Sudoku;

        private Action _OnConfirm = () => { };

        protected override void RegisterEvents()
        {
            Register<SendMessage>(Send);
        }

        protected override void Init() 
        {
            View
                .Layout()
                .BindListener<ButtonListener>(0, (listener) => Confirm());
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
}