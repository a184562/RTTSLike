using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLobby : MonoBehaviour
{
    [SerializeField]
    BatterAttributes[] batterAttributes;
    [SerializeField]
    BatterStat[] batterStat;

    int dataIndexNum = 0;

    [SerializeField] GameObject selectSlot;
    [SerializeField] TMP_Text nowSlotDataTxt;

    public TMP_Text NowSlotDataTxt { get => nowSlotDataTxt; set => nowSlotDataTxt = value; }

    private void Start()
    {
        ChangeSlotDataTxt();
    }

    public void OnGameStart()
    {
        GameManager.Instance.ChangeSaveData(batterAttributes[dataIndexNum], batterStat[dataIndexNum]);

        SceneManager.LoadScene("PlayScene");
    }

    public void OnCustomCharacter()
    {
        SceneManager.LoadScene("SaveLobby");
    }

    public void OnSelectSlot()
    {
        bool selectSlotActiveState = selectSlot.activeSelf;

        selectSlot.SetActive(!selectSlotActiveState);
    }

    public void OnClose()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ChangeSaveSlot(int SlotNum)
    {
        dataIndexNum = SlotNum;

        GameManager.Instance.ChangeSaveData(batterAttributes[dataIndexNum], batterStat[dataIndexNum]);
    }

    public void ChangeSlotDataTxt()
    {
        BatterAttributes slotAttributeData = batterAttributes[dataIndexNum];
        BatterStat slotStatData = batterStat[dataIndexNum];

        if(string.IsNullOrEmpty(slotAttributeData.m_strName))
        {
            NowSlotDataTxt.text = "Data is Empty";
        }
        else if(slotStatData.PA == 0)
        {
            NowSlotDataTxt.text = "No Stat";
        }
        else
        {
            NowSlotDataTxt.text = $"Name : {slotAttributeData.m_strName} \n" +
            $"AVG : {slotStatData.Average} - OBP : {slotStatData.OBP} - SLG : {slotStatData.SLG} \n" +
            $"Hit : {slotStatData.Hit} - HR : {slotStatData.HomeRun} \n" +
            $"RBI : {slotStatData.RBI} - R : {slotStatData.Run} - SB : {slotStatData.Steal}";
        };
    }
}
