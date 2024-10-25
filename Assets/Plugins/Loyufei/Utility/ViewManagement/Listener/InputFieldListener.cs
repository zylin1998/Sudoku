using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace Loyufei.ViewManagement
{
    public class InputFieldListener : MonoListenerAdapter<TMP_InputField>
    {
        public string Value => Listener.text;

        public void SetValue(string text, bool withoutNotify = true)
        {
            if (withoutNotify) { Listener.SetTextWithoutNotify(text); }

            else { Listener.text = text; }
        }

        public void SetLabel(string text) 
        {
            Listener.placeholder.To<TextMeshProUGUI>()?.SetText(text);
        }

        public void SetFont(TMP_FontAsset font) 
        {
            Listener.fontAsset = font;
        }

        public override void AddListener(Action<IListenerAdapter> callBack)
        {
            Listener.onEndEdit.AddListener(value => callBack.Invoke(this));
        }
    }
}