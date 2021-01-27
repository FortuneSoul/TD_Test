using System.Collections;
using UnityEngine;

public class WaypointFollowing : MonoBehaviour
{
	[SerializeField] private float _moveSpeed = default;
	[SerializeField] private float _rotationSpeed = default;

	private Vector2[] _waypoints;
	
	public event System.Action OnReachedLastWaypointEvent = delegate { };
	
	public void SetWaypoints(Vector2[] waypoints)
	{
		_waypoints = waypoints;
		StartCoroutine(FollowWaypointsRoutine());
	}

	private IEnumerator FollowWaypointsRoutine()
	{
		int currentWaypointIdx = 0;

		while (currentWaypointIdx < _waypoints.Length)
		{
			yield return StartCoroutine(RotateRoutine(_waypoints[currentWaypointIdx]));
			yield return StartCoroutine(MoveRoutine(_waypoints[currentWaypointIdx]));
			
			currentWaypointIdx++;
		}
		
		OnReachedLastWaypointEvent.Invoke();
	}

	private IEnumerator RotateRoutine(Vector2 target)
	{
		Vector2 direction = (target - (Vector2) transform.position).normalized;
		float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		float startAngle = transform.rotation.eulerAngles.z;
		float t = 0;

		float duration = Mathf.Abs(Mathf.DeltaAngle(startAngle, targetAngle) / _rotationSpeed);
		
		while (t < duration)
		{
			t += Time.deltaTime;
			float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, t / duration);
			transform.rotation = Quaternion.Euler(Vector3.forward * currentAngle);
			
			yield return null;
		}
	}

	private IEnumerator MoveRoutine(Vector2 target)
	{
		while (Vector2.Distance(transform.position, target) > .02f)
		{
			float velocityMagnitude = _moveSpeed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, target, velocityMagnitude);

			yield return null;
		}
	}
}