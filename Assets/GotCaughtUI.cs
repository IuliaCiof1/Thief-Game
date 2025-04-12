using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GotCaughtUI : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text reputationText;


    private void OnEnable()
    {
        moneyText.text = $"- {PlayerStats.money}$";
        reputationText.text = $"- {PlayerStats.reputation}";
    }
}
