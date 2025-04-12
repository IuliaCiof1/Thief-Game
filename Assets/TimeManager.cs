using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static PeriodEnum Period { get; private set; }
    public enum PeriodEnum { AM, PM}


    private float minuteToRealTime = 0.5f; //a minute in game is half a second in real time
    private float timer;

    public static Action OnDayEnded;
    public static Action OnMinuteChanged;

    [SerializeField] int dayStartHour = 10; //AM
    [SerializeField] int dayEndHour = 8; //PM

    void Start()
    {
        Period = PeriodEnum.AM;
        Minute = 0;
        Hour = dayStartHour;
        timer = minuteToRealTime;
    }


    void Update()
    {
        timer -= Time.deltaTime;

        //when minuteToRealTime (half a second) has passed, increment the minute
        if (timer <= 0)
        {
            OnMinuteChanged?.Invoke();
            Minute++;

            if (Minute >= 60)
            {
                Hour++;
                Minute = 0;
            }

            if(Hour==13 && Period == PeriodEnum.AM)
            {
                Period = PeriodEnum.PM;
            }

            if (Hour == dayEndHour && Period == PeriodEnum.PM)
                OnDayEnded?.Invoke();

            timer = minuteToRealTime;
        }

    }
}
