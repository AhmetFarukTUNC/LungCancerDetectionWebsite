using LungCancer.Data;
using LungCancer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.EntityFrameworkCore;


namespace LungCancer.Controllers
{
    public class PredictController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _context;

        public PredictController(IHttpClientFactory httpClientFactory, AppDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        // Upload sayfasını göster
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        // Dosya yükleme ve tahmin
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Result = "<span style='color:red;'>Dosya seçilmedi!</span>";
                return View();
            }

            // Session’dan UserId al
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                ViewBag.Result = "<span style='color:red;'>Kullanıcı giriş yapmamış!</span>";
                return View();
            }

            string flaskUrl = "http://127.0.0.1:5000/predict_web";

            using var client = _httpClientFactory.CreateClient();
            using var content = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream();
            content.Add(new StreamContent(fileStream), "file", file.FileName);

            var response = await client.PostAsync(flaskUrl, content);
            var resultString = await response.Content.ReadAsStringAsync();

            // Regex ile tahmin ve confidence çek
            var predClassMatch = Regex.Match(resultString, @"Tahmin: (.*?)</h2>");
            var confidenceMatch = Regex.Match(resultString, @"Oranı: %([0-9.,]+)</h3>");

            if (!predClassMatch.Success || !confidenceMatch.Success)
            {
                ViewBag.Result = "<span style='color:red;'>Tahmin sonucu alınamadı.</span>";
                return View();
            }

            var predClass = predClassMatch.Groups[1].Value.Replace("_", " ");
            var confidenceStr = confidenceMatch.Groups[1].Value.Replace(',', '.');
            double confidenceValue = double.Parse(confidenceStr, CultureInfo.InvariantCulture);
            if (confidenceValue <= 1) confidenceValue *= 100;

            string confidenceText = confidenceValue.ToString("F2", CultureInfo.InvariantCulture);

            // Prediction nesnesi oluştur
            var prediction = new Prediction
            {
                Filename = file.FileName,
                PredictedClass = predClass,
                Confidence = confidenceValue,
                CreatedAt = DateTime.Now,
                UserId = userId.Value
            };

            _context.Predictions.Add(prediction);
            await _context.SaveChangesAsync();

            ViewBag.Result = $"Tahmin: {predClass}, Doğruluk: %{confidenceText}";

            return View();
        }

        // Tahmin geçmişi sayfası
        [HttpGet]
        public async Task<IActionResult> History()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // login değilse yönlendir
            }

            var predictions = await _context.Predictions
                                            .Where(p => p.UserId == userId.Value)
                                            .OrderByDescending(p => p.CreatedAt)
                                            .ToListAsync();

            return View(predictions);
        }
    }
}
