using System.ComponentModel;

namespace Chat.Model.Common.Enum
{
    /// <summary>
    /// 漂流瓶状态（若两个人删除瓶子，则直接物理删除）
    /// </summary>
    public enum BottleStateEnum
    {
        [Description("初始状态")]
        Default = 0,
        [Description("被捡")]
        Received = 1,
        [Description("主人删除")]
        UserDelete=2,
        [Description("被捡到者删除")]
        OrigionDelete = 3,
        [Description("被厌恶扔回大海")]
        Hated=4,
        [Description("被举报")]
        Report=5,
        [Description("主人从对话删除消息")]
        UserDeleteChat = 6,
        [Description("被捡到者从对话删除消息")]
        OrigionDeleteChat = 7,
        [Description("两个人都从对话删除")]
        BothDelete =8

    }
}
