using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.Common.Enum
{
    /// <summary>
    /// 血型
    /// </summary>
    public enum BloodTypeEnum
    {
        [Description("")]
        Default = 0,
        [Description("A型")]
        A = 1,
        [Description("B型")]
        B = 2,
        [Description("O型")]
        O =3,
        [Description("AB型")]
        AB = 4,
        [Description("其它血型")]
        Other = 5
    }
}
