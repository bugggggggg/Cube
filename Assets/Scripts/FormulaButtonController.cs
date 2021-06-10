using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


class FormulaButton
{
    public Button button;
    public string FormulaType;
    public string Level;

    public FormulaButton(Button _button,string _FormulaType,string _Level)
    {
        button = _button;
        FormulaType = _FormulaType;
        Level = _Level;

        SetButtonText(button, FormulaType + "-" + Level);
        button.onClick.AddListener(Onclick);
    }

    static void SetButtonText(Button button, string content)
    {
        button.transform.Find("Text").GetComponent<Text>().text = content;
    }

    private void Onclick()
    {
        Debug.Log(FormulaType+"-"+Level);
        GameObject.Find("CubeController").gameObject.GetComponent<CubeController>().ShowFormula(FormulaType, int.Parse(Level));
    }
}


public class FormulaButtonController : MonoBehaviour
{
    public Button formulaButton;

    private List<FormulaButton> FormulaButtons = new List<FormulaButton>();

    // Start is called before the first frame update
    void Start()
    {
        
        while (GameObject.Find("CubeController").GetComponent<CubeController>() == null) ;

        //F2L
        for (int i = 0; i < Formula.F2Ls.Length; i++)
        {
            Button button = Instantiate(formulaButton);


            button.transform.SetParent(GameObject.Find("Canvas/FormulaList/Frame/List").gameObject.transform, false);

            FormulaButton new_formulaButton = new FormulaButton(button, "F2L", i.ToString());
            FormulaButtons.Add(new_formulaButton);

            //button.transform.SetParent(GameObject.Find("Canvas/ScrollView/Viewport/Content").gameObject.transform,false);
            //button.transform.localPosition.Set (0,i*5,0);


            //button.transform.parent = GameObject.Find("Canvas/Panel").gameObject.transform;
            //button.transform.localScale = GameObject.Find("Canvas/Panel")
            //    .gameObject.transform.GetChild(0).transform.localScale;

            //button.transform.localPosition
        }
        //OLL
        for (int i=0;i<Formula.OLLs.Length;i++)
        {
            Button button = Instantiate(formulaButton);


            button.transform.SetParent(GameObject.Find("Canvas/FormulaList/Frame/List").gameObject.transform, false);

            FormulaButton new_formulaButton = new FormulaButton(button, "OLL", i.ToString());
            FormulaButtons.Add(new_formulaButton);

        }
        //PLL
        for (int i = 0; i < Formula.PLLs.Length; i++)
        {
            Button button = Instantiate(formulaButton);


            button.transform.SetParent(GameObject.Find("Canvas/FormulaList/Frame/List").gameObject.transform, false);

            FormulaButton new_formulaButton = new FormulaButton(button, "PLL", i.ToString());
            FormulaButtons.Add(new_formulaButton);

        }
    }


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
