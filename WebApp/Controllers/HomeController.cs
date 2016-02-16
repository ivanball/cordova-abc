using Common;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // GET: Home/Encrypt
        public ActionResult Encrypt()
        {
            return View();
        }

        // POST: Home/Encrypt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Encrypt(string TextToEncrypt)
        {
            string strDecryptedText = Encryptor.Encrypt(TextToEncrypt);
            return View(model: strDecryptedText);
        }

        // GET: Home/Decrypt
        public ActionResult Decrypt()
        {
            return View();
        }

        // POST: Home/Decrypt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Decrypt(string TextToDecrypt)
        {
            string strClearText = Encryptor.Decrypt(TextToDecrypt);
            return View(model: strClearText);
        }

    }
}
