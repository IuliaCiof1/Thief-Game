using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReputationUI : MonoBehaviour
{
    [SerializeField] TMP_Text reputationStats;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        reputationStats.text = PlayerStats.reputation.ToString();
    }
}
