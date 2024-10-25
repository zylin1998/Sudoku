using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.DataUpdate
{
    public interface IContext : IIdentity
    {
        
    }

    public interface IContext<T> : IContext, IObserver<T> 
    {

    }
}