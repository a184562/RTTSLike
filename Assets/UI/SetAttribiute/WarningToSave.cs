using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningToSave : MonoBehaviour
{
    [SerializeField] GameObject uiPanel;
    [SerializeField] SaveSlot saveSlot;

    public void ClickSaveBtn()
    {
        saveSlot.OverwriteSave();

        gameObject.SetActive(false);
        uiPanel.SetActive(true);
    }

    public void ClickCancelBtn() 
    {
        gameObject.SetActive(false);
        uiPanel.SetActive(true);
    }
}
