using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(IHaveHealth))]
public class HealthbarHolder : MonoBehaviour
{
	[SerializeField] private Slider _healthBarPrefab = default;
	[SerializeField] private Vector2 _healthBarOffset = default;

	private IHaveHealth _healthInfo;
	private Slider _healthBarInstance;
	private UIManager _uiManager;

	private bool _quitting;
	
	[Inject]
	private void Construct(UIManager uiManager)
	{
		_uiManager = uiManager;
	}
	
	private void Awake()
	{
		_healthInfo = GetComponent<IHaveHealth>();
	}

	private void OnEnable()
	{
		Vector2 spawnPos = (Vector2) transform.position + _healthBarOffset;
		_healthBarInstance = Instantiate(_healthBarPrefab, spawnPos, Quaternion.identity, _uiManager.UICanvas.transform);
	}

	private void OnDisable()
	{
		if(!_quitting)
			Destroy(_healthBarInstance.gameObject);
	}

	private void OnApplicationQuit()
	{
		_quitting = true;
	}

	private void LateUpdate()
	{
		Vector2 pos = (Vector2) transform.position + _healthBarOffset;
		_healthBarInstance.transform.position = pos;

		float fillPercent = (float) _healthInfo.GetCurrentHealth() / _healthInfo.GetMaxHealth();
		_healthBarInstance.normalizedValue = fillPercent;
	}
}