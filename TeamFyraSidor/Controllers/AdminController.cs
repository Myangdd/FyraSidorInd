using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using Newtonsoft.Json;
using TeamFyraSidor.Data;
using TeamFyraSidor.Models.TableData;
using TeamFyraSidor.Models.VM;
using TeamFyraSidor.Service;

namespace TeamFyraSidor.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly IUSerRoleService _userRoleService;
        private readonly ISubscriptionTypeService _subTypeSer;

        private readonly ISubscriptionService _subscriptionSer;
        private readonly IInfoService _infoSer;

        //StatusMessage for Role admin
        [TempData]
        public required string StatusMessage { get; set; }

        public AdminController(ISubscriptionTypeService subTypeSer, IUSerRoleService userRoleService, ISubscriptionService subscriptionSer, IInfoService infoSer)
        {
            _subTypeSer = subTypeSer;
            _userRoleService = userRoleService;
            _subscriptionSer = subscriptionSer;
            _infoSer = infoSer;
        }

        public IActionResult Index()
        {
            return View();
        }



        // Below are Role CRUD operations
        public async Task<IActionResult> ListRoleName()
        {
            var listRoleName = await _userRoleService.ListRoleNameAsync();
            
            var listModel = listRoleName.Select(rn => new CreateRoleVM() { Name = rn }).ToList();
               
            return View(listModel);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _userRoleService.CreateRoleAsync(model.Name);
            if (result.Succeeded)
            {
                StatusMessage = "A new role has created successfully!";
                return RedirectToAction("ListRoleName");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }      
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                StatusMessage = "Error: Role name is empty/null!";
                return View();
            }
            var role = await _userRoleService.FindRoleByNameAsync(roleName);
            if (role == null)
            {
                StatusMessage = "Error: Role doesn't exist.";
                return View();
            }
            
            var model = new EditRoleVM { Id = role.Id, Name = role.Name! };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await _userRoleService.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError(string.Empty, "Role is already taken.");
                return View(model);
            }
            var role = await _userRoleService.FindRoleByIdAsync(model.Id);
            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Role doesn't exist.");
                return View(model);
            }
            role.Name = model.Name;
            var result = await _userRoleService.UpdateRoleAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = "Role name edited successfully!";
                return RedirectToAction("ListRoleName");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                StatusMessage = "Error: Role name is null!";
                return View();
            }
            var role = await _userRoleService.FindRoleByNameAsync(roleName);
            if (role == null)
            {
                StatusMessage = "Error: Role doesn't exist.";
                return View();
            }
            var model = new EditRoleVM { Id = role.Id, Name = role.Name! };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(EditRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }           
            var role = await _userRoleService.FindRoleByIdAsync(model.Id);
            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Role doesn't exist.");
                return View(model);
            }
            var result = await _userRoleService.DeleteRoleAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = "Role is deleted successfully!";
                return RedirectToAction("ListRoleName");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }



        // Below are Employee CRUD operations
        public async Task<IActionResult> ListEmployeeRole()
        {
            var listEmployeeRoles = await _userRoleService.ListEmployeeRolesAsync();

            return View(listEmployeeRoles);
        }

        [HttpGet]
        public async Task<IActionResult> RegisterUserRole()
        {
            var listRoleName = await _userRoleService.ListRoleNameAsync();
            TempData["listRoleName"] = JsonConvert.SerializeObject(listRoleName);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUserRole(CreateUserRoleVM model)
        {
            var listRoleName = await _userRoleService.ListRoleNameAsync();
            TempData["listRoleName"] = JsonConvert.SerializeObject(listRoleName);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var employee = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DOB = model.DOB,
                UserName = model.Email
            };

            var resultUser = await _userRoleService.RegisterEmployeeAsync(employee, model.Password);
            employee.EmailConfirmed = true;

            if (!resultUser.Succeeded)
            {
                foreach (var error in resultUser.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            else
            {
                var resultRole = await _userRoleService.AddToRoleAsync(employee, model.RoleName);

                if (resultRole.Succeeded)
                {
                    StatusMessage = "User with his/her role is registed successfully!";
                    return RedirectToAction("ListEmployeeRole");
                }
                else
                {
                    foreach (var error in resultRole.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
        }

        public async Task<IActionResult> DetailUserRole(string email)
        {
            var model = new EditUserAndRoleVM();
            if (string.IsNullOrEmpty(email))
            {
                model.ErrorMsg = "Email can not be null.";
                return View(model);
            }

            model = await _userRoleService.FindUserRolesByEmailAsync(email);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserInfo(string id)
        {
            var model = new EditUserVM();
            if (string.IsNullOrEmpty(id))
            {
                StatusMessage = "Id can not be null.";
                return View(model);
            }
            var user = await _userRoleService.FindUserByIdAsync(id);
            if (user == null)
            {
                StatusMessage = $"User with Id: {id} doesn't exsit.";
                return View(model);
            }
            var roles = await _userRoleService.GetRoleNamesAsync(user);
            model.Id = user.Id;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            if (user.PhoneNumber != null)
            {
                model.PhoneNumber = user.PhoneNumber;
            }
            else
            {
                model.PhoneNumber = "";
            }
            model.Email = user.Email!;
            model.DOB = user.DOB;
            model.ListRoleName = new List<string>();
            if (roles!=null)
            {
                model.ListRoleName = roles.ToList();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserInfo(EditUserVM model)
        {           
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userRoleService.FindUserByIdAsync(model.Id);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"User with Id: {model.Id} doesn't exsit.");
                return View(model);
            }
            
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.DOB = model.DOB;
            user.PhoneNumber = model.PhoneNumber;
            var result = await _userRoleService.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                StatusMessage = "User info successfully updated.";               
                return RedirectToAction("DetailUserRole", "Admin", new{ email= user.Email});
            }
            else
            {                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditUserRole(string id)
        {
            var model = new EditUserRoleVM();
            var listAllRoleName = await _userRoleService.ListRoleNameAsync();
            TempData["ListAllRoleName"] = JsonConvert.SerializeObject(listAllRoleName);

            if (string.IsNullOrEmpty(id))
            {
                TempData["msg"] = "Id can not be null.";
                return View();
            }
            var user = await _userRoleService.FindUserByIdAsync(id);
            if (user == null)
            {
                TempData["msg"] = $"User with Id: {id} doesn't exsit.";
                return View();
            }
            var roles = await _userRoleService.GetRoleNamesAsync(user);
            var listRoleName = new List<string>();
            if (roles != null)
            {
                listRoleName = roles.ToList();
            }

            model.UserId = id;
            model.Name = user.FirstName + " " + user.LastName;
            model.Email = user.Email;
            model.ListRoleName = listRoleName;
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRole(EditUserRoleVM model, string actionType)
        {
            var listAllRoleName = await _userRoleService.ListRoleNameAsync();
            TempData["ListAllRoleName"] = JsonConvert.SerializeObject(listAllRoleName);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userRoleService.FindUserByIdAsync(model.UserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"User with Id: {model.UserId} doesn't exsit.");
                return View(model);
            }
            var roles = await _userRoleService.GetRoleNamesAsync(user);
            var listRoleName = new List<string>();
            if (roles != null)
            {
                listRoleName = roles.ToList();
            }
            if (actionType == "AddRole")
            {
                var result = await _userRoleService.AddToRoleAsync(user, model.RoleToDo);
                if (result.Succeeded)
                {
                    StatusMessage = "Role successfully added.";
                    return RedirectToAction("DetailUserRole", "Admin", new { email = user.Email});
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            else if (actionType == "RemoveRole")
            {
                var result = await _userRoleService.RemoveFromRoleAsync(user, model.RoleToDo);
                if (result.Succeeded)
                {
                    StatusMessage = "Role successfully removed.";
                    return RedirectToAction("DetailUserRole", "Admin", new { email = user.Email });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            return RedirectToAction("EditUserRole", new { id = model.UserId });
        }



        // Below are User admin: for non-employee
        public async Task<IActionResult> ListUserRole()
        {
            var listUserRoles = await _userRoleService.ListUserRolesAsync();

            return View(listUserRoles);
        }



        //below are Subscription Type admin

        [HttpGet]
            public IActionResult CreateSubscriptionType() // View for the form.
            {
                return View();
            }
            [HttpPost]
            public IActionResult CreateSubscriptionType(SubscriptionType subscription)
            {
                if (ModelState.IsValid)
                {
                    _subTypeSer.CreateSubscriptionType(subscription);
                    return RedirectToAction("Index"); // Something else than Index
                }
                return RedirectToAction("GetAllSubscriptionsTypes");
            }
        public IActionResult UpdateSubscriptionType(int id)
        {
            var sub = _subTypeSer.GetSubscriptionType(id);
            if (sub != null)
            {
                return View(sub);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult UpdateSubscriptionType(int id, SubscriptionType subscription)
        {
            if (id != subscription.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _subTypeSer.UpdateSubscriptionType(subscription);
                return RedirectToAction("Index"); // Something else than Index
            }
            return View(subscription);
        }
        [HttpGet]
        public IActionResult DeleteSubscriptionType(int id)
        {
            var sub = _subTypeSer.GetSubscriptionType(id);
            if (sub == null)
                return NotFound();

            return View(sub);

        }
        [HttpPost]
        public IActionResult DeleteSubscriptionTypeConfirmed(int id)
        {
            _subTypeSer.DeleteSubscriptionType(id);
            return RedirectToAction("GetAllSubscriptionsTypes"); //Change Later
        }
        public IActionResult GetSubscriptionType(int id)
        {
            var sub = _subTypeSer.GetSubscriptionType(id);
            if (sub != null)
            {
                return View(sub);
            }
            return NotFound();
        }
        public IActionResult GetAllSubscriptionsTypes()
        {
            var subs = _subTypeSer.GetAllSubscriptionsTypes();
            if (subs == null)
            {
                return NotFound();
            }
            return View(subs);
        }



        // display weather history fr 
        public IActionResult GetInfo()
        {

            var info = _infoSer.GetInfo();
            return View(info);
        }

        // display el price history fr azure table
        public IActionResult GetElPrice()
        {

            var data = _infoSer.GetElPriceFrAzureTable();
            return View(data);
        }



        // Subscription Statistics
        public IActionResult SubscriptionStatistics()
        {
            var list2024 = _subscriptionSer.GetSubscriberData(2024);
            var list2025 = _subscriptionSer.GetSubscriberData(2025);

            return Json(new {l24=list2024, l25=list2025});
        }

        // Show subscription statistics chart
        public IActionResult ChartDisplay()
        {
            return View();
        }

    }
}
