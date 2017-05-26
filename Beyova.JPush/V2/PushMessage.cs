﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Beyova.JPush
{
    /// <summary>
    /// Class PushMessage.
    /// <see cref="PushMessageRequest"/> and <see cref="PushMessage"/> work together to send out push request.
    /// See official RESTful API: http://docs.jpush.cn/display/dev/Push+API+v2
    /// </summary>
    public class PushMessage
    {
        #region Constants

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the customized value.
        /// </summary>
        /// <value>The customized value.</value>
        public Dictionary<string, object> CustomizedValue { get; set; }

        #region iOS only

        /// <summary>
        /// Gets or sets the badge value.
        /// </summary>
        /// <value>The badge value.</value>
        public int BadgeValue { get; set; }

        /// <summary>
        /// Gets or sets the sound.
        /// For iOS only.
        /// </summary>
        /// <value>The sound.</value>
        public string Sound { get; set; }

        #endregion

        #region Android Only

        /// <summary>
        /// Gets or sets the push title.
        /// For Android Only.
        /// </summary>
        /// <value>The push title.</value>
        public string PushTitle { get; set; }

        /// <summary>
        /// Gets or sets the builder unique identifier.
        /// For Android Only.
        /// Default is 0. Valid value is 1-1000
        /// </summary>
        /// <value>The builder unique identifier.</value>
        public int BuilderId { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// To json.
        /// iOS Push Message example:
        /// <example>
        /// {"n_content":"通知内容", "n_extras":{"ios":{"badge":88, "sound":"happy"}, "user_param_1":"value1", "user_param_2":"value2"}}
        /// </example>
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns>System.String.</returns>
        public string ToJson(PushPlatform platform = PushPlatform.AndroidAndiOS)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            Dictionary<string, object> extra = new Dictionary<string, object>();

            result.Merge("n_content", this.Content.SafeToString());

            if (this.CustomizedValue != null)
            {
                foreach (var key in this.CustomizedValue.Keys)
                {
                    extra.Merge(key, this.CustomizedValue[key]);
                }
            }

            if (platform.HasFlag(PushPlatform.iOS))
            {
                Dictionary<string, object> iOSDictionary = new Dictionary<string, object>();

                iOSDictionary.Merge("badge", BadgeValue);

                if (!string.IsNullOrWhiteSpace(this.Sound))
                {
                    CustomizedValue.Merge("sound", this.Sound);
                }

                extra.Merge("ios", iOSDictionary);
            }

            if (platform.HasFlag(PushPlatform.Android))
            {
                if (!string.IsNullOrWhiteSpace(this.PushTitle))
                {
                    result.Merge("n_title", this.PushTitle);
                }

                if (this.BuilderId > 0 && this.BuilderId <= 1000)
                {
                    result.Merge("n_builder_id", this.BuilderId.ToString());
                }
            }

            result.Add("n_extras", extra);

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
