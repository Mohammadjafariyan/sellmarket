using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using sellmarket.Models;

namespace sellmarket.Controllers
{
    public abstract class BaseController<T> : Controller where T : class, HasImages, IModel, new()
    {
        [HttpGet]
        public ActionResult Images(int id, int skip = 0, int count = 20)
        {
            using (var db = new MarketContext())
            {
                var images = db.Images.AsQueryable();
                images=   WhereRefId(images, id);
                
                if (skip > 0)
                    images = images.Skip(skip);

                ViewBag.total = images.Count();
                ViewBag.page = skip;
                ViewBag.ProductTitle= SetProductTitleInImages(db ,id);
               // ViewBag.action = "Images";
                ViewBag.id = id;

                return View(images.Take(count).ToList());
            }
        }

        protected virtual string SetProductTitleInImages(MarketContext db, long id)
        {
            return "";
        }

        protected virtual IQueryable<Image> WhereRefId(IQueryable<Image> images, int id)
        {
            return images;
        }

        [HttpGet]
        public virtual async Task<byte[]> LoadImageFromWeb(string url)
        {
            using (var hc = new HttpClient())
            {
                var resp = await hc.GetAsync(url);

                if (!Equals(resp.Content.Headers.ContentType, new MediaTypeHeaderValue("image/png"))
                    && !Equals(resp.Content.Headers.ContentType, new MediaTypeHeaderValue("image/jpeg")))

                {
                    throw new HttpException((int)
                        HttpStatusCode.InternalServerError, "نوع فایل اشتباه است");
                }

                return await resp.Content.ReadAsByteArrayAsync();
            }
        }

        [HttpGet]
        public virtual ActionResult CreateImagePage(long id)
        {
            ViewBag.id = id;
            return View();
        }


        [HttpGet]
        public virtual FileResult ImageUrl(long id)
        {
            try
            {
                using (var db = new MarketContext())
                {
                    var image = db.Images.Find(id);

                    if (image == null)
                        throw new Exception("رکورد با ایدی داده شد یافت نشد");

                    return File(image.Source, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }

        [HttpGet]
        public virtual ActionResult DeleteImage(long id,long productId)
        {
            try
            {
                using (var db = new MarketContext())
                {
                    var img = db.Images.Find(id);
                    if (img == null)
                        throw new Exception("رکورد با ایدی داده شد یافت نشد");

                    db.Images.Remove(img);
                    db.SaveChanges();
                }

                return RedirectToAction("Images",new{id=productId});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return PartialView("Error", e);
            }
        }


        [HttpGet]
        public virtual ActionResult Delete(long id)
        {
            try
            {
                using (var db = new MarketContext())
                {
                    var models = db.Set<T>();
                    var model = models.Find(id);

                    if (model == null)
                        throw new Exception("رکورد با ایدی داده شد یافت نشد");

                    db.Entry(model).State = EntityState.Deleted;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Images");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return PartialView("Error", e);
            }
        }

        public ActionResult Index(int skip = 0, int count = 20)
        {
            using (var db = new MarketContext())
            {
                var records = db.Set<T>().AsQueryable();
                if (skip > 0)
                    records = records.Skip(skip);

                ViewBag.total = records.Count();
                ViewBag.page = skip;
                ViewBag.action = "Index";
                return View(records.Take(count).ToList());
            }
        }


        [HttpGet]
        public ActionResult Create(long id = 0)
        {
            if (id != 0)
            {
                using (var db = new MarketContext())
                {
                    var models = db.Set<T>();
                    var model = models.Find(id);
                    if (model == null)
                        throw new Exception("رکورد با ایدی داده شد یافت نشد");

                    return View(model);
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(T product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception(ModelState.ModelStateToString());
                }

                using (var db = new MarketContext())
                {
                    var models = db.Set<T>();

                    if (product.Id == 0)
                    {
                        models.Add(product);
                    }
                    else
                    {
                        var exist = models.Find(product.Id);
                        if (exist == null)
                            throw new Exception("محصول مورد نظر با کد داده شده یافت نشد");

                        db.Entry(exist).CurrentValues.SetValues(product);
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return PartialView("Error", e);
            }
        }


        [HttpPost]
        public virtual async Task<ActionResult> UploadImage(long id)
        {
            try
            {
                using (var db = new MarketContext())
                {
                    if (Request.Files.Count == 0 && string.IsNullOrEmpty(Request.Form["FileFromNet"]))
                        throw new Exception("هیچ فایلی ارسال نشده است");


                    var models = db.Set<T>();

                    var model = models.Find(id);
                    if (model == null)
                        throw new Exception("رکورد با ایدی داده شد یافت نشد");

                    foreach (HttpPostedFileBase fileBase in Request.Files)
                    {
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(fileBase.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(fileBase.ContentLength);
                        }

                        var img = new Image
                        {
                            Source = fileData, FileName = fileBase.FileName,
                            ContentType = fileBase.ContentType,
                        };

                        SetImageRefId(ref img,id);
                        db.Images.Add(img);
                    }


                    if (!string.IsNullOrEmpty(Request.Form["FileFromNet"]))
                    {
                        var url = Request.Form["FileFromNet"];
                        var byteArr = LoadImageFromWeb(url);

                        var img = new Image
                        {
                            Source = await byteArr, FileName = url,
                            ContentType = "image/jpeg",
                        };
                        
                        SetImageRefId(ref img,id);
                        db.Images.Add(img);
                    }

                    db.SaveChanges();
                    return RedirectToAction("Images",new {id=id});
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return PartialView("Error", e);
            }
        }

        protected virtual void SetImageRefId(ref Image img,long id)
        {
        }
    }
}