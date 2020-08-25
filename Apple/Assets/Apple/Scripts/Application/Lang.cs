using System.Collections;
using System.Collections.Generic;
using Apple.Domain.Model;
using Floppy;
using UnityEngine;

namespace Apple.Application
{
    public class Lang
    {
        private static Lang instance;
        public static Lang Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Lang();
                }
                return instance;
            }
        }

        private Lang()
        {
            Disk.Instance.Load<User>("user");
        }

        public bool isJa()
        {
            return Disk.Instance.Get<User>("user").Language == "ja";
        }
    }
}