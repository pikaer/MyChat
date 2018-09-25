namespace Chat.Model.Entity.Provinces
{
    /// <summary>
    /// 区
    /// </summary>
    public class Area
    {
        /// <summary>
        /// 区域Id
        /// </summary>
        public int AreaId{get;set;}
        /// <summary>
        /// 父级Id
        /// </summary>
        public int CityId { get;set;}
        /// <summary>
        /// 名称
        /// </summary>
        public string AreaName { get; set; }
    }
}
