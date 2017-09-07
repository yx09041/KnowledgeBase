using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Mysoft.Util;
using MySoft.Application.Business;
using MySoft.Application.Entity;
using Mysoft.Code;

namespace MyWeb.Areas.knowledge.Controllers
{
    public class KnowledgeController : BaseController
    {
        knowledgeApp _app = new knowledgeApp();
        #region 视图
        public ActionResult Index()
        {
            return View();
        }

        [HandlerLogin(LoginMode.Enforce)]
        public ActionResult MyIndex()
        {
            return View();
        }

        /// <summary>
        /// 我的知识收藏
        /// </summary>
        /// <returns></returns>
        [HandlerLogin(LoginMode.Enforce)]
        public ActionResult MyStoreIndex()
        {
            return View();
        }

        [HandlerLogin(LoginMode.Enforce)]
        public ActionResult Form()
        {
            return View();
        }

          [HandlerLogin(LoginMode.Enforce)]
        public ActionResult Edit(string id)
        {
            ViewBag.id = id;
            return View();
        }
        public ActionResult Detail(string id)
        {
            ViewBag.id = id;
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination)
        {
            List<KnowledgeInfoEntity> data = _app.GetList(pagination);
            var JsonData = new
            {
                data = data,
                total = pagination.records,
                page = pagination.page,
            };
            return Content(JsonData.ToJson("yyyy-MM-dd HH:mm"));
        }



        /// <summary>
        /// 通过搜索内容获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueryListJson(Pagination pagination, string KeyValue)
        {
            StringBuilder sbRtn = new StringBuilder();
            SearchParam searchParam = new SearchParam();
            searchParam.PageIndex = pagination.page;
            searchParam.PageSize = pagination.rows;
            searchParam.KeyValue = KeyValue;
            //查询出数据
            List<KnowledgeInfoEntity> listDatas = new PanGuManager().ShowDatasByTAndC(searchParam);
            var JsonData = new
            {
                data = listDatas,
                total = searchParam.TotalCount
            };
            return Content(JsonData.ToJson("yyyy-MM-dd HH:mm"));
        }

       /// <summary>
       /// 我的分享
       /// </summary>
       /// <param name="pagination"></param>
       /// <returns></returns>
       [HttpGet]
        public ActionResult GetMyListJson(Pagination pagination)
        {
            var data = _app.GetMyList(pagination);
            var JsonData = new
            {
                data = data,
                total = pagination.records,
                page = pagination.page,
            };
            return Content(JsonData.ToJson("yyyy-MM-dd HH:mm"));
        }

        /// <summary>
        /// 获取我的收藏
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetMyStoreListJson(Pagination pagination)
        {
            var data = _app.GetMyStoreList(pagination);
            var JsonData = new
            {
                data = data,
                total = pagination.records,
                page = pagination.page,
            };
            return Content(JsonData.ToJson("yyyy-MM-dd HH:mm"));
        }
        
        /// <summary>
        /// 获取实体json
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _app.GetEntity(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region 提交
        [ValidateInput(false)]  
        public ActionResult SubmitForm(KnowledgeInfoEntity entity, string keyValue)
        {
            var data = _app.SubmitForm(entity, keyValue);
            return Success("提交成功");
        }

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="knowledgeGUID"></param>
        /// <returns></returns>
        public ActionResult StoreKnowledge(string knowledgeGUID)
        {
            var data = _app.StoreKnowledge(knowledgeGUID);
            return Success("收藏成功");
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="knowledgeGUID"></param>
        /// <returns></returns>
        public ActionResult CancelStoreKnowledge(string knowledgeGUID)
        {
            var data = _app.CancelStoreKnowledge(knowledgeGUID);
            return Success("取消收藏成功");
        }


        public ActionResult CreateIndex()
        {
            var indexManage = IndexManager.GetInstance();
            indexManage.CreateIndex();
            return Success("创建全文索引成功");
        }
        #endregion



    }
}
