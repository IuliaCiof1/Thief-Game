using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EndingManager
{
    public enum EndingType { rentDue, reputationWithDeaths, allDead, bestEnding }

    public static event Action<string> endingEvent;


    public static void Trigger(EndingType endingType)
    {
        Dictionary<EndingType, string> endDict = new Dictionary<EndingType, string>{
            { EndingType.rentDue, "You did not pay the rent in time. \nYou got evicted and now you will live on the cold streets for the rest of your life"},
            {EndingType.reputationWithDeaths, "Congratulation, with your reputation you finally opened your own business and became a millionare!\n But you left most of your family to die" },
            {EndingType.allDead, "None of your family survied. \nYou are a reputable man, but at what cost.." },
            {EndingType.bestEnding,"You and your family are all well and happy\n With a good reputation, you and your family left the city on private cruise" }
        };

        
        Debug.Log(endingType);
        endingEvent.Invoke(endDict[endingType]);
    }

}
