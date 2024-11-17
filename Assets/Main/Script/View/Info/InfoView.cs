using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei;
using Loyufei.ViewManagement;

namespace Sudoku
{
    public class InfoView : MonoViewBase
    {
        [SerializeField]
        private Transform _Review;

        [Inject]
        private ReviewPool _ReviewPool;

        public List<ReviewListener> Reviews { get; } = new();

        public override object ViewId => Declarations.Info;

        public ILayout Layout(int length) 
        {
            for (int i = 1; i <= length; i++) 
            {
                var review = _ReviewPool.Spawn(i);

                review.transform.SetParent(_Review);
                review.transform.localScale = Vector3.one;

                Reviews.Add(review);
            }

            return Layout();
        }

        public override void RemoveLayout()
        {
            Reviews.ForEach(r =>
            {
                _ReviewPool.Despawn(r);
            });

            Reviews.Clear();
        }

        public void SetBinder(Action<IListenerAdapter> binder) 
        {
            _ReviewPool.Binder = binder;
        }
    }
}
