using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogging_app.Models;
using EncryptionDecryption_core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace blogging_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpPost("[action]")]
        public JsonResult authenticate()
        {
            try
            {
                Crypt cr = new Crypt();
                HomeModel HM = new HomeModel();
                string decryptedCookie_serialized = Request.Form["serialized_cookie"];
                cr.EncryptionKey = HM.getEncryptionKey();
                string DecryptedCookie = cr.Decrypt(decryptedCookie_serialized);
                //return Json(HM.verify(DecryptedCookie));
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return null;
        }

        [HttpPost("[action]")]
        public JsonResult add_blog()
        {
            try
            {
                Crypt cr = new Crypt();
                HomeModel HM = new HomeModel();
                string decryptedCookie_serialized = Request.Form["serialized_cookie"];
                cr.EncryptionKey = HM.getEncryptionKey();
                string DecryptedCookie = cr.Decrypt(decryptedCookie_serialized);

                string brief = Request.Form["brief"];
                string content = Request.Form["content"];

                Dictionary<string, string> user = JsonConvert.DeserializeObject<Dictionary<string, string>>(DecryptedCookie);
                string uid = user["uid"];

                return Json(HM.add_blog(brief, content, uid));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("[action]")]
        public JsonResult update_blog()
        {
            try
            {
                Crypt cr = new Crypt();
                HomeModel HM = new HomeModel();
                string decryptedCookie_serialized = Request.Form["serialized_cookie"];
                cr.EncryptionKey = HM.getEncryptionKey();
                string DecryptedCookie = cr.Decrypt(decryptedCookie_serialized);

                string brief = Request.Form["brief"];
                string content = Request.Form["content"];
                Dictionary<string, string> user = JsonConvert.DeserializeObject<Dictionary<string, string>>(DecryptedCookie);
                string uid = user["uid"];


                return Json(HM.update_blog(brief, content, uid));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("[action]")]
        public JsonResult delete_blog()
        {
            try
            {
                Crypt cr = new Crypt();
                HomeModel HM = new HomeModel();
                string decryptedCookie_serialized = Request.Form["serialized_cookie"];
                cr.EncryptionKey = HM.getEncryptionKey();
                string DecryptedCookie = cr.Decrypt(decryptedCookie_serialized);

                string Blog_id = Request.Form["bid"];
                Dictionary<string, string> user = JsonConvert.DeserializeObject<Dictionary<string, string>>(DecryptedCookie);
                string uid = user["uid"];

                return Json(HM.delete_blog(Blog_id,uid));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("[action]")]
        public JsonResult fetch_all_blogs()
        {
            try
            {
                Crypt cr = new Crypt();
                HomeModel HM = new HomeModel();
                string decryptedCookie_serialized = Request.Form["serialized_cookie"];
                cr.EncryptionKey = HM.getEncryptionKey();
                string DecryptedCookie = cr.Decrypt(decryptedCookie_serialized);
                Dictionary<string, string> user = JsonConvert.DeserializeObject<Dictionary<string, string>>(DecryptedCookie);
                string uid = user["uid"];

                return Json(HM.fetch_all_blogs(uid));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}