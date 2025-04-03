using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FamilyMemberHealthUI : MonoBehaviour
{
    [SerializeField] AIControl familyMember;
    [SerializeField] Slider slider;

    Quaternion initialRotation;
    public float baseRotation = 80f;  // Rotation when camera is right above
    public float rotationMultiplier = 2f;  // Adjust sensitivity
    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(familyMember.transform.position.x, transform.position.y ,familyMember.transform.position.z);

        float cameraZ = Camera.main.transform.position.z;

        // Get the difference in Z position
        float zDifference = cameraZ - transform.position.z;
        float xRotation = baseRotation + (zDifference * rotationMultiplier);

        // Clamp the rotation to prevent extreme tilting
        xRotation = Mathf.Clamp(xRotation, 50f, 180f);
       

        // Apply only X-axis rotation
        transform.rotation = Quaternion.Euler(xRotation, 0, 0);
    }


    public void SetSliderValueUI(float value)
    {
        //slider.value -= amount;
        slider.value = value;
    }

    public void SetMaxSliderValueUI(float value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

}
