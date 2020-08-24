using System;
using UnityEngine.SceneManagement;

namespace Apple.Application
{
    public class CustomUrlAnalyzer
    {
        private static CustomUrlAnalyzer instance;
        public static CustomUrlAnalyzer Instance {
            get {
                if (instance == null) {
                    instance = new CustomUrlAnalyzer();
                }
                return instance;
            }
        }

        public string Query;

        public void Analyze(string url)
        {
            var protocol = url.Substring(0, 9);
            
            if (protocol  != "sorami://") {
                throw new Exception("invalid url schema");
            }

            var rest = url.Substring(9);
            if (rest.Contains("?"))
            {
                string[] rests = rest.Split('?');
                Query = rests[1];
                SceneManager.LoadScene(rests[0]);
            }
            else {
                SceneManager.LoadScene(rest);
            }
        }
    }
}
