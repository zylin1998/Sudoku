using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;
using Zenject;
using System.Linq;

namespace Sudoku
{
    public class SceneProgress : AggregateRoot, IInitializable
    {
        public SceneProgress(DomainEventService service) : base(service)
        {

        }

        public void Initialize() 
        {
            var resolution = Screen.resolutions.Reverse().First();

            Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.ExclusiveFullScreen);
            
            this.SettleEvents(Declarations.Sudoku, new InitScene(), new StartScene());
        }
    }

    public class InitScene : DomainEventBase 
    {

    }

    public class StartScene : DomainEventBase 
    {

    }
}