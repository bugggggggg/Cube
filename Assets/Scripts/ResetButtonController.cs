using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButtonController : MonoBehaviour
{

    public Button ResetButton;

    // Start is called before the first frame update
    void Start()
    {
        while (GameObject.Find("CubeController").GetComponent<CubeController>() == null) ;
        ResetButton.onClick.AddListener(GameObject.Find("CubeController").GetComponent<CubeController>().Reset);
    }

    void Onclick()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
