
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_C_Sharp.Model.Post
{
    public class BodyCommentContent
    {
        #region Attributes
        public string text;
        public string code;
        public string language;
        public string image;
        #endregion

        #region Constructor
        public BodyCommentContent(string text, string code, string language, string image)
        {
            this.text = text;
            this.code = code;
            this.language = language;
            this.image = image;
        }
        #endregion

        #region Serialization for JSON
        public JObject serialize()
        {
            JObject json = new();

            json.Add("text", text);
            json.Add("code", code);
            json.Add("language", language);
            json.Add("image", image);

            return json;
        }
        #endregion
    }
}
