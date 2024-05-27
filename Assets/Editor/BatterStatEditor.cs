using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BatterStat))]
public class BatterStatEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BatterStat stat = (BatterStat)target;

        if (GUILayout.Button("Calculate Statistics"))
        {
            stat.CalculateStatistics();
            EditorUtility.SetDirty(stat); // 변경 사항을 저장하도록 Unity에 알림
        }
    }
}
