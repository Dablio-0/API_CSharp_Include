using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace API_C_Sharp.Model.User
{
    public class BodyMessage
    {
        #region Attributes
        public string text;
        public string code;
        public string language;
        #endregion

        #region Constructor
        public BodyMessage(string text, string code, string language)
        {
            this.text = text;
            this.code = code;
            this.language = language;
        }
        #endregion

        #region Serialization for JSON
        public JObject serialize()
        {
            JObject json = new();

            json.Add("text", text);
            json.Add("code", code);
            json.Add("language", language);

            return json;
        }
        #endregion
    }
}
