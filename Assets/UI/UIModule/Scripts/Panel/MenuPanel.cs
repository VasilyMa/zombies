using Statement;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : SourcePanel
{
    [SerializeField] Button _btnPlay;

    [UIInject] MenuState _state;

    public override void Init(SourceCanvas canvasParent)
    {
        _btnPlay.onClick.AddListener(onPlay);

        base.Init(canvasParent);
    }

    void onPlay()
    {
        _state.Play();
    }

    public override void OnDipose()
    {
        _btnPlay.onClick.RemoveListener(onPlay);
        base.OnDipose();
    }
}
