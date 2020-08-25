using System;
using System.Collections;
using Apple.Application;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        [SerializeField] Text prologueText = null;
        [SerializeField] Text prologueText2 = null;
        [SerializeField] Text prologueTextEn = null;
        [SerializeField] Text prologueTextEn2 = null;
        [SerializeField] GameObject blackBack = null;
        [SerializeField] Text speachText = null;
        [SerializeField] GameObject speachBack = null;
        [SerializeField] AudioClip typewriterSE = null;
        [SerializeField] AudioClip talkSE = null;
        [SerializeField] AudioClip haledSE = null;
        [SerializeField] AudioSource audioSource = null;
        private Vector3 webCamFocusPos;
        // スピード
        private float speed = 0.2f;
        //二点間の距離を入れる
        private float distance;
        private bool isPC = true;
        private bool isFocus;
        private int storyboardCounter = 0;
        private string sentence;
        private bool isPreventClick;

        private void Awake()
        {
            RecognizeDevice();
            webCamFocusPos = webCamTransform.position - new Vector3(0f, 0f, 0f);
            distance = Vector3.Distance(mainCamera.transform.position, webCamFocusPos);
            if (!isPC) {
                mainCamera.fieldOfView = 90;
            }
            FlowPrologueText();
        }

        private void FlowPrologueText()
        {
            blackBack.SetActive(true);
            if (Lang.Instance.isJa())
            {
                prologueText.gameObject.SetActive(true);
                prologueText2.gameObject.SetActive(true);
                StartCoroutine(
                    TypingAsync(
                        prologueText,
                        "2021年某所",
                        0.15f,
                        typewriterSE,
                        () => StartCoroutine(TypingAsync(prologueText2, "配信部屋にて", 0.15f, typewriterSE))
                    )
                );
            }
            else {
                prologueTextEn.gameObject.SetActive(true);
                prologueTextEn2.gameObject.SetActive(true);
                StartCoroutine(
                    TypingAsync(
                        prologueTextEn,
                        "In 2021, somewhere",
                        0.15f,
                        typewriterSE,
                        () => StartCoroutine(TypingAsync(prologueTextEn2, "at stream room", 0.15f, typewriterSE))
                    )
                );
            }
        }

        private void Update()
        {
            if (isFocus) {
                // 現在の位置
                speed += 0.2f;
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
                    CustomUrlAnalyzer.Instance.Analyze("sorami://CoreGame");
                }
            }
        }

        public IEnumerator TypingAsync(Text textField, string sentence, float span, AudioClip audioClip, Action callback = null)
        {

            isPreventClick = true;
            this.sentence = sentence;
            textField.text = string.Empty;
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < sentence.Length; i++)
            {
                textField.text += sentence[i];
                audioSource.PlayOneShot(audioClip);
                yield return new WaitForSeconds(span);
            }
            isPreventClick = false;
            if (callback != null) {
                callback();
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
            if (isPreventClick) {
                return;
            }
            switch (storyboardCounter) {
                case 0:
                    blackBack.SetActive(false);
                    prologueText.gameObject.SetActive(false);
                    prologueText2.gameObject.SetActive(false);
                    prologueTextEn.gameObject.SetActive(false);
                    prologueTextEn2.gameObject.SetActive(false);
                    break;
                case 1:
                    speachBack.SetActive(true);
                    speachText.gameObject.SetActive(true);
                    FlowSpeachText(Lang.Instance.isJa() ? "さて、配信やるかぁ" : "Let's begin to live!", 0.1f);
                    break;
                case 2:
                    FlowSpeachText(Lang.Instance.isJa() ? "・・・あれ？" : "...What?", 0.2f);
                    break;
                case 3:
                    speachBack.SetActive(false);
                    speachText.gameObject.SetActive(false);
                    audioSource.PlayOneShot(haledSE);
                    isFocus = true;
                    break;
            }

            storyboardCounter++;
        }

        private void FlowSpeachText(string talk, float span)
        {
            StartCoroutine(TypingAsync(speachText, talk, span, talkSE));
        }
    }
}