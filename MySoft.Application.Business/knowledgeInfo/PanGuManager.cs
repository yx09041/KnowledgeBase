using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Lucene.Net.Store;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using log4net;
using PanGu;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using MySoft.Application.Entity;

namespace MySoft.Application.Business
{
    public class PanGuManager
    {
        private ILog logger = LogManager.GetLogger(typeof(PanGuManager));
       static string __SearchAppIndexPath = HttpContext.Current.Server.MapPath("/Content/IndexData");
       static int __SearchAppResultMaxLength = 200;
       static int __SearchAppGaoLiangMaxLength = 200;



        /// <summary>
        /// 默认检索方式,按关键字,标题,内容检索,后期考虑做成配置
        /// </summary>
        /// <param name="SearchParam"></param>
        /// <returns></returns>
       public List<KnowledgeInfoEntity> ShowDatasByTAndC(SearchParam SearchParam)
       {
           string[] JS_Fields = new string[] {"Title","Summary","ContentNoHtml" };
           return ShowDatas(SearchParam, JS_Fields);
       }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="startRowIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<KnowledgeInfoEntity> ShowDatas(SearchParam SearchParam, string[] JS_Fields)
        {
            List<KnowledgeInfoEntity> listData = new List<KnowledgeInfoEntity>();

            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(__SearchAppIndexPath), new NoLockFactory());

            IndexReader reader = IndexReader.Open(directory, true);
            //IndexSearcher是进行搜索的类
            IndexSearcher searcher = new IndexSearcher(reader);
            BooleanQuery bQuery = new BooleanQuery();


            //被""包裹,不进行分词处理
            if (SearchParam.KeyValue.Length > 2
                && SearchParam.KeyValue.IndexOf("\"") == 0
                && SearchParam.KeyValue.LastIndexOf("\"") == SearchParam.KeyValue.Length - 1)
            {
                SearchParam.KeyValue = SearchParam.KeyValue.Substring(1, SearchParam.KeyValue.Length - 2);
            }
            else
            {
                SearchParam.KeyValue = ReplaceChar(SearchParam.KeyValue);
                ////分词
                SearchParam.KeyValue = GetKeyWordsSplitBySpace(SearchParam.KeyValue);
            }
            if (string.IsNullOrEmpty(SearchParam.KeyValue))
            {
                return listData;
            }

            QueryParser parse;
            Query query;
            foreach (string strField in JS_Fields)
            {
                //创建标题索引搜索
                parse = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, strField, new PanGuAnalyzer());
                query = parse.Parse(SearchParam.KeyValue);
                parse.SetDefaultOperator(QueryParser.Operator.AND);
                bQuery.Add(query, BooleanClause.Occur.SHOULD);
            }

            TopScoreDocCollector collector = TopScoreDocCollector.create(__SearchAppResultMaxLength, true);
            //Sort sort = new Sort(new SortField("Addtime", SortField.DOC, true));
            Filter filter = null;
            if (!string.IsNullOrEmpty(SearchParam.DataType))
            {
                filter = new TermRangeFilter("DataType", SearchParam.DataType, SearchParam.DataType, true, true);
            }
            searcher.Search(bQuery, filter, collector);


            SearchParam.TotalCount = collector.GetTotalHits();//返回总条数
            //分页,下标从0开始，0是第一条记录
            ScoreDoc[] docs = collector.TopDocs((SearchParam.PageIndex - 1) * SearchParam.PageSize, SearchParam.PageSize).scoreDocs;


            for (int i = 0; i < docs.Length; i++)
            {
                //取文档的编号，这个是主键，lucene.net分配
                int docID = docs[i].doc;
                //检索结果中只有文档的id，如果要取Document，则需要Doc再去取
                //降低内容占用
                Document doc = searcher.Doc(docID);

                string content= doc.Get("ContentNoHtml");
                if (content.Length > 200)
                {
                    content = content.Substring(0, 199) + "……";
                }

                KnowledgeInfoEntity itemData = new KnowledgeInfoEntity()
                {
                    knowledgeGUID = new Guid(doc.Get("knowledgeGUID")).ToString(),
                    Title = doc.Get("Title"),
                    Summary = doc.Get("Summary"),
                    ContentNoHtml = content,
                    ContentHtml = doc.Get("ContentHtml"),
                    CreateBy = doc.Get("CreateBy"),
                    UpdateDate = DateTime.Parse(doc.Get("UpdateDate")),
                    ViewCount = int.Parse(doc.Get("ViewCount")),
                    DataType = doc.Get("DataType"),
                };
                //高亮
                itemData.Title = Preview(itemData.Title, SearchParam.KeyValue);
                itemData.ContentNoHtml = Preview(itemData.ContentNoHtml, SearchParam.KeyValue);
                itemData.Summary = Preview(itemData.Summary, SearchParam.KeyValue);
                listData.Add(itemData);

            }
            return listData;
        }
        public string ReplaceChar(string strKeyWord)
        {
            strKeyWord = strKeyWord.Replace(@"\", @"\\");
            string strFilter = ":*?~!@^-+'\"\\{}[]()";
            char[] arrFilterChar = strFilter.ToCharArray();
            foreach (char c in arrFilterChar)
            {
                //strKeyWord = strKeyWord.Replace("" + c, @"\" + c);
                strKeyWord = strKeyWord.Replace("" + c, @" ");
            }
            return strKeyWord;
        }
        /// <summary>
        /// 设置高亮
        /// </summary>
        /// <param name="body"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private string Preview(string body, string keyword)
        {
            int FragmentSize = __SearchAppGaoLiangMaxLength;
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter = new PanGu.HighLight.SimpleHTMLFormatter("<em class='DataKey'>", "</em>");
            PanGu.HighLight.Highlighter highlighter = new PanGu.HighLight.Highlighter(simpleHTMLFormatter, new Segment());
            highlighter.FragmentSize = FragmentSize;
            string bodyPreview = highlighter.GetBestFragment(keyword, body);
            if (string.IsNullOrEmpty(bodyPreview))
            {
                if (body.Length > FragmentSize)
                {
                    bodyPreview = body.Substring(0, FragmentSize);
                }
                else
                {
                    bodyPreview = body;
                }
            }
            return bodyPreview;
        }

        /// <summary>
        /// 拆成分词
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public string GetKeyWordsSplitBySpace(string keywords)
        {
            PanGuTokenizer ktTokenizer = new PanGuTokenizer();
            StringBuilder result = new StringBuilder();
            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);
            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }
            return result.ToString().Trim();

        }


    }
}
