
namespace Mysoft.Code
{
    /// <summary>
    /// 版 本 1.2
    /// Copyright (c) 2013-2017 十堰和协商务软件有限公司
    /// 创建人：HYF
    /// 日 期：2017.05.23 13:48
    /// 描 述：当前操作者回话接口
    /// </summary>
    public interface OperatorIProvider
    {
        /// <summary>
        /// 写入登录信息
        /// </summary>
        /// <param name="user">成员信息</param>
        void AddCurrent(Operator user);
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        Operator Current();
        /// <summary>
        /// 删除当前用户
        /// </summary>
        void EmptyCurrent();
        /// <summary>
        /// 是否过期
        /// </summary>
        /// <returns></returns>
        bool IsOverdue();
        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        int IsOnLine();
    }
}
