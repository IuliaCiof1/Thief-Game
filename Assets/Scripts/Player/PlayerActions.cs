using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;



public class PlayerActions : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject stealCanvas;
    [SerializeField] StealTimer stealTImer;
    [SerializeField] Animator animator;


    bool isStealing = false;
    //bool inTram = false;

    ThirdPersonController thirdPersonController;


    [SerializeField] private Transform rayPosition;
    [SerializeField] private float radius = 0.6f;
    private GameObject currentTarget;



    [SerializeField] TMP_Text keyboardHintText;
    GameObject keyboardHintPanel;
    bool showKeyboardHint;
    public bool actionInProgress;

    [SerializeField] PocketItemsGeneraton pocketItemsGeneraton;


    NPC npc;

    public static event Action OnStealUIDisable;

     Inventory playerInventory;
    ObjectiveManager objectiveManager;

    [SerializeField] float maxDistance = 1.24f;
    [SerializeField] LayerMask obstructionLayers;
    TramStation tramStation_ = null;
    //public static event Action StartPickpocket;

    // Start is called before the first frame update
    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        keyboardHintPanel = keyboardHintText.transform.parent.gameObject;
        playerInventory = FindAnyObjectByType<Inventory>();
        objectiveManager = FindAnyObjectByType<ObjectiveManager>();
    }

    private void OnDrawGizmos()
    {
        if (rayPosition == null) return;

        Vector3 direction = rayPosition.forward;
        Vector3 start = rayPosition.position;
        Vector3 end = start + direction.normalized * maxDistance;

        Gizmos.color = Color.red;

        // Start and end spheres
        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);

        // Connecting lines to form the "cylinder"
        Vector3 up = Vector3.up * radius;
        Vector3 right = Vector3.right * radius;

        Gizmos.DrawLine(start + up, end + up);
        Gizmos.DrawLine(start - up, end - up);
        Gizmos.DrawLine(start + right, end + right);
        Gizmos.DrawLine(start - right, end - right);
    }

    //private void DebugDrawSphereCast(Vector3 origin, Vector3 direction, float radius, float distance, Color color)
    //{
    //    Vector3 end = origin + direction.normalized * distance;

    //    // Draw the central ray
    //    Debug.DrawLine(origin, end, color);

    //    // Draw circles at start and end to represent the sphere
    //    DebugExtension.DrawCircle(origin, Vector3.up, color, radius);
    //    DebugExtension.DrawCircle(end, Vector3.up, color, radius);
    //}

    void GetTarget()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Camera.main.transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, distance)
        // Physics.Raycast(rayPosition.position, rayPosition.forward, out RaycastHit hitInfo, distance)

        //Collider[] colliders = Physics.OverlapSphere(rayPosition.position, radius);


        //foreach (Collider collider in colliders)
        //{
        bool targetFound = false;

        Vector3 direction = rayPosition.forward;

        // Store all hits in an array
        RaycastHit[] hits = Physics.SphereCastAll(rayPosition.position, radius, direction, maxDistance, ~0);
        //DebugDrawSphereCast(rayPosition.position, direction, radius, maxDistance, Color.red);
        foreach (RaycastHit hit in hits)
        {
            Collider collider = hit.collider;
            Vector3 targetPos = hit.collider.bounds.center;
            Vector3 origin = rayPosition.position;
            Vector3 dir = (targetPos - origin).normalized;
            float distanceToTarget = Vector3.Distance(origin, targetPos);
            Debug.DrawLine(origin, targetPos, Color.green, 0.1f);
            // Linecast to see if target is obstructed
            if (!Physics.Linecast(origin, targetPos, obstructionLayers))
            {

                if (!actionInProgress)
                {
                    //showKeyboardHint = false;



                    if (collider.TryGetComponent(out NPC npc) && npc.isInspecting)
                    {
                        currentTarget = collider.gameObject;

                        showKeyboardHint = true;
                        keyboardHintText.text = "<size=26><sprite=64></size>  Pickpocket";

                        return;
                        break;
                    }
                    else if (collider.TryGetComponent(out HomeEntrance homeEntrance))
                    {
                        currentTarget = collider.gameObject;

                        showKeyboardHint = true;
                        keyboardHintText.text = "<size=26><sprite=64></size>  " + homeEntrance.GetMessage();
                        targetFound = true;
                        return;
                        break;
                    }
                    else if (collider.TryGetComponent(out LandLord landlord))
                    {

                        foreach (Objective objective in landlord.GetComponent<MemberObjectives>().possibleObjectives)
                        {
                            if (!objective.itemRequierd && objective.isActive && PlayerStats.Instance.money >= objective.moneyNeeded)
                            {

                                currentTarget = collider.gameObject;

                                showKeyboardHint = true;
                                keyboardHintText.text = $"<size=26><sprite=64> Give {objective.moneyNeeded}$ {objective.name}";

                                return;
                            }
                        }
                    }
                    else if (collider.TryGetComponent(out MemberObjectives familyMember))
                    {
                        print("player actions :: member objective");

                        bool itemFound = false;


                        foreach (Objective objective in objectiveManager.activeobjectives)
                        {
                            if (objective.member.ToLower().Contains(familyMember.gameObject.name.ToLower()))
                            {
                                print("player actions :: " + objective.name);
                                if (!objective.itemRequierd && objective.isActive && PlayerStats.Instance.money >= objective.moneyNeeded)
                                {
                                    itemFound = true;


                                    currentTarget = collider.gameObject;

                                    showKeyboardHint = true;
                                    keyboardHintText.text = $"<size=26><sprite=64> Give {objective.moneyNeeded}$ {objective.name}";

                                    return;
                                }
                                foreach (InventoryItem item in playerInventory.ownedItems)
                                {
                                    print("player actions :: take item " + item.name + item.id + " " + objective.objectNeeded.id);
                                    if (objective.isActive && item.id == objective.objectNeeded.id)
                                    {
                                        print("player actions :: item found " + item.name);
                                        itemFound = true;


                                        currentTarget = collider.gameObject;

                                        showKeyboardHint = true;
                                        keyboardHintText.text = "<size=26><sprite=64> Give " + objective.objectNeeded.name;

                                        return;
                                    }


                                }
                            }
                        }
                        if (itemFound)
                        {
                            return;
                            break;
                        }


                    }
                    else if (collider.TryGetComponent(out Shop shop))
                    {
                        currentTarget = collider.gameObject;

                        showKeyboardHint = true;
                        keyboardHintText.text = "<size=26><sprite=64></size>  Buy " + shop.GetGoodsName() + " for " + shop.GetGoodsPrice() + "$";
                        return;
                        break;
                    }
                    else if (collider.TryGetComponent(out TramStation tramStation))
                    {
                        if (tramStation.IsTramInStation() != null)
                        {
                            //if (collider.TryGetComponent(out Tram tram))
                            //{

                            //    currentTarget = collider.gameObject;
                            //    showKeyboardHint = true;
                            //    keyboardHintText.text = "<size=26><sprite=64></size>  Get Off ";
                            //}

                            if (!thirdPersonController.inTram)
                            {
                                print("tram in station");
                                //tramStation_ = tramStation;
                                currentTarget = collider.gameObject;

                                showKeyboardHint = true;
                                keyboardHintText.text = "<size=26><sprite=64></size>  Buy tram ticket for " + tramStation.GetTicketPrice() + "$";
                                return;
                            }


                        }


                    }
                    else if (thirdPersonController.inTram)
                    {
                        
                        //collider.TryGetComponent(out Tram tram);

                        if (thirdPersonController.currentPlatform.TryGetComponent<Tram>(out Tram tram))
                        {
                            tramStation_ = tram.GetTramStation();
                            if (tramStation_ != null)
                            {
                             

                                currentTarget = tramStation_.gameObject;

                                showKeyboardHint = true;
                                keyboardHintText.text = "<size=26><sprite=64></size>  Get Off";
                                return;
                            }
                            
                        }
                       

                       
                    }

                   //NO target found
                        currentTarget = null;
                        showKeyboardHint = false;
                    
                }

            }
        }


        //Debug.Draw(rayPosition.position, rayPosition.forward * distance, Color.red);
    }


    // Update is called once per frame
    void Update()
    {
        GetTarget();

        if (!showKeyboardHint)
            keyboardHintPanel.SetActive(false);
        else
            keyboardHintPanel.SetActive(true);

        //steal action
        if (Input.GetKeyDown(KeyCode.E) && currentTarget && !TutorialMain.uiActive)
        {
            
            StopAllCoroutines();

            //actionInProgress = true;

            if (currentTarget.TryGetComponent(out npc) && npc.isInspecting)
            {


                if (!isStealing)
                {
                    actionInProgress = true;
                  
                    showKeyboardHint = false;


                    //cameraController.RotateCamera(Quaternion.identity);
                    pocketItemsGeneraton.CreateItems(npc);
                   
                    if(!thirdPersonController.inTram)
                        cameraController.SetStealCamera();
                    
                    //cameraController.ToggleComponent<CinemachineFollowZoom>(true);
                    stealCanvas.SetActive(true);

                    //thirdPersonController.enabled = false;
                    thirdPersonController.stopMovement = true;
                    animator.ResetTrigger("Idle");
                    animator.SetTrigger("Steal");


                    TutorialMain.OnPickpocket?.Invoke();
                }
                else if (isStealing)
                {
                    //animator.SetTrigger("PutInBag");


                    StartCoroutine(DisableStealUI());


                }

                isStealing = !isStealing;
            }
            if (currentTarget.TryGetComponent(out HomeEntrance homeEntrance))
            {
                
                homeEntrance.GoHome();
            }
            if (currentTarget.TryGetComponent(out LandLord landlord))
            {


                Objective objective = landlord.GetComponent<MemberObjectives>().possibleObjectives[0];
                if (!objective.itemRequierd && objective.isActive && PlayerStats.Instance.money >= objective.moneyNeeded)
                {
                    PlayerStats.Instance.BuyWithMoney(objective.moneyNeeded);
                    landlord.GetComponent<Health>().GiveHealth(objective.healthTaken * 2);
                    landlord.Leave();
                    objectiveManager.HandleObjectiveCompleted(objective);
                    showKeyboardHint = false;

                    actionInProgress = false;
                    return;
                }
            }
            if (currentTarget.TryGetComponent(out MemberObjectives familyMember))
            {

                for (int i = 0; i < objectiveManager.activeobjectives.Count; i++)
                {
                    Objective objective = objectiveManager.activeobjectives[i];
                    if (!objective.itemRequierd && objective.isActive && PlayerStats.Instance.money >= objective.moneyNeeded)
                    {
                        PlayerStats.Instance.BuyWithMoney(objective.moneyNeeded);
                        familyMember.GetComponent<Health>().GiveHealth(objective.healthTaken * 2);
                        objectiveManager.HandleObjectiveCompleted(objective);
                        showKeyboardHint = false;

                        actionInProgress = false;
                        return;
                    }
                    foreach (InventoryItem item in playerInventory.ownedItems)
                    {

                        if (objective.isActive && item.name.Contains(objective.objectNeeded.name))
                        {
                           
                            //print("complete " + objective.title);
                            //objective.Complete();
                            familyMember.GetComponent<Health>().GiveHealth(objective.healthTaken * 2);
                            objectiveManager.HandleObjectiveCompleted(objective);
                            //familyMember.currentObjective = null;
                            showKeyboardHint = false;

                            actionInProgress = false;

                            playerInventory.RemoveFromInventory(item);

                           // Destroy(item.gameObject);
                            
                            return;
                        }
                    }



                }
               
            }
            if (currentTarget.TryGetComponent(out Shop shop))
            {

                shop.BuyGoods();
            }
            if (currentTarget.TryGetComponent(out TramStation tramStation))
            {
                print("current target tram station");
                if (tramStation.IsTramInStation())
                {
                    print("Tram is indexer station");
                    if (tramStation.IsTramInStation() != null)
                    {
                        print("in station");
                        if (thirdPersonController.inTram)
                        {
                            print("get off");
                           // inTram = false;
                            tramStation.GetOff();
                        }
                        else
                        {
                            print("player actions:: get tram");
                            //inTram = true;
                            //thirdPersonController.inTram = true;
                            tramStation.GetTram();
                        }
                    }
                }
            }

            

        }

        if (isStealing)
        {
         
            //print(stealTImer.Value);
            if (stealTImer.Value <= 0)
            {
               
               

                StartCoroutine(DisableStealUI());

                npc.StartDeadzoe();
                //thirdPersonController.enabled = true;
                //stealCanvas.SetActive(false);
                //cameraController.ResetCameraRotation();
                //animator.SetTrigger("Idle");
                isStealing = !isStealing;
                stealTImer.Value = 2;
               
            }
            else if (!npc.isInspecting)
            {
                StartCoroutine(DisableStealUI());

               
                isStealing = !isStealing;
                stealTImer.Value = 2;
            }
        }


        //if (stealTImer.Value <= 0)
        //{
        //    stealCanvas.SetActive(false);
        //}


        //DebugExtension.DebugWireSphere(transform.position, debugWireSphere_Color, debugWireSphere_Radius); //debug spherecast
    }

    IEnumerator DisableStealUI()
    {
        actionInProgress = false;
        OnStealUIDisable?.Invoke();
        print("pocket item disable ui!!!!");
        yield return new WaitForSeconds(0);
        
        stealCanvas.SetActive(false);
        if (thirdPersonController.inTram)
            cameraController.SetTramCamera();
        else
            cameraController.ResetCamera();

        //thirdPersonController.enabled = true;
        thirdPersonController.stopMovement = false;
        animator.ResetTrigger("Idle");

        TutorialMain.OnClosePickpocket?.Invoke();
    }


    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}

}
