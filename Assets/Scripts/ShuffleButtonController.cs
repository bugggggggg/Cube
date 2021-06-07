using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuffleButtonController : MonoBehaviour
{
    public Button ShuffleButton;

    // Start is called before the first frame update
    void Start()
    {
        while (GameObject.Find("CubeController").GetComponent<CubeController>() == null) ;
        ShuffleButton.onClick.AddListener(GameObject.Find("CubeController").GetComponent<CubeController>().Shuffle);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
