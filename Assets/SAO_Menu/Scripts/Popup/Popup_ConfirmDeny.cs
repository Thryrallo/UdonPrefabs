
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Popup_ConfirmDeny : UdonSharpBehaviour
{
    public Canvas canvas;
    public Animator animator;

    public UnityEngine.UI.Text titleText;
    public UnityEngine.UI.Text descriptionText;

    private string title;
    private string description;
    private UdonBehaviour acceptBehaviour;
    private string acceptMethod;
    private object acceptParam;
    private UdonBehaviour denyBehaviour;
    private string denyMethod;
    private object denyParam;

    public bool IsAvailable()
    {
        return canvas.gameObject.activeSelf;
    }

    public bool Open(string title, string description, UdonBehaviour acceptBehaviour, string acceptMethod, object acceptParam,
        UdonBehaviour denyBehaviour, string denyMethod, object denyParam)
    {
        if (!IsAvailable()) return false;
        this.title = title;
        this.description = description;
        this.acceptBehaviour = acceptBehaviour;
        this.acceptMethod = acceptMethod;
        this.acceptParam = acceptParam;
        this.denyBehaviour = denyBehaviour;
        this.denyMethod = denyMethod;
        this.denyParam = denyParam;

        titleText.text = title;
        descriptionText.text = description;

        animator.SetTrigger("open");
        animator.ResetTrigger("open");

        return true;
    }

    public void Accept()
    {
        if(acceptBehaviour != null && acceptMethod!= null)
        {
            if (acceptParam != null) acceptBehaviour.SetProgramVariable(acceptMethod+"_param0", acceptParam);
            acceptBehaviour.SendCustomEvent(acceptMethod);
        }

        animator.SetTrigger("close");
        animator.ResetTrigger("close");
    }

    public void Deny()
    {
        if (denyBehaviour != null && denyMethod != null)
        {
            if (denyParam != null) denyBehaviour.SetProgramVariable(denyMethod + "_param0", denyParam);
            denyBehaviour.SendCustomEvent(denyMethod);
        }

        animator.SetTrigger("close");
        animator.ResetTrigger("close");
    }
}
