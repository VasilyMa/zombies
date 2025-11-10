using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Statement
{
    public abstract class State : MonoBehaviour
    {
        protected static State _instance;
        public static State Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<State>();
                }
                return _instance;
            }
        } 

        public virtual void Awake()
        {

        }
        public abstract void Start();
        public abstract void Update();
        public abstract void FixedUpdate();
        public virtual void OnDestroy()
        { 
        }

        public virtual Coroutine RunCoroutine(IEnumerator coroutine, Action callback = null)
        {
            if (Instance != null)
            {
                return StartCoroutine(Instance.CoroutineWrapper(coroutine, callback));
            }
            else
            {
                return null;
            }
        }
        public virtual Coroutine RunCoroutine(IEnumerator coroutine, params Action[] callback)
        {
            if (Instance != null)
            {
                return StartCoroutine(Instance.CoroutineWrapper(coroutine, callback));
            }
            else
            {
                return null;
            }
        }
        protected virtual IEnumerator CoroutineWrapper(IEnumerator coroutine, Action callback = null)
        {
            yield return StartCoroutine(coroutine);

            callback?.Invoke();
        }
        protected virtual IEnumerator CoroutineWrapper(IEnumerator coroutine, params Action[] callback)
        {
            yield return StartCoroutine(coroutine);

            for (int i = 0; i < callback.Length; i++)
            {
                callback[i]?.Invoke();
            }
        } 
        public void RequestLoadingScene(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}