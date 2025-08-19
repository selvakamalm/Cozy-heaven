using CozyHaven.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CozyHaven.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // ========== AUTHENTICATION ==========

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLogin login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7298/api/Auth/login", login);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid login attempt";
                return View(login);
            }

            var token = await response.Content.ReadFromJsonAsync<JwtResponse>();

            // Save token in session
            HttpContext.Session.SetString("JWToken", token.Token);
            // HttpContext.Session.SetString("Username", token.Username); // optional if username is available
            // got to admin dash
            return RedirectToAction("Dashboard", "Admin");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ========== DASHBOARD ==========

        public IActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken")))
                return RedirectToAction("Login");

            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        // ========== USERS ==========
        [Route("Admin/Dashboard/Users")]
        public async Task<IActionResult> Users()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));

            try
            {
                var response = await client.GetAsync("https://localhost:7298/api/User");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Could not retrieve users.";
                    return View(new List<UserViewModel>());
                }

                var users = await response.Content.ReadFromJsonAsync<List<UserViewModel>>();
                return View(users ?? new List<UserViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Server error: " + ex.Message;
                return View(new List<UserViewModel>());
            }
        }

        //==========Delete User==========
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));

            var response = await client.DeleteAsync($"https://localhost:7298/api/User/{id}");

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Failed to delete user.";

            return RedirectToAction("Users");
        }

        //==========Get User Details==========
        public async Task<IActionResult> UserDetails(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));

            var response = await client.GetAsync($"https://localhost:7298/api/User/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not fetch user details.";
                return RedirectToAction("Users");
            }

            var user = await response.Content.ReadFromJsonAsync<UserViewModel>();
            return View(user);
        }

        // ========== HOTELS ==========
        [Route("Admin/Dashboard/Hotels")]
        public async Task<IActionResult> Hotels()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "JWT token missing. Please login again.";
                return RedirectToAction("Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7298/api/Hotel");

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                ViewBag.Error = $"Unable to fetch hotels. Status: {response.StatusCode} | {errorText}";
                return View(new List<HotelListViewModel>());
            }

            var rawJson = await response.Content.ReadAsStringAsync();

            var hotels = JsonSerializer.Deserialize<List<HotelListViewModel>>(rawJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(hotels ?? new List<HotelListViewModel>());
        }

        //========== VIEW HOTEL DETAILS ==========

        public async Task<IActionResult> ViewHotel(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));

            var response = await client.GetAsync($"https://localhost:7298/api/Hotel/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Hotels");

            var hotel = await response.Content.ReadFromJsonAsync<HotelViewModel>();
            return View(hotel);
        }

        //========== Edit HOTEL ==========

        [HttpGet]
        public async Task<IActionResult> EditHotel(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));

            var response = await client.GetAsync($"https://localhost:7298/api/Hotel/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Hotels");

            var hotel = await response.Content.ReadFromJsonAsync<HotelViewModel>();
            return View(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> EditHotel(HotelViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));

            var response = await client.PutAsJsonAsync($"https://localhost:7298/api/Hotel/{model.Id}", model);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed to update hotel.";
                return View(model);
            }

            return RedirectToAction("Hotels");
        }

        //========== Delete HOTEL ==========

        [HttpPost]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));

            var response = await client.DeleteAsync($"https://localhost:7298/api/Hotel/{id}");

            return RedirectToAction("Hotels");
        }

        [HttpGet]
        public IActionResult AddHotel()
        {
            return View(new HotelViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddHotel(HotelViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("https://localhost:7298/api/Hotel", model);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed to create hotel.";
                return View(model);
            }

            return RedirectToAction("Hotels");
        }

        // ========== BOOKINGS ==========
        [Route("Admin/Dashboard/Bookings")]
        public async Task<IActionResult> Bookings()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7298/api/Booking");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to fetch bookings.";
                return View(new List<BookingViewModel>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var bookings = JsonSerializer.Deserialize<List<BookingViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(bookings ?? new List<BookingViewModel>());
        }

        //========== Edit BOOKING DETAILS ==========

        [HttpGet]
        public async Task<IActionResult> EditBooking(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"https://localhost:7298/api/Booking/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Bookings");

            var booking = await response.Content.ReadFromJsonAsync<BookingViewModel>();
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> EditBooking(BookingViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsJsonAsync($"https://localhost:7298/api/Booking/{model.Id}", model);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed to update booking.";
                return View(model);
            }

            return RedirectToAction("Bookings");
        }

        //========== Delete Booking ==========

        [HttpPost]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"https://localhost:7298/api/Booking/{id}");

            return RedirectToAction("Bookings");
        }

        //======= === ADD BOOKING ==========

        [HttpGet]
        public IActionResult AddBooking()
        {
            return View(new BookingViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(BookingViewModel booking)
        {
            if (!ModelState.IsValid)
                return View(booking);

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("https://localhost:7298/api/Booking", booking);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed to create booking.";
                return View(booking);
            }

            return RedirectToAction("Bookings");
        }
    }
}
