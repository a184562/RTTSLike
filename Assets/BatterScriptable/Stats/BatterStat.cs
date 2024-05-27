using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "BatterStats", menuName = "Baseball/Batter Stats")]
public class BatterStat : ScriptableObject
{
    public int PA;                  // Ÿ����
    public int AB;                  // Ÿ��
    public int Hit;                  // ��Ÿ
    public int Double;           // 2��Ÿ
    public int Triple;             // 3��Ÿ
    public int HomeRun;      // Ȩ��
    public int RBI;                // Ÿ��
    public int Run;               // ����
    public int Steal;             // ����
    public int Games;          // ����
    public int BB;                // ��籸
    public int SO;                // ����
    public int HBP;             // �籸
    public int SF;                // ���Ÿ


    public float Average;           // Ÿ��
    public float OBP;               // �����
    public float SLG;               // ��Ÿ��
    public float OPS;               // OPS
    public float BBPercent;         // ��籸Ȯ��
    public float SOPercent;         // ������
    public float BBPerSO;           // ��籸 ��� ������

    public void CalculateStatistics()
    {
        Average = AB > 0 ? (float)Hit / AB : 0;
        OBP = AB + BB + HBP + SF > 0 ? (float)(Hit + BB + HBP) / PA : 0;
        SLG = AB > 0 ? (float)(Hit + 2 * Double + 3 * Triple + 4 * HomeRun) / AB : 0;
        OPS = OBP + SLG;
        BBPercent = PA > 0 ? (float)BB / PA : 0;
        SOPercent = PA > 0 ? (float)SO / PA : 0;
        BBPerSO = SO > 0 ? (float)BB / SO : 0;
    }
}


