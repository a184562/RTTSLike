using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;

    public PlayerBatting Batter;
    public Pitcher_AI Pitcher;

    public int m_NBattingLine = 0;

    private void Start()
    {
        instance = this;

        if(Batter.CharacterAttribute.TotalStat > 80)
        {
            int checkBattingLine = Batter.CharacterAttribute.m_fPowerLeft + Batter.CharacterAttribute.m_fPowerRight;
            if (checkBattingLine > 160)
            {
                m_NBattingLine = 3;
            }
            else if(checkBattingLine > 140)
            {
                m_NBattingLine = 4;
            }
            else
            {
                m_NBattingLine = 2;
            }
        }
        else if(Batter.CharacterAttribute.TotalStat > 70)
        {
            int checkBattingLine = Batter.CharacterAttribute.m_fPowerLeft + Batter.CharacterAttribute.m_fPowerRight;
            if (checkBattingLine > 140)
            {
                m_NBattingLine = 5;
            }
            else if (checkBattingLine > 120)
            {
                m_NBattingLine = 6;
            }
            else
            {
                m_NBattingLine = 1;
            }
        }
        else
        {
            int checkBattingLine = Batter.CharacterAttribute.m_fPowerLeft + Batter.CharacterAttribute.m_fPowerRight;
            if (checkBattingLine > 120)
            {
                m_NBattingLine = 7;
            }
            else if (checkBattingLine > 100)
            {
                m_NBattingLine = 8;
            }
            else
            {
                m_NBattingLine = 9;
            }
        }
    }
}
