
using System;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SimpleUIToggleGroup : SimpleUIInput
{
    public Toggle[] Toggles;
    private ToggleGroup _group;

    protected override Type VariableType => typeof(int);

    protected override object UIValue
    {
        get
        {
            for (int i = 0; i < Toggles.Length; i++)
            {
                if (Toggles[i].isOn)
                {
                    return i;
                }
            }
            return -1;
        }
        set
        {
            _supressExecutes = true;
            Toggles[(int)value].isOn = true;
            for (int i = 0; i < Toggles.Length; i++)
            {
                if ((int)value != i)
                    Toggles[i].isOn = false;
            }
            _supressExecutes = false;
        }
    }

    protected override void Init() 
    {
        _group = GetComponent<ToggleGroup>();
    }
}
