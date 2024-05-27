using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FieldingManager : MonoBehaviour
{
    public static FieldingManager instance;

    [SerializeField] GameObject[] FielderSpot;
    public GameObject[] FieldersPosition;

    [HideInInspector]
    public Vector3 m_vecLandingSpot = Vector3.zero;

    [HideInInspector]
    public bool m_isFieldingState = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fielding(Vector3 LandingSpot)
    {
        float m_fDistanceToBallFromFielder = 100000000.0f;
        int m_nIndex = 9;

        for(int i = 0; i < 9; i++)
        {
            if(m_fDistanceToBallFromFielder >= Vector3.Distance(LandingSpot, FieldersPosition[i].transform.position))
            {
                m_fDistanceToBallFromFielder = Mathf.Min(m_fDistanceToBallFromFielder, Vector3.Distance(LandingSpot, FieldersPosition[i].transform.position));
                m_nIndex = i;
            }
        }

        Fielder nowFielder = FieldersPosition[m_nIndex].GetComponent<Fielder>();

        // nowFielder.m_isFielding = true;
        nowFielder.ballLandingPosition = LandingSpot;
    }
}
