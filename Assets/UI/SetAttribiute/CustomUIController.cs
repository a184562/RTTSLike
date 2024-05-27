using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomUIController : MonoBehaviour
{
    [SerializeField] TMP_InputField nameText;
    [SerializeField] TextMeshProUGUI m_TMPConL;
    [SerializeField] TextMeshProUGUI m_TMPConR;
    [SerializeField] TextMeshProUGUI m_TMPPowL;
    [SerializeField] TextMeshProUGUI m_TMPPowR;
    [SerializeField] TextMeshProUGUI m_TMPSpd;
    [SerializeField] TextMeshProUGUI m_TMPVis;
    [SerializeField] TextMeshProUGUI m_TMPAcc;
    [SerializeField] TextMeshProUGUI m_TMPArm;
    [SerializeField] TMP_Dropdown styleDropDown;
    [SerializeField] TMP_Dropdown handDropdown;
    [SerializeField] TMP_Dropdown positionDropdown;
    BatterAttributes customAttribute;
    [SerializeField] GameObject m_UISaveSlot;
    [SerializeField] GameObject m_UISaveFailWarning;
    [SerializeField] GameObject m_UICanvas;


    public void SetAttributeByStyle()
    {
        customAttribute = ScriptableObject.CreateInstance<BatterAttributes>();


        if (styleDropDown.value == 0)        // 파워히터
        {
            switch(handDropdown.value)
            {
                case 0:         // 우타자
                    customAttribute.m_fContactLeft = 45;
                    customAttribute.m_fContactRight = 40;
                    customAttribute.m_fPowerLeft = 60;
                    customAttribute.m_fPowerRight = 55;
                    customAttribute.m_fSpeedRun = 40;
                    customAttribute.m_fThrowAccuracy = 50;
                    customAttribute.m_fThrowPower = 55;
                    customAttribute.m_fVision = 50;
                    break;
                case 1:         // 좌타자
                    customAttribute.m_fContactLeft = 40;
                    customAttribute.m_fContactRight = 45;
                    customAttribute.m_fPowerLeft = 55;
                    customAttribute.m_fPowerRight = 60;
                    customAttribute.m_fSpeedRun = 40;
                    customAttribute.m_fThrowAccuracy = 50;
                    customAttribute.m_fThrowPower = 55;
                    customAttribute.m_fVision = 50;
                    break;
                case 2:         // 스위치
                    customAttribute.m_fContactLeft = 40;
                    customAttribute.m_fContactRight = 40;
                    customAttribute.m_fPowerLeft = 60;
                    customAttribute.m_fPowerRight = 60;
                    customAttribute.m_fSpeedRun = 40;
                    customAttribute.m_fThrowAccuracy = 50;
                    customAttribute.m_fThrowPower = 55;
                    customAttribute.m_fVision = 50;
                    break;
            }
        }
        else if(styleDropDown.value == 1)       // 컨택형
        {
            switch (handDropdown.value)
            {
                case 0:         // 우타자
                    customAttribute.m_fContactLeft = 60;
                    customAttribute.m_fContactRight = 55;
                    customAttribute.m_fPowerLeft = 45;
                    customAttribute.m_fPowerRight = 40;
                    customAttribute.m_fSpeedRun = 55;
                    customAttribute.m_fThrowAccuracy = 50;
                    customAttribute.m_fThrowPower = 40;
                    customAttribute.m_fVision = 50;
                    break;
                case 1:         // 좌타자
                    customAttribute.m_fContactLeft = 55;
                    customAttribute.m_fContactRight = 60;
                    customAttribute.m_fPowerLeft = 40;
                    customAttribute.m_fPowerRight = 45;
                    customAttribute.m_fSpeedRun = 55;
                    customAttribute.m_fThrowAccuracy = 50;
                    customAttribute.m_fThrowPower = 40;
                    customAttribute.m_fVision = 50;
                    break;
                case 2:         // 스위치
                    customAttribute.m_fContactLeft = 60;
                    customAttribute.m_fContactRight = 60;
                    customAttribute.m_fPowerLeft = 40;
                    customAttribute.m_fPowerRight = 40;
                    customAttribute.m_fSpeedRun = 55;
                    customAttribute.m_fThrowAccuracy = 50;
                    customAttribute.m_fThrowPower = 40;
                    customAttribute.m_fVision = 50;
                    break;
            }
        }
        else                                // 수비형
        {
            switch (handDropdown.value)
            {
                case 0:         // 우타자
                    customAttribute.m_fContactLeft = 45;
                    customAttribute.m_fContactRight = 40;
                    customAttribute.m_fPowerLeft = 45;
                    customAttribute.m_fPowerRight = 40;
                    customAttribute.m_fSpeedRun = 55;
                    customAttribute.m_fThrowAccuracy = 60;
                    customAttribute.m_fThrowPower = 60;
                    customAttribute.m_fVision = 50;
                    break;
                case 1:         // 좌타자
                    customAttribute.m_fContactLeft = 40;
                    customAttribute.m_fContactRight = 45;
                    customAttribute.m_fPowerLeft = 40;
                    customAttribute.m_fPowerRight = 45;
                    customAttribute.m_fSpeedRun = 55;
                    customAttribute.m_fThrowAccuracy = 60;
                    customAttribute.m_fThrowPower = 60;
                    customAttribute.m_fVision = 50;
                    break;
                case 2:         // 스위치
                    customAttribute.m_fContactLeft = 40;
                    customAttribute.m_fContactRight = 40;
                    customAttribute.m_fPowerLeft = 45;
                    customAttribute.m_fPowerRight = 45;
                    customAttribute.m_fSpeedRun = 55;
                    customAttribute.m_fThrowAccuracy = 60;
                    customAttribute.m_fThrowPower = 60;
                    customAttribute.m_fVision = 50;
                    break;
            }
        }

        SetAttributeText();
    }

    void SetAttributeText()
    {
        m_TMPConL.text = "ConL : " + customAttribute.m_fContactLeft.ToString();
        m_TMPConR.text = "ConR : " + customAttribute.m_fContactRight.ToString();
        m_TMPPowL.text = "PowL : " + customAttribute.m_fPowerLeft.ToString();
        m_TMPPowR.text = "PowR : " + customAttribute.m_fPowerRight.ToString();
        m_TMPSpd.text = "SPD : " + customAttribute.m_fSpeedRun.ToString();
        m_TMPVis.text = "VIS : " + customAttribute.m_fVision.ToString();
        m_TMPAcc.text = "ACC : " + customAttribute.m_fThrowAccuracy.ToString();
        m_TMPArm.text = "ARM : " + customAttribute.m_fThrowPower.ToString();
    }

    public void OnSaveSlot()
    {
        SaveSlot saveSlot = m_UISaveSlot.GetComponent<SaveSlot>();
        saveSlot.nowAttributesSaveSlot = saveSlot.attributesSave1;
        saveSlot.m_objSaveDropDown.value = 0;
        m_UISaveSlot.SetActive(!m_UISaveSlot.activeSelf);
    }

    public void OnSave(BatterAttributes toSaveAttribute)
    {
        if(styleDropDown.value == 3)
        {
            m_UISaveFailWarning.SetActive(true);
            m_UICanvas.SetActive(false);
        }
        else
        {
            toSaveAttribute.m_fContactLeft = customAttribute.m_fContactLeft;
            toSaveAttribute.m_fContactRight = customAttribute.m_fContactRight;
            toSaveAttribute.m_fPowerLeft = customAttribute.m_fPowerLeft;
            toSaveAttribute.m_fPowerRight = customAttribute.m_fPowerRight;
            toSaveAttribute.m_fSpeedRun = customAttribute.m_fSpeedRun;
            toSaveAttribute.m_fThrowAccuracy = customAttribute.m_fThrowAccuracy;
            toSaveAttribute.m_fThrowPower = customAttribute.m_fThrowPower;
            toSaveAttribute.m_fVision = customAttribute.m_fVision;
            toSaveAttribute.m_BatterHand = (BatterHand)handDropdown.value;
            toSaveAttribute.m_MPosition = (MainPosition)positionDropdown.value;
            toSaveAttribute.m_strName = nameText.text;
        }
        SaveSlot saveSlot = m_UISaveSlot.GetComponent<SaveSlot>();
        saveSlot.nowAttributesSaveSlot = saveSlot.attributesSave1;
        m_UISaveSlot.SetActive(false);
    }

    public void GoToMainLobby()
    {
        SceneManager.LoadScene("MainLobby");
    }
}
