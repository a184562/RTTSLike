using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public static SaveSlot Instance;

    public BatterAttributes attributesSave1;
    [SerializeField] BatterAttributes attributesSave2;
    [SerializeField] BatterAttributes attributesSave3;
    [SerializeField] BatterAttributes attributesSave4;
    [SerializeField] BatterAttributes attributesSave5;
    [SerializeField] BatterAttributes attributesSave6;
    [SerializeField] BatterAttributes attributesSave7;
    [SerializeField] BatterAttributes attributesSave8;
    [SerializeField] BatterAttributes attributesSave9;
    [SerializeField] BatterAttributes attributesSave10;

    public TMP_Dropdown m_objSaveDropDown;

    [SerializeField] GameObject m_txtName;
    [SerializeField] GameObject m_txtHand;
    [SerializeField] GameObject m_txtPos;
    [SerializeField] GameObject m_txtConL;
    [SerializeField] GameObject m_txtConR;
    [SerializeField] GameObject m_txtPowL;
    [SerializeField] GameObject m_txtPowR;
    [SerializeField] GameObject m_txtSpd;
    [SerializeField] GameObject m_txtVis;
    [SerializeField] GameObject m_txtAcc;
    [SerializeField] GameObject m_txtThr;

    public BatterAttributes nowAttributesSaveSlot;

    [SerializeField] GameObject warningPanel;
    [SerializeField] GameObject uiPanel;

    [SerializeField] CustomUIController uiController;

    private void Start()
    {
        Instance = this;

        nowAttributesSaveSlot = attributesSave1;
        ChangeSlotData(nowAttributesSaveSlot);
    }

    public void SlotChange()
    {
        
        switch (m_objSaveDropDown.value)
        {
            case 0:
                nowAttributesSaveSlot = attributesSave1;
                break;
            case 1:
                nowAttributesSaveSlot = attributesSave2;
                break;
            case 2:
                nowAttributesSaveSlot = attributesSave3;
                break;
            case 3:
                nowAttributesSaveSlot = attributesSave4;
                break;
            case 4:
                nowAttributesSaveSlot = attributesSave5;
                break;
            case 5:
                nowAttributesSaveSlot = attributesSave6;
                break;
            case 6:
                nowAttributesSaveSlot = attributesSave7;
                break;
            case 7:
                nowAttributesSaveSlot = attributesSave8;
                break;
            case 8:
                nowAttributesSaveSlot = attributesSave9;
                break;
            case 9:
                nowAttributesSaveSlot = attributesSave10;
                break;
        }

        Debug.Log(nowAttributesSaveSlot.name.ToString());

        ChangeSlotData(nowAttributesSaveSlot);
    }

    public  void ChangeSlotData(BatterAttributes nowSaveSlot)
    {
        TMP_Text Name = m_txtName.GetComponent<TMP_Text>();
        TMP_Text Hand = m_txtHand.GetComponent<TMP_Text>();
        TMP_Text Position = m_txtPos.GetComponent<TMP_Text>();
        TMP_Text ConL = m_txtConL.GetComponent<TMP_Text>();
        TMP_Text ConR = m_txtConR.GetComponent<TMP_Text>();
        TMP_Text PowL = m_txtPowL.GetComponent<TMP_Text>();
        TMP_Text PowR = m_txtPowR.GetComponent<TMP_Text>();;
        TMP_Text Speed = m_txtSpd.GetComponent<TMP_Text>();
        TMP_Text Vision = m_txtVis.GetComponent<TMP_Text>();
        TMP_Text Acc = m_txtAcc.GetComponent<TMP_Text>();
        TMP_Text Throw = m_txtThr.GetComponent<TMP_Text>();

        Debug.Log(attributesSave1.m_strName.ToString());

        if(!string.IsNullOrEmpty(nowSaveSlot.m_strName))
        {
            Name.text = "Name : " + nowSaveSlot.m_strName;
            switch((int)nowSaveSlot.m_BatterHand)
            {
                case 0:
                    Hand.text = "Hand : R";
                    break;
                case 1:
                    Hand.text = "Hand : L";
                    break;
                case 2:
                    Hand.text = "Hand : S";
                    break;
            }
            switch((int)nowSaveSlot.m_MPosition)
            {
                case 0:
                    Position.text = "Pos : C";
                    break;
                case 1:
                    Position.text = "Pos : 1B";
                    break;
                case 2:
                    Position.text = "Pos : 2B";
                    break;
                case 3:
                    Position.text = "Pos : 3B";
                    break;
                case 4:
                    Position.text = "Pos : SS";
                    break;
                case 5:
                    Position.text = "Pos : LF";
                    break;
                case 6:
                    Position.text = "Pos : CF";
                    break;
                case 7:
                    Position.text = "Pos : RF";
                    break;
                case 8:
                    Position.text = "Pos : DH";
                    break;
            }
            ConL.text = "ConL : " + nowSaveSlot.m_fContactLeft.ToString();
            ConR.text = "ConR : " + nowSaveSlot.m_fContactRight.ToString();
            PowL.text = "PowL : " + nowSaveSlot.m_fPowerLeft.ToString();
            PowR.text = "PowR : " + nowSaveSlot.m_fPowerRight.ToString();
            Speed.text = "SPD : " + nowSaveSlot.m_fSpeedRun.ToString();
            Vision.text = "VIS : " + nowSaveSlot.m_fVision.ToString();
            Acc.text = "ACC : " + nowSaveSlot.m_fThrowAccuracy.ToString();
            Throw.text = "ARM : " + nowSaveSlot.m_fThrowPower.ToString();
        }
        else
        {
            Name.text = "Name : ";
            Hand.text = "Hand : ";
            Position.text = "Pos : ";
            ConL.text = "ConL : ";
            ConR.text = "ConR : ";  
            PowL.text = "PowL : ";
            PowR.text = "PowR : ";
            Speed.text = "SPD : ";
            Vision.text = "VIS : ";
            Acc.text = "ACC : ";
            Throw.text = "ARM : ";
        }
    }

    public void SaveAttributeData()
    {
        if (!string.IsNullOrEmpty(nowAttributesSaveSlot.m_strName))
        {
            warningPanel.SetActive(true);
            uiPanel.SetActive(false);
        }
        else
        {
            uiController.OnSave(nowAttributesSaveSlot);
        }
    }

    public void OverwriteSave()
    {
        uiController.OnSave(nowAttributesSaveSlot);
    }
}
