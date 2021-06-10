using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeController : MonoBehaviour
{

    public GameObject CubeletPrefab;
    

    List<GameObject> Cubelets = new List<GameObject>();//魔方方块集合
    List<GameObject> MovingCubelets = new List<GameObject>();//需要转动面的方块集合
    List<TwistAction> HistoryTwistActions = new List<TwistAction>();//历史操作集合

    private int type = 90;
    private bool isRotating;//是否在旋转
    private bool isFixFloarError;//当前帧是否需要修复浮点误差
    private bool cubeOperatLock;//在播放打乱、还原等操作时，禁止交互操作魔方
    private MouseDownType MouseDown=MouseDownType.NOT_DOWN;//鼠标点到的类型
    private float lastx, lasty;//鼠标刚点下时的坐标
    private int rotateId;//旋转轴编号：Y,X,Z;
    private int rotateAngle;//鼠标点下后总共旋转的角度
    private const int ROTATE_SPEED = 20;//旋转速度

    private Vector3 hitCubeletVec3;//转某个面时，第一次被点到的方块坐标

    private const int FRAME_COUNT = 5;
    private int frameCount;

    private const int MID_CUBE = 13;//中间方格编号
    private Vector3 Y = new Vector3(0, 1f, 0);
    private Vector3 X = new Vector3(1f, 0, 0);
    private Vector3 Z = new Vector3(0, 0, 1f);

    private const int YID = 1, XID = 2, ZID = 3;

    //面材质
    public Material BlackMaterial;
    public Material BlueMaterial;
    public Material GreenMaterial;
    public Material OrangeMaterial;
    public Material RedMaterial;
    public Material WhiteMaterial;
    public Material YellowMaterial;

    //面对应的儿子编号
    private const int UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3, FRONT = 5, BACK = 4;


    //存教学模式的公式
    private List<TwistAction> TeachTwistActions = new List<TwistAction>();
    private int TeachTwistActions_point;//记录走到了公式哪一步
    private Vector3[,] CubeletsState = new Vector3[27, 2];//切换模式时记录原始魔方状态
    private List<TwistAction> HistoryTwistActionsState = new List<TwistAction>();//切换模式时记录历史操作集合

    private Mode mode = Mode.FREE_PLAY;
    
    enum MouseDownType { NOT_DOWN , ROTATE_PLANE, ROTATE_WHOLE, ROTATE_PLANE_WAIT, ROTATE_WHOLE_WAIT };

    //自由玩模式、教学模式、辅助还原模式
    enum Mode { FREE_PLAY,TEACH, AUXILIARY};

    Vector3[] RotationVectors =
    {
        new Vector3(0,1f,0),//Y
        new Vector3(1f,0,0),//X
        new Vector3(0,0,1f) //Z
    };//整体转动的3条轴


    // Start is called before the first frame update
    void Start()
    {
        //print("Black"+BlackMaterial.color);
        //print("Blue" + BlueMaterial.color);
        //print("Green" + GreenMaterial.color);
        //print("ORANGE" + OrangeMaterial.color);
        //print("Red" + RedMaterial.color);
        //print("White" + WhiteMaterial.color);
        //print("Yellow" + YellowMaterial.color);

        

        CreateCubes();


        LoadFormula();

        InitCudeColors();

      //  Debug.Log("#####" + Cubelets[1].transform.GetChild(BACK).GetComponent<Renderer>().material.color);

        //bool ok = false;
        //Color orange=new Color();

        //foreach(GameObject cube in Cubelets)
        //{
        //    if(Mathf.RoundToInt(cube.transform.localPosition.x)==1)
        //    {
        //        if(!ok)
        //        {
        //            ok = true;
        //            orange = cube.transform.GetChild(LEFT).gameObject.GetComponent<Renderer>().material.color;
        //        }
        //        else if(orange== cube.transform.GetChild(LEFT).gameObject.GetComponent<Renderer>().material.color)
        //            print("same");
        //    }
        //}

        //  RotateSlowly(Cubelets, Vector3.zero, Y, 90,10);

        // RotateOnce(Cubelets,Vector3.zero,X, 110);

        // FixFloatError();

        // FixFloatError();
        // CubeletPrefab.transform.Rotate(new Vector3(60, 0, 0));
        // print(CubeletPrefab.transform.localEulerAngles.x);

    }

    // Update is called once per frame
    void Update()
    {
        //if(type>0)
        //{
        //    for(int i=0;i<9;i++)
        //    {
        //        Cubelets[i].transform.RotateAround(Vector3.zero,new Vector3(1f,0,0),3f);
        //    }

        //    type-=3;
        //}

       
        if(!cubeOperatLock&&!isRotating)//没锁
        {
            SolveInput();
        }
       
        
    }


    void SolveInput()
    {
        if (Input.GetMouseButton(1))//&&Input.mousePosition.y>100)//
        {
            if(MouseDown==MouseDownType.NOT_DOWN)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
               // print(Input.mousePosition);

                //记录鼠标最先点下的位置
                lastx = Input.mousePosition.x;
                lasty = Input.mousePosition.y;

                //鼠标点下后经过的帧数
                frameCount = 0;

                //确定转动的轴
                //float theta_x = Cubelets[MID_CUBE].transform.localEulerAngles.x;
                //float theta_y = Cubelets[MID_CUBE].transform.localEulerAngles.y;
                //float theta_z = Cubelets[MID_CUBE].transform.localEulerAngles.z;
                //var rotation_x = Quaternion.AngleAxis(theta_x, X);
                //var rotation_y = Quaternion.AngleAxis(theta_y, Y);
                //var rotation_z = Quaternion.AngleAxis(theta_z, Z);
                //RotationVectors[0] = rotation_x * rotation_z * Y;
                //RotationVectors[1] = rotation_y * rotation_z * X;
                //RotationVectors[2] = rotation_x * rotation_y * Z;

                if (Physics.Raycast(ray, out hit))
                {
                    //print(hit.transform.name);

                    if (hit.transform.name == "Cube(Clone)"
                        || hit.transform.parent.name == "Cube(Clone)")//点到方块，转动某个面
                    {
                        //print("hit!Rotating a plane....");
                        if (hit.transform.name == "Cube(Clone)")
                        {
                            hitCubeletVec3 = hit.transform.position;
                        }
                        else
                        {
                            hitCubeletVec3 = hit.transform.parent.position;
                        }
                        //MouseDown = MouseDownType.ROTATE_PLANE_WAIT;
                        MouseDown = MouseDownType.ROTATE_PLANE;


                    }
                   
                }
                else//转动整个魔方
                {
                    //print("Rotating whole cube....");
                    MouseDown = MouseDownType.ROTATE_WHOLE;

                }

                return;//按下鼠标左键，记录点击位置。
            }
            
            //float dx = Input.GetAxis("Mouse X");
            //float dy = Input.GetAxis("Mouse Y");
            //print(dx + " " + dy);
        }
        else//松开鼠标左键
        {
            if(MouseDown!=MouseDownType.NOT_DOWN)
            {
                if(rotateId!=0)//可能出现单击的情况（鼠标按下5帧后才能决定旋转轴）
                {
                    //修复回整数
                    if (MouseDown == MouseDownType.ROTATE_PLANE)
                    {
                        int angle = GetFixedAngle(rotateAngle);

                        if (rotateId == YID)
                        {
                            RotateOnce(MovingCubelets, Vector3.zero, RotationVectors[rotateId - 1], angle);
                            rotateAngle += angle;
                        }
                        else
                        {
                            RotateOnce(MovingCubelets, Vector3.zero, RotationVectors[rotateId - 1], angle);
                            rotateAngle += angle;
                        }
                        // isFixFloarError = true;
                        FixFloatError();


                        //记录旋转操作
                        if (rotateId == YID)
                        {
                            InsertTwistAction(new TwistAction("Y", rotateAngle, Mathf.RoundToInt(hitCubeletVec3.y)));
                        }
                        else if (rotateId == XID)
                        {
                            InsertTwistAction(new TwistAction("X", rotateAngle , Mathf.RoundToInt(hitCubeletVec3.x)));
                        }
                        else
                        {
                            InsertTwistAction(new TwistAction("Z", rotateAngle , Mathf.RoundToInt(hitCubeletVec3.z)));
                        }

                    }
                    else if (MouseDown == MouseDownType.ROTATE_WHOLE)
                    {
                        int angle = GetFixedAngle(rotateAngle);

                        if (rotateId == YID)
                        {
                            RotateOnce(Cubelets, Vector3.zero, RotationVectors[rotateId - 1], angle);
                            rotateAngle += angle;
                        }
                        else
                        {
                            RotateOnce(Cubelets, Vector3.zero, RotationVectors[rotateId - 1], angle);
                            rotateAngle += angle;
                        }
                        // isFixFloarError = true;
                        FixFloatError();


                        //记录旋转操作
                        if (rotateId == YID)
                        {
                           // print("Y" +" " +rotateAngle);
                            InsertTwistAction(new TwistAction("ALLY", rotateAngle ));
                        }
                        else if (rotateId == XID)
                        {
                          //  print("X" +  " " + rotateAngle);
                            InsertTwistAction(new TwistAction("ALLX", rotateAngle ));
                        }
                        else
                        {
                         //   print("Z" + " " + rotateAngle);
                            InsertTwistAction(new TwistAction("ALLZ", rotateAngle ));
                        }

                    }
                }

                



                MouseDown = MouseDownType.NOT_DOWN;

                rotateAngle = 0;
                rotateId = 0;
                frameCount = 0;
                MovingCubelets.Clear();
            }
               
            //print("Mouse Up!");
            return;
        }

        //if(MouseDown==MouseDownType.ROTATE_PLANE_WAIT)
        //{
        //    frameCount++;
        //    if(frameCount==FRAME_COUNT)
        //    {
        //        MouseDown = MouseDownType.ROTATE_PLANE;
        //    }
        //}
        //else if(MouseDown==MouseDownType.ROTATE_WHOLE_WAIT)
        //{
        //    frameCount++;
        //    if (frameCount == FRAME_COUNT)
        //    {
        //        MouseDown = MouseDownType.ROTATE_WHOLE;
        //    }
        //}

       // print("___");

        if(MouseDown==MouseDownType.ROTATE_PLANE)
        {
            //要记转的是哪一个面

            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");

            ++frameCount;

            if (frameCount < FRAME_COUNT)
            {
                return;
            }
            else if (frameCount == FRAME_COUNT)
            {
                dx = Input.mousePosition.x - lastx;
                dy = Input.mousePosition.y - lasty;
            }//取前FRAME_COUNT帧的移动总和来决定移动对象

            if (rotateId==0)
            {
                if(Mathf.Abs(dx)*0.85>Mathf.Abs(dy))//Y
                {
                    rotateId = YID;
                    int w = Mathf.RoundToInt(hitCubeletVec3.y);
                    MovingCubelets = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.y) == w);
                }
                else if ((dx > 0&&dy>0)||(dx<0&&dy<0)) //绕Z轴(这里不能用dx*dy>0,因为dx*dy很有可能被近似为0了)
                {
                    rotateId = ZID;
                    int w = Mathf.RoundToInt(hitCubeletVec3.z);
                    MovingCubelets = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.z) == w);
                }
                else//X
                {
                    rotateId =XID;
                    int w = Mathf.RoundToInt(hitCubeletVec3.x);
                    MovingCubelets = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.x) == w);
                }
                
            }


            dx = Input.GetAxis("Mouse X");
            dy = Input.GetAxis("Mouse Y");


            int speed = ROTATE_SPEED;

            if (rotateId == YID)
            {
                int angle = -(int)(dx * speed);
                RotateOnce(MovingCubelets, Vector3.zero, RotationVectors[rotateId - 1], angle);
                rotateAngle += angle;
            }
            else
            {
                int angle = -(int)(dy * speed);
                if (rotateId == ZID) angle = -angle;
                RotateOnce(MovingCubelets, Vector3.zero, RotationVectors[rotateId - 1], angle);
                rotateAngle += angle;
            }



        }
        else if(MouseDown==MouseDownType.ROTATE_WHOLE)
        {
            /*
                 魔方整体转动
            */

            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");

            ++frameCount;

            if(frameCount<FRAME_COUNT)
            {
                return;
            }
            else if(frameCount==FRAME_COUNT)
            {
                dx = Input.mousePosition.x - lastx;
                dy = Input.mousePosition.y - lasty;
            }//取前FRAME_COUNT帧的移动总和来决定移动对象

           

            float nowx = Input.mousePosition.x;
            float nowy = Input.mousePosition.y;

            if(rotateId==0)
            {
                if(Mathf.Abs(dy)<Mathf.Abs(dx))//绕Y轴
                {
                    rotateId = YID;
                }
                else if(lastx>650)//绕X轴
                {
                    rotateId = XID;
                }
                else//绕Z轴
                {
                    rotateId = ZID;
                }
            }

            //print((int)dx+","+(int)dy);

            dx = Input.GetAxis("Mouse X");
            dy = Input.GetAxis("Mouse Y");

            int speed = ROTATE_SPEED;

            if(rotateId==YID)
            {
                int angle=-(int)(dx*speed) ;
                RotateOnce(Cubelets, Vector3.zero, RotationVectors[rotateId - 1], angle);
                rotateAngle += angle;
            }
            else
            {
                int angle = -(int)(dy * speed) ;
                if (rotateId == ZID) angle = -angle;
                RotateOnce(Cubelets, Vector3.zero, RotationVectors[rotateId - 1], angle);
                rotateAngle += angle;
            }

            

        }

    }



    private int GetFixedAngle(int angle)
    {
        angle %= 90;
        if (angle >= 45) return 90 - angle;
        if (angle >= 0) return -angle;
        if (angle > -45) return -angle;
        return -90 - angle;
    }

    //修复浮点误差
    private void FixFloatError()
    {
        //调用前必须保证误差很小
        foreach (GameObject cubelet in Cubelets)
        {
            cubelet.transform.localPosition = new Vector3(
                    Mathf.Round(cubelet.transform.localPosition.x),
                    Mathf.Round(cubelet.transform.localPosition.y),
                    Mathf.Round(cubelet.transform.localPosition.z));
            cubelet.transform.localRotation = Quaternion.Euler(
                    Mathf.Round(cubelet.transform.localRotation.eulerAngles.x),
                    Mathf.Round(cubelet.transform.localRotation.eulerAngles.y),
                    Mathf.Round(cubelet.transform.localRotation.eulerAngles.z));
        }




    }



    /*
        旋转点集、旋转向量起点、旋转向量、旋转角度、转速 
    */
    //IEnumerator Rotate(List<GameObject> cubelets, Vector3 point ,Vector3 rotationVector, int angle, int speed)
    //{
    //    speed = Mathf.Abs(speed);//

    //    //while (cubeOperatLock) ;
    //    //cubeOperatLock = true;
    //    isRotating = true;

    //    if (angle>0)
    //    {
    //        while (angle > 0)
    //        {
    //           // print("rotating....");
    //            int rotateAngle = angle >= speed ? speed : angle;
    //            foreach (GameObject cubelet in cubelets)
    //                cubelet.transform.RotateAround(point, rotationVector, rotateAngle);
    //            angle -= rotateAngle;
                
    //            yield return null;//下一帧继续执行
    //        }
    //    }
    //    else
    //    {
    //        angle = -angle;
    //        while (angle > 0)
    //        {
    //            // print("rotating....");
    //            int rotateAngle = angle >= speed ? speed : angle;
    //            foreach (GameObject cubelet in cubelets)
    //                cubelet.transform.RotateAround(point, rotationVector, -rotateAngle);
    //            angle -= rotateAngle;
    //            yield return null;//下一帧继续执行
    //        }
    //    }

    //    isRotating = false;
    //    // FixFloatError();

    //    // cubeOperatLock = false;
    //}
    //public void RotateSlowly(List<GameObject> cubelets, Vector3 point, Vector3 rotationVector, int angle,int speed)
    //{
    //    // print(angle + "   " + speed);
    //    List<Rotation> rotations = new List<Rotation>();
    //    rotations.Add(new Rotation(point, rotationVector, angle, speed));
    //    StartCoroutine(Rotate(cubelets,rotations));
    //}
    public void RotateOnce(List<GameObject> cubelets, Vector3 point, Vector3 rotationVector, int angle)
    {
        //while (cubeOperatLock) ;
        //cubeOperatLock = true;

        //RotateSlowly(cubelets, point, rotationVector, angle, angle + 10);
        foreach (GameObject cubelet in cubelets)
             cubelet.transform.RotateAround(point, rotationVector, angle);

       // cubeOperatLock = false;
        
    }


    //必须传整个魔方
    public void RotateSlowly(List<GameObject> cubelets, TwistAction twist, int speed)
    {
        // print(angle + "   " + speed);
        List<TwistAction> twistActions = new List<TwistAction>();
        twistActions.Add(twist);
        StartCoroutine(Rotate(cubelets, twistActions, speed));
    }


    //必须传整个魔方
    public void RotateSlowly(List<GameObject> cubelets, List<TwistAction> twistActions,int speed)
    {
        // print(angle + "   " + speed);
        StartCoroutine(Rotate(cubelets,twistActions,speed));
    }


    //执行一组旋转
    //该函数必须只有1个，不能重载,所有需要多帧的旋转必须由此函数操作
    IEnumerator Rotate(List<GameObject> Cubelets, List<TwistAction>twistActions,int speed)
    {
        

        //while (cubeOperatLock) ;
        //cubeOperatLock = true;
        isRotating = true;
        List<GameObject> cubelets = new List<GameObject>();

        foreach (TwistAction twist in twistActions)
        {
            int angle = twist.sign * 90 * twist.times;
            Vector3 point = Vector3.zero;
            Vector3 rotationVector = new Vector3();
            switch (twist.type)
            {
                case "ALLX":
                    {
                        cubelets = Cubelets;
                        rotationVector = X;
                        break;
                    }
                case "ALLY":
                    {
                        cubelets = Cubelets;
                        rotationVector = Y;
                        break;
                    }
                case "ALLZ":
                    {

                        cubelets = Cubelets;
                        rotationVector = Z;
                        break;
                    }
                case "X":
                    {
                        cubelets = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.x) == twist.axisValue);
                        rotationVector = X;
                        
                        break;
                    }
                case "Y":
                    {
                        cubelets = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.y) == twist.axisValue);
                        rotationVector = Y;
                        break;
                    }
                case "Z":
                    {
                        cubelets = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.z) == twist.axisValue);
                        rotationVector = Z;
                        break;
                    }
                default: break;
            }

            if (angle > 0)
            {
                while (angle > 0)
                {
                    // print("rotating....");
                    int rotateAngle = angle >= speed ? speed : angle;
                    foreach (GameObject cubelet in cubelets)
                        cubelet.transform.RotateAround(point, rotationVector, rotateAngle);
                    angle -= rotateAngle;

                    yield return null;//下一帧继续执行
                }
            }
            else
            {
                angle = -angle;
                while (angle > 0)
                {
                    // print("rotating....");
                    int rotateAngle = angle >= speed ? speed : angle;
                    foreach (GameObject cubelet in cubelets)
                        cubelet.transform.RotateAround(point, rotationVector, -rotateAngle);
                    angle -= rotateAngle;
                    yield return null;//下一帧继续执行
                }
            }
        }

        

        isRotating = false;
         FixFloatError();

        // cubeOperatLock = false;
    }


    public void CreateCubes()
    {
        print("start creating cubes.....");

        for (int x = -1; x <= 1; x++) 
        {
            for (int y = -1; y <= 1; y++) 
            {
                for (int z = -1; z <= 1; z++) 
                {
                    GameObject cubelet = Instantiate(CubeletPrefab, transform, false);
                    cubelet.transform.localPosition = new Vector3(-x, -y, z);
                    Cubelets.Add(cubelet);
                }
            }
        }
    }


    //给魔方每个面染色
    public void InitCudeColors()
    {
        foreach(GameObject cube in Cubelets)
        {
            for(int i=0;i<6;i++)
            {
                cube.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = BlackMaterial;
            }
            if(Mathf.RoundToInt(cube.transform.localPosition.x)==1)
            {
                cube.transform.GetChild(LEFT).gameObject.GetComponent<Renderer>().material = OrangeMaterial;
            }
            else if (Mathf.RoundToInt(cube.transform.localPosition.x) == -1)
            {
                cube.transform.GetChild(RIGHT).gameObject.GetComponent<Renderer>().material = RedMaterial;
            }

            if (Mathf.RoundToInt(cube.transform.localPosition.y) == 1)
            {
                cube.transform.GetChild(UP).gameObject.GetComponent<Renderer>().material = BlueMaterial;
            }
            else if (Mathf.RoundToInt(cube.transform.localPosition.y) == -1)
            {
                cube.transform.GetChild(DOWN).gameObject.GetComponent<Renderer>().material = GreenMaterial;
            }

            if (Mathf.RoundToInt(cube.transform.localPosition.z) == 1)
            {
                cube.transform.GetChild(FRONT).gameObject.GetComponent<Renderer>().material = YellowMaterial;
            }
            else if (Mathf.RoundToInt(cube.transform.localPosition.z) == -1)
            {
                cube.transform.GetChild(BACK).gameObject.GetComponent<Renderer>().material = WhiteMaterial;
            }
        }
    }


    //重置
    //彻底恢复魔方(每面的颜色、每个方块的独自坐标轴)
    public void ResetCubeTransformAndColors()
    {
        print("重置");

        //重置方向
        int i = 0;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    GameObject cubelet = Cubelets[i];
                    cubelet.transform.localPosition = new Vector3(-x, -y, z);
                    cubelet.transform.localEulerAngles = new Vector3(0, 0, 0);
                    ++i;
                }
            }
        }
        //重置颜色
        InitCudeColors();

    }

    public void Shuffle()
    {
        if (cubeOperatLock||isRotating ) return;//被锁了，正在进行其他操作。
        cubeOperatLock = true;


        print("shuffle!");

        //InitCudeColors();

        List<GameObject> moveCubes = new List<GameObject>();

        for(int moveCount=Random.Range(10,20);moveCount>0;--moveCount)
        {
            int axis = Random.Range(1, 3);
            int axisValue = Random.Range(-1, 1);
            if (axis == YID)
            {
                moveCubes = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.y) == axisValue);
                RotateOnce(moveCubes, Vector3.zero, Y,90);
            }
            else if(axis==XID)
            {
                moveCubes = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.x) == axisValue);
                RotateOnce(moveCubes, Vector3.zero, X, 90);
            }
            else
            {
                moveCubes = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.z) == axisValue);
                RotateOnce(moveCubes, Vector3.zero, Z, 90);
            }


        }

        FixFloatError();


        cubeOperatLock = false;
    }


    void InsertTwistAction(TwistAction twistAction)
    {
        
        if(twistAction.times!=0)
        {
            //print(twistAction.type + " " + twistAction.sign + " " + twistAction.times);
            HistoryTwistActions.Add(twistAction);
        }
        
    }


    //回退一步
    public void UnmakeTwistAction()
    {
        if (HistoryTwistActions.Count == 0) return;

        if (cubeOperatLock || isRotating) return;
        cubeOperatLock = true;

        HistoryTwistActions[HistoryTwistActions.Count - 1].UnmakeTwist(Cubelets);
        HistoryTwistActions.RemoveAt(HistoryTwistActions.Count - 1);

        

        

        cubeOperatLock = false;
    }

    public int GetTwistActionCnt()
    {
        return HistoryTwistActions.Count;
    }


    //还原魔方
    public void Restore()
    {
        print("restore");
        if (cubeOperatLock || isRotating) return;//被锁了，正在进行其他操作。
        cubeOperatLock = true;
        print("in restore");

        CubeSolver CubeSolver = new CubeSolver(Cubelets);
       
        List<TwistAction> twists=CubeSolver.Solve();

        RotateSlowly(Cubelets, twists, ROTATE_SPEED);

        //这里不能操作魔方，rotateslow还在进行
        
        cubeOperatLock = false ;
    }


    //重置
    public void Reset()
    { 
        print("reset");
        if (cubeOperatLock || isRotating) return;//被锁了，正在进行其他操作。
        cubeOperatLock = true;
    
        ResetCubeTransformAndColors();
        HistoryTwistActions.Clear();

        cubeOperatLock = false;
    }


    //导入公式
    void LoadFormula()
    {
        Formula.InitFormula();
        //InitColorUp();

        ////InitCudeColors();
        //List<TwistAction> twistActions = Formula.OLL_TwistActions[48];
        //int pp = 0;
        //print(Formula.OLLs[48]);
        //foreach (TwistAction twist in twistActions)
        //{
        //    ++pp;
        //    print(pp + ": " + twist.type + "  " + twist.sign + " " + twist.times + " " + twist.axisValue);
        //}

        //for (int i = twistActions.Count - 1; i >= 0; i--)
        //{
        //    TwistAction twist = twistActions[i];
        //    twist.UnmakeTwistQuickly(Cubelets);
        //}


        //foreach (TwistAction twist in twistActions)
        //{
        //    twist.MakeTwistQuickly(Cubelets);
        //}


    }


    //只给Up面染色
    void InitColorUp()
    {
        foreach (GameObject cube in Cubelets)
        {
            for (int i = 0; i < 6; i++)
            {
                cube.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = BlackMaterial;
            }
            if (Mathf.RoundToInt(cube.transform.localPosition.y) == 1)
            {
                cube.transform.GetChild(UP).gameObject.GetComponent<Renderer>().material = YellowMaterial;
            }

        }
    }


    
    void InitColorFirstSecondLayer()
    {
        foreach (GameObject cube in Cubelets)
        {
            for (int i = 0; i < 6; i++)
            {
                cube.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = BlackMaterial;
            }

            //是最顶层的，不染
            if (Mathf.RoundToInt(cube.transform.localPosition.y) == 1)
            {
                continue;
            }

            if (Mathf.RoundToInt(cube.transform.localPosition.x) == 1)
            {
                cube.transform.GetChild(LEFT).gameObject.GetComponent<Renderer>().material = OrangeMaterial;
            }
            else if (Mathf.RoundToInt(cube.transform.localPosition.x) == -1)
            {
                cube.transform.GetChild(RIGHT).gameObject.GetComponent<Renderer>().material = RedMaterial;
            }

            if (Mathf.RoundToInt(cube.transform.localPosition.y) == 1)
            {
                cube.transform.GetChild(UP).gameObject.GetComponent<Renderer>().material = BlueMaterial;
            }
            else if (Mathf.RoundToInt(cube.transform.localPosition.y) == -1)
            {
                cube.transform.GetChild(DOWN).gameObject.GetComponent<Renderer>().material = GreenMaterial;
            }

            if (Mathf.RoundToInt(cube.transform.localPosition.z) == 1)
            {
                cube.transform.GetChild(FRONT).gameObject.GetComponent<Renderer>().material = YellowMaterial;
            }
            else if (Mathf.RoundToInt(cube.transform.localPosition.z) == -1)
            {
                cube.transform.GetChild(BACK).gameObject.GetComponent<Renderer>().material = WhiteMaterial;
            }
        }
    }





    //进入教学模式
    //教学模式，逐步演示公式
    //"F2L","OLL","PLL"  0,1,2,^^^
    public void ShowFormula(string formulatype,int id)
    {
        
        if (cubeOperatLock || isRotating) return;//被锁了，正在进行其他操作。
        cubeOperatLock = true;

        if(mode!=Mode.TEACH)//首次进入教学模式，记录原来魔方状态
        {
            RecordCubeletsStates();
            mode = Mode.TEACH;
            LockButtonWhenFreePlayToTeach();
        }

        ResetCubeTransformAndColors();
        
        if(formulatype=="F2L")
        {
            InitColorFirstSecondLayer();
            TeachTwistActions = Formula.F2L_TwistActions[id];
        }
        else if(formulatype=="OLL")
        {
            InitColorUp();
            TeachTwistActions = Formula.OLL_TwistActions[id];
        }
        else
        {
            InitCudeColors();
            TeachTwistActions = Formula.PLL_TwistActions[id];
        }

        for (int i = TeachTwistActions.Count - 1; i >= 0; i--)
        {
            TeachTwistActions[i].UnmakeTwistQuickly(Cubelets);
        }
        TeachTwistActions_point = 0;

        cubeOperatLock = false;
    }


    //返回自由玩模式
    public void ReturnToFreePlayMode()
    {
        if (cubeOperatLock || isRotating) return;//被锁了，正在进行其他操作。
        cubeOperatLock = true;
        if (mode != Mode.FREE_PLAY)
        {
            mode = Mode.FREE_PLAY;
            ResetCubeTransformAndColors();//恢复现场
            RestoreCubeletsStates();//
            LockButtonWhenTeachToFreePlay();//自由玩模式的按钮

        }

        cubeOperatLock = false;
    }


    void RecordCubeletsStates()
    {
        for(int i=0;i<27;i++)
        {
            GameObject cube = Cubelets[i];
            CubeletsState[i,0] = cube.transform.localPosition;
            CubeletsState[i, 1] = cube.transform.localEulerAngles;
        }
        HistoryTwistActionsState.Clear();
        foreach (TwistAction twist in HistoryTwistActions)
        {
            HistoryTwistActionsState.Add(new TwistAction(twist));
        }
 
        HistoryTwistActions.Clear();
    }

    void RestoreCubeletsStates()
    {
        for (int i = 0; i < 27; i++)
        {
            GameObject cube = Cubelets[i];
            cube.transform.localPosition=CubeletsState[i, 0] ;
            cube.transform.localEulerAngles=CubeletsState[i, 1] ;
        }
        HistoryTwistActions.Clear();
        foreach (TwistAction twist in HistoryTwistActionsState)
        {
            HistoryTwistActions.Add(new TwistAction(twist));
        }
        HistoryTwistActionsState.Clear();
    }




    void LockButtonWhenFreePlayToTeach()
    {
        GameObject.Find("ShuffleButton").GetComponent<ShuffleButtonController>().ShuffleButton.interactable = false;
        GameObject.Find("ResetButton").GetComponent<ResetButtonController>().ResetButton.interactable = false;
        GameObject.Find("RestoreButton").GetComponent<RestoreButtonController>().RestoreButton.interactable = false;
        GameObject.Find("ReturnToFreePlayButton").GetComponent<ReturnToFreePlayButtonController>().button.interactable = true;
    }

    void LockButtonWhenTeachToFreePlay()
    {
        GameObject.Find("ShuffleButton").GetComponent<ShuffleButtonController>().ShuffleButton.interactable = true;
        GameObject.Find("ResetButton").GetComponent<ResetButtonController>().ResetButton.interactable = true;
        GameObject.Find("RestoreButton").GetComponent<RestoreButtonController>().RestoreButton.interactable = true;
        GameObject.Find("ReturnToFreePlayButton").GetComponent<ReturnToFreePlayButtonController>().button.interactable = false;
    }
}
