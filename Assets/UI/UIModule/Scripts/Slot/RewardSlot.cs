using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlot : SourceSlot
{
    [SerializeField] Image Icon;
    [SerializeField] Image Background;

    public override SourceSlot Init(SourceLayout layout)
    {
        gameObject.SetActive(false); 
        return this;
    }
    public override void OnActive()
    { 
    }

    public override void OnClick()
    {     
    }

    public override void OnHold(float holdTime)
    { 
    }

    public override void OnPress()
    { 
    }

    public override void OnRelease()
    { 
    }

    public override void UpdateView(object data)
    { 

    }

    void OnOpen()
    {
        transform.DOScale(1, 0.1f).SetEase(Ease.InOutCubic);
    }
}
