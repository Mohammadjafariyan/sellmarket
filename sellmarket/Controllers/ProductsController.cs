using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using sellmarket.Models;

namespace sellmarket.Controllers
{
    public class ProductsController : BaseController<Product>
    {

        protected override string SetProductTitleInImages(MarketContext db, long id)
        {
            return db.Products.Find(id).Name;
        }
        protected override IQueryable<Image> WhereRefId(IQueryable<Image> images, int id)
        {
            return images.Where(i => i.ProductId == id);
        }
        
        [HttpGet]
        public virtual async Task<ActionResult> LoadImageFromWeb(string url)
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

                return File(await resp.Content.ReadAsStreamAsync(), System.Net.Mime.MediaTypeNames.Image.Jpeg);
            }
        }


        protected override void SetImageRefId(ref Image img,long id)
        {
            img.ProductId = id;
        }
    }


    public static class MyGlobalExtentions
    {
        public static string ModelStateToString(this ModelStateDictionary modelState)
        {
            string messages = string.Join("; ", modelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            return messages;
        }
    }
}