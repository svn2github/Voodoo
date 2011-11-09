using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Text.RegularExpressions;

namespace Voodoo
{
    /// <summary>
    /// 反射相关类
    /// </summary>
    public class Reflection
    {
        /// <summary>
        /// 执行某个方法
        /// </summary>
        /// <param name="className">类，包括命名空间</param>
        /// <param name="methodName">方法名</param>
        /// <param name="objParas">参数</param>
        /// <returns></returns>
        public static object ExecMethod(string className, string methodName, object[] objParas)
        {
            Type t = Type.GetType(className);
            /*实例化这个类*/
            ConstructorInfo constructor = t.GetConstructor(new Type[0]);//将得到的类型传给一个新建的构造器类型变量
            object obj = constructor.Invoke(new object[0]);//使用构造器对象来创建对象
            /*执行Insert方法*/
            MethodInfo m = t.GetMethod(methodName);
            return m.Invoke(obj, objParas);
        }
    }
}
