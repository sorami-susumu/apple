using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Apple.UI.Scene
{
    public class Language : MonoBehaviour
    {
        [SerializeField] AudioClip hoverSE;
        [SerializeField] AudioClip clickSE;
        [SerializeField] AudioSource audioSource;
        private bool isSelect;

        public void Hover(BaseEventData e)
        {
            if (isSelect) return;
            audioSource.PlayOneShot(hoverSE);
        }

        public void Select()
        {
            if (isSelect) return;
            audioSource.PlayOneShot(clickSE);
            isSelect = true;
        }

        public void Update()
        {
            if (isSelect && !audioSource.isPlaying)
            {
                SceneManager.LoadScene("Prologue");
            }
        }
    }
}