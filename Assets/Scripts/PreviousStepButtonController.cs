using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviousStepButtonController : MonoBehaviour
{
    public Button PreviousStepButton;
    // Start is called before the first frame update
    void Start()
    {
        while (GameObject.Find("CubeController").GetComponent<CubeController>() == null) ;
        PreviousStepButton.onClick.AddListener(GameObject.Find("CubeController").GetComponent<CubeController>().TeachModePreviousStep);
        PreviousStepButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
