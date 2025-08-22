using TopDown;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoad
{
    public class SceneLoader :Loader
    {
        private JsonSceneLoader _loader;

//        private void OnDestroy()
//        {
//            Unload();
//            UnsubscribeLoaderEvents();
//        }
//        private void OnEnable()
//        {
//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
//#endif
//        }

//        private void OnDisable()
//        {
//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeChanged;
//#endif
//        }
        public override void Load()
        {
            //_loader = new JsonSceneLoader();

            Unload();
            SetSceneLocator();
            SubscribeLoaderEvents();
            _loader.Load();
        }

        public override void Unload()
        {
            if (_loader != null)
                _loader.Unload();
        }

        //public void Init() //Calling from the manager class
        //{
        //    InitializeLoader();
        //}
        //private void InitializeLoader()
        //{
          

        //}
        //private void UnloadPreviousLoader()
        //{
        //    if (_loader != null)
        //        _loader.Unload();
        //}
        private void SetSceneLocator()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            _loader.SettingLocator(sceneName);
        }
        private void SubscribeLoaderEvents()
        {
            _loader.OnProgress += OnProgress;
            _loader.OnSuccess += OnSuccess;
            _loader.OnFailure += OnFailure;
        }
        private void UnsubscribeLoaderEvents()
        {
            _loader.OnProgress -= OnProgress;
            _loader.OnSuccess -= OnSuccess;
            _loader.OnFailure -= OnFailure;
        }
        #region Loader Callbacks

        private void OnProgress(float p)
        {
            Debug.Log($"진행률: {p * 100}%");

        }
        private void OnSuccess() => Debug.Log("성공");
        private void OnFailure() => Debug.Log("실패!");
        #endregion

        #region 에디터 
#if UNITY_EDITOR
        private void OnPlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                Debug.Log("에디터 중단 Unload 호출");
                _loader?.Unload();
            }
        }

     
#endif
        #endregion
    }
}