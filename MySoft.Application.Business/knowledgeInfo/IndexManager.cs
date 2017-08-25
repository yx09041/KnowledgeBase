using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Store;
using System.IO;
using Lucene.Net.Documents;
using System.Threading;
using Lucene.Net.Analysis.PanGu;
using System.Text.RegularExpressions;
using System.Web;
using System.Reflection;
using System.Net;
using MySoft.Application.Entity;

namespace MySoft.Application.Business
{
    /// <summary>
    /// 索引维护类 
    /// </summary>
    public class IndexManager
    {
        private static ILog logger = LogManager.GetLogger(typeof(IndexManager));
        private static Thread threadIndexUpData = null;
        private static Thread threadBaseEntity = null;
        static string __SearchAppIndexPath = HttpContext.Current.Server.MapPath("/Content/IndexData");
        #region 创建单例
        // 定义一个静态变量来保存类的实例
        private static IndexManager uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();


        // 定义私有构造函数，使外界不能创建该类实例
        private IndexManager()
        { }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static IndexManager GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            lock (locker)
            {
                // 如果类的实例不存在则创建，否则直接返回
                if (uniqueInstance == null)
                {
                    uniqueInstance = new IndexManager();
                }
            }

            return uniqueInstance;
        }
        #endregion

        //任务队列,保存生产出来的任务和更新使用,不使用list避免移除时数据混乱问题
        private Queue<IndexData> jobs = new Queue<IndexData>();
        //搜索记录
        private Queue<BaseEntity2> listBaseEntity = new Queue<BaseEntity2>();

        #region 服务端索引库维护
        /// <summary>
        /// 启动维护索引线程,需加入到程序启动时
        /// </summary>
        public void StartUpDataIndex()
        {

            log4net.Config.XmlConfigurator.Configure();
            threadIndexUpData = new Thread(DataIndexOn);
            threadIndexUpData.Name = "threadUpDataIndex";
            threadIndexUpData.IsBackground = true;
            threadIndexUpData.Start();
        }

        /// <summary>
        /// 数据索引任务线程
        /// </summary>
        private void DataIndexOn()
        {
            //logger.Debug("索引任务线程启动");
            while (true)
            {
                if (jobs.Count <= 0)
                {
                    Thread.Sleep(5 * 1000);
                    continue;
                }
                ProcessJobs();
                //logger.Debug("全部索引完毕");
            }
        }
        /// <summary>
        /// 添加数据更新索引任务
        /// </summary>
        /// <param name="job"></param>
        public void AddJobs(IndexData job)
        {
            if (threadIndexUpData == null)
            {
                StartUpDataIndex();
            }
            string strLog = string.Format(@"
                            添加任务 - 数据：{0}，类型：{1}。", job.Data.knowledgeGUID.ToString(), job.JobType.ToString());
            //logger.Debug(strLog);
            jobs.Enqueue(job);
        }
        /// <summary>
        /// 处理实际数据
        /// </summary>
        /// <param name="writer"></param>
        private void ProcessJobs()
        {
            Regex r = new Regex("<img.[^>]+[>]+", RegexOptions.ExplicitCapture);
            KnowledgeInfoEntity dataManager = new KnowledgeInfoEntity();
            while (jobs.Count != 0)
            {
                IndexData job = jobs.Dequeue();
                string IndexDic = __SearchAppIndexPath;
                //创建索引目录
                if (!System.IO.Directory.Exists(IndexDic))
                {
                    System.IO.Directory.CreateDirectory(IndexDic);
                }
                FSDirectory directory = FSDirectory.Open(new DirectoryInfo(IndexDic), new NativeFSLockFactory());
                bool isUpdate = IndexReader.IndexExists(directory);
                //logger.Debug("索引库存在状态" + isUpdate);
                if (isUpdate)
                {
                    //如果索引目录被锁定（比如索引过程中程序异常退出），则首先解锁
                    if (IndexWriter.IsLocked(directory))
                    {
                        //logger.Debug("开始解锁索引库");
                        IndexWriter.Unlock(directory);
                        //logger.Debug("解锁索引库完成");
                    }
                }
                IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);


                writer.DeleteDocuments(new Term("knowledgeGUID", job.Data.knowledgeGUID.ToString()));
                //如果是“添加任务”再添加，
                if (job.JobType == JobType.Up)
                {
                    KnowledgeInfoEntity data = job.Data;
                    /*去除为NULL数据*/
                    if (string.IsNullOrEmpty(data.Title))
                    {
                        data.Title = string.Empty;
                    }
                    if (string.IsNullOrEmpty(data.ContentNoHtml))
                    {
                        data.ContentNoHtml = string.Empty;
                    }
                    if (string.IsNullOrEmpty(data.Summary))
                    {
                        data.Summary = string.Empty;
                    }
                    if (string.IsNullOrEmpty(data.CreateBy))
                    {
                        data.CreateBy = string.Empty;
                    }

                    Document document = new Document();
                    document.Add(new Field("KnowledgeGUID", data.knowledgeGUID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("Title", data.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("ContentNoHtml", r.Replace(data.ContentNoHtml, ""), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("Summary", data.Summary, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("CreateBy", data.CreateBy, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    writer.AddDocument(document);
                    //logger.Debug("索引" + job.Data.DataGUID.ToString() + "完毕");
                }
                writer.Close();
                directory.Close();//不要忘了Close，否则索引结果搜不到
            }
        }
        #endregion

        #region 客户端搜索监控
        /// <summary>
        /// 启动维护搜索记录线程.
        /// </summary>
        public void StartThreadBaseEntity()
        {

            log4net.Config.XmlConfigurator.Configure();
            threadBaseEntity = new Thread(UpBaseEntity);
            threadBaseEntity.Name = "threadBaseEntity";
            threadBaseEntity.IsBackground = true;
            threadBaseEntity.Start();
        }
        /// <summary>
        /// 添加数据更新索引任务
        /// </summary>
        /// <param name="job"></param>
        public void AddBaseEntity(BaseEntity2 item)
        {
            if (threadBaseEntity == null)
            {
                StartThreadBaseEntity();
            }

            listBaseEntity.Enqueue(item);
        }
        /// <summary>
        /// 索引任务线程
        /// </summary>
        private void UpBaseEntity()
        {
            //logger.Debug("索引任务线程启动");
            while (true)
            {
                if (listBaseEntity.Count <= 0)
                {
                    Thread.Sleep(5 * 1000);
                    continue;
                }
                while (listBaseEntity.Count != 0)
                {
                    BaseEntity2 item = listBaseEntity.Dequeue();
                    switch (item.JobType)
                    {
                        case JobType.New:
                            //if (item.SourceName == "LI_SearchHistory")
                            //{
                            //    Zsk_SearchHistory search = (Zsk_SearchHistory)item.BaseEntity;
                            //    search.ClientName = Dns.GetHostEntry(search.ClientIP).HostName;
                            //}
                            //else if (item.SourceName == "LI_DwonFileLog")
                            //{
                            //    Zsk_DwonFileLog search = (Zsk_DwonFileLog)item.BaseEntity;
                            //    search.ClientName = Dns.GetHostEntry(search.ClientIP).HostName;
                            //}
                            //item.BaseEntity.Insert();
                            break;
                        case JobType.Remove:
                            //item.BaseEntity.Delete();
                            break;
                        case JobType.Up:
                            //item.BaseEntity.Update();
                            break;
                    }
                }
                //logger.Debug("全部索引完毕");
            }
        } 
        #endregion

        #region 索引创建方法

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CreateIndex()
        {
            StringBuilder sbRtn = new StringBuilder();
            knowledgeApp   _knowledgeApp = new knowledgeApp();
            string strIndexPath = string.Empty;
            try
            {
                strIndexPath = __SearchAppIndexPath;
                IList<KnowledgeInfoEntity> dataList = _knowledgeApp.GetAllList();
                bool IsCreateIndex = IndexManager.GetInstance().CreateIndex(dataList, strIndexPath);
                if (IsCreateIndex)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                sbRtn.Append(e.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// 创建知识库所有数据的索引库
        /// </summary>
        /// <returns></returns>
        public bool CreateIndex(IList<KnowledgeInfoEntity> listDatas, string strIndexPath)
        {
            Regex r = new Regex("<img.[^>]+[>]+", RegexOptions.ExplicitCapture);
            bool isRtn = true;
            IndexWriter write = null;
            FSDirectory directory = null;

            try
            {
                //创建索引目录
                if (!System.IO.Directory.Exists(strIndexPath))
                {
                    System.IO.Directory.CreateDirectory(strIndexPath);
                }

                directory = FSDirectory.Open(new DirectoryInfo(strIndexPath), new NativeFSLockFactory());

                bool isUpdate = IndexReader.IndexExists(directory);
                if (isUpdate)
                {
                    if (IndexWriter.IsLocked(directory))
                    {
                        IndexWriter.Unlock(directory);
                    }
                }
                write = new IndexWriter(directory, new PanGuAnalyzer(), true, IndexWriter.MaxFieldLength.UNLIMITED);

                foreach (KnowledgeInfoEntity data in listDatas)
                {

                    write.DeleteDocuments(new Term("knowledgeGUID", data.knowledgeGUID.ToString()));

                    Document document = new Document();
                    document.Add(new Field("knowledgeGUID", data.knowledgeGUID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("Title", data.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("ContentNoHtml", r.Replace(data.ContentNoHtml, ""), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("Summary", data.Summary, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("CreateBy", data.CreateBy, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("UpdateDate", data.UpdateDate.Value.ToString("yyyy-MM-dd"), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("ViewCount", data.ViewCount.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("DataType", data.DataType, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    write.AddDocument(document);
                    //logger.Debug("索引" + data.DataGUID.ToString() + "完毕");

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //write.Optimize();
                if (write != null)
                {
                    write.Close();
                }
                if (directory != null)
                {
                    directory.Close();
                }
                //logger.Debug("全部索引完毕");

            }

            return isRtn;
        } 
        #endregion
    }
}
