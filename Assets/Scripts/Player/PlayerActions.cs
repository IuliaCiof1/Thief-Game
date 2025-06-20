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


    [SerializeField] private Transform rayOrigin;
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
        if (rayOrigin == null) return;

        Vector3 direction = rayOrigin.forward;
        Vector3 start = rayOrigin.position;
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

    void GetTarget()
    {
        RaycastHit[] hits = Physics.SphereCastAll(rayOrigin.position, radius, rayOrigin.forward, maxDistance, ~0);
       
        foreach (RaycastHit hit in hits)
        {
            Collider collider = hit.collider;
            Vector3 targetPos = hit.collider.bounds.center;
            
            Debug.DrawLine(rayOrigin.position, targetPos, Color.green, 0.1f);

            // Linecast to see if target is obstructed
            if (!Physics.Linecast(rayOrigin.position, targetPos, obstructionLayers))
            {
                if (!actionInProgress)
                {
                    if (collider.TryGetComponent(out NPC npc) && npc.isInspecting)
                    {
                        currentTarget = collider.gameObject;

                        showKeyboardHint = true;
                        keyboardHintText.text = "<size=26><sprite=64></size>  Pickpocket";

                        return;
                    }
                    else if (collider.TryGetComponent(out HomeEntrance homeEntrance))
                    {
                        currentTarget = collider.gameObject;

                        showKeyboardHint = true;
                        keyboardHintText.text = "<size=26><sprite=64></size>  " + homeEntrance.GetMessage();
                      
                        return;
                    }
                    else if (collider.TryGetComponent(out LandLord landlord))
                    {

                        foreach (Objective objective in landlord.GetComponent<MemberObjectives>().activeObjectives)
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
                        foreach (Objective objective in familyMember.activeObjectives)
                        {
                            if (!objective.isActive)
                                continue;

                            // Money-based objective
                            if (!objective.itemRequierd && PlayerStats.Instance.money >= objective.moneyNeeded)
                            {
                                currentTarget = collider.gameObject;
                                showKeyboardHint = true;
                                keyboardHintText.text = $"<size=26><sprite=64> Give {objective.moneyNeeded}$ {objective.name}";
                                return;
                            }

                            // Item-based objective
                            if (objective.itemRequierd)
                            {
                                foreach (InventoryItem item in playerInventory.ownedItems)
                                {
                                    if (item.id == objective.objectNeeded.id)
                                    {
                                        currentTarget = collider.gameObject;
                                        showKeyboardHint = true;
                                        keyboardHintText.text = $"<size=26><sprite=64> Give {objective.objectNeeded.name}";
                                        return;
                                    }
                                }
                            }
                        
                    
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


    void Update()
    {
        GetTarget();

        if (!showKeyboardHint)
            keyboardHintPanel.SetActive(false);
        else
            keyboardHintPanel.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E) && currentTarget && !TutorialMain.uiActive)
        {
            StopAllCoroutines();

            if (currentTarget.TryGetComponent(out npc) && npc.isInspecting)
            {
                if (!isStealing)
                {
                    actionInProgress = true;
                    showKeyboardHint = false;

                    pocketItemsGeneraton.CreateItems(npc);
                   
                    if(!thirdPersonController.inTram)
                        cameraController.SetStealCamera();
                  
                    stealCanvas.SetActive(true);

                    thirdPersonController.stopMovement = true;
                    animator.ResetTrigger("Idle");
                    animator.SetTrigger("Steal");

                    TutorialMain.OnPickpocket?.Invoke();
                }
                else if (isStealing)
                {
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


                Objective objective = landlord.GetComponent<MemberObjectives>().activeObjectives[0];
                if (!objective.itemRequierd && objective.isActive && PlayerStats.Instance.money >= objective.moneyNeeded)
                {
                    PlayerStats.Instance.BuyWithMoney(objective.moneyNeeded);
                    landlord.GetComponent<Health>().GiveHealth(objective.healthTaken * 2);
                    landlord.Leave();
                    objectiveManager.CompleteObjective(objective);
                    showKeyboardHint = false;

                    actionInProgress = false;
                    return;
                }
            }
            if (currentTarget.TryGetComponent(out MemberObjectives familyMember))
            {

                for (int i = 0; i < familyMember.activeObjectives.Count; i++)
                {
                    Objective objective = familyMember.activeObjectives[i];
                    if (!objective.itemRequierd && objective.isActive && PlayerStats.Instance.money >= objective.moneyNeeded)
                    {
                        PlayerStats.Instance.BuyWithMoney(objective.moneyNeeded);
                        familyMember.GetComponent<Health>().GiveHealth(objective.healthTaken * 2);
                        objectiveManager.CompleteObjective(objective);
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
                            objectiveManager.CompleteObjective(objective);
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
