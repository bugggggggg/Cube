using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;




public class CubeSolver
{
    /// <summary>
    /// 从底面开始，一层一层还原(层先法)
    /// UPD：改用csop法
    ///
    /// UPD：下面这条保证不成立
    /// 该类使用的还原法保证不进行"ALLX","ALLZ"操作，即底面、顶面中心块颜色保证不变。
    /// </summary>
    /// 

    private Vector3 Y = new Vector3(0, 1f, 0);
    private Vector3 X = new Vector3(1f, 0, 0);
    private Vector3 Z = new Vector3(0, 0, 1f);

    private List<GameObject> Cubelets = new List<GameObject>();//待解的魔方
    private List<TwistAction> TwistActions = new List<TwistAction>();//解方案集合
    private Color DownColor, UpColor;//底面和顶面的颜色

    //和CubeController里统一
    private const int UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3, FRONT = 5, BACK = 4;
    private static Vector3[] DirectionVectors =
    {
        new Vector3(0,1f,0),
        new Vector3(0,-1f,0),
        new Vector3(1f,0,0),
        new Vector3(-1f,0,0),
        new Vector3(0,0,-1f),
        new Vector3(0,0,1f),
    };


    public CubeSolver(List<GameObject>_Cubelets)
    {
        foreach(GameObject cube in _Cubelets)
        {
            GameObject newCube = UnityEngine.Object.Instantiate(cube);
            Cubelets.Add(newCube);
        }

        //DownColor = GetCenterCubeColor(DOWN);
        //UpColor = GetCenterCubeColor(UP);

        TwistActions.Clear();
    }


    public List<TwistAction> Solve()
    {
        //foreach (GameObject cube in Cubelets)
        //{
        //    if (Mathf.RoundToInt(cube.transform.localPosition.x) == 0
        //        && Mathf.RoundToInt(cube.transform.localPosition.y) == 1
        //        && Mathf.RoundToInt(cube.transform.localPosition.z) == 1)
        //    {
        //        Debug.Log(GetCubeColorByDirection(cube, FRONT));
        //        break;
        //    }
        //}
        //SolveThirdLayerEdge();


        //SolveByLayerFirst();
        SolveByCFOP();

        //Color FrontColor = GetCenterCubeColor(FRONT);
        //Debug.Log("___"+ GetCubeColorByDirection(Cubelets[1], BACK));
        //Debug.Log("___" + FrontColor);
        //int needId = GetCubeIdByColor(DownColor, FrontColor);
        //  Debug.Log(needId);
        foreach (GameObject cube in Cubelets)
        {
            GameObject.Destroy(cube);
        }
        //InsertTwistAction("ALLX", 1, 90);
        //InsertTwistAction("ALLY", 1, 90);
        return TwistActions;
    }



    //CFOP法
    void SolveByCFOP()
    {
        SolveFirstLayerEdge();//CROSS
        SolveFirstTwoLayerEdge();//F2L
        SolveThirdLayerPlane();//OLL
        SolveThirdLayerEdge();//PLL
    }

    //层优先法
    void SolveByLayerFirst()
    {
        SolveFirstLeyer();
        SolveSecondLayer();
        SolveThirdLayer();
    }

    //还原底层
    void SolveFirstLeyer()
    {
        SolveFirstLayerEdge();
        SolveFirstLayerCorner();
    }

    //还原底层边块
    void SolveFirstLayerEdge()
    {
        //一共4个边块
        for (int _ = 0; _ < 4; _++)
        {
            Color FrontColor = GetCenterCubeColor(FRONT);
            Color DownColor = GetCenterCubeColor(DOWN);
            int needId = GetCubeIdByColor(DownColor, FrontColor);
            GameObject cube = Cubelets[needId];
            int cnt = 0;
           // Debug.Log(cube.transform.localPosition);
            //return;
            //Debug.Log(FrontColor+"___" + DownColor);
            //for (int j = 0; j < 6; j++)
            //{
            //    Color color = cube.transform.GetChild(j).GetComponent<Renderer>().material.color;
            //    Debug.Log(color);

            //}
            {//把cube移到Front-Up位置
                int y = Mathf.RoundToInt(cube.transform.localPosition.y);
                if(y==-1)
                {
                    if(Mathf.Abs(Mathf.RoundToInt(cube.transform.localPosition.x))==1)
                    {
                        InsertTwistAction("X",Mathf.RoundToInt(cube.transform.localPosition.x), 180);
                    }
                    else
                    {
                        InsertTwistAction("Z", Mathf.RoundToInt(cube.transform.localPosition.z), 180);
                    }
                }
                else if(y==0)
                {
                    int x = Mathf.RoundToInt(cube.transform.localPosition.x);
                    int z = Mathf.RoundToInt(cube.transform.localPosition.z);
                    if(x==1&&z==1)
                    {
                        InsertTwistAction("Z", 1, 90);
                    }
                    else if(x==1&&z==-1)
                    {
                        InsertTwistAction("Z", -1, 90);
                        InsertTwistAction("Y", 1, 90);
                        InsertTwistAction("Z", -1, -90);
                    }
                    else if(x==-1&&z==1)
                    {
                        InsertTwistAction("Z", 1, -90);
                        
                    }
                    else if(x==-1&&z==-1)
                    {
                        InsertTwistAction("Z", -1, -90);
                        InsertTwistAction("Y", 1, 90);
                        InsertTwistAction("Z", -1, 90);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                    
                    
                    
                }
                //Debug.Log(cube.transform.localPosition);
                

                cnt = 0;
                while (cnt<4&&(Mathf.RoundToInt(cube.transform.localPosition.x) != 0
                        || Mathf.RoundToInt(cube.transform.localPosition.z) != 1))
                {
                    ++cnt;
                    InsertTwistAction("Y", 1, 90);
                }
                Debug.Assert(cnt != 4);
            }



           // Debug.Log(GetCubeColorByDirection(cube, FRONT));
            if(GetCubeColorByDirection(cube,FRONT)!=DownColor)
            {
                InsertTwistAction("Z", 1, 180);
            }
            else
            {
                InsertTwistAction("Y", 1, 90);
                InsertTwistAction("X", 0, -90);
                InsertTwistAction("Y", 1, -90);
                InsertTwistAction("X", 0, 90);

                
            }
            
            InsertTwistAction("ALLY", 0, 90);
        }
    }


    void SolveFirstLayerCorner()
    {
        //四个角块
        for(int _=0;_<4;_++)
        {
            Color FrontColor = GetCenterCubeColor(FRONT);
            Color LeftColor = GetCenterCubeColor(LEFT);
            Color DownColor = GetCenterCubeColor(DOWN);
            int needId = GetCubeIdByColor(FrontColor, LeftColor, DownColor);
            GameObject cube = Cubelets[needId];
            int cnt = 0;
            if(Mathf.RoundToInt(cube.transform.localPosition.y)==-1)
            {//转到顶层
                int x = Mathf.RoundToInt(cube.transform.localPosition.x);
                int z = Mathf.RoundToInt(cube.transform.localPosition.z);

                if(x==1&&z==1)
                {
                    InsertTwistAction("Z", 1, 90);
                    InsertTwistAction("Y", 1, 90);
                    InsertTwistAction("Z", 1, -90);
                }
                else if(x==1&&z==-1)
                {
                    InsertTwistAction("Z", -1, 90);
                    InsertTwistAction("Y", 1, -90);
                    InsertTwistAction("Z", -1, -90);
                }
                else if(x==-1&&z==1)
                {
                    InsertTwistAction("Z", 1, -90);
                    InsertTwistAction("Y", 1, -90);
                    InsertTwistAction("Z", 1, 90);
                }
                else if(x==-1&&z==-1)
                {
                    InsertTwistAction("Z", -1, -90);
                    InsertTwistAction("Y", 1, 90);
                    InsertTwistAction("Z", -1, 90);
                }
                else
                {
                    Debug.Assert(false);
                }
            }

            //转到Front-Up-Left位置
            cnt = 0;
            while (cnt < 4 && (Mathf.RoundToInt(cube.transform.localPosition.x) != 1
                        || Mathf.RoundToInt(cube.transform.localPosition.z) != 1))
            {
                ++cnt;
                InsertTwistAction("Y", 1, 90);
            }

            
            InsertTwistAction("Y", 1, 90);
            InsertTwistAction("Z", 1, 90);
            InsertTwistAction("Y", 1, -90);
            InsertTwistAction("Z", 1, -90);

            //调整方向
            cnt = 0;
            while(cnt<3&&GetCubeColorByDirection(cube,DOWN)!=DownColor)
            {
                ++cnt;
                InsertTwistAction("Z", 1, 90);
                InsertTwistAction("Y", 1, -90);
                InsertTwistAction("Z", 1, -90);

                InsertTwistAction("Y", 1, 90);
                InsertTwistAction("Z", 1, 90);
                InsertTwistAction("Y", 1, -90);
                InsertTwistAction("Z", 1, -90);
            }
            Debug.Assert(cnt != 3);

            

            

            InsertTwistAction("ALLY", 1, 90);
        }
    }


    void SolveSecondLayer()
    {
        SolveSecondLeyerEdge();
    }

    void SolveSecondLeyerEdge()
    {
        //F2L_Same();
        //return;
        for (int _=0;_<4;_++)
        {
            Color FrontColor = GetCenterCubeColor(FRONT);
            Color LeftColor = GetCenterCubeColor(LEFT);
            int needId = GetCubeIdByColor(FrontColor, LeftColor);
            GameObject cube = Cubelets[needId];
            //Debug.Log(cube.transform.localPosition);
            
            int cnt = 0;

            if(Mathf.RoundToInt(cube.transform.localPosition.y)==0)
            {//移到顶层
                cnt = 0;
                while (cnt < 4 && (Mathf.RoundToInt(cube.transform.localPosition.x) != 1
                        || Mathf.RoundToInt(cube.transform.localPosition.z) != 1))
                {
                    ++cnt;
                    InsertTwistAction("ALLY", 1, 90);
                }
                Debug.Assert(cnt != 4);
                F2L_Same();
                while(cnt>0)
                {
                    --cnt;
                    InsertTwistAction("ALLY", 1, -90);
                }
            }
            
            //移到Up-Front
            cnt = 0;
            while (cnt < 4 && (Mathf.RoundToInt(cube.transform.localPosition.x) != 0
                        || Mathf.RoundToInt(cube.transform.localPosition.z) != 1))
            {
                ++cnt;
                InsertTwistAction("Y", 1, 90);
            }
            
            if(GetCubeColorByDirection(cube,FRONT)==GetCenterCubeColor(FRONT))
            {
                F2L_Same();
            }
            else
            {
                F2L_Contrary();
            }

            InsertTwistAction("ALLY", 1, 90);
        }
    }

    void SolveThirdLayer()//这个过程中DownColor和UpColor改变了
    {
        SolveThirdLayerPlane();
        SolveThirdLayerEdge();
    }




    //这里还有问题______________________________________________________________(solved)
    //还原顶层面
    void SolveThirdLayerPlane()
    {
        //已经完成
        if (JudgeCompleteByDirection(UP))
        {
            Debug.Log("UP is done");
            return;
        }
        for (int i=0;i<4;i++)
        {
            for (int j = 0; j < i; j++) 
            {
                (new TwistAction("Y", 90, 1)).MakeTwistQuickly(Cubelets);
            }
            foreach(List<TwistAction> twistActions in Formula.OLL_TwistActions)
            {
                foreach(TwistAction twist in twistActions)
                {
                    twist.MakeTwistQuickly(Cubelets);
                }
                if(JudgeCompleteByDirection(UP))
                {
                    for (int j = 0; j < i; j++)
                    {
                        InsertTwistActionOnlyAdd(new TwistAction("Y", 90, 1));
                    }
                    foreach (TwistAction twist in twistActions)
                    {
                        InsertTwistActionOnlyAdd(twist);

                    }
                    
                    return;
                }
                for(int j=twistActions.Count-1;j>=0;j--)
                {
                    TwistAction twist = twistActions[j];
                    twist.UnmakeTwistQuickly(Cubelets);
                }
                
            }
            //撤销
            for (int j = 0; j < i; j++)
            {
                (new TwistAction("Y", -90, 1)).MakeTwistQuickly(Cubelets);
            }
        }

        Debug.Assert(false);
        
    }



    //还原顶层边
    void SolveThirdLayerEdge()
    {
        if (JudgeCompleteAll()) return;

        //整体转
        for(int _=0;_<4;_++)
        {
            for (int j = 0; j < _; j++)
            {
                (new TwistAction("ALLY", 90, 1)).MakeTwistQuickly(Cubelets);
            }

            //顶层转
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    (new TwistAction("Y", 90, 1)).MakeTwistQuickly(Cubelets);
                }

                //按PLL公式转
                foreach (List<TwistAction> twistActions in Formula.PLL_TwistActions)
                {
                    foreach (TwistAction twist in twistActions)
                    {
                        twist.MakeTwistQuickly(Cubelets);
                    }
                    if (JudgeCompleteAll())//成功了
                    {
                        for (int j = 0; j < _; j++)
                        {
                            InsertTwistActionOnlyAdd(new TwistAction("ALLY", 90, 1));
                        }
                        for (int j = 0; j < i; j++)
                        {
                            InsertTwistActionOnlyAdd(new TwistAction("Y", 90, 1));
                        }
                        foreach (TwistAction twist in twistActions)
                        {
                            InsertTwistActionOnlyAdd(twist);

                        }

                        return;
                    }
                    for (int j = twistActions.Count - 1; j >= 0; j--)
                    {
                        TwistAction twist = twistActions[j];
                        twist.UnmakeTwistQuickly(Cubelets);
                    }

                }
                //撤销
                for (int j = 0; j < i; j++) 
                {
                    (new TwistAction("Y", -90, 1)).MakeTwistQuickly(Cubelets);
                }
            }

            //撤销
            for (int j = 0; j < _; j++)
            {
                (new TwistAction("ALLY", 90, 1)).UnmakeTwistQuickly(Cubelets);
            }
        }
        

        Debug.Assert(false);
    }



    //F2L公式
    void SolveFirstTwoLayerEdge()
    {
        //四个角
        for (int _ = 0; _ < 4; _++)
        {
           
            //把需要的角块转到Front-Up-Left位置
            {
                Color FrontColor = GetCenterCubeColor(FRONT);
                Color LeftColor = GetCenterCubeColor(LEFT);
                Color DownColor = GetCenterCubeColor(DOWN);
                int needId = GetCubeIdByColor(FrontColor, LeftColor, DownColor);
                GameObject cube = Cubelets[needId];
                int cnt = 0;
                if (Mathf.RoundToInt(cube.transform.localPosition.y) == -1)
                {//转到顶层
                    int x = Mathf.RoundToInt(cube.transform.localPosition.x);
                    int z = Mathf.RoundToInt(cube.transform.localPosition.z);

                    if (x == 1 && z == 1)
                    {
                        InsertTwistAction("Z", 1, 90);
                        InsertTwistAction("Y", 1, 90);
                        InsertTwistAction("Z", 1, -90);
                    }
                    else if (x == 1 && z == -1)
                    {
                        InsertTwistAction("Z", -1, 90);
                        InsertTwistAction("Y", 1, -90);
                        InsertTwistAction("Z", -1, -90);
                    }
                    else if (x == -1 && z == 1)
                    {
                        InsertTwistAction("Z", 1, -90);
                        InsertTwistAction("Y", 1, -90);
                        InsertTwistAction("Z", 1, 90);
                    }
                    else if (x == -1 && z == -1)
                    {
                        InsertTwistAction("Z", -1, -90);
                        InsertTwistAction("Y", 1, 90);
                        InsertTwistAction("Z", -1, 90);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }

                //转到Front-Up-Left位置
                cnt = 0;
                while (cnt < 4 && (Mathf.RoundToInt(cube.transform.localPosition.x) != 1
                            || Mathf.RoundToInt(cube.transform.localPosition.z) != 1))
                {
                    ++cnt;
                    InsertTwistAction("Y", 1, 90);
                }

            }

            
            //不在顶面(猜测不存在这种情况)//猜测错误。。。
            //把需要的边块转到顶面，同时保证需要的角块在Front-Up-Left位置
            {
                Color LeftColor = GetCenterCubeColor(LEFT);
                Color FrontColor = GetCenterCubeColor(FRONT);
                int needId = GetCubeIdByColor(LeftColor, FrontColor);
                GameObject cube = Cubelets[needId];

                if(Mathf.RoundToInt(cube.transform.localPosition.y)!=1)
                {

                    int x = Mathf.RoundToInt(cube.transform.localPosition.x);
                    int z = Mathf.RoundToInt(cube.transform.localPosition.z);
                    if (x == 1 && z == 1)//有公式
                    {

                    }
                    else if (x == 1 && z == -1)
                    {
                        //Debug.Log("@@@");
                        InsertTwistAction("Z", -1, 90);
                        InsertTwistAction("Y", 1, -90);
                        InsertTwistAction("Z", -1, -90);
                        InsertTwistAction("Y", 1, 90);
                    }
                    else if (x == -1 && z == 1)
                    {
                        //Debug.Log("###");
                        InsertTwistAction("Y", 1, 90);
                        InsertTwistAction("Z", 1, -90);
                        InsertTwistAction("Y", 1, 90);
                        InsertTwistAction("Z", 1, 90);
                        InsertTwistAction("Y", 1, 90);
                        InsertTwistAction("Y", 1, 90);
                    }
                    else if (x == -1 && z == -1)
                    {
                       // Debug.Log("$$$");
                        InsertTwistAction("X", -1, 90);
                        InsertTwistAction("Y", 1, 90);
                        InsertTwistAction("X", -1, -90);
                        InsertTwistAction("Y", 1, -90);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }

            }

            
            //套F2L公式
            {

                //录入的公式求的是在Down-Front-Right位置
                InsertTwistAction("ALLY", 1, -90);

                Color FrontColor = GetCenterCubeColor(FRONT);
                Color RightColor = GetCenterCubeColor(RIGHT);
                Color DownColor = GetCenterCubeColor(DOWN);
                int needId = GetCubeIdByColor(FrontColor, RightColor, DownColor);
                GameObject cube1 = Cubelets[needId];
                needId = GetCubeIdByColor(RightColor, FrontColor);
                GameObject cube2 = Cubelets[needId];

                if(GetCubeColorByDirection(cube1,FRONT)!=FrontColor
                    || GetCubeColorByDirection(cube1, DOWN) != DownColor
                    || GetCubeColorByDirection(cube1, RIGHT) != RightColor
                    || GetCubeColorByDirection(cube2, FRONT)!=FrontColor
                    || GetCubeColorByDirection(cube2, RIGHT) != RightColor)
                {
                    bool ok = false;
                    foreach (List<TwistAction> twistActions in Formula.F2L_TwistActions)
                    {
                        foreach (TwistAction twist in twistActions)
                        {
                            twist.MakeTwistQuickly(Cubelets);
                        }

                        

                        if (GetCubeColorByDirection(cube1, FRONT) == FrontColor
                            && GetCubeColorByDirection(cube1, DOWN) == DownColor
                            && GetCubeColorByDirection(cube1, RIGHT) == RightColor
                            && GetCubeColorByDirection(cube2, FRONT) == FrontColor
                            && GetCubeColorByDirection(cube2, RIGHT) == RightColor)
                        {
                            foreach (TwistAction twist in twistActions)
                            {
                                InsertTwistActionOnlyAdd(twist);
                            }
                            
                            ok = true;
                            break;
                        }

                        //回退
                        
                        for (int j = twistActions.Count - 1; j >= 0; j--)
                        {
                            TwistAction twist = twistActions[j];
                            twist.UnmakeTwistQuickly(Cubelets);
                        }

                    }
                    //if (!ok)
                    //{
                    //    Debug.Log("Error");
                    //    return;
                    //}
                    Debug.Assert(ok);
                }

                InsertTwistAction("ALLY", 1, 90);
            }
            


            InsertTwistAction("ALLY", 1, 90);
            
        }

        
    }


    


    //判断Direction面颜色是否都相同
    bool JudgeCompleteByDirection(int Direction)
    {
        Color Color = GetCenterCubeColor(Direction);
        List<GameObject> cubes;
        if (Direction == UP) cubes = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.y) == 1);
        else if (Direction == DOWN) cubes = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.y) == -1);
        else if (Direction == LEFT) cubes = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.x) == 1);
        else if (Direction == RIGHT) cubes = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.x) == -1);
        else if (Direction == FRONT) cubes = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.z) == 1);
        else cubes = Cubelets.FindAll(cube => Mathf.RoundToInt(cube.transform.localPosition.z) == -1);

        foreach (GameObject cube in cubes)
        {
            if(GetCubeColorByDirection(cube,Direction)!=Color)
            {
                return false;
            }
        }

        return true;
    }


    //判断是否还原成功
    bool JudgeCompleteAll()
    {
        //检查6个面
        for (int i = 0; i < 6; i++) 
        {
            if(!JudgeCompleteByDirection(i))
            {
                return false;
            }
        }
        return true;
    }


    //执行一次操作并记录
    void InsertTwistAction(string axis, int axisValue, int angle = 90)
    {
        TwistAction twist = new TwistAction(axis, angle, axisValue);
        twist.MakeTwistQuickly(Cubelets);
        TwistActions.Add(twist);
       // Debug.Log(axis + " " + angle + " " + axisValue);
    }

    void InsertTwistAction(TwistAction twist)
    {
        twist.MakeTwistQuickly(Cubelets);
        TwistActions.Add(twist);
        // Debug.Log(axis + " " + angle + " " + axisValue);
    }
    void InsertTwistActionOnlyAdd(TwistAction twist)
    {
        
        TwistActions.Add(twist);
        // Debug.Log(axis + " " + angle + " " + axisValue);
    }



    //cube的Direction面的颜色
    static Color GetCubeColorByDirection(GameObject cube,int Direction)
    {
        //float theta_x = cube.transform.localEulerAngles.x;
        //float theta_y = -cube.transform.localEulerAngles.y;
        //float theta_z = -cube.transform.localEulerAngles.z;
        //var rotation_x = Quaternion.AngleAxis(theta_x, X);
        //var rotation_y = Quaternion.AngleAxis(theta_y, Y);
        //var rotation_z = Quaternion.AngleAxis(theta_z, Z);
        //Vector3 vec = rotation_x * rotation_y * rotation_z * DirectionVectors[Direction];
        
        Quaternion rotation = Quaternion.Euler(cube.transform.localEulerAngles);
        
        int newDirection=0;
        for (int i = 0; i < 6; i++)
        {
            Vector3 vec = rotation * DirectionVectors[i];
            vec.x = Mathf.RoundToInt(vec.x);
            vec.y = Mathf.RoundToInt(vec.y);
            vec.z = Mathf.RoundToInt(vec.z);
            //某条轴经过相对方块的旋转后得到了Direction方向，那么Direction对应原来的i方向

            if (vec == DirectionVectors[Direction])
            {
                newDirection = i;
               // Debug.Log("newDirection:" + i);
                return cube.transform.GetChild(newDirection).GetComponent<Renderer>().material.color;
                
            }
        }
        Debug.Assert(false);
        return cube.transform.GetChild(newDirection).GetComponent<Renderer>().material.color;
    }

    //找每个面中心块的颜色
    Color GetCenterCubeColor(int Direction)
    {

        int x = Mathf.RoundToInt(DirectionVectors[Direction].x);
        int y = Mathf.RoundToInt(DirectionVectors[Direction].y);
        int z = Mathf.RoundToInt(DirectionVectors[Direction].z);

        foreach (GameObject cube in Cubelets)
        {
            int _x = Mathf.RoundToInt(cube.transform.localPosition.x);
            int _y = Mathf.RoundToInt(cube.transform.localPosition.y);
            int _z = Mathf.RoundToInt(cube.transform.localPosition.z);
            if (x == _x && y == _y && z == _z)
            {
                Color color = new Color();
                //中间的面有5个黑面，
                for (int i = 0; i < 5; i++) 
                {
                    if(cube.transform.GetChild(i).GetComponent<Renderer>().material.color 
                        == cube.transform.GetChild(i+1).GetComponent<Renderer>().material.color)
                    {
                        color = cube.transform.GetChild(i).GetComponent<Renderer>().material.color;
                        break;
                    }
                }
                //for (int i = 0; i < 6; i++) Debug.Log("__" + cube.transform.GetChild(i).GetComponent<Renderer>().material.color);
                for (int i = 0; i < 6; i++)
                {
                    if (cube.transform.GetChild(i).GetComponent<Renderer>().material.color!=color)
                    {
                        return cube.transform.GetChild(i).GetComponent<Renderer>().material.color;
                    }
                }
            }
        }
        //跑到这里就出错了
        Debug.Assert(false);
        return Color.green;
    }


    //通过颜色找到对应块id
    int GetCubeIdByColor(Color color1,Color color2,Color color3)
    {
        for (int i = 0; i < Cubelets.Count; i++) 
        {
            GameObject cube = Cubelets[i];
            int cnt = 0;
            //
            for(int j=0;j<6;j++)
            {
                Color color = cube.transform.GetChild(j).GetComponent<Renderer>().material.color;
                if (color == color1) cnt++;
                else if (color == color2) cnt++;
                else if (color == color3) cnt++;
            }
            if (cnt == 3) return i;
        }

        //不存在
        Debug.Assert(false);
        return 0;
    }

    //通过颜色找到对应块id
    int GetCubeIdByColor(Color color1, Color color2)
    {
        for (int i = 0; i < Cubelets.Count; i++)
        {
            GameObject cube = Cubelets[i];
            int cnt = 0;
            //
            for (int j = 0; j < 6; j++)
            {
                Color color = cube.transform.GetChild(j).GetComponent<Renderer>().material.color;
                if (color == color1) cnt++;
                else if (color == color2) cnt++;
                
            }
            int sum = Math.Abs(Mathf.RoundToInt(cube.transform.localPosition.x))
                + Math.Abs(Mathf.RoundToInt(cube.transform.localPosition.y))
                + Math.Abs(Mathf.RoundToInt(cube.transform.localPosition.z));
            if (cnt == 2&&sum==2) return i;
        }

        //不存在
        Debug.Assert(false);
        return 0;
    }



    //对应F2L-25
    //F'L'FUFU'F'L
    void F2L_Same()
    {
        InsertTwistAction("Z", 1, -90);
        InsertTwistAction("X", 1, -90);
        InsertTwistAction("Z", 1, 90);
        InsertTwistAction("Y", 1, 90);
        InsertTwistAction("Z", 1, 90);
        InsertTwistAction("Y", 1, -90);
        InsertTwistAction("Z", 1, -90);
        InsertTwistAction("X", 1, 90);
    }

    //对应F2L-25
    //UUFU'F'U'y'F'UFy
    void F2L_Contrary()
    {
        InsertTwistAction("Y", 1, 90);
        InsertTwistAction("Y", 1, 90);
        InsertTwistAction("Z", 1, 90);
        InsertTwistAction("Y", 1, -90);

        InsertTwistAction("Z", 1, -90);
        InsertTwistAction("Y", 1, -90);
        InsertTwistAction("ALLY", 1, -90);
        InsertTwistAction("Z", 1, -90);

        InsertTwistAction("Y", 1, 90);
        InsertTwistAction("Z", 1, 90);
        InsertTwistAction("ALLY", 1, 90);
    }

}

