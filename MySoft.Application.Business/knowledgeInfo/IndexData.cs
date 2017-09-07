using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MySoft.Application.Entity;

namespace MySoft.Application.Business
{
    [Serializable]
    public class IndexData : ICloneable
    {
        public JobType JobType { get; set; }
        public KnowledgeInfoEntity Data;

        public Object Clone()
        //return this;                                        //返回同一个对象的引用
        //return this.MemberwiseClone();    //返回一个浅表副本
        //return new CloneClass();                //返回一个深层副本
        {                                                                 //返回一个内存副本
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;

            return formatter.Deserialize(stream);


        }
    }
    /// <summary>
    /// 枚举,操作类型是增加还是删除
    /// </summary>
    public enum JobType
    {
        /// <summary>
        /// 新增
        /// </summary>
        New,
        /// <summary>
        /// 更新
        /// </summary>
        Up,
        /// <summary>
        /// 删除
        /// </summary>
        Remove
    }
}
