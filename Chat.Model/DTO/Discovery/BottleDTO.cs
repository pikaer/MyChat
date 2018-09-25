using Chat.Model.Common.Enum;
using Chat.Model.Entity.Discovery;

namespace Chat.Model.DTO.Discovery
{
    public class BottleDTO : Bottle
    {
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        
        /// <summary>
        /// 所在城市
        /// </summary>
        public string HomeCityName { get; set; }
        
    }
}
