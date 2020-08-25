using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Apple.Application
{
    public class LoadSceneManager : MonoBehaviour
    {
        private Canvas canvas;
        private Slider slider;

        private void Awake()
        {
            canvas = new Canvas();
            slider = gameObject.AddComponent<Slider>();
            slider.transform.parent = canvas.transform;
            DontDestroyOnLoad(this);
        }
        /// <summary>
        /// シーンを読み込む
        /// </summary>
        /// <param name="name"></param>
        public void LoadScene(string name)
        {
            // コルーチンでロード画面を実行
            StartCoroutine(LoadSceneExecute(name));
        }
        IEnumerator LoadSceneExecute(string name)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
            asyncLoad.allowSceneActivation = false;
            // スライダーの値更新とロード画面の表示
            slider.value = 0f;
            canvas.gameObject.SetActive(true);
            while (true)
            {
                yield return null;
                slider.value = asyncLoad.progress;
                if (asyncLoad.progress >= 0.9f)
                {
                    slider.value = 1f;
                    asyncLoad.allowSceneActivation = true;
                    // ロードバーが100%になっても1秒だけ表示維持
                    yield return new WaitForSeconds(1f);
                    break;
                }
            }
            // ロード画面の非表示
            canvas.gameObject.SetActive(false);
        }
    }
}