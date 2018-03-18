using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DykoFrame
{
    public class DykoText : MonoBehaviour
    {
        private string parse_text;
        private UnityEngine.UI.Text uiText;

        public UnityEngine.UI.Text UIText
        {
            get { return uiText; }
            set { uiText = value; }
        }

        void Start()
        {
            uiText = GetComponent<UnityEngine.UI.Text>();
            parse_text = uiText.text;
        }

        public string this[string key]
        {
            set
            {
                uiText.text = parse_text.Replace( "%" + key, value);
            }
        }
    }
}
