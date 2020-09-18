using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPrompt
{
    [DataContract]
    public class BrandingPolicy
    {
        [DataMember]
        public string BannerColor
        {
            get;
            set;
        }
        /// <summary>
        /// Applies color on ZAPP title bar buttons min/max/close
        /// </summary>
        [DataMember]
        public string ButtonIconColor
        {
            get;
            set;
        }

        [DataMember]
        public string FontsColor
        {
            get;
            set;
        }

        [DataMember]
        public string TitleImage
        {
            get;
            set;
        }

        [DataMember]
        public string TitleImageExt
        {
            get;
            set;
        }

        [DataMember]
        public Boolean isBackgroundImageAvailable
        {
            get;
            set;
        }

        [DataMember]
        public string BackgroundImage
        {
            get;
            set;
        }

        [DataMember]
        public string BackgroundImageExt
        {
            get;
            set;
        }

        [DataMember]
        public string BackgroundColor
        {
            get;
            set;
        }

        [DataMember]
        public string IconImage
        {
            get;
            set;
        }

        [DataMember]
        public string IconImageExt
        {
            get;
            set;
        }

        [DataMember]
        public string DisplaySize
        {
            get;
            set;
        }
    }
}
