namespace Chat.Model.Common
{
    /// <summary>
    /// Json返回实体
    /// </summary>
    public class JsonMess
    {
        public JsonMess()
        {
            Success = true;
            Message = "操作成功";
        }
        public JsonMess(bool success, string message, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        /// <summary>
        /// 是否操作成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
