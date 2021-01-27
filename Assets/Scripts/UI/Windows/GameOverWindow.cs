using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class GameOverWindow : UIWindow
{
	[SerializeField] private TMP_Text _scoreText = default;
	[SerializeField] private string _scoreTextTemplate = default;
	
	[Space]
	
	[SerializeField] private Animation _animation = default;
	
	[AnimationSelector]
	[SerializeField] 
	private string _showAnimation = default;
	
	[AnimationSelector]
	[SerializeField] 
	private string _hideAnimation = default;
	
	private Session _session;

	[Inject]
	private void Construct(Session session)
	{
		_session = session;
	}
	
	private void OnEnable()
	{
		_animation.Play(_showAnimation);

		int score = GetScore();
		RefreshScoreText(score);
	}

	private int GetScore()
	{
		return _session.GetSessionScore();
	}

	private void RefreshScoreText(int score)
	{
		_scoreText.text = string.Format(_scoreTextTemplate, score);
	}
	
	public override void Hide()
	{
		StartCoroutine(HideRoutine());
	}

	private IEnumerator HideRoutine()
	{
		_animation.Play(_hideAnimation);

		while (_animation.IsPlaying(_hideAnimation))
			yield return null;
		
		gameObject.SetActive(false);
	}
	
	public override WindowType GetWindowType()
	{
		return WindowType.GameOverWindow;
	}
}