using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnType : MainMenu
{
    public BTNType currentType;
    public void OncBtnclick()
    {
        switch (currentType)
        {
            case BTNType.Load:
                Debug.Log("LOAD");
                break;

            case BTNType.Option:
                Debug.Log("Option");
                break;

            case BTNType.Quit:
                Debug.Log("Quit");
                break;
        }
    }

    public void OnclickOption()
    {

    }

    public void OnclickQuit()
    {

    }
}
