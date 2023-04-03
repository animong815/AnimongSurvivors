using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (BgObjectList))]
public class EditorBgObjectList : Editor
{
	public override void OnInspectorGUI()
	{
		if(GUILayout.Button("Set List Object"))
		{
			((BgObjectList)target).SetObjectList();
		}

		base.OnInspectorGUI();

	}
}
