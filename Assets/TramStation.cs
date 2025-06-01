using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramStation : MonoBehaviour
{

    [SerializeField] int ticketPrice;
    [SerializeField] Tram tram;
    ThirdPersonController player;
    Transform playerParent;
    [SerializeField] CameraController cameraController;

    private void Start()
    {
        
        tram = null;
        //tram = FindObjectOfType<Tram>();
        player = FindObjectOfType<ThirdPersonController>();
        playerParent = player.transform.parent;
    }

    public Tram IsTramInStation()
    {
        print("is tram in sstation? ");


        if (tram is null)
        {
            print("tram is null");
            return null;
        }
        print(" tram.gameObject.name");
        return tram;
    }

    public void GetTram()
    {
        //if (PlayerStats.BuyWithMoney(ticketPrice))
        //{
        //    player.transform.SetParent(tram.transform);

        //player.lastPlatformPosition = tram.transform.position;
        //player.lastPlatformRotation = tram.transform.rotation;
        tram.SwitchTramRoof();

        player.gameObject.SetActive(false);
        
        player.currentPlatform = tram.transform;

        player.transform.position = tram.transform.position;
        player.enabled = false;

        player.transform.SetParent(tram.transform);
        player.gameObject.SetActive(true);
        player.enabled = true;
        
        player.transform.localPosition = Vector3.zero;
        player.inTram = true;
        //Invoke("SetPlayerActive", 0.1f);
        //cameraController.SetCollisionCamera();
        cameraController.SetTramCamera();
        //}
    }

    public void GetOff()
    {
        tram.SwitchTramRoof();
        player.gameObject.SetActive(false);
        player.transform.position = transform.position;
        player.gameObject.SetActive(true);
        cameraController.ResetCamera();
        player.inTram = false;
    }

    private void SetPlayerActive() { player.gameObject.SetActive(true); player.enabled = true; }

    public int GetTicketPrice() { return ticketPrice; }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.TryGetComponent<Tram>(out Tram tram_))
            tram = tram_;
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.TryGetComponent<Tram>(out Tram tram_))
            tram = null;
    }
}
