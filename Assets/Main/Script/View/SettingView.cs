using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku
{
    public class SettingView : MenuBase
    {
        public override Tween Open()
        {
            gameObject.SetActive(true);

            return base.Open();
        }

        public override Tween Close()
        {
            return base.Close().OnComplete(() => gameObject.SetActive(false));
        }
    }
}