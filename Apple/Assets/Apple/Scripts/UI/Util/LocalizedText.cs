using Apple.Domain.Model;
using Floppy;
using UnityEngine;
using UnityEngine.UI;

namespace Apple.UI.Util
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField, Multiline(5)]
        private string Japanese;
        [SerializeField, Multiline(5)]
        private string English;
        [SerializeField]
        private Text text;

        private void Awake()
        {
            Disk.Instance.Load<User>("user");
            text.text = Disk.Instance.Get<User>("user").Language == "en" ? English : Japanese;
        }
    }
}
    