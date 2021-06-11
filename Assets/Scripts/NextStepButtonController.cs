using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextStepButtonController : MonoBehaviour
{
    public Button NextStepButton;

    // Start is called before the first frame update
    void Start()
    {
        while (GameObject.Find("CubeController").GetComponent<CubeController>() == null) ;
        NextStepButton.onClick.AddListener(GameObject.Find("CubeController").GetComponent<CubeController>().TeachModeNextStep);
        NextStepButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
