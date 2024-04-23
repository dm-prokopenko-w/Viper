using System;
using LevelsSystem;
using GameplaySystem;
using PlayerSystem;
using ItemSystem;
using VContainer.Unity;
using VContainer;
using ContentCellSystem;
using CellsSystem;
using CoroutineSystem;
using EnemySystem;
using VFXSystem;

namespace Game
{
    public class BootStarter : LifetimeScope
    {
		protected override void Configure(IContainerBuilder builder)
        {            
            builder.Register<ItemController>(Lifetime.Scoped);
            builder.Register<ContentCellController>(Lifetime.Scoped);
            builder.Register<GameplayController>(Lifetime.Scoped).As<GameplayController, IStartable>();
            builder.Register<CellsController>(Lifetime.Scoped).As<CellsController, IStartable>();
            builder.Register<PopupController>(Lifetime.Scoped).As<PopupController, IStartable>();
            builder.Register<PlayerController>(Lifetime.Scoped).As<PlayerController, IStartable, IDisposable>();
            
            builder.RegisterComponentInHierarchy<AssetLoader>();
            builder.RegisterComponentInHierarchy<CoroutineHandler>();
		}       
    }
}