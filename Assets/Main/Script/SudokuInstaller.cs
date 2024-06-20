using Loyufei.DomainEvents;
using UnityEngine;
using Zenject;

namespace Sudoku
{
    public class SudokuInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject  _Area;
        [SerializeField]
        private GameObject  _Number;
        [SerializeField]
        private GameObject  _Review;
        [SerializeField]
        private GameObject  _Input;
        [SerializeField]
        private ColorEffect _ColorEffect;

        public override void InstallBindings()
        {
            Container
                .Bind<SudokuMetrix>()
                .AsSingle();

            Container
                .Bind<ColorEffect>()
                .FromInstance(_ColorEffect)
                .AsSingle();

            #region Factory

            Container
                .BindMemoryPool<AreaView, AreaPool>()
                .FromComponentInNewPrefab(_Area)
                .AsCached();

            Container
                .BindMemoryPool<NumberListener, NumberPool>()
                .FromComponentInNewPrefab(_Number)
                .AsCached();

            Container
                .BindMemoryPool<ReviewListener, ReviewPool>()
                .FromComponentInNewPrefab(_Review)
                .AsCached();

            Container
                .BindMemoryPool<InputListener, InputPool>()
                .FromComponentInNewPrefab(_Input)
                .AsCached();

            #endregion

            #region Model

            Container
                .Bind<SudokuConstructModel>()
                .AsSingle();

            Container
                .Bind<SudokuQueryModel>()
                .AsSingle();

            #endregion

            #region Presenter

            Container
                .BindInterfacesAndSelfTo<SceneProgress>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<SudokuViewPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<SettingViewPresenter>()
                .AsSingle()
                .NonLazy();

            Container
               .Bind<MaskViewPresenter>()
               .AsSingle()
               .NonLazy();

            Container
               .Bind<MessageViewPresenter>()
               .AsSingle()
               .NonLazy();

            Container
               .Bind<SudokuConstructPresenter>()
               .AsSingle()
               .NonLazy();

            Container
               .Bind<SudokuQueryPresenter>()
               .AsSingle()
               .NonLazy();

            #endregion

            #region Event

            Container
                .DeclareSignal<IDomainEvent>()
                .WithId(Declarations.Sudoku);

            Container
                .Bind<IDomainEventBus>()
                .To<DomainEventBus>()
                .AsCached()
                .WithArguments(Declarations.Sudoku);

            SignalBusInstaller.Install(Container);

            Container
                .Bind<DomainEventService>()
                .AsSingle();

            #endregion
        }
    } 
}