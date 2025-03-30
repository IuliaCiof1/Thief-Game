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

    [SerializeField] GameObject playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        keyboardHintPanel = keyboardHintText.transform.parent.gameObject;
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
                    
                    if (familyMember.currentObjective)
                    {
                        bool itemFound = false;
                        foreach (Transform item in playerInventory.transform)
                        {
                            if (item.name.Contains(familyMember.currentObjective.objectNeeded.name))
                            {
                                itemFound = true;
                              

                                currentTarget = collider.gameObject;

                                showKeyboardHint = true;
                                keyboardHintText.text = "<size=26><sprite=64> Give " + familyMember.currentObjective.objectNeeded.name;

                                break;
                            }
                        }
                        if (itemFound)
                            break;
                    }
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
            print(showKeyboardHint);
            StopAllCoroutines();

            //actionInProgress = true;

            if (currentTarget.TryGetComponent(out npc) && npc.isInspecting)
            {


                if (!isStealing)
                {
                    actionInProgress = true;
                    print("showkeayboardhint");
                    showKeyboardHint = false;


                    //cameraController.RotateCamera(Quaternion.identity);
                    pocketItemsGeneraton.CreateItems(npc);
                    cameraController.SetStealCamera();
                    //cameraController.ToggleComponent<CinemachineFollowZoom>(true);
                    stealCanvas.SetActive(true);

                    thirdPersonController.enabled = false;
                    print("steal");
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
                if (familyMember.currentObjective)
                {
                    //If player has the required item of the familyMember currentObjective
                    //then destroy the item, complete the currentObjective and make currentObjective null
                    foreach (Transform item in playerInventory.transform)
                    {
                        if(item.name.Contains(familyMember.currentObjective.objectNeeded.name))
                        {
                            familyMember.currentObjective.Complete();
                            familyMember.currentObjective = null;
                            showKeyboardHint = false;

                            actionInProgress = false;
                            print(item.name);
                            Destroy(item.gameObject);
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
