using Chat.Ultilities.Extensions;
namespace Chat.Model.Common.Helper
{
    /// <summary>
    /// 业务操作结果信息类，对操作结果进行封装
    /// </summary>
    public class OperationResult<T>
    {
        #region 构造函数

        public OperationResult()
        {
            ResultType = OperationResultType.Success;
            Message = "操作成功";
            Content = default(T);
        }
        /// <summary>
        /// 初始化一个 业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="resultType">业务操作结果类型</param>
        public OperationResult(OperationResultType resultType)
        {
            ResultType = resultType;
            Content = default(T);
        }

        /// <summary>
        ///初始化一个 定义返回消息的业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="resultType">业务操作结果类型</param>
        /// <param name="message">业务返回消息</param>
        public OperationResult(OperationResultType resultType, string message)
        {
            ResultType = resultType;
            Message = message;
            Content = default(T);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 动态实体
        /// </summary>
        public T Content { get; set; }
        /// <summary>
        /// 获取或设置 操作结果类型
        /// </summary>
        public OperationResultType ResultType { get; set; }

        /// <summary>
        /// 获取或设置 操作返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 得到Json格式的回调值
        /// </summary>
        public JsonMess JsonMess()
        {
            bool bSuccess = ResultType == OperationResultType.Success || ResultType == OperationResultType.Warning;
            string mess = string.IsNullOrEmpty(Message) ? ResultType.ToDescription() : Message;
            return new JsonMess(bSuccess, mess, Content);
        }
        #endregion
    }
}
