namespace Chat.Model.Entity.Provinces
{
    public class City
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 所在省Id
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 城市名
        /// </summary>
        public string CityName { get; set; }
    }
}
