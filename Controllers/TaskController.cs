using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskManagementPortal.EndPoints;
using TaskManagementPortal.Models.UserManagement;
using TaskManagementPortal.Models;
using TaskManagementPortal.Models.TaskManagement;
using TaskManagementPortal.CommonRequestHandler;
using System.Net;
using System.Text.RegularExpressions;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagementPortal.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ICommanWebRequestHandler _commanWebRequestHandler;

        public TaskController(ILogger<TaskController> logger, ICommanWebRequestHandler commanWebRequestHandler)
        {
            _logger = logger;
            _commanWebRequestHandler = commanWebRequestHandler;
        }

        [HttpGet]
        public async Task<IActionResult> TaskList()
        {
            ViewBag.LoggedUserName = HttpContext.Session.GetString("LoggedUserName");
            ViewBag.LoggedUser = HttpContext.Session.GetString("LoggedUser");
            List<TaskList> taskList = new List<TaskList>();
            try
            {
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                   "tmsService",
                   TaskEndPoints._getTaskList,
                   "GET",
                   "",
                   "retrieving task list"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    taskList = JsonConvert.DeserializeObject<List<TaskList>>(apiResponse);
                    if (taskList != null)
                    {
                        ViewBag.taskList = taskList;
                    }
                    _logger.LogInformation("User List successfully retrieved");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving task list {ex.Message}");
                ViewBag.Status = -1;
            }
            return View(taskList);
        }

        [HttpGet]
        public async Task<IActionResult> AddTask()
        {
            ViewBag.Repoter = HttpContext.Session.GetString("LoggedUser");
            using (var response = await _commanWebRequestHandler.WebRequestExecute(
                  "ekmsservice",
                  TaskEndPoints._getEmail,
                  "GET",
                  "",
                  "retrieving email list"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                List<string> Email = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                if (Email != null)
                {
                    ViewBag.Email = Email;
                }
                _logger.LogInformation("User list successfully retrieved");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(AddTask addTask)
        {
            try
            {
                addTask.StartDate = addTask.reservation.Split('-')[0].Trim();
                addTask.EndDate = addTask.reservation.Split('-')[1].Trim();
                if (ModelState.IsValid)
                {
                    string base64String = null;

                    if (addTask.AttachFile != null && addTask.AttachFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await addTask.AttachFile.CopyToAsync(memoryStream);
                            byte[] fileBytes = memoryStream.ToArray();
                            base64String = Convert.ToBase64String(fileBytes);
                        }
                    }

                    addTask.AttachFileInBase64 = base64String;
                    addTask.AttachFile = null;

                    using (var response = await _commanWebRequestHandler.WebRequestExecute(
                        "tmsService",
                        TaskEndPoints._addTask,
                        "POST",
                        JsonConvert.SerializeObject(addTask),
                        $"adding task {addTask.TaskName}"))
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
                            var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(apiResponse);
                            if (baseResponse!.status == 1)
                            {
                                TempData["AlertStatus"] = baseResponse!.status;
                                TempData["AlertMessage"] = baseResponse!.succMsg;
                                addTask = new AddTask();
                                ModelState.Clear();
                                _logger.LogInformation(baseResponse.succMsg);
                                return RedirectToAction("TaskList");
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
                _logger.LogError($"Error occurred while adding task {addTask.TaskName} {ex.Message}");
                ViewBag.Status = -1;
                ViewBag.Response = $"Error occurred while adding user {addTask.TaskName}";
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTask(string ID)
        {
            AddTask taskList = new();
            try
            {
                ViewBag.Repoter = HttpContext.Session.GetString("LoggedUser");
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                      "ekmsservice",
                      TaskEndPoints._getEmail,
                      "GET",
                      "",
                      "retrieving email list"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    List<string> Email = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                    if (Email != null)
                    {
                        ViewBag.Email = Email;
                    }
                    _logger.LogInformation("User list successfully retrieved");
                }
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                "tmsService",
                TaskEndPoints._getTaskDetails + ID,
                "GET",
                "",
                $"retrieving details"))
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
                        taskList = JsonConvert.DeserializeObject<AddTask>(apiResponse);
                        _logger.LogInformation($"Successfully retrieved details of Task");
                    }
                }
            }
            catch (Exception)
            {
                _logger.LogError($"Error occurred while retrieved details of task");
            }
            return View(taskList);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTask(AddTask addTask)
        {
            try
            {
                addTask.StartDate = addTask.reservation.Split('-')[0].Trim();
                addTask.EndDate = addTask.reservation.Split('-')[1].Trim();
                if (ModelState.IsValid)
                {
                    string base64String = null;

                    if (addTask.AttachFile != null && addTask.AttachFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await addTask.AttachFile.CopyToAsync(memoryStream);
                            byte[] fileBytes = memoryStream.ToArray();
                            base64String = Convert.ToBase64String(fileBytes);
                        }
                    }

                    addTask.AttachFileInBase64 = base64String;
                    addTask.AttachFile = null;

                    using (var response = await _commanWebRequestHandler.WebRequestExecute(
                        "tmsService",
                        TaskEndPoints._updateTask,
                        "POST",
                        JsonConvert.SerializeObject(addTask),
                        $"adding task {addTask.TaskName}"))
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
                            var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(apiResponse);
                            if (baseResponse!.status == 1)
                            {
                                TempData["AlertStatus"] = baseResponse!.status;
                                TempData["AlertMessage"] = baseResponse!.succMsg;
                                addTask = new AddTask();
                                ModelState.Clear();
                                _logger.LogInformation(baseResponse.succMsg);
                                return RedirectToAction("TaskList");
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
                _logger.LogError($"Error occurred while adding task {addTask.TaskName} {ex.Message}");
                ViewBag.Status = -1;
                ViewBag.Response = $"Error occurred while adding user {addTask.TaskName}";
            }
            return View(addTask);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTask(int ID)
        {
            DeleteTask deleteTask = new();
            try
            {
                deleteTask.Email = HttpContext.Session.GetString("LoggedUser");
                deleteTask.Id = ID;
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                "tmsService",
                TaskEndPoints._deleteTask,
                "POST",
                JsonConvert.SerializeObject(deleteTask),
                $"deleting task."))
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
                return RedirectToAction("TaskList");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting task {ex.Message}");
                TempData["AlertStatus"] = -1;
                TempData["AlertMessage"] = $"Error occurred while deleting task.";
                return RedirectToAction("TaskList");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPdf(string ID)
        {
            AddTask taskList = new();
            try
            {
                using (var response = await _commanWebRequestHandler.WebRequestExecute(
                "tmsService",
                TaskEndPoints._getTaskDetails + ID,
                "GET",
                "",
                $"retrieving details"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    taskList = JsonConvert.DeserializeObject<AddTask>(apiResponse);
                    if (string.IsNullOrEmpty(taskList.AttachFileInBase64) || !taskList.AttachFileInBase64.Any())
                    {
                        TempData["AlertStatus"] = -1;
                        TempData["AlertMessage"] = "No PDF data found.";
                        return RedirectToAction("TaskList");
                    }
                    byte[] pdfBytes;
                    try
                    {
                        pdfBytes = Convert.FromBase64String(taskList.AttachFileInBase64.Replace("\n", "").Replace("\r", ""));
                        if (pdfBytes.Length == 0)
                        {
                            TempData["AlertStatus"] = -1;
                            TempData["AlertMessage"] = "The base64 string does not decode to a valid PDF.";
                            return RedirectToAction("TaskList");
                        }
                    }
                    catch (FormatException)
                    {
                        TempData["AlertStatus"] = -1;
                        TempData["AlertMessage"] = "The base64 string is not in a valid format.";
                        return RedirectToAction("TaskList");
                    }

                    var contentType = "application/pdf";
                    var fileName = $"{taskList.TaskName}.pdf";

                    return File(pdfBytes, contentType, fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while downloading PDF: {ex.Message}");
                TempData["AlertStatus"] = -1;
                TempData["AlertMessage"] = "An error occurred while processing your request.";
                return RedirectToAction("TaskList");
            }
        }

    }
}