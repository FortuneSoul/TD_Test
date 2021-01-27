using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
	[SerializeField] private LevelSettings _level = default;
	
	private LevelGenerator _levelGenerator;
	private UIManager _uiManager;
	private LevelData _levelData;
	
	public Session Session { get; private set; }

	[Inject]
	private void Construct(SignalBus signalBus, LevelGenerator levelGenerator, Session session, UIManager uiManager)
	{
		_levelGenerator = levelGenerator;
		_uiManager = uiManager;
		
		Session = session;
	}
	
	private void Start()
	{
		StartSession();
	}

	private void StartSession()
	{
		_levelData = _levelGenerator.CreateLevel(_level);
		
		Session.Start(_levelData);
	}
	
	public void RestartSession()
	{
		Session.Start(_levelData);
	}

	public void OnPlayerDead()
	{
		EndSession();
	}
	
	public void EndSession()
	{
		_uiManager.ShowWindow(WindowType.GameOverWindow);
		Session.End();
	}
}