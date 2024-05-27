using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BatterAttributes))]
public class BatterAttirbutesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // 기본 프로퍼티들을 그리는 메서드 호출

        BatterAttributes batter = (BatterAttributes)target;

        // 계산된 TotalStat을 인스펙터에 표시
        EditorGUI.BeginDisabledGroup(true); // 수정 불가능하도록 설정
        EditorGUILayout.IntField("Total Stat", batter.TotalStat);
        EditorGUI.EndDisabledGroup();
    }
}
