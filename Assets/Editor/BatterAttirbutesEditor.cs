using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BatterAttributes))]
public class BatterAttirbutesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // �⺻ ������Ƽ���� �׸��� �޼��� ȣ��

        BatterAttributes batter = (BatterAttributes)target;

        // ���� TotalStat�� �ν����Ϳ� ǥ��
        EditorGUI.BeginDisabledGroup(true); // ���� �Ұ����ϵ��� ����
        EditorGUILayout.IntField("Total Stat", batter.TotalStat);
        EditorGUI.EndDisabledGroup();
    }
}
