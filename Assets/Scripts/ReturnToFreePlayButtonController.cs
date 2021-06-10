using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToFreePlayButtonController : MonoBehaviour
{
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        while (GameObject.Find("CubeController").GetComponent<CubeController>() == null) ;
        button.onClick.AddListener(GameObject.Find("CubeController").GetComponent<CubeController>().ReturnToFreePlayMode);
        button.interactable = false;//初始为教学状态，不开启返回按钮
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
