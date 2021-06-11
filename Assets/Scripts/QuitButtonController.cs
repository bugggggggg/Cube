using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButtonController : MonoBehaviour
{
    public Button QuitButton;
    // Start is called before the first frame update
    void Start()
    {
        while (GameObject.Find("CubeController").GetComponent<CubeController>() == null) ;
        QuitButton.onClick.AddListener(GameObject.Find("CubeController").GetComponent<CubeController>().Quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
