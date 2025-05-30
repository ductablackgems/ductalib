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
        private string sceneName;
        public bool ignoreStart;
        protected override void Init()
        {
            base.Init();
            if (!ignoreStart)
            {
                content.ShowObject();
                Master.ChangeAlpha(fade, 0);
                fade.ShowObject();
                StartCoroutine(Hide());
            }
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
            asyncLoad.allowSceneActivation = true;
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            if (sceneName != menu)
            {
                AudioController.Instance.StopBGMMenu();
            }
        }
        private IEnumerator Hide()
        {
            yield return new WaitForSeconds(1f);
            yield return fade.DOFade(1, 0.3f).SetUpdate(true).WaitForCompletion();
            content.HideObject();
            fade.HideObject();
        }
    }
}