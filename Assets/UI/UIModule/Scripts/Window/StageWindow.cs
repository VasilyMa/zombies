using Statement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageWindow : SourceWindow
{
    [SerializeField] Button _btnPlay;
    [SerializeField] Button _btnReward;

    [SerializeField] Image _mainImage;
    [SerializeField] TextMeshProUGUI _namePlate;

    [UIInject] MenuState _state;

    public override SourceWindow Init(SourcePanel panel)
    {
        _btnPlay.onClick.AddListener(onPlay);
        _btnReward.onClick.AddListener(onReward);

        return base.Init(panel);
    }

    public override void OnOpen(params object[] data)
    {
        var player = PlayerEntity.Instance;

        var stageConfig = ConfigModule.GetConfig<StageConfig>();

        if (stageConfig.TryGetStage(player.CurrentStageKey, out var stage))
        {
            var level = stage.GetLevelByID(player.LevelID);

            _namePlate.text = $"{stage.Number}.{level.Number} {level.Name}";
            _mainImage.sprite = level.Icon;
        }

        base.OnOpen(data);
    }

    void onPlay()
    {
        _state.Play();      
    }

    void onReward()
    {
        _panel.OpenWindow<RewardWindow>();
    }

    public override void Dispose()
    { 
        _btnPlay.onClick.RemoveAllListeners();
        _btnReward.onClick.RemoveAllListeners();
    } 
}
