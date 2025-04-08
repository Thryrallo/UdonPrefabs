
using System;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SimpleUIToggle : SimpleUIInput
{
    Toggle _toggle;

    protected override Type VariableType => typeof(bool);

    protected override void Init()
    {
        _toggle = gameObject.GetComponent<Toggle>();
    }

    protected override object UIValue
    {
        get
        {
            return _toggle.isOn;
        }
        set
        {
            _toggle.SetIsOnWithoutNotify((bool)value);
        }
    }
}
