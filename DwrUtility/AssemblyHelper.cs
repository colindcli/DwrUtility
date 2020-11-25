using System;
using System.Reflection;

namespace DwrUtility
{
    /// <summary>
    /// 反射类
    /// </summary>
    public class AssemblyHelper
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T">返回对象</typeparam>
        /// <param name="assemblyString">一般是Dll名称(不带后缀名)</param>
        /// <param name="namespaceClass">命名空间.类名</param>
        /// <returns></returns>
        public static T CreateObject<T>(string assemblyString, string namespaceClass)
        {
            return (T)Assembly.Load(assemblyString).CreateInstance(namespaceClass);
        }

        /// <summary>
        /// 调用对象的私有方法
        /// </summary>
        /// <param name="instance">对象</param>
        /// <param name="methodName">方法名</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static object CallObjectMethod<T>(T instance, string methodName, object[] param = null)
        {
            var method = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return method?.Invoke(instance, param);
        }

        /// <summary>
        /// 创建静态类型
        /// </summary>
        /// <param name="assemblyString">一般是Dll名称(不带后缀名)</param>
        /// <param name="namespaceClass">命名空间.类名</param>
        /// <returns></returns>
        public static Type CreateStaticClass(string assemblyString, string namespaceClass)
        {
            return Assembly.Load(assemblyString).GetType(namespaceClass);
        }

        /// <summary>
        /// 调用类静态方法
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="methodName">方法名</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static object CallStaticMethod(Type type, string methodName, object[] param = null)
        {
            return type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)?.Invoke(null, param);
        }
    }
}
