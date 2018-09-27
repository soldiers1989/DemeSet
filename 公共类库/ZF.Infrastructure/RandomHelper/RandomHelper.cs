using System;
using System.Collections;
using System.Text;

namespace ZF.Infrastructure.RandomHelper
{
    /// <summary>
    /// RandomHelper 的摘要说明（随机数的算法）
    /// </summary>
    public class RandomHelper
    {
        public RandomHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 获取最小数
        /// </summary>
        /// <param name="argDigit">几位数字</param>
        /// <returns>返回 1/10/100等等</returns>
        /// <example>
        /// GetMin(2) 返回10（即两位数的最小值）
        /// </example>
        private static string GetMin(int argDigit)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("1");
            for (int i = 0; i < argDigit - 1; i++)
            {
                sb.Append("0");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取随机数（Int类型）
        /// </summary>
        /// <param name="argDigit">需要获取几位随机数</param>
        /// <param name="argSize">多少个随机数</param>
        /// <returns></returns>
        public static Hashtable GetRandom(int argDigit, int argSize)
        {
            Hashtable ht = new Hashtable();
            int min = Convert.ToInt32(GetMin(argDigit));
            int max = min * 10;

            Random random = new Random();
            for (int i = 0; i < argSize; i++)
            {
                long value = random.Next(min, max);     //Random类的Next函数只支持int类型

                //保证Hashtable中的数值是不重复的
                if (!ht.ContainsValue(value))
                {
                    ht.Add(i, value);
                }
                else
                {
                    i--;
                }
            }

            return ht;
        }

        /// <summary>
        /// 获取随机数（线性随机数算法）
        /// </summary>
        /// <param name="argStartValue">从那个数值开始算</param>
        /// <param name="argSize">多少个随机数</param>
        /// <param name="argMaxValue">最大值是不能超过多少（不能为0）</param>
        /// <returns></returns>
        public static Hashtable GetRandom(int argStartValue, int argSize, int argMaxValue)
        {
            if (argSize <= 0 || argMaxValue <= 0) return null;

            Hashtable ht = new Hashtable();

            Random random = new Random();
            for (int i = 0; i < argSize; i++)
            {
                argStartValue = (argStartValue * 29 + 3) % argMaxValue;     //Random类的Next函数只支持int类型

                //保证Hashtable中的数值是不重复的
                if (!ht.ContainsValue(argStartValue))
                {
                    ht.Add(i, argStartValue);
                }
                else
                {
                    i--;
                }
                //防止循环结束仍没有达到想要的数量
                if (i == argSize - 1 && ht.Count < argSize)
                {
                    i = 0;
                }
                if (ht.Count >= argSize)
                {
                    break;
                }
            }

            return ht;
        }
    }
}