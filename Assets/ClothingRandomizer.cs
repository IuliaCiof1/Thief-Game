using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingRandomizer : MonoBehaviour
{
    [SerializeField] List<GameObject> upperClothes;
    [SerializeField] List<Color> upperClothesColors;

    [SerializeField] List<GameObject> lowerClothes;
    [SerializeField] List<Color> lowerClothesColors;

    [SerializeField] List<GameObject> outerClothes;
    [SerializeField] List<Color> outerClothesColors;

    [SerializeField] List<GameObject> shoes;
    [SerializeField] List<Color> shoesColors;

    [SerializeField] List<GameObject> facialHair;
    [SerializeField] List<Color> facialHairColors;

    [SerializeField] List<GameObject> hairs;
    [SerializeField] List<Color> hairColors;

    [SerializeField] float shadeOffset;
    [SerializeField] Renderer renderer_;

    // Start is called before the first frame update
    void Start()
    {
        GameObject clothing = GetRandomClothing(upperClothes);
        GetRandomColor(clothing, upperClothesColors);

        clothing = GetRandomClothing(lowerClothes);
        GetRandomColor(clothing, lowerClothesColors);


        clothing = GetRandomClothing(shoes);
        GetRandomColor(clothing, shoesColors);

        clothing = GetRandomClothingOrNot(outerClothes);
        GetRandomColor(clothing, outerClothesColors);


        ////Hair - hair color will match with facial hair color
        //clothing = GetRandomClothing(hair);
        //int hairColorIndex = GetRandomColor(clothing, hairColors);

        GameObject hair = GetRandomClothing(hairs);
        Color hairColor = GetRandomColor(hair, hairColors);


        clothing = GetRandomClothingOrNot(facialHair);
        SetColor(clothing, hairColor);
        //GetRandomHair();


    }

    //void GetRandomHair()
    //{
    //    GameObject clothing = GetRandomClothing(hair);
    //    int hairColorIndex = GetRandomColor(clothing, hairColors);


    //    clothing = GetRandomClothingOrNot(facialHair);

    //    Material toonMaterial = clothing.GetComponent<SkinnedMeshRenderer>().material;;
    //    toonMaterial.SetColor("_BaseColor", hairColors[hairColorIndex]);


    //    float h, s, v;
    //    Color.RGBToHSV(hairColors[hairColorIndex], out h, out s, out v);
    //    v = v - shadeOffset;
    //    Color ShadeColor1 = Color.HSVToRGB(h, s, v);

    //    toonMaterial.SetColor("_1st_ShadeColor", ShadeColor1);

    //    float h1, s1, v1;
    //    Color.RGBToHSV(ShadeColor1, out h1, out s1, out v1);
    //    v = v - shadeOffset;
    //    Color ShadeColor2 = Color.HSVToRGB(h1, s1, v1);

    //    toonMaterial.SetColor("_2nd_ShadeColor", ShadeColor2);

        
    //}

    GameObject GetRandomClothing(List<GameObject> clothings)
    {
        int index;
        index = Random.Range(0, clothings.Count);
        clothings[index].SetActive(true);

        return clothings[index];
    }

    Color GetRandomColor(GameObject clothing, List<Color> materialColor)
    {
        if (clothing != null)
        {
            int index;

            //Material toonMaterial = clothing.GetComponent<SkinnedMeshRenderer>().material;
            index = Random.Range(0, materialColor.Count);

            SetColor(clothing, materialColor[index]);

            return materialColor[index];
        }

        return Color.black;
    }


    void SetColor(GameObject clothing, Color materialColor)
    {
        if (clothing != null)
        {
            Material toonMaterial = clothing.GetComponent<SkinnedMeshRenderer>().material;
            toonMaterial.SetColor("_BaseColor", materialColor);


            float h, s, v;
            Color.RGBToHSV(materialColor, out h, out s, out v);
            v = v - shadeOffset;
            Color ShadeColor1 = Color.HSVToRGB(h, s, v);

            toonMaterial.SetColor("_1st_ShadeColor", ShadeColor1);

            float h1, s1, v1;
            Color.RGBToHSV(ShadeColor1, out h1, out s1, out v1);
            v = v - shadeOffset;
            Color ShadeColor2 = Color.HSVToRGB(h1, s1, v1);

            toonMaterial.SetColor("_2nd_ShadeColor", ShadeColor2);


            //This how we know which shader property to choose
            //
            //for (int property = 0; property < toonMaterial.shader.GetPropertyCount(); property++)
            //{
            //    Debug.Log(toonMaterial.shader.GetPropertyName(property));
            //}
        }
    }

    GameObject GetRandomClothingOrNot(List<GameObject> clothing)
    {
        int index;
        index = Random.Range(-1, clothing.Count);
        if (index >= 0)
        {
            clothing[index].SetActive(true);
            return clothing[index];
        }

        return null;
    }
}
