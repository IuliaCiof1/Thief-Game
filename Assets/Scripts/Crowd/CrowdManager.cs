using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> goalLocations; // Walking destinations
    [SerializeField] private List<GameObject> inspectionPoints; // Inspection locations (e.g., windows)

    [Header("Animator parameters")]
    [SerializeField] private Vector2 walkOffsetRange = new Vector2(0f, 2f);
    [SerializeField] private Vector2 speedMultiplierRange = new Vector2(0.5f, 2f);

    [Header("Inspection")]
    [Tooltip("Seconds between every inspection point search")]
    [SerializeField] private int inspectionCooldown = 7;
    [Tooltip("The time in seconds that the agent waits at the inspection point")]
    [SerializeField] private Vector2 timeRangeAtInspectionPoint = new Vector2(5, 10);
    [Tooltip("The agent will go to any inspection point closer than this distance")]
    [SerializeField] private float minDistanceInspectionPoint = 15f;

    [Header("Deadzone")]
    //[SerializeField] private GameObject deadzoneObject;
    [SerializeField] private float growingSpeed;


    [Header("Fleeing")]
    [SerializeField] float detectionRadius;
    [SerializeField] float fleeRadius;

    [SerializeField] float avoidDistance;
    [SerializeField] Transform agentsContainer;
    public ThirdPersonController player { get; private set; }

    public List<GameObject> GoalLocations { get { return goalLocations; } private set { } }
    public List<GameObject> InspectionPoints { get { return inspectionPoints; } private set { } }
    public Vector2 WalkOffsetRange { get { return walkOffsetRange; } private set { } }
    public Vector2 SpeedMultiplier { get { return speedMultiplierRange; } private set { } }
    public int InspectionCooldown { get { return inspectionCooldown; } private set { } }
    public Vector2 TimeRangeAtInspectionPoint { get { return timeRangeAtInspectionPoint; } private set { } }
    public float MinDistanceInspectionPoint { get { return minDistanceInspectionPoint; } private set { } }
    //public GameObject DeadzoneObject { get { return deadzoneObject; } private set { } }
    public float GrowingSpeed { get { return growingSpeed; } private set { } }
    public float DetectionRadius { get { return detectionRadius; } private set { } }
    public float FleeRadius { get { return fleeRadius; } private set { } }
    public float AvoidDistance { get { return avoidDistance; } private set { } }
    public Transform AgentsContainer { get { return agentsContainer; } private set { } }

    //Awake runs before all start methods. We use this to make sure the goalLocations and inspectionPoints are all initialised first
    void Awake()
    {
        //Finds all goal point and inspection points in the scene
        goalLocations.AddRange(GameObject.FindGameObjectsWithTag("goal"));
        inspectionPoints.AddRange(GameObject.FindGameObjectsWithTag("inspection"));
        player = FindAnyObjectByType<ThirdPersonController>();
    }

    public void StopAgentsFromColliding(AIControl agent1, AIControl agent2)
    {
        StartCoroutine(agent1.Wait());
    }

}
