using System.Collections.Generic;

namespace Chat.Model
{
    /// <summary>
    /// Bootstrap-table响应体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BootstrapTableResponse<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BootstrapTableResponse()
        {
            rows = new List<T>();
            total = rows.Count;
        }
        public BootstrapTableResponse(int count)
        {
            total = count;
        }
        /// <summary>
        /// 总行数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 响应实体
        /// </summary>
        public List<T> rows { get; set; }
    }
}
