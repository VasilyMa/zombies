using Leopotam.EcsLite;
using Statement;
using TMPro;
using UnityEngine;

public class WeaponSlot : SourceSlot
{ 
    private WeaponBase _data;
    [SerializeField] private TextMeshProUGUI _title;
    [UIInject] protected EcsWorld _world;
    [UIInject] protected BattleState _state;
    public override void OnActive()
    { 
        
    }

    public override void OnClick()
    {
        Debug.Log("Click!");

        _data.Init(_world, _state);

        if (UIModule.TryGetCanvas<BattleCanvas>(out var battleCanvas))
        {
            if (battleCanvas.TryGetPanel<BattlePanel>(out var battlePanel))
            {
                battlePanel.CloseWindow<StartWeaponWindow>();
            }
        }
    }

    public override void UpdateView(object d)
    {
        if (d is WeaponBase data)
        {
            _data = data;
            _title.text = _data.Name;
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
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
}
