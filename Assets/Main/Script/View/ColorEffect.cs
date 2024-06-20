using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku
{
    [Serializable]
    public class ColorEffect
    {
        [SerializeField]
        private Color _Normal;
        [SerializeField]
        private Color _Warning;
        [SerializeField]
        private Color _Review;

        public Color Normal => _Normal;
        public Color Warning => _Warning;
        public Color Review => _Review;
    }
}
