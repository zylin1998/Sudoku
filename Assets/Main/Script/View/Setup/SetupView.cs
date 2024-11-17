using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Loyufei.ViewManagement;
using TMPro;

namespace Sudoku
{
    public class SetupView : MonoViewBase
    {
        public override object ViewId => Declarations.Setting;

        private IListenerAdapter[] Listeners;

        private Dictionary<int, DropdownListener> _Dropdowns;

        public override ILayout Layout()
        {
            Listeners = GetComponentsInChildren<IListenerAdapter>();

            _Dropdowns = Listeners
                .OfType<DropdownListener>()
                .ToDictionary(k => k.Id);

            return base.Layout();
        }

        protected override IEnumerable<IListenerAdapter> GetListenerAdapter()
        {
            foreach (var listener in Listeners)
            {
                yield return listener;
            }
        }

        public void SetSizeOptions(IEnumerable<int> sizes) 
        {
            if(_Dropdowns.TryGetValue(0, out var dropdown)) 
            {
                var listener = dropdown.Listener;

                listener.options.Clear();

                listener.AddOptions(GetOptions(sizes).ToList());
            }
        }

        public void SetDisplayOptions(IEnumerable<int> displays)
        {
            if (_Dropdowns.TryGetValue(1, out var dropdown))
            {
                var listener = dropdown.Listener;

                listener.options.Clear();

                listener.AddOptions(GetOptions(displays).ToList());
            }
        }

        private IEnumerable<TMP_Dropdown.OptionData> GetOptions(IEnumerable<int> contens) 
        {
            foreach(var content in contens) 
            {
                yield return new(content.ToString());
            }
        }
    }
}