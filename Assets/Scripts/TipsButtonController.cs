using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsButtonController : MonoBehaviour
{
    public Button TipsButton;


    // Start is called before the first frame update
    void Start()
    {
        while (GameObject.Find("CubeController").GetComponent<CubeController>() == null) ;
        TipsButton.onClick.AddListener(GameObject.Find("CubeController").GetComponent<CubeController>().ShowRestoreTips);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
