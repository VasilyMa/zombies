using UnityEngine;
using UnityEngine.SceneManagement;

namespace Statement
{
    public class MenuState : State
    {
        public static new MenuState Instance
        {
            get
            {
                return (MenuState)State.Instance;
            }
        }
        public override void Awake()
        {
        }
        public override void Start()
        {
            if (UIModule.OpenCanvas<MenuCanvas>(out var menuCanvas))
            {
                if (menuCanvas.TryOpenPanel<MenuPanel>(out var panel))
                { 
                    UIModule.Inject(this);
                }
            }
        }
        public override void Update()
        {
        }
        public override void FixedUpdate()
        {
        }

        public void Play()
        {
            if (UIModule.OpenCanvas<LoadingCanvas>(out var loadingCanvas))
            {
                if (loadingCanvas.TryOpenPanel<LoadingPanel>(out var loadingPanel))
                {
                    var async = SceneManager.LoadSceneAsync(2);

                    loadingPanel.SetLoadingData(async);
                }
            }
        }
    }
}