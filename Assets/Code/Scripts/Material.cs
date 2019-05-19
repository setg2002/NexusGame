using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Material : MonoBehaviour {
    public Text materialsText;
    public int materials;

    // Start is called before the first frame update
    void Start()
    {
        materialsText.text = "Materials: " + materials;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMaterials(int numberOfMaterials)
    {
        materials += numberOfMaterials;
        materialsText.text = "Materials: " + materials;
    }
}
