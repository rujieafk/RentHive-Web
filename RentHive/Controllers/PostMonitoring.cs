using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentHive.Models;
using System.Globalization;

namespace RentHive.Controllers
{
    public class PostMonitoring : Controller
    {
        public async Task<IActionResult> HiveUserPostList(UserDataGetter TempData)
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
                                var userObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                                ViewBag.Acc_Id = TempData.Acc_id;
                                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                                ViewBag.Acc_LastName = TempData.Acc_LastName;
                                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                                ViewBag.Acc_UserType = TempData.Acc_UserType;
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
        public async Task<IActionResult> HiveUserPostDetails(int userID, UserDataGetter TempData)
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
                        int userId = userID;
                        var data = new Dictionary<string, string>
                        {
                            {"userId", userId.ToString()}
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

                                ViewBag.UserId = userID; // the selected user

                                //Admin info
                                ViewBag.Acc_Id = TempData.Acc_id;
                                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                                ViewBag.Acc_LastName = TempData.Acc_LastName;
                                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                                ViewBag.Acc_UserType = TempData.Acc_UserType;

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
        public async Task<IActionResult> ListOfPost(UserDataGetter TempData, string sortBy)
        {
            try
            {
                var userData = HttpContext.Session.GetString("UserData");
                if (string.IsNullOrEmpty(userData))
                {
                    return RedirectToAction("Login", "UserManagement");
                }

                string url = "https://renthive.online/Admin_API/PostListGetter.php";
                string formattedCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                string formattedTime = DateTime.Now.ToString("hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();

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
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = "No users found.";
                        }
                        else
                        {
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);

                            switch (sortBy)
                            {
                                case "Post_id":
                                    newuserObject = newuserObject.OrderByDescending(item => item.Post_id).ToList();
                                    break;
                                case "Post_Title":
                                    newuserObject = newuserObject.OrderBy(item => item.Post_Title).ToList();
                                    break;
                                case "Rental_Category":
                                    newuserObject = newuserObject.OrderBy(item => item.Rental_Category).ToList();
                                    break;
                                case "Post_Status":
                                    newuserObject = newuserObject.OrderBy(item => item.Post_Status).ToList();
                                    break;
                                case "Post_DatePosted":
                                    newuserObject = newuserObject.OrderBy(item => item.Post_DatePosted).ToList();
                                    break;
                                case "1":
                                    newuserObject = newuserObject.Where(item => item.Rental_Category == "item").ToList();
                                    break;
                                case "2":
                                    newuserObject = newuserObject.Where(item => item.Rental_Category == "storage").ToList();
                                    break;
                                case "3":
                                    newuserObject = newuserObject.Where(item => item.Rental_Category == "living space").ToList();
                                    break;
                                default:
                                    newuserObject = newuserObject.OrderBy(item => item.Post_id).ToList();
                                    break;
                            }

                            ViewBag.SortedModel = newuserObject;
                            ViewBag.userId = TempData.userId;
                            ViewBag.Acc_id = TempData.Acc_id;

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
                        ViewBag.ErrorMessage = "API request failed";
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
        public async Task<IActionResult> SelectedPostDetails(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/PostDetailsGetter.php";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                        {
                            {"userId", TempData.userId.ToString() },
                            {"postId", TempData.Post_id.ToString() }
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
                            ViewBag.Acc_id = TempData.Acc_id;
                            ViewBag.userId = TempData.userId;
                             
                            //user
                            ViewBag.CurrentDate = DateTime.Now.ToString("MMMM dd, yyyy"); ;
                            ViewBag.CurrentTime = DateTime.Now.ToString("hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;

                            //admin
                            ViewBag.Temp_FirstName = TempData.Temp_FirstName;
                            ViewBag.Temp_MiddleName = TempData.Temp_MiddleName;
                            ViewBag.Temp_LastName = TempData.Temp_LastName;
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
        public async Task<IActionResult> StatusChecker(UserDataGetter TempData)
        {
            string url = "https://renthive.online/Admin_API/StatusChecker.php";

            try
            {
                string formattedCurrentDateTime = DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();
                string origin = "Posts Page";
                string sysResponse = "Success";

                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            { "postId", TempData.Post_id.ToString() },
                            { "numHolder", TempData.NumHolder.ToString() },

                            {"adminid" , TempData.Acc_id.ToString()},
                            {"CurrentDate", formattedCurrentDateTime},
                            {"origin", origin},
                            {"sysResponse", sysResponse },
                        };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "Update successful.")
                        {
                            // Update successful.
                            return RedirectToAction("SelectedPostDetails", new
                            {
                                userId = TempData.userId,
                                Post_id = TempData.Post_id,
                                Acc_id = TempData.Acc_id
                            });
                        }
                        else
                        {
                            ViewBag.ErrorMessage = responseData;
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "API request failed";
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


        public async Task<IActionResult> ListOfRenters(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/RenterListGetter.php";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                        {
                            {"postId", TempData.Post_id.ToString()}
                        };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

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

                            ViewBag.userId = TempData.userId;
                            ViewBag.Acc_id = TempData.Acc_id;
                            ViewBag.Post_id = TempData.Post_id;
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

        public async Task<IActionResult> SelectedRenterDetails(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/SelectedRenterGetter.php";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                        {
                            {"rentId", TempData.Rent_id.ToString()}
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
                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.Post_id = TempData.Post_id;
                            ViewBag.userId = TempData.userId;
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

        public async Task<IActionResult> ListOfRented(UserDataGetter TempData, string sortBy)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/RentedListGetter.php";
            try
            {

                string formattedCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                string formattedTime = DateTime.Now.ToString("hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();

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
                                case "Rent_id":
                                    newuserObject = newuserObject.OrderByDescending(item => item.Rent_id).ToList();
                                    break;
                                case "Post_Title":
                                    newuserObject = newuserObject.OrderByDescending(item => item.Post_Title).ToList();
                                    break;
                                case "Post_RentPeriod":
                                    newuserObject = newuserObject.OrderByDescending(item => item.Post_RentPeriod).ToList();
                                    break;
                                case "Post_Price":
                                    newuserObject = newuserObject.OrderByDescending(item => item.Post_Price).ToList();
                                    break;
                                case "1":
                                    newuserObject = newuserObject.Where(item => item.Post_RentPeriod == "term").ToList();
                                    break;
                                case "2":
                                    newuserObject = newuserObject.Where(item => item.Post_RentPeriod == "daily").ToList();
                                    break;
                                case "3":
                                    newuserObject = newuserObject.Where(item => item.Post_RentPeriod == "monthly").ToList();
                                    break;
                                default:
                                    // Default sorting, if sortBy is not recognized
                                    newuserObject = newuserObject.OrderBy(item => item.Rent_id).ToList();
                                    break;
                            }
                            ViewBag.userId = TempData.userId;
                            ViewBag.Acc_id = TempData.Acc_id;

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

        [HttpGet]
        public async Task<IActionResult> SelectedRentedDetails(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/RentalDetailsGetter.php";
            try
            {
                string formattedCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                string formattedTime = DateTime.Now.ToString("hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();

                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                        {
                            {"rentId", TempData.Rent_id.ToString() },
                            {"postId", TempData.Post_id.ToString() }
                        };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No record found")
                        {
                            ViewBag.ErrorMessage = string.Format("No record found.");
                        }
                        else
                        {
                            var userObject = JsonConvert.DeserializeObject<UserDataGetter>(responseData);
                            ViewBag.Acc_id = TempData.Acc_id;
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
        /*[HttpPost]
        public async Task<IActionResult> PrintCopy(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");

            if (string.IsNullOrEmpty(userData))
            {
                return RedirectToAction("Login", "UserManagement");
            }

            string url = "https://renthive.online/Admin_API/PrintCopy.php";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                    {
                        {"startDate",TempData.startDate .ToString() },
                        {"endDate", TempData.endDate.ToString() },
                        {"userId", TempData.userId.ToString() }
                    };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData.Contains("No record found"))
                        {
                            ViewBag.ErrorMessage = "No record found.";
                        }
                        else
                        {
                            var userObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            ViewBag.Acc_id = TempData.Acc_id;
                            return View(userObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "API request failed: " + response.ReasonPhrase;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "An error occurred while processing the request.";
                ViewBag.Acc_id = TempData.Acc_id;
                return View(new List<UserDataGetter>());
            }

            return View();
        }*/

    }
}