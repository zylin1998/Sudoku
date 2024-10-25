using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public class FlexibleRepositoryBase<TId, TData> : FlexibleRepositoryBase<TId, TData, RepositBase<TId, TData>>
    {
        public FlexibleRepositoryBase() : base()
        {
            
        }

        public FlexibleRepositoryBase(int capacity) : this(capacity, false, 0)
        {
            
        }

        public FlexibleRepositoryBase(int capacity, bool limited, int maxCapacity) 
            : base(capacity, limited, maxCapacity)
        {
            
        }

        public FlexibleRepositoryBase(IEnumerable<RepositBase<TId, TData>> reposits, bool limited, int maxCapacity)
            : base(reposits, limited, maxCapacity)
        {
            
        }
    }

    public class FlexibleRepositoryBase<TId, TData, TReposit> : RepositoryBase<TId, TData, TReposit>
        , IFlexibleRepository<TData>
        where TReposit : IReposit<TData>
    {
        public FlexibleRepositoryBase() : base()
        {
            _Limited     = false;
            _MaxCapacity = 0;
        }

        public FlexibleRepositoryBase(int capacity) : this(capacity, true,  capacity)
        {

        }

        public FlexibleRepositoryBase(int capacity, bool limited, int maxCapacity) : base(capacity)
        {
            _Limited     = limited;
            _MaxCapacity = maxCapacity;
        }

        public FlexibleRepositoryBase(IEnumerable<TReposit> reposits, bool limited, int maxCapacity) 
            : base(reposits)
        {
            _Limited     = limited;
            _MaxCapacity = maxCapacity;
        }

        [SerializeField]
        protected bool _Limited;
        [SerializeField]
        protected int _MaxCapacity;
        
        public bool Limited  => _Limited;
        
        public IEnumerable<IReposit<TData>> Create(int amount)
        {
            var limit  = _MaxCapacity - Capacity;
            var count  = Limited ? Mathf.Clamp(amount, 0, limit) : amount;
            
            for(var i = 0; i < count; i++) 
            {
                var expand = Activator.CreateInstance<TReposit>();

                _Reposits.Add(expand);

                yield return expand;
            }
        }

        public void Release(int index) 
        {
            if (index >= _Reposits.Count) { return; }

            _Reposits.RemoveRange(index, _Reposits.Count - index);
        }
    }
}
