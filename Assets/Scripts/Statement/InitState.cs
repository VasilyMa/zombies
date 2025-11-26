using UnityEngine;
using UnityEngine.SceneManagement;

namespace Statement
{
    public class InitState : State
    {
        [SerializeField] private StageBase startStage;

        public static new InitState Instance
        {
            get
            {
                return (InitState)State.Instance;
            }
        } 
        public override void Awake()
        {
        }
        public override void Start()
        {
            EntityModule.Initialize();

            UIModule.Initialize();

            ConfigModule.Initialize(this, onConfigLoaded); 
        }
        public override void Update()
        {

        }
        public override void FixedUpdate()
        {

        }

        void onConfigLoaded()
        {
            if (startStage != null)
            {
                PlayerEntity.Instance.CurrentStageKey = startStage.KeyName;
                PlayerEntity.Instance.LevelID = 0;
            }

            SceneManager.LoadScene(1); 
        }  
    }
}