using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;

namespace Sudoku
{
    public class Buffer
    {
        private List<(Offset2DInt offset, int number)> _Displays;
        
        public void SetDisplay(params (Offset2DInt offset, int number)[] displays)
        {
            _Displays = displays.ToList();
        }

        public void SetDisplay(IEnumerable<(Offset2DInt offset, int number)> displays) 
        {
            _Displays = displays.ToList();
        }

        public IEnumerable<(Offset2DInt offset, int number)> GetDisplay() 
        {
            foreach (var display in _Displays) 
            {
                yield return display;
            }

            _Displays.Clear();
        }
    }
}