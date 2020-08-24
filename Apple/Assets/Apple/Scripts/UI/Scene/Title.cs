using Apple.Domain.Model;
using Floppy;
using UnityEngine;
using Apple.Application;

namespace Apple.UI.Scene
{
    public class Title : MonoBehaviour
    {
        private void Awake()
        {
            RedirectIfNoLang();
        }

        private void RedirectIfNoLang()
        {
            Disk.Instance.Load<User>("user");
            if (Disk.Instance.Get<User>("user")?.Language == null)
            {
                GotoSetupLang();
            }
        }

        public void GotoSetupLang()
        {
            CustomUrlAnalyzer.Instance.Analyze("sorami://Language");
        }

        public void TapToStart()
        {
            Disk.Instance.Load<User>("user");
            if (Disk.Instance.Get<User>("user")?.LastUrl == null || Disk.Instance.Get<User>("user")?.LastUrl == "")
            {
                CustomUrlAnalyzer.Instance.Analyze("sorami://Prologue");
                return;
            }
            CustomUrlAnalyzer.Instance.Analyze(Disk.Instance.Get<User>("user").LastUrl);
        }
    }
}