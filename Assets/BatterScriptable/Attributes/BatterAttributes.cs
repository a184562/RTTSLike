using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BatterAttributes", menuName = "Baseball/Batter Attributes")]
public class BatterAttributes : ScriptableObject
{
    public string m_strName;
    // public bool m_isRightHands;
    public BatterHand m_BatterHand;
    [Range(0, 100)]
    public int m_fContactLeft;
    [Range(0, 100)]
    public int m_fContactRight;
    [Range(0, 100)]
    public int m_fPowerLeft;
    [Range(0, 100)]
    public int m_fPowerRight;
    [Range(0, 100)]
    public int m_fSpeedRun;
    [Range(0, 100)]
    public int m_fThrowAccuracy;
    [Range(0, 100)]
    public int m_fThrowPower;
    [Range(0, 100)]
    public int m_fVision;
    public Position m_Position;
    public MainPosition m_MPosition;

    public int TotalStat { get {
            return (m_fContactLeft + m_fContactRight + m_fPowerLeft + m_fPowerRight + m_fSpeedRun + m_fThrowAccuracy + m_fThrowPower + m_fVision) / 8 + 10;
     } }
}

public enum BatterHand { Right, Left, Switch };

[System.Serializable]
public class Position
{
    public bool catcher;
    public bool firstBase;
    public bool secondBase;
    public bool thirdBase;
    public bool shortStop;
    public bool leftFielder;
    public bool centerFielder;
    public bool rightFielder;
}

public enum MainPosition
{
    Catcher,
    FirstBaseMan,
    SecondBaseMan,
    ThirdBaseMans,
    ShortStop,
    LeftFielder,
    CenterFielder,
    RightFielder,
    DesignatedHitter,
}