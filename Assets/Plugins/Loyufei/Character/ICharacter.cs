using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Character
{
    public interface ICharacter
    {
        public TElement Get<TElement>();
    }
}