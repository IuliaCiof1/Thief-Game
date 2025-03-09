using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionPoint : MonoBehaviour
{
    [SerializeField] int stoppingChance;

    public int StoppingChange{ get { return stoppingChance; } private set { } }
}
