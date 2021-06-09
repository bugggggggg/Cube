using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestoreButtonController : MonoBehaviour
{

    public Button RestoreButton;
    // Start is called before the first frame update
    void Start()
    {
        while (GameObject.Find("CubeController").GetComponent<CubeController>() == null) ;
        RestoreButton.onClick.AddListener(GameObject.Find("CubeController").GetComponent<CubeController>().Restore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
