using System;
using System.Linq.Expressions;

namespace DwrUtility.Expressions
{
    /// <summary>
    /// 表达式
    /// </summary>
    public static class ExpressionExt
    {
        /// <summary>
        /// Or 运算
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            return ExpressionHelper.Or(expr1, expr2);
        }

        /// <summary>
        /// And 运算
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            return ExpressionHelper.And(expr1, expr2);
        }
    }
}
