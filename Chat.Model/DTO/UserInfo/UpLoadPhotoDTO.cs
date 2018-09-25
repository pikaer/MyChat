using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Chat.Model.DTO.UserInfo
{
    /// <summary>
    /// 上传头像辅助类
    /// </summary>
    public class UpLoadPhotoDTO
    {
        public decimal posX { get; set; }
        public decimal posY { get; set; }
        public decimal Height { get; set; }

        public decimal Width { get; set; }
        public string Path { get; set; }
    }
}
