using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.Common.Request
{
    /// <summary>
    /// 上传头像请求类
    /// </summary>
    public class UpLoadPhotoRequest
    {
        /// <summary>
        /// 图片裁剪位置X横坐标
        /// </summary>
        public decimal x { get; set; }
        /// <summary>
        /// 图片裁剪位置Y坐标
        /// </summary>
        public decimal y { get; set; }
        /// <summary>
        /// 裁剪高度
        /// </summary>
        public decimal height { get; set; }
        /// <summary>
        /// 裁剪宽度
        /// </summary>
        public decimal width { get; set; }
        /// <summary>
        /// 旋转角度
        /// </summary>
        public int rotate { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Path { get; set; }
    }
}
