using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "BatterStats", menuName = "Baseball/Batter Stats")]
public class BatterStat : ScriptableObject
{
    public int PA;                  // 타석수
    public int AB;                  // 타수
    public int Hit;                  // 안타
    public int Double;           // 2루타
    public int Triple;             // 3루타
    public int HomeRun;      // 홈런
    public int RBI;                // 타점
    public int Run;               // 득점
    public int Steal;             // 도루
    public int Games;          // 경기수
    public int BB;                // 사사구
    public int SO;                // 삼진
    public int HBP;             // 사구
    public int SF;                // 희생타


    public float Average;           // 타율
    public float OBP;               // 출루율
    public float SLG;               // 장타율
    public float OPS;               // OPS
    public float BBPercent;         // 사사구확률
    public float SOPercent;         // 삼진률
    public float BBPerSO;           // 사사구 대비 삼진율

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


