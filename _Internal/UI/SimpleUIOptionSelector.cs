
using System;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SimpleUIOptionSelector : SimpleUIInput
{
    public string[] Options;
    public Text Label;
    private int _selectedOption = 0;

    protected override Type VariableType => typeof(int);

    protected override object UIValue
    {
        get => _selectedOption;
        set
        {
            _selectedOption = (int)value;
            Label.text = Options[_selectedOption];
        }
    }
    
    protected override void Init(){}

    public void Next()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        UIValue = (_selectedOption + 1) % Options.Length;
        Execute();
    }

    public void Prev()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        UIValue = (_selectedOption - 1 + Options.Length) % Options.Length;
        Execute();
    }

    public void OnLocalizationChanged()
    {
        Label.text = Options[_selectedOption];
    }
}
