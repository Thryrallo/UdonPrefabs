
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UIToggle : UdonSharpBehaviour
{
    public bool isSynced;

    [UdonSynced, FieldChangeCallback(nameof(Value))]
    private bool _value;

    UnityEngine.UI.Toggle _toggle;
    Animator _animator;

    public UdonBehaviour targetBehaviour;
    public string targetVariableName;

    void Start()
    {
        _toggle = GetComponent<UnityEngine.UI.Toggle>();
        _animator = GetComponent<Animator>();
        Value = _toggle.isOn;
    }

    public bool Value
    {
        set
        {
            _value = value;
            _toggle.isOn = _value;
            _animator.SetBool("on", _value);
            if (targetBehaviour != null) targetBehaviour.SetProgramVariable(targetVariableName, _value);
        }
        get => _value;
    }

    public void OnValueChange()
    {
        if (Value == _toggle.isOn) return;
        if (isSynced)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Value = _toggle.isOn;
            RequestSerialization();
        }
        else
        {
            Value = !Value;
        }
    }
}
