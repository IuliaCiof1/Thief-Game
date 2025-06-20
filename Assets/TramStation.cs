using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramStation : MonoBehaviour
{

    [SerializeField] int ticketPrice;
    [SerializeField] Tram tram;
    ThirdPersonController player;
    Transform iniPlayerParent;
    Quaternion iniPlayerRotation;
    [SerializeField] CameraController cameraController;

    private void Start()
    {
        
        tram = null;
        //tram = FindObjectOfType<Tram>();
        player = FindObjectOfType<ThirdPersonController>();
        iniPlayerParent = player.transform.parent;
        iniPlayerRotation = player.transform.rotation;
    }

    public Tram IsTramInStation()
    {
        return tram;
    }

    public void GetTram()
    {
        if (!PlayerStats.Instance.BuyWithMoney(ticketPrice))
        {
            print("no money");
            return;
        }

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
        
        cameraController.SetTramCamera();
    }

    public void GetOff()
    {
        tram.SwitchTramRoof();
        player.gameObject.SetActive(false);
        player.transform.position = transform.position;
        player.gameObject.SetActive(true);
        player.transform.SetParent(iniPlayerParent);
        player.transform.rotation = iniPlayerRotation;
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
