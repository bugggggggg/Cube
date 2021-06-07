using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnmakeButtonController : MonoBehaviour
{

    public Button UnmakeButton;
    // Start is called before the first frame update
    void Start()
    {
        while (GameObject.Find("CubeController").GetComponent<CubeController>() == null) ;
        UnmakeButton.onClick.AddListener(GameObject.Find("CubeController").GetComponent<CubeController>().UnmakeTwistAction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
