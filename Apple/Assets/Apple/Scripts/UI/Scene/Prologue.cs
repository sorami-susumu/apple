using UnityEngine;
using UnityEngine.EventSystems;

namespace Apple.UI.Scene
{
    public class Prologue : MonoBehaviour
    {
        [SerializeField] Camera mainCamera = null;
        [SerializeField] Skybox skybox = null;
        [SerializeField] GameObject directionalLight;
        [SerializeField] GameObject webCam;
        [SerializeField] GameObject monitor;
        [SerializeField] Transform webCamTransform = null;
        private Vector3 webCamFocusPos;
        // スピード
        public float speed = 0.1f;
        //二点間の距離を入れる
        private float distance;
        private bool isPC = true;
        private bool isFocus;

        private void Awake()
        {
            RecognizeDevice();
            webCamFocusPos = webCamTransform.position - new Vector3(0f, 0f, 0f);
            distance = Vector3.Distance(mainCamera.transform.position, webCamFocusPos);
            if (!isPC) {
                mainCamera.fieldOfView = 90;
            }
        }

        private void Update()
        {
            if (isFocus) {
                // 現在の位置
                speed += 0.1f;
                float presentLocation = (Time.deltaTime * speed) / distance;
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, webCamFocusPos, presentLocation);
                mainCamera.transform.Rotate(0, 0, -10);
                float focusPos = Vector3.Distance(webCamFocusPos, mainCamera.transform.position);
                if (focusPos < 0.2f) {
                    isFocus = false;
                    directionalLight.SetActive(false);
                    skybox.material.color = Color.black;
                    webCam.SetActive(false);
                    monitor.SetActive(false);
                }
            }
        }

        private void RecognizeDevice()
        {
#if UNITY_EDITOR
            isPC = Screen.width > Screen.height;
#elif UNITY_IOS || UNITY_ANDROID
            isPC = false;
#endif
        }

        public void Next(BaseEventData e)
        {
            isFocus = true;
        }
    }
}