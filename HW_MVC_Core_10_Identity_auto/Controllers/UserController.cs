

namespace CW_MVC_Core_10_Auth2.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET method to display forms for creating users and roles
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST method to create a new user and assign them to a role
        [HttpPost]
        public async Task<IActionResult> Create(string email, string password, string role)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email and password are required.");
            }

            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Check if the role exists, create it if not
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                // Assign the user to the specified role
                await _userManager.AddToRoleAsync(user, role);

                return Ok("User created and assigned to role.");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        // POST method to create a new role
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is required.");
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole { Name = roleName };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return Ok("Role created.");
                }
            }

            return BadRequest("Role already exists.");
        }

        // POST method to assign a user to a role
        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string email, string password, string role)
        {
            
                //var user = new IdentityUser { UserName = email, Email = email };
                //var result = await _userManager.CreateAsync(user, password);
                //if (result.Succeeded)
                //{
                //    await _userManager.AddToRoleAsync(user, role);
                //    return Ok("User created and role assigned.");
                //}
                //return BadRequest(result.Errors);
            
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                return NotFound("Role not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return Ok("User assigned to role.");
            }

            return BadRequest("Failed to assign user to role.");
        }

        // POST method to remove a user from a role
        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                return NotFound("Role not found.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                return Ok("User removed from role.");
            }

            return BadRequest("Failed to remove user from role.");
        }
    }

}

//using Microsoft.AspNetCore.Identity;   // Подключаем пространство имен для работы с UserManager и IdentityUser
//using Microsoft.AspNetCore.Mvc;       // Подключаем пространство имен для использования контроллеров и действий
//using System.Threading.Tasks;         // Подключаем пространство имен для работы с асинхронными методами

//public class UserController : Controller
//{
//    // Поле для хранения экземпляра UserManager, который отвечает за управление пользователями
//    private readonly UserManager<IdentityUser> _userManager;

//    // Конструктор контроллера. Через Dependency Injection (DI) внедряем UserManager для управления пользователями
//    public UserController(UserManager<IdentityUser> userManager)
//    {
//        _userManager = userManager;  // Присваиваем экземпляр UserManager локальной переменной
//    }

//    // GET-метод для отображения страницы с формами создания, удаления и обновления пользователей
//    [HttpGet]
//    public IActionResult Index()
//    {
//        // Возвращаем представление, которое отображает страницу с пользовательскими операциями
//        return View();
//    }

//    // POST-метод для создания нового пользователя
//    [HttpPost]
//    public async Task<IActionResult> Create(string email, string password)
//    {
//        // Проверяем, что email и пароль были переданы
//        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
//        {
//            // Если email или пароль отсутствуют, возвращаем ошибку
//            return BadRequest("Email и пароль обязательны.");
//        }

//        // Создаем нового пользователя IdentityUser с указанным email
//        var user = new IdentityUser { UserName = email, Email = email };

//        // Используем UserManager для создания пользователя с переданным паролем
//        var result = await _userManager.CreateAsync(user, password);

//        // Если создание прошло успешно
//        if (result.Succeeded)
//        {
//            // Возвращаем успешный ответ с сообщением
//            return Ok("Пользователь создан.");
//        }

//        // Если возникли ошибки, добавляем их в ModelState для отображения пользователю
//        foreach (var error in result.Errors)
//        {
//            ModelState.AddModelError(string.Empty, error.Description);
//        }

//        // Возвращаем ошибки, если не удалось создать пользователя
//        return BadRequest(ModelState);
//    }

//    // POST-метод для удаления пользователя по его идентификатору (userId)
//    [HttpPost]
//    public async Task<IActionResult> Delete(string userId)
//    {
//        // Находим пользователя по его идентификатору (userId)
//        var user = await _userManager.FindByIdAsync(userId);

//        // Если пользователь не найден, возвращаем ошибку "Не найден"
//        if (user == null)
//        {
//            return NotFound("Пользователь не найден.");
//        }

//        // Удаляем пользователя с помощью UserManager
//        var result = await _userManager.DeleteAsync(user);

//        // Если удаление прошло успешно, возвращаем сообщение об успешном удалении
//        if (result.Succeeded)
//        {
//            return Ok("Пользователь удален.");
//        }

//        // Если произошла ошибка, возвращаем сообщение об ошибке
//        return BadRequest("Не удалось удалить пользователя.");
//    }

//    // POST-метод для обновления данных пользователя (email) по его идентификатору (userId)
//    [HttpPost]
//    public async Task<IActionResult> Update(string userId, string newEmail)
//    {
//        // Находим пользователя по его идентификатору (userId)
//        var user = await _userManager.FindByIdAsync(userId);

//        // Если пользователь не найден, возвращаем ошибку "Не найден"
//        if (user == null)
//        {
//            return NotFound("Пользователь не найден.");
//        }

//        // Обновляем email и имя пользователя
//        user.Email = newEmail;
//        user.UserName = newEmail;

//        // Используем UserManager для обновления данных пользователя
//        var result = await _userManager.UpdateAsync(user);

//        // Если обновление прошло успешно, возвращаем сообщение об успешном обновлении
//        if (result.Succeeded)
//        {
//            return Ok("Данные пользователя обновлены.");
//        }

//        // Если произошла ошибка, возвращаем сообщение об ошибке
//        return BadRequest("Не удалось обновить данные пользователя.");
//    }
//}

