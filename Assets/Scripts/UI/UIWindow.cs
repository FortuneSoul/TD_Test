using UnityEngine;

public enum WindowType
{
	None = 0,
	GameOverWindow
}

public interface IUIWindow
{
	void Show();
	void Hide();

	WindowType GetWindowType();
}

public abstract class UIWindow : MonoBehaviour, IUIWindow
{
	public abstract WindowType GetWindowType();

	public virtual void Show()
	{
		gameObject.SetActive(true);
	}

	public virtual void Hide()
	{
		gameObject.SetActive(false);
	}
}
