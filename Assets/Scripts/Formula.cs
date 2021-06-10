using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Formula
{
    
    //调试时改为了public,最后要改回private
    static public string[] F2Ls =
    {
        "U(RU'R')",
        "y'U'(R'UR)y",
        "y'(R'U'R)y",
        "(RUR')",
        "U'(RUR'U2) (RU'R')",
        "U'rU' (R'URUr')",
        "U'(RU'2R'U2) (RU'R')",
        "d(R'U2RU'2) (R'UR)y",
        "U'(RU'R'U) y'(R'U'R)y",
        "U2(RU'R'U') (RUR')",
        "U'(RU2'R'U) y'(R'U'R)y",
        "(RU'R'U) (RU'R'U2) (RU'R')",
        "d(R'URU') (R'U'R)y",
        "U'(RU'R'U) (RUR')",
        "(R'D'RU') (R'DRU) (RU'R')",
        "(RU'R'U2) y'(R'U'R)y",
        "(RU'2R'U') (RUR')",
        "y'(R'U2RU) (R'U'R)y",
        "U(RU'2R'U) (RU'R')",
        "y'U'(R'U2RU') (R'UR)y",
        "(RU'R'U2) (RUR')",
        "(rU'r'U2) (rUr')",
        "U(RU'R'U') (RU'R'URU'R')",
        "F(URU'R') (F'RU'R')",
        "(R'F'RU) (RU'R'F)",
        "U(RU'R'U') y'(R'UR)y",
        "(RU'R'U) (RU'R')",
        "y'(R'URU') (R'UR)y",
        "y'(R'U'RU) (R'U'R)y",
        "(RUR'U') (RUR')",
        "U'(R'FRF') (RU'R')",
        "(URU'R')3",
        "U'(RU'R'U2) (RU'R')",
        "U'(RU2'R'U) (RUR')",
        "U2(RU'R'U') y'(R'U'R)y",
        "U(F'U'FU') (RUR')",
        "(R'FRF') (RU'R'URU'R'U2RU'R')",
        "(RU'R'U') (RUR'U2) (RU'R')",
        "(RU'R'U) (RU'2R'U) (RU'R')",
        "(rU'r'U2rUr') (RUR')",
        "(RU'R') (rU'r'U2rUr')",
    };

    static public string[] OLLs =
    {
        "(RU'R2'D') (rU'r'D) R2UR'",
        "(RU'R2'D') (rUr'D) R2UR'",
        "M'U (r'U2'rU) (R'UR2r')",
        "(R'F2R2U2'R') (F'RU2'R2'F2R)",
        "l'U2 (LUL'U)l",
        "rU2'R'U'RU'r'",
        "rUR'URU'2r'",
        "l'U'LU'L'U2l",
        "(RUR'U') (R'FR2U) (R'U'F')",
        "(RUR'U) (R'FRF') (RU'2R')",
        "r'(R2UR'URU'2R') UM'",
        "r (R2'U'RU'R'U2R) U'Rr'",
        "FU (RU'R2'F'R) (URU'R')",
        "(rUR'U') r'F (R2UR'U') F'",
        "R'F'R (L'U'LU) R'FR",
        "(rUr')(RUR'U')(rU'r')",
        "(FR'F'R2) (r'URU'R'U'M')",
        "RD (r'U'rD') R'U' (R2'FRF'R)",
        "r'RU(RUR'U') M' (R'FRF')",
        "(rUR'U') rR'M'U (RU'R'U') M'",
        "(RUR'U) (RU'R'U) RU2'R'",
        "RU'2(R'2U') (R2U') R'2U'2R",
        "R2D (R'U2RD') R'U2R'",
        "(rUR'U') (r'FRF')",
        "F'(rUR'U') (r'FR)",
        "RU'2R'U'RU'R'",
        "RUR'URU2'R'",
        "(rUR'U') (r'RU) (RU'R')",
        "RF (R'URF') (R2'FRU') R'F'R",
        "FR'F (R2U'R'U') (RUR'F2)",
        "(R'U') (FURU'R'F') R",
        "RU(B'U'R'URB) R'",
        "(RUR'U') (R'FRF')",
        "(RUR2U') (R'FRU) (RU'F')",
        "(RU'2R'2) (FRF') (RU'2R')",
        "(L'U'LU') (L'ULU) (LF'L'F)",
        "(FR'F'R) (URU'R')",
        "(RUR'U) (RU'R'U') (R'FRF')",
        "f'(rUr'U')(r'FrS)",
        "(R'FRUR'U') (F'UR)",
        "(RUR'U) (RU2R') F(RUR'U')F'",
        "(R'U2'RUR'UR) U (FRUR'U'F')",
        "R'U' F' U F R",
        "f(RUR'U')f'",
        "F(RUR'U')F'",
        "R'U' (R'FRF') UR",
        "F'(L'U'LU)2F",
        "F(RUR'U')2F'",
        "RB'R2'FR2BR2'F'R",
        "R'FR2B'R2'F'R2BR'",
        "f(RUR'U')2f'",
        "(R'F'U'FU') (RUR'UR)",
        "(l'U'LU') (L'ULU') L'U2l",
        "(rUR'U) (RU'R'U) (RU2r')",
        "(R'FRU) (RU'R2'F') (R2U'R'U) (RUR')",
        "R'F'R (U'L'UL)2 R'FR",
        "(RUR'U') r(R'URU'r')",
    };

    static public string[] PLLs =
    {
        "(l'UR'D2) (RU'R'D2) R2x'",
        "(lU'RD2) (R'URD2) R2'x",
        "x'(RU'R'D) (RUR'D') (RUR'D) (RU'R'D')x",
        "(R'U'F')(RUR'U') (R'FR2U'R'U')(RUR'UR)",
        "R2U (R'UR'U') (RU'R2U'D) (R'URD')",
        "(R'd'F)(R2u) (R'URU'Ru'R'2)y'",
        "R2'U' (RU'RU) (R'UR2UD') (RU'R'D)",
        "(RUR'U'D) (R2U'RU') (R'UR'U)R2D'",
        "M2U M2U2 M2U M2",
        "(rR2'FRF'RU2') (r'UrU2'r')",
        "(RUR'F') (RUR'U') (R'F)(R2U'R')",
        "(F'RUR'U'R'FR2) (FU'R'U'RUF'R')",
        "(R'URU') (R'F'U'F) (RUR'U') RU'fRf'",
        "(RU'R'U') (RURD) (R'U'RD') R'U2R'",
        "(R2'FRURU'R'F') (RU2'R'U2R)",
        "(RUR'U')(R'F) (R2U'R'U')(RUR'F')",
        "(RU'R)(URUR) (U'R'U'R2)",
        "(R2U)(RUR'U') (R'U')(R'UR')",
        "RU' (RUR'D) (RD'RU'D) (R2'UR2D'R2')",
        "F(RU'R'U')(RUR'F') (RUR'U')(R'FRF')",
        "(M2'U2MU) (M2'UM2'UM)",
    };

    static public List<List<TwistAction>> F2L_TwistActions = new List<List<TwistAction>>();
    static public List<List<TwistAction>> OLL_TwistActions = new List<List<TwistAction>>();
    static public List<List<TwistAction>> PLL_TwistActions = new List<List<TwistAction>>();

    static bool isInit;
    

    /// <summary>
    /// 把一个表示旋转的字符串转换成一个TwistAction序列
    /// D F M R U d f l r u x y L 2 3 ' (  ) S B
    /// 对应关系：
    /// 大写单字母表示顺时针(从这个方向看的顺时针)转90度，加'表示逆时针
    /// 加2表示执行2次
    /// 小写表示转2层
    /// U:
    /// D:
    /// L:
    /// R:
    /// M:
    /// 
    /// </summary>
    /// 
    static private List<TwistAction> FormulaStringToTwistActions(string formula)
    {
        List<TwistAction> twistActions = new List<TwistAction>();

        int lastcnt = 0;
        int lastLeftBracket = 0;
        for(int i=0; i< formula.Length; ++i)
        {
            if (formula[i] == 'D')
            {
                twistActions.Add(new TwistAction("Y", -90, -1));
                lastcnt = 1;
            }
            else if (formula[i] == 'd')
            {
                twistActions.Add(new TwistAction("Y", -90, -1));
                twistActions.Add(new TwistAction("Y", -90, 0));
                lastcnt = 2;
            }
            else if (formula[i] == 'F')
            {
                twistActions.Add(new TwistAction("Z", 90, 1));
                lastcnt = 1;
            }
            else if (formula[i] == 'f')
            {
                twistActions.Add(new TwistAction("Z", 90, 1));
                twistActions.Add(new TwistAction("Z", 90, 0));
                lastcnt = 2;
            }
            else if (formula[i] == 'M'||formula[i]=='m')
            {
                twistActions.Add(new TwistAction("X", 90, 0));
                lastcnt = 1;
            }
            else if (formula[i] == 'R')
            {
                twistActions.Add(new TwistAction("X", -90, -1));
                lastcnt = 1;
            }
            else if (formula[i] == 'r')
            {
                twistActions.Add(new TwistAction("X", -90, -1));
                twistActions.Add(new TwistAction("X", -90, 0));
                lastcnt = 2;
            }
            else if (formula[i] == 'U')
            {
                twistActions.Add(new TwistAction("Y", 90, 1));
                lastcnt = 1;
            }
            else if (formula[i] == 'u')
            {
                twistActions.Add(new TwistAction("Y", 90, 1));
                twistActions.Add(new TwistAction("Y", 90, 0));
                lastcnt = 2;
            }
            else if (formula[i] == 'l')
            {
                twistActions.Add(new TwistAction("X", 90, 1));
                twistActions.Add(new TwistAction("X", 90, 0));
                lastcnt = 2;
            }
            else if (formula[i] == 'L')
            {
                twistActions.Add(new TwistAction("X", 90, 1));
                lastcnt = 1;
            }
            else if (formula[i] == 'S'||formula[i]=='s')
            {
                twistActions.Add(new TwistAction("Z", 90, 0));
                lastcnt = 1;
            }
            else if (formula[i] == 'B')
            {
                twistActions.Add(new TwistAction("Z", -90, -1));
                lastcnt = 1;
            }
            else if (formula[i] == 'b')
            {
                twistActions.Add(new TwistAction("Z", -90, -1));
                twistActions.Add(new TwistAction("Z", -90, 0));
                lastcnt = 2;
            }
            else if (formula[i] == 'e'|| formula[i]=='E')
            {
                twistActions.Add(new TwistAction("Y", -90, 0));
                
                lastcnt = 1;
            }
            else if (formula[i] == 'x'||formula[i]=='X')
            {
                twistActions.Add(new TwistAction("ALLX", -90));
                lastcnt = 1;
            }////x是整体逆时针90度
            else if (formula[i] == 'y'||formula[i]=='Y')
            {
                twistActions.Add(new TwistAction("ALLY", 90));
                lastcnt = 1;
            }
            else if (formula[i] == 'z' || formula[i] == 'Z')
            {
                twistActions.Add(new TwistAction("ALLZ", 90));
                lastcnt = 1;
            }
            else if (formula[i] == '\'')
            {

                for (int j = twistActions.Count - 1; j >= twistActions.Count - lastcnt; j--)
                {
                    twistActions[j].sign = -twistActions[j].sign;
                }
            }
            else if (formula[i] == '2' || formula[i] == '3')
            {
                if (formula[i - 1] != ')')
                {
                    for (int j = twistActions.Count - 1; j >= twistActions.Count - lastcnt; j--)
                    {
                        twistActions[j].times = twistActions[j].times * 2;
                    }
                }
                else
                {
                    List<TwistAction> tmp_twistActions = FormulaStringToTwistActions(formula.Substring(lastLeftBracket + 1, i - lastLeftBracket-1));
                    int cnt = formula[i] - '0';
                    while(cnt>1)
                    {
                        foreach (TwistAction twist in tmp_twistActions)
                        {
                            twistActions.Add(twist);
                        }
                        --cnt;
                    }
                    
                }
            }
            else if(formula[i]=='(')
            {
                lastLeftBracket = i;
            }
            

        }


        return twistActions;
    }


    static public void InitFormula()
    {
        if (isInit) return;
        isInit = true;

        InitF2LFormula();
        InitOLLFormula();
        InitPLLFormula();
    }

    static private void InitOLLFormula()
    {


        foreach (string formula in OLLs)
        {
            OLL_TwistActions.Add(FormulaStringToTwistActions(formula));
        }
        //OLL_TwistActions.Add(FormulaStringToTwistActions("M'U"));
        //Debug.Log(OLLs[0]);
    }

    static private void InitPLLFormula()
    {


        foreach (string formula in PLLs)
        {
            PLL_TwistActions.Add(FormulaStringToTwistActions(formula));
        }
        
    }

    static private void InitF2LFormula()
    {


        foreach (string formula in F2Ls)
        {
            F2L_TwistActions.Add(FormulaStringToTwistActions(formula));
        }
   
    }


}

