using Loyufei.DomainEvents;
using Loyufei.ViewManagement;
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
                .Bind<PlayerMetrix>()
                .AsSingle();

            Container
                .Bind<ColorEffect>()
                .FromInstance(_ColorEffect)
                .AsSingle();

            #region Factory

            Container
                .BindMemoryPool<Area, AreaPool>()
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
                .Bind<ViewManager>()
                .AsSingle();

            Container
                .Bind<SudokuModel>()
                .AsSingle();

            Container
                .Bind<Buffer>()
                .AsSingle();

            #endregion

            #region Presenter

            Container
                .Bind<GridViewPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<SetupViewPresenter>()
                .AsSingle()
                .NonLazy();

            Container
               .Bind<InputViewPresenter>()
               .AsSingle()
               .NonLazy();

            Container
               .Bind<ConfirmViewPresenter>()
               .AsSingle()
               .NonLazy();

            Container
               .Bind<InfoViewPresenter>()
               .AsSingle()
               .NonLazy();

            Container
               .Bind<SudokuModelPresenter>()
               .AsSingle()
               .NonLazy();

            #endregion

            #region Event

            Container
                .Bind<IDomainEventBus>()
                .To<DomainEventBus>()
                .AsSingle();

            #endregion
        }
    } 
}