using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIWindow[] _windows = default;
    [SerializeField] private Canvas _uiCanvas = default;

    public Canvas UICanvas => _uiCanvas;
    
    private readonly Dictionary<WindowType, IUIWindow> _windowsDict = new Dictionary<WindowType, IUIWindow>();
    
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (UIWindow uiWindow in _windows)
            _windowsDict.Add(uiWindow.GetWindowType(), uiWindow);
    }

    public void ShowWindow(WindowType windowType)
    {
        _windowsDict[windowType].Show();        
    }

    public void HideWindow(WindowType windowType)
    {
        _windowsDict[windowType].Hide();
    }
}
