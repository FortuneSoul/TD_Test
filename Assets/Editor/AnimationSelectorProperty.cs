using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(AnimationSelector))]
public class AnimationSelectorProperty : PropertyDrawer
{
	private Animation _animation;

	private void Refresh(SerializedProperty property)
	{
		if (_animation)
			return;

		string field = (attribute as AnimationSelector).FieldName;
		if (string.IsNullOrEmpty(field))
		{
			Component component = (property.serializedObject.targetObject as Component);
			_animation = component.GetComponent<Animation>();
		}
		else
		{
			_animation = property.serializedObject.FindProperty(field)?.objectReferenceValue as Animation;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		Refresh(property);

		return _animation == null ? 0 : base.GetPropertyHeight(property, label);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		Refresh(property);

		if (!_animation)
			return;

		Rect rect = EditorGUI.PrefixLabel(position, label, property.prefabOverride ? EditorStyles.boldLabel : EditorStyles.label);
		rect.xMin -= (float)(property.depth * 16);

		List<string> animations = new List<string>();
		animations.Add("<None>");

		AnimationUtility.GetAnimationClips(_animation.gameObject).ToList().ForEach(x => animations.Add(x.name));

		int idx = string.IsNullOrEmpty(property.stringValue) ? 0 : animations.IndexOf(property.stringValue);

		int newIdx = EditorGUI.Popup(rect, idx, animations.ToArray());
		if (idx != newIdx)
			property.stringValue = newIdx == 0 ? string.Empty : animations[newIdx];
	}
}