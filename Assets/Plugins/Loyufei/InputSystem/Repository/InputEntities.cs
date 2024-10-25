using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loyufei
{
    [Serializable]
    public class InputEntities : EntityForm<InputEntity, InputEntity>
    {
        public InputEntities(IEnumerable<InputEntity> entities) : base(default)
        {
            _Entities = entities.ToList();
        }
        
        [SerializeField]
        private List<InputEntity> _Entities;

        public override Dictionary<object, InputEntity> Dictionary
            => _Entities.ToDictionary(k => k.Identity);

        public void Default(InputEntities entities) 
        {
            foreach(InputEntity entity in entities) 
            {
                var target = _Entities.FirstOrDefault(t => Equals(entity.Identity, t.Identity));

                if (target.IsDefault()) { _Entities.Add(entity); }

                else { target.Default(entity); }
            }
        }
    }
}