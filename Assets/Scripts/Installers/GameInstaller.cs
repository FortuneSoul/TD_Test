using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameManager _gameManager = default;
    [SerializeField] private LevelGenerator _levelGenerator = default;
    [SerializeField] private Spawner _spawner = default;

    [Space] 
    
    [SerializeField] private GameSettings _gameSettings = default;

    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle();
        Container.Bind<LevelGenerator>().FromInstance(_levelGenerator).AsSingle();
        Container.Bind<Spawner>().FromInstance(_spawner).AsSingle();

        Container.BindInstance(_gameSettings).AsSingle();
        
        Container.BindInterfacesAndSelfTo<Session>().AsSingle();

        Container.Bind<Registry<Tower>>().AsSingle();
        Container.Bind<Registry<Enemy>>().AsSingle();
        
        Container.BindFactory<EnemyCommonSettings, Enemy, Enemy.Factory>()
            .FromPoolableMemoryPool<EnemyCommonSettings, Enemy, EnemyPool>(poolBinder => 
                poolBinder.WithInitialSize(20)
                    .FromComponentInNewPrefab(_gameSettings.enemyPrefab)
                    .UnderTransformGroup("Enemies"));

        Container.BindFactory<Tower, Tower.Factory>().FromComponentInNewPrefab(_gameSettings.towerPrefab).UnderTransformGroup("Towers");
        
        Container.Bind<PlayerStats>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerHealthObserver>().AsSingle();
        Container.Bind<TowerUpgradeController>().AsSingle();
        
        Container.Bind<UIManager>().FromComponentInHierarchy().AsCached();
        
        GameSignalsInstaller.Install(Container);
    }
    
    private class EnemyPool : MonoPoolableMemoryPool<EnemyCommonSettings, IMemoryPool, Enemy> {}
}