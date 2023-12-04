using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentHive.Models;
using System.Globalization;

namespace RentHive.Controllers
{
    public class SystemManagement : Controller
    {
        [HttpGet]
        public async Task<IActionResult> HiveUserList(UserDataGetter TempData, string sortBy)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }

            string url = "https://renthive.online/Admin_API/ViewUsersList.php";
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            switch (sortBy)
                            {
                                case "Acc_id":
                                    newuserObject = newuserObject.OrderBy(item => item.Acc_id).ToList();
                                    break;
                                case "Acc_DisplayName":
                                    newuserObject = newuserObject.OrderBy(item => item.Acc_DisplayName).ToList();
                                    break;
                                case "Acc_Email":
                                    newuserObject = newuserObject.OrderBy(item => item.Acc_Email).ToList();
                                    break;
                                default:
                                    // Default sorting, if sortBy is not recognized
                                    newuserObject = newuserObject.OrderBy(item => item.Acc_id).ToList();
                                    break;
                            }
                            ViewBag.Acc_Id = TempData.Acc_id;
                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage });
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> HiveAdminList(UserDataGetter TempData, string sortBy)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewAdminList.php";
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            switch(sortBy)
                            {
                                case "Acc_id":
                                    newuserObject = newuserObject.OrderBy(item => item.Acc_id).ToList();
                                    break;
                                case "Acc_DisplayName":
                                    newuserObject = newuserObject.OrderBy(item => item.Acc_DisplayName).ToList();
                                    break;
                                case "Acc_Email":
                                    newuserObject = newuserObject.OrderBy(item => item.Acc_Email).ToList();
                                    break;
                                default:
                                    // Default sorting, if sortBy is not recognized
                                    newuserObject = newuserObject.OrderBy(item => item.Acc_id).ToList();
                                    break;
                                }
                                ViewBag.Acc_Id = TempData.Acc_id;
                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage });
            }
            return View();
        }
        [HttpPost]
        public IActionResult HiveAdminList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> HiveUserDetails(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewUsersDetails.php";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                    {
                        {"userId", TempData.userId.ToString()},
                        {"accId", TempData.Acc_id.ToString()}
                    };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "Something went wrong.")
                        {
                            ViewBag.ErrorMessage = string.Format("Something went wrong.");
                        }
                        else
                        {
                            var userObject = JsonConvert.DeserializeObject<UserDataGetter>(responseData);
                            //Admin info
                            ViewBag.AdminID = TempData.Acc_id; //27
                            return View(userObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage });
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> HiveUserLog(UserDataGetter TempData, string sortBy)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewUserLog.php";
            try
            {
                using (var httpClient = new HttpClient())
                { 
                    var data = new Dictionary<string, string>
                    {
                        {"userId", TempData.userId.ToString()}
                    };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string formattedCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                        string formattedTime = DateTime.Now.ToString("hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();

                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            /*ViewBag.ErrorMessage = string.Format("Successfully retrieving data ");*/
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            switch (sortBy)
                            {
                                /*case "ul_id":
                                    newuserObject = newuserObject.OrderBy(item => item.ul_id).ToList();
                                    break;*/
                                case "ul_Origin":
                                    newuserObject = newuserObject.OrderByDescending(item => item.ul_Origin).ToList();
                                    break;
                                case "ul_Timestamp":
                                    newuserObject = newuserObject.OrderBy(item => item.ul_Timestamp).ToList();
                                    break;
                                case "ul_Action":
                                    newuserObject = newuserObject.OrderBy(item => item.ul_Action).ToList();
                                    break;
                                case "ul_SysResponse":
                                    newuserObject = newuserObject.OrderBy(item => item.ul_SysResponse).ToList();
                                    break;
                                default:
                                    // Default sorting, if sortBy is not recognized
                                    newuserObject = newuserObject.OrderBy(item => item.Acc_id).ToList();
                                    break;
                            }

                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.userId = TempData.userId;

                            //user
                            ViewBag.CurrentDate = formattedCurrentDate;
                            ViewBag.CurrentTime = formattedTime;
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;

                            //admin
                            ViewBag.Temp_FirstName = TempData.Temp_FirstName;
                            ViewBag.Temp_MiddleName = TempData.Temp_MiddleName;
                            ViewBag.Temp_LastName = TempData.Temp_LastName;


                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage });
            }
            return View();
        }
        [HttpPost]
        public IActionResult HiveUserLog()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> HiveUserPayment(UserDataGetter TempData,string sortBy)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewUserPayment.php";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                    {
                        {"userId", TempData.userId.ToString()}
                    };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string formattedCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                        string formattedTime = DateTime.Now.ToString("hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();

                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            switch (sortBy)
                            {
                                case "About":
                                    newuserObject = newuserObject.OrderBy(item => item.About).ToList();
                                    break;
                                case "PaymentAmount":
                                    newuserObject = newuserObject.OrderByDescending(item => item.PaymentAmount).ToList();
                                    break;
                                case "PaymentMode":
                                    newuserObject = newuserObject.OrderBy(item => item.PaymentMode).ToList();
                                    break;
                                case "AdvDeposit":
                                    newuserObject = newuserObject.OrderBy(item => item.AdvDeposit).ToList();
                                    break;
                                case "Date":
                                    newuserObject = newuserObject.OrderBy(item => item.Date).ToList();
                                    break;
                                default:
                                    // Default sorting, if sortBy is not recognized
                                    newuserObject = newuserObject.OrderBy(item => item.About).ToList();
                                    break;
                            }
                            /*ViewBag.ErrorMessage = string.Format("Successfully retrieving data ");*/
                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.userId = TempData.userId;

                            //user
                            ViewBag.CurrentDate = formattedCurrentDate;
                            ViewBag.CurrentTime = formattedTime;
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;

                            //admin
                            ViewBag.Temp_FirstName = TempData.Temp_FirstName;
                            ViewBag.Temp_MiddleName = TempData.Temp_MiddleName;
                            ViewBag.Temp_LastName = TempData.Temp_LastName;

                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage }); 
            }
            return View();
        }
        [HttpPost]
        public IActionResult HiveUserPayment()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> HiveUserStatus(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url1 = "https://renthive.online/Admin_API/ViewUserStatus.php";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var data1 = new Dictionary<string, string>
                    {
                        {"userId", TempData.userId.ToString()}
                    };

                    var content1 = new FormUrlEncodedContent(data1);
                    var response1 = await httpClient.PostAsync(url1, content1);

                    if (response1.IsSuccessStatusCode)
                    {
                        var responseData1 = await response1.Content.ReadAsStringAsync();

                        if (responseData1 == "Something went wrong.")
                        {
                            ViewBag.ErrorMessage = string.Format("Something went wrong.");
                        }
                        else
                        {
                            var userObject = JsonConvert.DeserializeObject<UserDataGetter>(responseData1);
                            ViewBag.CheckCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy"); // this is only for if condition purposes only
                            //admin info
                            ViewBag.Acc_Id = TempData.Acc_id;
                            return View(userObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage });
            }
            return View();
        }

        public IActionResult sampleView(int sample1, string sample2) //THIS CONTROLLER IS A SAMPLE VIEW ONLY
        {
            ViewBag.sam1 = sample1;
            ViewBag.sam2 = sample2;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HiveUserStatusPost1(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            if (TempData.Acc_Ban == "0" || TempData.Acc_Ban == null)
            {
                string url = "https://renthive.online/Admin_API/BanUser.php";
                try
                {
                    //userlog
                    string formattedCurrentDateTime = DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();
                    string origin = "Ban User Page";
                    string sysResponse = "Success";
                    string action = "Ban user";

                    /*string userBanDate = DateTime.Now.ToString("MMMM dd, yyyy");*/
                    /*DateTime currentDate = DateTime.Now;
                    DateTime userBanEndDate = currentDate.AddDays(3);

                    string formattedCurrentDate = currentDate.ToString("MMMM dd, yyyy");
                    string formattedEndDate = userBanEndDate.ToString("MMMM dd, yyyy");*/

                    string formattedCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                    string formattedEndDate = DateTime.Now.AddDays(3).ToString("MMMM dd, yyyy");

                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            { "userBanDate", formattedCurrentDate },
                            { "userBanEnd", formattedEndDate },
                            //userlog
                            {"adminid", TempData.Acc_id.ToString()},
                            {"CurrentDate", formattedCurrentDateTime},
                            {"origin", origin},
                            {"sysResponse", sysResponse },
                            {"action", action }
                        };
                        var content = new FormUrlEncodedContent(data);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();

                            if (responseData == "Something went wrong.")
                            {
                                ViewBag.ErrorMessage = string.Format("Something went wrong.");
                            }
                            else
                            {
                                // Update successful.
                                return RedirectToAction("HiveUserStatus", new
                                {
                                    userID = TempData.userId, //selected user

                                    //admin info
                                    Acc_id = TempData.Acc_id
                                });
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("API request failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                    return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage });
                }
            }
            else
            {
                //userlog
                string formattedCurrentDateTime = DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();
                string origin = "Ban User Page";
                string sysResponse = "Success";
                string action = "Unban user";

                string url = "https://renthive.online/Admin_API/UnbanUser.php";
                try
                {
                    string userBanDate = "NULL";

                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            { "userBanDate", userBanDate},
                            //userlog
                            {"adminid", TempData.Acc_id.ToString()},
                            {"CurrentDate", formattedCurrentDateTime},
                            {"origin", origin},
                            {"sysResponse", sysResponse },
                            {"action", action }
                        };
                        var content = new FormUrlEncodedContent(data);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();

                            if (responseData == "Something went wrong.")
                            {
                                ViewBag.ErrorMessage = string.Format("Something went wrong.");
                            }
                            else
                            {
                                // Update successful.
                                return RedirectToAction("HiveUserStatus", new
                                {
                                    userID = TempData.userId, //selected user

                                    //admin info
                                    Acc_id = TempData.Acc_id
                                });
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("API request failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                    return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage });
                }
            }
            return RedirectToAction("HiveUserStatus", new
            {
                userID = TempData.userId, //selected user

                //admin info
                Acc_id = TempData.Acc_id,
                Acc_FirstName = TempData.Acc_FirstName,
                Acc_MiddleName = TempData.Acc_MiddleName,
                Acc_LastName = TempData.Acc_LastName,
                Acc_DisplayName = TempData.Acc_DisplayName,
                Acc_UserType = TempData.Acc_UserType
            });
        }
        [HttpPost]
        public async Task<IActionResult> WeekBanUser(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            if (TempData.Acc_Ban == "0" || TempData.Acc_Ban == null)
            {
                string url = "https://renthive.online/Admin_API/WeeKBanUser.php";
                try
                {
                    /*string userBanDate = DateTime.Now.ToString("MMMM dd, yyyy");*/
                    /*DateTime currentDate = DateTime.Now;
                    DateTime userBanEndDate = currentDate.AddDays(3);

                    string formattedCurrentDate = currentDate.ToString("MMMM dd, yyyy");
                    string formattedEndDate = userBanEndDate.ToString("MMMM dd, yyyy");*/

                    string formattedCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                    string formattedEndDate = DateTime.Now.AddDays(7).ToString("MMMM dd, yyyy");

                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            { "userBanDate", formattedCurrentDate },
                            { "userBanEnd", formattedEndDate },
                            { "strike", TempData.Acc_Strikes.ToString() }
                        };
                        var content = new FormUrlEncodedContent(data);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();

                            if (responseData == "Something went wrong.")
                            {
                                ViewBag.ErrorMessage = string.Format("Something went wrong.");
                            }
                            else
                            {
                                // Update successful.
                                return RedirectToAction("HiveUserStatus", new
                                {
                                    userID = TempData.userId, //selected user

                                });
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("API request failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                    return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage });
                }
            }
            return RedirectToAction("HiveUserStatus", new
            {
                userID = TempData.userId, //selected user
            });
        }

        [HttpPost]
        public async Task<IActionResult> HiveUserStatusPost2(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            if (TempData.NumHolder == 1)
            {
                string formattedCurrentDateTime = DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();
                string origin = "Delete Account Page";
                string sysResponse = "Success";
                string action = "Deactivate / Delete Account";

                //deactive account
                string url = "https://renthive.online/Admin_API/StatusTO_0.php";
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            //userlog
                            {"adminid", TempData.Acc_id.ToString() },
                            {"CurrentDate", formattedCurrentDateTime},
                            {"origin", origin},
                            {"sysResponse", sysResponse },
                            {"action", action }
                        };
                        var content = new FormUrlEncodedContent(data);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();

                            if (responseData == "Something went wrong.")
                            {
                                ViewBag.ErrorMessage = string.Format("Something went wrong.");
                            }
                            else
                            {
                                // Update successful.
                                return RedirectToAction("HiveUserStatus", new
                                {
                                    userID = TempData.userId, //selected user

                                    //admin info
                                    Acc_id = TempData.Acc_id
                                });

                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("API request failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                    return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage });
                }
            }
            else
            {
                string formattedCurrentDateTime = DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();
                string origin = "Delete Account Page";
                string sysResponse = "Success";
                string action = "Activate Account";

                //activate account
                string url = "https://renthive.online/Admin_API/StatusTO_1.php";
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            //userlog
                            {"adminId", TempData.Acc_id.ToString() },
                            {"CurrentDate", formattedCurrentDateTime},
                            {"origin", origin},
                            {"sysResponse", sysResponse },
                            {"action", action }
                        };
                        var content = new FormUrlEncodedContent(data);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();

                            if (responseData == "Something went wrong.")
                            {
                                ViewBag.ErrorMessage = string.Format("Something went wrong.");
                            }
                            else
                            {
                                // Update successful.
                                return RedirectToAction("HiveUserStatus", new
                                {
                                    userID = TempData.userId, //selected user

                                    //admin info
                                    Acc_id = TempData.Acc_id
                                }) ;

                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("API request failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = string.Format("There was an error processing your request.");
                    return RedirectToAction("ErrorMessage", "ErrorView", new { ErrorMessage = ViewBag.ErrorMessage });
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult UserPayment_Details(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            ViewBag.userId = TempData.userId;
            ViewBag.Trans_id = TempData.Trans_id;
            ViewBag.About = TempData.About;
            ViewBag.PaymentAmount = TempData.PaymentAmount;
            ViewBag.PaymentMode = TempData.PaymentMode;
            ViewBag.AdvDeposit = TempData.AdvDeposit;
            ViewBag.Delivery = TempData.Delivery;
            ViewBag.TransDetails = TempData.TransDetails;
            ViewBag.Status = TempData.Status;
            ViewBag.TransDate = TempData.TransDate;

            ViewBag.Acc_Id = TempData.Acc_id;

            string formattedCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
            string formattedTime = DateTime.Now.ToString("hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();

            //user
            ViewBag.CurrentDate = formattedCurrentDate;
            ViewBag.CurrentTime = formattedTime;
            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
            ViewBag.Acc_LastName = TempData.Acc_LastName;

            //admin
            ViewBag.Temp_FirstName = TempData.Temp_FirstName;
            ViewBag.Temp_MiddleName = TempData.Temp_MiddleName;
            ViewBag.Temp_LastName = TempData.Temp_LastName;

            return View();
        }
    }
}