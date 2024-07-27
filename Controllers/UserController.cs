using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TaskManagementPortal.CommonRequestHandler;
using TaskManagementPortal.EndPoints;
using TaskManagementPortal.Models;
using TaskManagementPortal.Models.UserManagement;

namespace TaskManagementPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly ICommanWebRequestHandler _commanWebRequestHandler;

        public UserController(ILogger<UserController> logger, ICommanWebRequestHandler commanWebRequestHandler)
        {
            _logger = logger;
            _commanWebRequestHandler = commanWebRequestHandler;
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            ViewBag.LoggedUserName = HttpContext.Session.GetString("LoggedUserName") ?? "Guest";
            ViewBag.LoggedUser = HttpContext.Session.GetString("LoggedUser") ?? "Guest";
            List<UsersInfo> UserList = new List<UsersInfo>();
            try
            {
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                   "tmsService",
                   UsersEndPoints._getUserList,
                   "GET",
                   "",
                   "retrieving userlist"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    UserList = JsonConvert.DeserializeObject<List<UsersInfo>>(apiResponse);
                    if (UserList != null)
                    {
                        ViewBag.userList = UserList;
                    }
                    _logger.LogInformation("User List successfully retrieved");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving userlist {ex.Message}");
                ViewBag.Status = -1;
            }
            return View(UserList);
        }

        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUser addUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var response = await _commanWebRequestHandler.WebRequestExecute(
                    "tmsService",
                    UsersEndPoints._addUser,
                    "POST",
                    JsonConvert.SerializeObject(addUser),
                    $"adding user {addUser.Email}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            var myData = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                            var errMsg = string.Join(", ", myData!);
                            TempData["AlertStatus"] = -1;
                            TempData["AlertMessage"] = errMsg;
                            _logger.LogError(errMsg);
                        }
                        else
                        {
                            HttpContext.Session.SetString("addedUser", addUser!.Email!);
                            var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(apiResponse);
                            if (baseResponse!.status == 1)
                            {
                                TempData["AlertStatus"] = baseResponse!.status;
                                TempData["AlertMessage"] = baseResponse!.succMsg;
                                addUser = new AddUser();
                                ModelState.Clear();
                                _logger.LogInformation(baseResponse.succMsg);
                                return RedirectToAction("UserList");
                            }
                            else
                            {
                                TempData["AlertStatus"] = baseResponse!.status;
                                TempData["AlertMessage"] = baseResponse.errMsg;
                                _logger.LogInformation(baseResponse.errMsg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding user {addUser.Email} {ex.Message}");
                ViewBag.Status = -1;
                ViewBag.Response = $"Error occurred while adding user {addUser.Email}";
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string email)
        {
            ApproveUser approveUser = new ApproveUser();
            try
            {
                approveUser.ApprovedBy = HttpContext.Session.GetString("LoggedUser");
                approveUser.Username = email;
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                "tmsService",
                UsersEndPoints._deleteUser,
                "POST",
                JsonConvert.SerializeObject(approveUser),
                $"deleting user {email}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var myData = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                        var errMsg = string.Join(", ", myData!.ToArray());
                        TempData["AlertStatus"] = -1;
                        TempData["AlertMessage"] = errMsg;
                        _logger.LogError(errMsg);
                    }
                    else
                    {
                        var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(apiResponse);
                        if (baseResponse!.status == 1)
                        {
                            TempData["AlertStatus"] = baseResponse!.status;
                            TempData["AlertMessage"] = baseResponse.succMsg;
                            _logger.LogInformation(baseResponse.succMsg);
                        }
                        else
                        {
                            TempData["AlertStatus"] = baseResponse!.status;
                            TempData["AlertMessage"] = baseResponse.errMsg;
                            _logger.LogInformation(baseResponse.errMsg);
                        }
                    }
                }
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting user {email}  {ex.Message}");
                TempData["AlertStatus"] = -1;
                TempData["AlertMessage"] = $"Error occurred while deleting user {email}";
                return RedirectToAction("UserList");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ApproveUser(string email)
        {
            ApproveUser approveUser = new ApproveUser();
            try
            {
                approveUser.ApprovedBy = HttpContext.Session.GetString("LoggedUser");
                approveUser.Username = email;
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                "ekmsservice",
                UsersEndPoints._approveUser,
                "POST",
                JsonConvert.SerializeObject(approveUser),
                $"approving user {email}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var myData = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                        var errMsg = string.Join(", ", myData!.ToArray());
                        TempData["AlertStatus"] = -1;
                        TempData["AlertMessage"] = errMsg;
                        _logger.LogError(errMsg);
                    }
                    else
                    {
                        var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(apiResponse);
                        if (baseResponse!.status == 1)
                        {
                            TempData["AlertStatus"] = baseResponse!.status;
                            TempData["AlertMessage"] = baseResponse.succMsg;
                            _logger.LogInformation(baseResponse.succMsg);
                        }
                        else
                        {
                            TempData["AlertStatus"] = baseResponse!.status;
                            TempData["AlertMessage"] = baseResponse.errMsg;
                            _logger.LogInformation(baseResponse.errMsg);
                        }
                    }
                }
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while approving user {email}  {ex.Message}");
                TempData["AlertStatus"] = -1;
                TempData["AlertMessage"] = $"Error occurred while approving user {email}";
                return RedirectToAction("UserList");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ReactivateUser(string email)
        {
            ApproveUser approveUser = new ApproveUser();
            try
            {
                approveUser.ApprovedBy = HttpContext.Session.GetString("LoggedUser");
                approveUser.Username = email;
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                "ekmsservice",
                UsersEndPoints._reactivateUser,
                "POST",
                JsonConvert.SerializeObject(approveUser),
                $"reactivating user {email}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var myData = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                        var errMsg = string.Join(", ", myData!.ToArray());
                        TempData["AlertStatus"] = -1;
                        TempData["AlertMessage"] = errMsg;
                        _logger.LogError(errMsg);
                    }
                    else
                    {
                        var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(apiResponse);
                        if (baseResponse!.status == 1)
                        {
                            TempData["AlertStatus"] = baseResponse!.status;
                            TempData["AlertMessage"] = baseResponse.succMsg;
                            _logger.LogInformation(baseResponse.succMsg);
                        }
                        else
                        {
                            TempData["AlertStatus"] = baseResponse!.status;
                            TempData["AlertMessage"] = baseResponse.errMsg;
                            _logger.LogInformation(baseResponse.errMsg);
                        }
                    }
                }
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while reactivating user {email} {ex.Message}");
                TempData["AlertStatus"] = -1;
                TempData["AlertMessage"] = $"Error occurred while reactivating user {email}";
                return RedirectToAction("UserList");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UnblockUser(string email)
        {
            ApproveUser approveUser = new ApproveUser();
            try
            {
                approveUser.ApprovedBy = HttpContext.Session.GetString("LoggedUser");
                approveUser.Username = email;
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                "ekmsservice",
                UsersEndPoints._unBlockUser,
                "POST",
                JsonConvert.SerializeObject(approveUser),
                $"unblocking user {email}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var myData = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                        var errMsg = string.Join(", ", myData!.ToArray());
                        TempData["AlertStatus"] = -1;
                        TempData["AlertMessage"] = errMsg;
                        _logger.LogError(errMsg);
                    }
                    else
                    {
                        var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(apiResponse);
                        if (baseResponse!.status == 1)
                        {
                            TempData["AlertStatus"] = baseResponse!.status;
                            TempData["AlertMessage"] = baseResponse.succMsg;
                            _logger.LogInformation(baseResponse.succMsg);
                        }
                        else
                        {
                            TempData["AlertStatus"] = baseResponse!.status;
                            TempData["AlertMessage"] = baseResponse.errMsg;
                            _logger.LogInformation(baseResponse.errMsg);
                        }
                    }
                }
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while unblocking user {email} {ex.Message}");
                TempData["AlertStatus"] = -1;
                TempData["AlertMessage"] = $"Error occurred while unblocking user {email}";
                return RedirectToAction("UserList");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string email)
        {
            UpdateUser updateUser = new UpdateUser();
            try
            {
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                "tmsService",
                UsersEndPoints._getUserDetails + email,
                "GET",
                "",
                $"retrieving user {email} details"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var myData = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                        var errMsg = string.Join(", ", myData!.ToArray());
                        ViewBag.Status = -1;
                        ViewBag.Response = errMsg;
                        _logger.LogError(errMsg);
                    }
                    else
                    {
                        updateUser = JsonConvert.DeserializeObject<UpdateUser>(apiResponse);
                        HttpContext.Session.SetString("UserType", updateUser!.IsAdmin.ToString());
                        _logger.LogInformation($"Successfully retrieved details of user {email}");
                    }
                }
            }
            catch (Exception)
            {
                _logger.LogError($"Error occurred while retrieved details of user {email}");
            }
            return View(updateUser);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUser updateUser)
        {
            try
            {
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                "tmsService",
                UsersEndPoints._updateUser,
                "POST",
                JsonConvert.SerializeObject(updateUser),
                $"updating user {updateUser.Email}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var myData = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                        var errMsg = string.Join(", ", myData!.ToArray());
                        TempData["AlertStatus"] = -1;
                        TempData["AlertMessage"] = errMsg;
                        _logger.LogError(errMsg);
                    }
                    else
                    {
                        var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(apiResponse);
                        if (baseResponse!.status == 1)
                        {
                            TempData["AlertStatus"] = baseResponse!.status;
                            TempData["AlertMessage"] = baseResponse!.succMsg;
                            _logger.LogInformation(baseResponse.succMsg);
                            return RedirectToAction("UserList");
                        }
                        else
                        {
                            TempData["AlertStatus"] = baseResponse!.status;
                            TempData["AlertMessage"] = baseResponse!.errMsg;
                            _logger.LogInformation(baseResponse.errMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating user {updateUser.Email} {ex.Message}");
                ViewBag.Status = -1;
                ViewBag.Response = $"Error occurred while updating user {updateUser.Email}";
            }
            return View(updateUser);
        }
    }
}