using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.CartDtoModule
{
    /// <summary>
    /// 购物车出参
    /// </summary>
   public class CartModelOutput
    {
        /// <summary>
        /// 明细id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string RegisterUserId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 优惠价
        /// </summary>
        public decimal FavourablePrice { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string CourseIamge { get; set; }

        /// <summary>
        /// 0  课程  1套餐课程
        /// </summary>
        public int CourseType { get; set; }

    }
}
