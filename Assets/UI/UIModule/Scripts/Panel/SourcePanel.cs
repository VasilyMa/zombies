using DG.Tweening;
using Leopotam.EcsLite;
using Statement;
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public abstract class SourcePanel : MonoBehaviour
{
    public UnityEvent OnOpenEvent;
    public UnityEvent OnCloseEvent;

    public Ease EaseOpen = Ease.InBack;
    public Ease EaseClose = Ease.OutBack;
    public bool IsScaleAnimation = false;
    public float DurationAnimate = 0.5f;
    
    protected List<SourceWindow> _windows;
    protected List<SourceLayout> _layouts;
    protected List<SourceBar> _bars;
    protected List<SourceDisplay> _displays;
    protected ButtonHoldListener[] _btns;

    protected CanvasGroup _canvasGroup;
    protected RectTransform _rectTransform;
    protected SourceCanvas _sourceCanvas;
    protected Sequence _sequenceHide;
    protected Sequence _sequenceShow;
    protected Action[] _callbacks = new Action[0];

    [UIInject] protected BattleState _state;
    [UIInject] protected EcsWorld _world;

    public bool isOpenOnInit;
    public bool isAlwaysOpen;
    [HideInInspector] public bool isOpen;

    public virtual void Init(SourceCanvas canvasParent)
    {
        _sourceCanvas = canvasParent;
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _windows = new List<SourceWindow>();
        _layouts = new List<SourceLayout>();
        _bars = new List<SourceBar>();
        _displays = new List<SourceDisplay>();

        var windows = GetComponentsInChildren<SourceWindow>(true);
        var layouts = GetComponentsInChildren<SourceLayout>(true);
        var bars = GetComponentsInChildren<SourceBar>(true);
        var displays = GetComponentsInChildren<SourceDisplay>(true);

        for (int i = 0; i < windows.Length; i++)
        {
            _windows.Add(windows[i].Init(this));
        }

        for (int i = 0; i < layouts.Length; i++)
        {
            _layouts.Add(layouts[i].Init(this));
        }

        for (int i = 0; i < bars.Length; i++)
        {
            _bars.Add(bars[i].Init(this));
        } 

        for (int i = 0;i < displays.Length; i++)
        {
            _displays.Add(displays[i].Init(this));
        }

        isOpen = false;

        if (isOpenOnInit)
            OnOpen();
        else
            gameObject.SetActive(false);

        _btns = GetComponentsInChildren<ButtonHoldListener>(true);

        for (int i = 0; i < _btns.Length; i++)
        {
            _btns[i].Init();
        } 
    }
    /// <summary>
    /// Invoke when injected all UIInject fields
    /// </summary>
    public virtual void OnInject()
    {

    }

    public virtual T GetDisplay<T>() where T : SourceDisplay
    {
        SourceDisplay returnedDisplay = null;

        foreach (var sourceDisplay in _displays)
        {
            if (sourceDisplay is T display)
            {
                returnedDisplay = display;
                break;
            } 
        } 

        return returnedDisplay as T;
    }

    public virtual T OpenDisplay<T>() where T : SourceDisplay
    {
        SourceDisplay returnedDisplay = null;

        foreach (var sourceDisplay in _displays)
        {
            if (sourceDisplay is T display)
            {
                returnedDisplay = display;
            }
            else
            {
                sourceDisplay.OnClose();
            }
        }

        returnedDisplay.OnOpen();

        return returnedDisplay as T;
    }

    public virtual T CloseDisplay<T>() where T : SourceDisplay
    {
        SourceDisplay returnedDisplay = null;

        foreach (var sourceDisplay in _displays)
        {
            if (sourceDisplay is T display)
            {
                returnedDisplay = display;
                break;
            } 
        }

        returnedDisplay.OnClose();

        return returnedDisplay as T;
    }
    public virtual T OpenWindow<T>() where T : SourceWindow
    {
        SourceWindow returnedWindow = null;

        foreach (var sourceWindow in _windows)
        {
            if (sourceWindow is T panel)
            {
                returnedWindow = panel;
            }
            else
            {
                sourceWindow.OnClose();
            }
        }

        returnedWindow.OnOpen();

        return returnedWindow as T;
    }
    public virtual T CloseWindow<T>() where T : SourceWindow
    {
        SourceWindow returnedWindow = null;

        foreach (var sourceWindow in _windows)
        {
            if (sourceWindow is T panel)
            {
                returnedWindow = panel;
            }
        }

        returnedWindow.OnClose();

        return returnedWindow as T;
    }
    public virtual T GetLayout<T>() where T : SourceLayout
    {
        SourceLayout returnedWindow = null;

        foreach (var sourceWindow in _layouts)
        {
            if (sourceWindow is T panel)
            {
                returnedWindow = panel;
            } 
        } 

        return returnedWindow as T;
    }
    public virtual T OpenLayout<T>() where T : SourceLayout
    {
        SourceLayout returnedLayout = null;

        foreach (var sourceWindow in _layouts)
        {
            if (sourceWindow is T panel)
            {
                returnedLayout = panel;
            }
            else
            {
                sourceWindow.OnClose();
            }
        }

        returnedLayout.OnOpen();

        return returnedLayout as T;
    }
    public virtual T CloseLayout<T>() where T : SourceLayout
    {
        SourceLayout returnedLayout = null;

        foreach (var sourceWindow in _layouts)
        {
            if (sourceWindow is T panel)
            {
                returnedLayout = panel;
            }
        }

        returnedLayout.OnClose();

        return returnedLayout as T;
    }
    public virtual void OnOpen(params Action[] onComplete)
    {
        gameObject.SetActive(true);

        if (isOpen) return;

        Show(onComplete);
    }
    protected virtual void AddCallback(params Action[] additionalCallbacks)
    {
        if (additionalCallbacks == null || additionalCallbacks.Length == 0)
            return;

        var combined = new Action[_callbacks.Length + additionalCallbacks.Length];
        _callbacks.CopyTo(combined, 0);
        additionalCallbacks.CopyTo(combined, _callbacks.Length);

        _callbacks = combined;
    }

    protected virtual void Show(params Action[] onComplete)
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        if(IsScaleAnimation) _rectTransform.localScale = Vector3.one * 1.1f;

        // Анимация появления
        _sequenceShow = DOTween.Sequence();
        _sequenceShow.SetUpdate(UpdateType.Normal, true);
        _sequenceShow.Append(_canvasGroup.DOFade(1f, DurationAnimate));

        if(IsScaleAnimation) _sequenceShow.Join(_rectTransform.DOScale(1f, DurationAnimate).SetEase(EaseOpen));
         
        var combined = new Action[_callbacks.Length + onComplete.Length];
        _callbacks.CopyTo(combined, 0);
        onComplete.CopyTo(combined, _callbacks.Length);

        _sequenceShow.OnComplete(() =>
        {
            foreach (var action in combined)
            {
                action?.Invoke();
            }

            OnOpenEvent?.Invoke();

            isOpen = true;
        });
    }
    public virtual void OnCLose(params Action[] onComplete)
    {
        if (isOpen) Hide(onComplete);
    }
    protected virtual void Hide(params Action[] onComplete)
    {
        // Анимация исчезновения
        _sequenceHide = DOTween.Sequence();
        _sequenceHide.SetUpdate(UpdateType.Normal, true);
        _sequenceHide.Append(_canvasGroup.DOFade(0f, DurationAnimate));

        if (IsScaleAnimation) _sequenceHide.Join(_rectTransform.DOScale(1.1f, DurationAnimate).SetEase(EaseClose));
         
        _sequenceHide.OnComplete(() =>
            {
                if (onComplete.Length > 0)
                {
                    foreach (var action in onComplete)
                    {
                        action?.Invoke();
                    }
                }

                OnCloseEvent?.Invoke();

                // Полностью отключаем объект после анимации
                gameObject.SetActive(false);
                isOpen = false;
            });
    }
    public virtual void OnDipose()
    {
        for (int i = 0; i < _layouts.Count; i++)
        {
            _layouts[i].Dispose();
        }

        for (int i = 0; i < _windows.Count; i++)
        {
            _windows[i].Dispose();
        }

        _sequenceHide?.Kill();
        _sequenceShow?.Kill();
    }
}
