using System.Collections;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace _0.DucTALib.Scripts.Loading
{
    public class LoadingScene : SingletonMono<LoadingScene>
    {
        public string menu;
        public string gameplay;
        public Transform content;
        public Image fade;
        private Coroutine loadingCoroutine;
        
        protected override void Init()
        {
            base.Init();
            transform.SetParent(DDOL.Instance.transform);
        }

        public void LoadMenu()
        {
            LoadScene(menu);
        }

        public void LoadGame()
        {
            LoadScene(gameplay);
        }
        public void LoadScene(string sceneName)
        {
            content.ShowObject();
            Master.ChangeAlpha(fade, 0);
            fade.ShowObject();
            if(loadingCoroutine != null) StopCoroutine(loadingCoroutine);
            loadingCoroutine = StartCoroutine(LoadSceneIE(sceneName));
        }

        private IEnumerator LoadSceneIE(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }


            asyncLoad.allowSceneActivation = true;
            
            yield return null;

            yield return new WaitForSeconds(6f);
            yield return fade.DOFade(1, 0.3f).WaitForCompletion();
            if (sceneName != menu)
            {
                AudioManager.Instance.StopBGMMenu();
            }
            content.HideObject();
            fade.HideObject();
        }
    }
}