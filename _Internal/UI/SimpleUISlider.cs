
using Newtonsoft.Json.Linq;
using System;
using Thry.Udon.UI;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SimpleUISlider : SimpleUIInput
{
    Slider _slider;
    Type _type;

    public Text Handle;
    public string HandlePrefix;
    public string HandlePostfix;

    protected override Type VariableType
    {
        get
        {
            if (_type != null) return _type;
            Init();
            return _type;
        }
    }

    protected override void Init()
    {
        _slider = gameObject.GetComponent<Slider>();
        if(_slider.wholeNumbers)
            _type = typeof(int);
        else
            _type = typeof(float);
        _vibrationFeedback = false;
    }

    protected override object UIValue
    {
        get
        {
            if (_type == typeof(int))
                return 0 + (int)_slider.value; // 0 + to cast to object. Hack cause udon seems to ignore the cast otherwise
            else
                return _slider.value;
        }
        set
        {
            _slider.SetValueWithoutNotify((float)value);
        }
    }

    protected override void UpdateOptionals(bool wasUserInput, bool valueChanged)
    {
        base.UpdateOptionals(wasUserInput, valueChanged);
        if (Handle != null && valueChanged)
        {
            Handle.text = HandlePrefix + _slider.value + HandlePostfix;
        }
    }
}
