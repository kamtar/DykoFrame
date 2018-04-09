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

        private Dictionary<string, string> variables;

        public DykoText()
        {
            variables = new Dictionary<string, string>();
        }

        public UnityEngine.UI.Text UIText
        {
            get;
            private set;
        }

        void Start()
        {
            UIText = GetComponent<UnityEngine.UI.Text>();

            if (UIText == null)
                throw new BadUsageException("UnityEngine.UI.Text not found");

            parse_text = UIText.text;
        }

        public string this[string key]
        {
            set
            {
                if (variables.ContainsKey(key))
                    variables[key] = value;
                else
                    variables.Add(key, value);

                var tempStr = parse_text;

                foreach (KeyValuePair<string,string> k in variables)
                    tempStr = tempStr.Replace( "%" + k.Key, k.Value);

                UIText.text = tempStr;
            }
        }
    }
}
