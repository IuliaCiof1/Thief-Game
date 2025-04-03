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


    ThirdPersonController thirdPersonController;


    [SerializeField] private Transform rayPosition;
    [SerializeField] private float radius;
    private GameObject currentTarget;



    [SerializeField] TMP_Text keyboardHintText;
    GameObject keyboardHintPanel;
    bool showKeyboardHint;
    public bool actionInProgress;

    [SerializeField] PocketItemsGeneraton pocketItemsGeneraton;


    NPC npc;

    public static event Action OnStealUIDisable;

     GameObject playerInventory;
    ObjectiveManager objectiveManager;

    // Start is called before the first frame update
    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        keyboardHintPanel = keyboardHintText.transform.parent.gameObject;
        playerInventory = FindAnyObjectByType<Inventory>().gameObject;
        objectiveManager = FindAnyObjectByType<ObjectiveManager>();
    }



    void GetTarget()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Camera.main.transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, distance)
        // Physics.Raycast(rayPosition.position, rayPosition.forward, out RaycastHit hitInfo, distance)

        Collider[] colliders = Physics.OverlapSphere(rayPosition.position, radius);


        foreach (Collider collider in colliders)
        {
            if (!actionInProgress)
            {
                //showKeyboardHint = false;

                

                if (collider.TryGetComponent(out NPC npc) && npc.isInspecting)
                {
                    currentTarget = collider.gameObject;
                   
                    showKeyboardHint = true;
                    keyboardHintText.text = "<size=26><sprite=64></size>  Pickpocket";

                    break;
                }
                else if(collider.TryGetComponent(out HomeEntrance homeEntrance))
                {
                    currentTarget = collider.gameObject;

                    showKeyboardHint = true;
                    keyboardHintText.text = "<size=26><sprite=64></size>  Go Home";

                    break;
                }
                else if(collider.TryGetComponent(out FamilyMember familyMember))
                {
                    
                   
                        bool itemFound = false;
                        foreach (Transform item in playerInventory.transform)
                        {
                            
                            foreach (Objective objective in familyMember.possibleObjectives)
                            {
                               
                                if (objective.isActive && item.name.Contains(objective.objectNeeded.name))
                                {
                                    itemFound = true;


                                    currentTarget = collider.gameObject;

                                    showKeyboardHint = true;
                                    keyboardHintText.text = "<size=26><sprite=64> Give " + objective.objectNeeded.name;

                                    return;
                                }
                            }
                        }
                        if (itemFound)
                            break;
                    
                }
                else if (collider.TryGetComponent(out Shop shop))
                {
                    currentTarget = collider.gameObject;

                    showKeyboardHint = true;
                    keyboardHintText.text = "<size=26><sprite=64></size>  Buy "+ shop.GetGoodsName();

                    break;
                }
                else
                    showKeyboardHint = false;
            }
            

        }


        //Debug.Draw(rayPosition.position, rayPosition.forward * distance, Color.red);
    }


    // Update is called once per frame
    void Update()
    {
        GetTarget();

        if(!showKeyboardHint)
            keyboardHintPanel.SetActive(false);
        else
            keyboardHintPanel.SetActive(true);

        //steal action
        if (Input.GetKeyDown(KeyCode.E) && currentTarget)
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
                    cameraController.SetStealCamera();
                    //cameraController.ToggleComponent<CinemachineFollowZoom>(true);
                    stealCanvas.SetActive(true);

                    thirdPersonController.enabled = false;
                   
                    animator.ResetTrigger("Idle");
                    animator.SetTrigger("Steal");
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
            if (currentTarget.TryGetComponent(out FamilyMember familyMember))
            {

                print("Pressed E to give to");
                foreach (Transform item in playerInventory.transform)
                {
                    print(familyMember.name + " some " + item.name);

                    for(int i =0; i< familyMember.possibleObjectives.Count; i++)
                    {
                        Objective objective = familyMember.possibleObjectives[i];
                        print(" for objective " + objective.title);
                        if (objective.isActive && item.name.Contains(objective.objectNeeded.name))
                        {
                            print("you gave to" + familyMember.name + " some " + item.name);
                            //print("complete " + objective.title);
                            //objective.Complete();
                            familyMember.GiveHealth(objective.healthTaken*2);
                            objectiveManager.HandleObjectiveCompleted(objective, i);
                            //familyMember.currentObjective = null;
                            showKeyboardHint = false;

                            actionInProgress = false;
                            print(item.name);
                            Destroy(item.gameObject);
                            print(item.name + "destroyed");
                            return;
                        }
                    }

                  
                }
               
            }
            if (currentTarget.TryGetComponent(out Shop shop))
            {

                shop.BuyGoods();
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

        yield return new WaitForSeconds(0);
        
        stealCanvas.SetActive(false);
        cameraController.ResetCameraRotation();
        
        thirdPersonController.enabled = true;
        animator.ResetTrigger("Idle");
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
