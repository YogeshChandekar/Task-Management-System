using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json;

using System.Security.Claims;
using TaskManagementPortal.Models;
using TaskManagementPortal.EndPoints;
using TaskManagementPortal.CommonRequestHandler;
using TaskManagementPortal.Models.LoginManagement;
using TaskManagementPortal.Models.UserManagement;

namespace TaskManagementPortal.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ICommanWebRequestHandler _commanWebRequestHandler;

        public LoginController(ILogger<LoginController> logger, ICommanWebRequestHandler commanWebRequestHandler)
        {
            _logger = logger;
            _commanWebRequestHandler = commanWebRequestHandler;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AuthenticateUser authenticateUser)
        {
            authenticateUser.Password = Utility.Decrypt(authenticateUser.Password);
            try
            {
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                "tmsService",
                LoginEndPoints._authentication,
                "POST",
                JsonConvert.SerializeObject(authenticateUser),
                $"logging user {authenticateUser.Email}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        List<Claim> claims = new() { new Claim(ClaimTypes.Name, authenticateUser.Email) };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties authProperties = new AuthenticationProperties();

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                        var myData = JsonConvert.DeserializeObject<UsersInfo>(apiResponse);
                        HttpContext.Session.SetString("LoggedUser", authenticateUser.Email!);
                        HttpContext.Session.SetString("LoggedUserName", myData!.Name);

                        _logger.LogInformation($"{authenticateUser.Email} logged in successfully");
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // array
                        var myData = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                        _logger.LogError($"Bad Request occurred while logging user {authenticateUser.Email}");
                    }
                    else
                    {
                        // base object
                        var myData = JsonConvert.DeserializeObject<BaseResponse>(apiResponse);
                        ViewBag.Response = myData!.errMsg;
                        _logger.LogError($"Error occurred while logging user {authenticateUser.Email}");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while logging user {authenticateUser.Email} {ex.Message}");
                ViewBag.Response = $"Error occurred while logging user {authenticateUser.Email}";
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout(int status)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("3cf33913ae42372f8e635e66cbb0e9987a265f52be6554be058f83cc97dbdf68");
            Response.Cookies.Delete("28f7776fa3b890a713ec04d093a2afcd6cf50276e69288056ac5f725f44bae98");
            HttpContext.Session.Clear();
            await HttpContext.Session.CommitAsync();
            if (Request.Cookies["3cf33913ae42372f8e635e66cbb0e9987a265f52be6554be058f83cc97dbdf68"] != null)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(-20)
                };
                Response.Cookies.Append("3cf33913ae42372f8e635e66cbb0e9987a265f52be6554be058f83cc97dbdf68", string.Empty, cookieOptions);
            }
            if (Request.Cookies["28f7776fa3b890a713ec04d093a2afcd6cf50276e69288056ac5f725f44bae98"] != null)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(-20)
                };
                Response.Cookies.Append("28f7776fa3b890a713ec04d093a2afcd6cf50276e69288056ac5f725f44bae98", string.Empty, cookieOptions);
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
