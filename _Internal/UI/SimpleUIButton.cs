
using System;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SimpleUIButton : SimpleUIInput
{
    protected override Type VariableType => null;

    protected override object UIValue { get; set; }

    protected override void Init() {}
}
