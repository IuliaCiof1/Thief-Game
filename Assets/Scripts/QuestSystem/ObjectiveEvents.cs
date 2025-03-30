using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectiveEvents
{
    public static event Action<Objective> OnObjectiveCompleted;


    public static event Action OnEmailSent;
    //private static event Action OnSpookyEmailRead;
    public static event Action OnOpenSpookyMail;
    public static event Action OnCheckOutRoom;
    public static event Action OnTakeLift;
    public static event Action OnFindExit;
    public static event Action OnFindEquipment;




    private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>
    {
        { "OnEmailSent", OnEmailSent},
        { "OnOpenSpookyMail", OnOpenSpookyMail},
        {"OnCheckOutRoom", OnCheckOutRoom },
        {"OnTakeLift", OnTakeLift },
        {"OnFindExit", OnFindExit},
        {"OnFindEquipment", OnFindEquipment}

    };


    public static void SubscribeEvent(string eventName, Action listener)
    {

        if (!eventDictionary.ContainsKey(eventName))
        {
            Debug.Log("not in dic");
            eventDictionary[eventName] = null; // Initialize the event if not present
        }
        Debug.Log(eventName + listener.ToString());
        eventDictionary[eventName] += listener;
    }

    public static void UnsubscribeEvent(string eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;

            if (eventDictionary[eventName] == null)
            {
                eventDictionary.Remove(eventName); // Remove the event if no listeners remain
            }
        }
    }


    public static void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke(); // Invoke the event via the dictionary
        }
    }

    public static void ObjectiveCompleted(Objective objective)
    {
        Debug.Log("objective completed");
        OnObjectiveCompleted?.Invoke(objective);
    }

    //public static void SendEmail()
    //{
    //    TriggerEvent("OnEmailSent"); //use trigger event makes OnEmailSent invoke succecfully
    //    OnEmailSent?.Invoke();

    //}
    ////public static void SendSpookyEmail() => OnSpookyEmailSent?.Invoke();
    ////public static void OpenSpookyMail() => OnOpenSpookyMail?.Invoke();
    //public static void OpenSpookyMail(DOOR_Interacting doorToUnlock)
    //{
    //    doorToUnlock.isLocked = false;
    //    TriggerEvent("OnOpenSpookyMail");

    //}

    //public static void CheckOutRoom()
    //{
       
    //    TriggerEvent("OnCheckOutRoom");
    //}

    //public static void TakeLift()
    //{
    //    TriggerEvent("OnTakeLift");
    //}

    //public static void FindExit()
    //{
    //    TriggerEvent("OnFindExit");
    //}

    //public static void FindEquipment()
    //{
    //    TriggerEvent("OnFindEquipment");
    //}
}
