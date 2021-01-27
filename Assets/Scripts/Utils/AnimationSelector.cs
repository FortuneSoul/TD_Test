using UnityEngine;

public class AnimationSelector : PropertyAttribute
{
	public string FieldName { get; }

	public AnimationSelector()
	{
	}

	public AnimationSelector(string fieldName)
	{
		FieldName = fieldName;
	}
}