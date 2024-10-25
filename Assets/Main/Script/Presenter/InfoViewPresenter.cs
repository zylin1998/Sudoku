using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using Loyufei.MVP;
using Loyufei.ViewManagement;

namespace Sudoku 
{
    public class InfoViewPresenter : ViewPresenter<InfoView>
    {
        public override object GroupId => Declarations.Sudoku;

        private int _Tips = 0;

        private OptionListener _TipOption; 

        protected override void RegisterEvents()
        {
            Register<SudokuSetup>(Setup);
            Register<GameOver>   (GameOver);
        }

        protected override void Init()
        {
            var layout = View.Layout();

            layout.BindListener<OptionListener>(0, Setting);
            layout.BindListener<OptionListener>(1, Tips);
            layout.BindListener<OptionListener>(2, ShowAnswer);
            layout.BindListener<OptionListener>(3, Quit);
            
            View.SetBinder((listener) => SettleEvents(new ReviewNumber(listener.Id, listener.To<ReviewListener>().Listener.isOn)));
        }

        private void Setting(IListenerAdapter listener) 
        {
            SettleEvents(new Setting());
        }

        private void Tips(IListenerAdapter listener) 
        {
            SettleEvents(new AskTip());

            --_Tips;

            listener.To<OptionListener>().SetText(string.Format("提示 x {0}", _Tips));

            listener.To<OptionListener>().Listener.interactable = _Tips > 0;
        }

        private void ShowAnswer(IListenerAdapter listener)
        {
            SettleEvents(new AskQueryAll());
        }

        private void Quit(IListenerAdapter listener)
        {
            SettleEvents(new SendMessage("離開遊戲", Application.Quit));
        }

        private void Setup(SudokuSetup setup) 
        {
            View.RemoveLayout();

            _Tips = setup.Tips;

            var layout = View.Layout(setup.Size);

            _TipOption = layout
                .Find<OptionListener>((option) => option.Id == 1);

            _TipOption.SetText(string.Format("提示 x {0}", _Tips));
            _TipOption.Listener.interactable = true;
        }

        private void GameOver(GameOver gameOver) 
        {
            _TipOption.Listener.interactable = false;
        }
    }
}
