using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StealTimer : MonoBehaviour
{
    
    Slider slider;
   public float Value { get;  set; }
    public float MaxValue { get; private set; }

   

    private void OnEnable()
    {
        slider = GetComponent<Slider>();
        Value = slider.value;
        slider.value = slider.maxValue;
    }

    private void OnDisable()
    {
        Value = slider.value;
        slider.value = slider.maxValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

        Value = slider.value;
        slider.value = slider.maxValue;

        MaxValue = slider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        Value -= Time.deltaTime;
       slider.value = Value;
    }
}
