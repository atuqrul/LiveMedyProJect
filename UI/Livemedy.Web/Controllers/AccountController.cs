using DNTCaptcha.Core;
using Livemedy.Domain.Entities.Users;
using Livemedy.Domain.Repositories.Base;
using Livemedy.Domain.Repositories.Users;
using Livemedy.Services.Core.Helpers;
using Livemedy.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;
using static Livemedy.Services.Core.Helpers.GlobalEnums;

namespace Livemedy.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUnitOfWork _repository;
    private readonly IDNTCaptchaValidatorService _validatorService;

    public AccountController(IDNTCaptchaValidatorService validatorService, IUnitOfWork repository)
    {
        _validatorService = validatorService;
        _repository = repository;
    }

    public async Task<IActionResult> Users()
    {
        int customerCount = 0, managerCount = 0;
        var users = await _repository.Users.GetAllAsync(null);

        if (users != null)
        {
            customerCount = users.Where(u => u.RoleId == (int)Roles.Customer).Count();
            managerCount = users.Where(u => u.RoleId == (int)Roles.Manager).Count();
        }

        ViewBag.CustomerCount = customerCount;
        ViewBag.ManagerCount = managerCount;


        return View(users);
    }


    public IActionResult Login() => View(new Login());

    [HttpPost]
    [ValidateDNTCaptcha(ErrorMessage = "Lütfen güvenlik kodunu yazınız.",
    CaptchaGeneratorLanguage = Language.Turkish, CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbers)]
    public async Task<IActionResult> Login(Login login)
    {
        if (!_validatorService.HasRequestValidCaptchaEntry(Language.Turkish, DisplayMode.SumOfTwoNumbers))
        {
            TempData["captchaError"] = "Lütfen geçerli güvenlik kodunu giriniz.";
            return View(login);
        }

        var user = await _repository.Users.GetAsync(u => u.EmailAddress == login.EmailAddress && u.Password == login.Password);

        if (user == null)
        {
            TempData["Error"] = "Hatalı bilgi. Tekrar deneyiniz";
            return View(login);
        }
        else
        {
            List<Claim> userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Name, user.EmailAddress));
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            userClaims.Add(new Claim(ClaimTypes.Role,user.RoleId.ToString()));

            var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }
    }


    public IActionResult Register(string em = "")
    {
        Register model = new Register() { RoleId = (int)Roles.Customer };

        if (em != "")
        {
            model.EmailAddress = Encryption.Decrypt(em);
            model.RoleId = (int)Roles.Manager;
        }
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Register(Register register)
    {
        if (!ModelState.IsValid)
        {
            return View(register);
        }

        var user = await _repository.Users.GetAsync(u => u.EmailAddress == register.EmailAddress);

        if (user != null)
        {
            TempData["Error"] = "Bu E-mail adresi ile daha evvel kayıt olunmuş!!!";
            return View(register);
        }

        var names = register.FullName.Split(' ');

        if (names.Length == 1)
        {
            if (register.Password.Contains(names[0]))
            {
                TempData["Error"] = "İsminizi şifre içinde kullanamazsınız!!!";
                return View(register);
            }
        }
        else
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (register.Password.Contains(names[i]))
                {
                    TempData["Error"] += "İsminizi şifre içinde kullanamazsınız!!!";
                    return View(register);
                }
            }
        }


        bool errMsg = ValidatePassword(register.Password);

        if (!errMsg)
        {
            TempData["Error"] = "Şifreniz en az bir küçük harf,\r\nEn az bir büyük harf,\r\nEn az özel karakter,\r\nEn az bir numara\r\nEn az 8 karakter uzunluğunda olmalıdır";
            return View(register);
        }

        var newUser = new User()
        {
            EmailAddress = register.EmailAddress,
            IsDelete = false,
            Name = register.FullName,
            Password = register.Password,
            UserName = register.EmailAddress,
            RoleId = register.RoleId
        };

        var newUserResponse = await _repository.Users.AddAsync(newUser);
        if (newUserResponse != null)
            return View("RegisterCompleted");

        return Content("Hata Oluştu. Lütfen daha sonra tekrar deneyiniz");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    static bool ValidatePassword(string passWord)
    {
        int validConditions = 0;
        foreach (char c in passWord)
        {
            if (c >= 'a' && c <= 'z')
            {
                validConditions++;
                break;
            }
        }
        foreach (char c in passWord)
        {
            if (c >= 'A' && c <= 'Z')
            {
                validConditions++;
                break;
            }
        }
        if (validConditions == 0) return false;
        foreach (char c in passWord)
        {
            if (c >= '0' && c <= '9')
            {
                validConditions++;
                break;
            }
        }
        if (validConditions == 1) return false;
        if (validConditions == 2)
        {
            char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' }; // or whatever    
            if (passWord.IndexOfAny(special) == -1) return false;
        }
        return true;
    }
}


