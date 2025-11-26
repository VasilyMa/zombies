using Statement;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : SourcePanel
{
    [SerializeField] Button _btnPlay;
    [SerializeField] Button _btnInv;
    [SerializeField] Button _btnSkill;

    public override void Init(SourceCanvas canvasParent)
    {
        _btnInv.interactable = false;
        _btnPlay.onClick.AddListener(onPlay);
        _btnSkill.onClick.AddListener(onSkillTree);

        base.Init(canvasParent);
    }

    void onSkillTree()
    {
        OpenWindow<SkillWindow>();
    }

    void onInventory()
    {
        //TODO OPEN INVENTORY
    }

    void onPlay()
    {
        OpenWindow<StageWindow>();
    }

    public override void OnDipose()
    {
        _btnPlay.onClick.RemoveAllListeners();
        _btnSkill.onClick.RemoveAllListeners();
        base.OnDipose();
    }
}
