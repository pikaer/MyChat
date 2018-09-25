using Chat.Model;
using Chat.Model.Common.Helper;
using Chat.Model.DTO.Discovery;
using Chat.Model.Entity.Discovery;
using System.Collections.Generic;

namespace Chat.Service.IService
{
    /// <summary>
    /// 漂流瓶接口
    /// </summary>
    public interface IBottleService
    {
        /// <summary>
        /// 扔一个瓶子
        /// </summary>
        OperationResult<bool> ThrowOneBottle(Bottle request);

        /// <summary>
        /// 捡一个瓶子
        /// </summary>
        OperationResult<BottleDTO> NewBottle(long userId);

        /// <summary>
        /// 我捡过的瓶子列表
        /// </summary>
        OperationResult<List<BottleListDTO>> AllBottleList(long userId);

        /// <summary>
        /// 某一个瓶子回复列表
        /// </summary>
        OperationResult<List<MyBottleDTO>> ReplyBottleList(CommonRequest request);

        /// <summary>
        /// 回复漂流瓶
        /// </summary>
        OperationResult<bool> ReplyBottle(CommonRequest request);

        /// <summary>
        /// 我的回复过的和我扔出去且被回复的瓶子列表
        /// </summary>
        OperationResult<List<MyBottleDTO>> AllReplyBottleList(long userId);

        /// <summary>
        /// 删除或者举报瓶子(一级列表）
        /// </summary>
        OperationResult<bool> UpdateBottle(CommonRequest request);

        /// <summary>
        /// 删除我的瓶子（二级列表）
        /// </summary>
        OperationResult<bool> DeleteBottleReply(CommonRequest request);

        /// <summary>
        /// 删除漂流瓶对话（三级列表）
        /// </summary>
        OperationResult<bool> DeleteBottleChat(CommonRequest request);

        /// <summary>
        /// 某一漂流瓶详情
        /// </summary>
        Bottle GetBottle(long bottleId);
    }
}
