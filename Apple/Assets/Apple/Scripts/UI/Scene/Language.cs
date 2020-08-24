using UnityEngine;
using UnityEngine.EventSystems;
using Floppy;
using Apple.Domain.Model;
using Apple.Application;

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

        public void Select(string lang)
        {
            if (isSelect) return;
            Disk.Instance.Store("user", new User() { Language = lang });
            Disk.Instance.Save("user");
            audioSource.PlayOneShot(clickSE);
            isSelect = true;
        }

        public void Update()
        {
            if (isSelect && !audioSource.isPlaying)
            {
                CustomUrlAnalyzer.Instance.Analyze("sorami://Title");
            }
        }
    }
}