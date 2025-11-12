using Leopotam.EcsLite;
using Statement;
using System;
using System.Collections.Generic; 
using UnityEngine;

public abstract class SourceCanvas : MonoBehaviour
{
    protected Canvas _canvas;
    protected List<SourcePanel> _panels;
    [UIInject] protected BattleState _state;
    [UIInject] protected EcsWorld _world;
    public bool isInited { get; protected set; }

    public bool IsOpenOnStart;

    public virtual void Init()
    {
        _canvas = GetComponent<Canvas>();

        _panels = new List<SourcePanel>();

        var panels = GetComponentsInChildren<SourcePanel>(true);

        foreach (var panel in panels) _panels.Add(panel);

        _panels.ForEach(panel => panel.Init(this));

        _canvas.enabled = IsOpenOnStart;

        foreach (var panel in _panels)
        {
            if (panel.isOpenOnInit) panel.OnOpen();
            else panel.OnCLose();
        }

        isInited = true;

        gameObject.SetActive(true);
    }

    public virtual void OnInject()
    {

    }

    public virtual void InvokeCanvas()
    {
        _canvas.enabled = true;
    }
    public virtual void CloseCanvas()
    {
        _canvas.enabled = false;

        _panels.ForEach(panel => panel.OnCLose());
    }
    public virtual void Dispose()
    {
        _panels.ForEach(panel => panel.OnDipose()); 
    }

    public virtual T ClosePanel<T>() where T : SourcePanel
    {
        SourcePanel returnedPanel = null;

        foreach (var sourcePanel in _panels)
        {
            if (sourcePanel.isAlwaysOpen)
            {
                sourcePanel.OnOpen();
                continue;
            }

            if (sourcePanel is T panel)
            {
                returnedPanel = panel;
            }
        }

        returnedPanel.OnCLose();

        return returnedPanel as T;
    }
    public virtual T OpenPanel<T>(params Action[] callback) where T : SourcePanel
    {
        SourcePanel returnedPanel = null;

        // Сначала ищем нужную панель
        foreach (var sourcePanel in _panels)
        {
            if (sourcePanel.isAlwaysOpen)
            {
                sourcePanel.OnOpen(callback);
                continue;
            }

            if (sourcePanel is T panel)
            {
                returnedPanel = panel;
            }
        }

        // Если нашли — закрываем остальные и открываем нужную
        if (returnedPanel != null)
        {
            foreach (var sourcePanel in _panels)
            {
                if (sourcePanel != returnedPanel && !sourcePanel.isAlwaysOpen)
                {
                    sourcePanel.OnCLose();
                }
            }

            if (!returnedPanel.isOpen)
                returnedPanel.OnOpen(callback);
        }

        return returnedPanel as T;
    }


    public virtual bool TryOpenPanel<T>(out T returnedPanel, params Action[] callback) where T : SourcePanel
    {
        returnedPanel = null;

        // Сначала ищем нужную панель
        foreach (var sourcePanel in _panels)
        {
            if (sourcePanel.isAlwaysOpen)
            {
                sourcePanel.OnOpen(callback);
                continue;
            }

            if (sourcePanel is T panel)
            {
                returnedPanel = panel;
            }
        }

        // Если нашли — закрываем остальные и открываем нужную
        if (returnedPanel != null)
        {
            foreach (var sourcePanel in _panels)
            {
                if (sourcePanel != returnedPanel && !sourcePanel.isAlwaysOpen)
                {
                    sourcePanel.OnCLose();
                }
            }

            if (!returnedPanel.isOpen)
                returnedPanel.OnOpen(callback);

            return true;
        }

        return false;
    }

    public virtual T GetPanel<T>() where T : SourcePanel
    {
        SourcePanel returnedPanel = null;

        foreach (var sourcePanel in _panels)
        {
            if (sourcePanel is T panel)
            {
                returnedPanel = panel;
            }
        }

        return returnedPanel as T;
    }

    public virtual bool TryGetPanel<T>(out T returnedPanel) where T : SourcePanel
    {
        returnedPanel = null;

        foreach (var sourcePanel in _panels)
        {
            if (sourcePanel is T panel)
            {
                returnedPanel = panel;
                break;
            }
        }

        return returnedPanel != null;
    }
}
