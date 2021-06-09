using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class TwistAction 
{
 
    /// <summary>
    /// "X":取x=axisValue的面绕X转
    /// "Y"
    /// "Z"
    /// "ALLX":整体绕X转
    /// "ALLY"
    /// "ALLZ"
    /// </summary>
    public string type;
    public int axisValue;
    public int sign;//+1:顺时针，-1:逆时针
    public int times;//旋转次数，1次90度



    static Vector3 Y = new Vector3(0, 1f, 0);
    static Vector3 X = new Vector3(1f, 0, 0);
    static Vector3 Z = new Vector3(0, 0, 1f);
    static int ROTATE_SPEED=10;//每帧转动角度
    

    public TwistAction()
    {

    }

    public TwistAction(TwistAction twistAction)
    {
        type = twistAction.type;
        axisValue = twistAction.axisValue;
        sign = twistAction.sign;
        times = twistAction.times;
    }



    public TwistAction(string _type, int _sign, int _times, int _axisValue = 0)
    {
        times %= 4;
        if (sign > 0) sign = 1;
        else if (sign < 0) sign = -1;

        type = _type;
        axisValue = _axisValue;
        sign = _sign;
        times = _times;
    }
    public TwistAction(string _type, int angle, int _axisValue = 0)
    {
        int _sign;
        if (angle > 0) _sign = 1;
        else
        {
            _sign = -1;
            angle = -angle;
        }

        type = _type;
        axisValue = _axisValue;
        sign = _sign;
        times = angle / 90 % 4;
    }

    

    //执行该twist
    public void MakeTwist(List<GameObject>cubelets)
    {
        sign = -sign;
        UnmakeTwist(cubelets);
        sign = -sign;
    }

    //快速执行该twist
    public void MakeTwistQuickly(List<GameObject> cubelets)
    {
        sign = -sign;
        UnmakeTwistQuickly(cubelets);
        sign = -sign;
    }

    //反向快速执行该twist
    public void UnmakeTwistQuickly(List<GameObject> cubelets)
    {
        switch (type)
        {
            case "ALLX":
                {
                    GameObject.Find("CubeController").GetComponent<CubeController>()
                        .RotateOnce(cubelets, Vector3.zero, X, -sign * 90 * times);
                    break;
                }
            case "ALLY":
                {
                    GameObject.Find("CubeController").GetComponent<CubeController>()
                        .RotateOnce(cubelets, Vector3.zero, Y, -sign * 90 * times);
                    break;
                }
            case "ALLZ":
                {

                    GameObject.Find("CubeController").GetComponent<CubeController>()
                        .RotateOnce(cubelets, Vector3.zero, Z, -sign * 90 * times);
                    break;
                }
            case "X":
                {
                    GameObject.Find("CubeController").GetComponent<CubeController>()
                        .RotateOnce(cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.x) == axisValue)
                        , Vector3.zero, X, -sign * 90 * times);
                    break;
                }
            case "Y":
                {
                    GameObject.Find("CubeController").GetComponent<CubeController>()
                        .RotateOnce(cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.y) == axisValue)
                        , Vector3.zero, Y, -sign * 90 * times);
                    break;
                }
            case "Z":
                {
                    GameObject.Find("CubeController").GetComponent<CubeController>()
                        .RotateOnce(cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.z) == axisValue)
                        , Vector3.zero, Z, -sign * 90 * times);
                    break;
                }
            default: break;
        }
    }



    //反向执行该Twist（即撤销）
    public void UnmakeTwist(List<GameObject> cubelets)
    {
        TwistAction _twist = new TwistAction(this);
        _twist.sign = -_twist.sign;
        GameObject.Find("CubeController").GetComponent<CubeController>()
                        .RotateSlowly(cubelets, _twist, ROTATE_SPEED);
        


    }

    
}

