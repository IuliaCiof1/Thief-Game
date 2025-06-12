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
        moneyText.text = $"- {PlayerStats.Instance.money}$";
        print("reputation "+ PlayerStats.Instance.GetReputationLoss());
        reputationText.text = $"- {PlayerStats.Instance.GetReputationLoss()}";
    }
}
