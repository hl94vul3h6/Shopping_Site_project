using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carts.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            //宣告回傳商品列表result
            List<Models.product> result = new List<Models.product>();

            //接收轉導的成功訊息
            ViewBag.ResultMessage = TempData["ResultMessage"];


            //使用CartsEntities 類別,名稱為db
            using ( Models.CartsEntities db = new Models.CartsEntities())
            {

                result = (from s in db.products select s).ToList();
            }
            return View(result);
        }
        //建立商品頁面
        public ActionResult Create()
        {
            return View();
        }
        //建立商品頁面,資料傳回處理
        public ActionResult Edit(int id)
        {
            using (Models.CartsEntities db = new Models.CartsEntities())
            {
                //抓取PRODUCT.ID等於輸入ID的資料
                var result = (from s in db.products where s.Id == id select s).FirstOrDefault();
                if( result != default( Models.product ) ) //判斷此ID是否有資料
                {
                    return View(result); //如果有回傳編輯商品頁面
                }
                else
                {  //如果沒有資料則顯示錯誤訊息並導回INDEX頁面
                    TempData["resultMessage"] = "資料有誤，請重新操作";
                    
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpPost] //指定只有使用POST方法才可進入
        public ActionResult Create( Models.product postback)
        {
            if (this.ModelState.IsValid) //如果資料驗證成功
            {
                using (Models.CartsEntities db = new Models.CartsEntities())
                {
                    //將回傳資料POSTBACK加入至PRODUCTS
                    db.products.Add(postback);
                    //儲存異動資料
                    db.SaveChanges();
                    //設定成功訊息
                    TempData["ResultMessage"] = String.Format("商品[{0}]成功建立", postback.Name);
                    //轉導Product/Index頁面
                    return RedirectToAction("Index");
                }
            }
            //失敗訊息
            ViewBag.ResultMessage = "資料有誤，請檢查";
            //停留在Create頁面
            return View(postback);
        }
        [HttpPost]
        public ActionResult Edit(Models.product postback)
        {
            if (this.ModelState.IsValid) //判斷使用者輸入資料是否正確
            {
                using (Models.CartsEntities db = new Models.CartsEntities())
                {
                    //抓取Product.Id等於回傳輸入postback.Id的資料
                    var result = (from s in db.products where s.Id == postback.Id select s).FirstOrDefault();
                    //儲存使用者變更資料
                    result.Name = postback.Name;
                    result.PublishDate = postback.PublishDate;
                    result.Status = postback.Status;
                    result.DefaultImageId = postback.DefaultImageId;
                    result.Price = postback.Price;
                    result.Quantity = postback.Quantity;
                    result.CategoryId = postback.CategoryId;
                    result.Description = postback.Description;
                    result.DefaultImageURL = postback.DefaultImageURL;

                    //儲存所有變更
                    db.SaveChanges();
                    //設定成功訊息並導回INDEX頁面
                    TempData["ResultMessage"] = String.Format("商品[{0}]成功建立", postback.Name);
                    //轉導Product/Index頁面
                    return RedirectToAction("Index");
                }
            }
            else //如果資料不正確則導向自己(edit頁面)
            {
                return View(postback);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (Models.CartsEntities db = new Models.CartsEntities())
            {
                //抓取Product.ID等於輸入ID的資料
                var result = (from s in db.products where s.Id == id select s).FirstOrDefault();
                if( result != default( Models.product ) ) //判斷此ID是否有資料
                {
                    db.products.Remove(result);

                    //儲存所有變更
                    db.SaveChanges();

                    //設定成功訊息並導回index頁面
                    TempData["ResultMessage"] = String.Format("商品[{0}]成功刪除", result.Name);
                    return RedirectToAction("Index");
                }
                else
                {    //如果沒有資料則顯示錯誤訊息並導回index頁面
                    TempData["resultMessage"] = "指定資料不存在，無法刪除，請重新操作";
                    return RedirectToAction("Index");
                }
            }
        }
    }
}