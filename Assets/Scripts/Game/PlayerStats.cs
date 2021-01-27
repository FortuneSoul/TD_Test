public class PlayerStats
{
	public static event System.Action OnPlayerStatsUpdatedEvent = delegate { };
	
	private int _gold;
	public int Gold
	{
		get => _gold;
		set
		{
			_gold = value;
			OnPlayerStatsUpdatedEvent.Invoke();
		}
	}

	private int _health;
	public int Health
	{
		get => _health;
		set
		{
			_health = value;
			OnPlayerStatsUpdatedEvent.Invoke();
		}
	}
}