using JKNews.Data;
using JKNews.Extensions;
using JKNews.Models;
using JKNews.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace JKNews.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;

    public AccountController(UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.AsNoTracking().ToListAsync();
        var admins = (await _userManager.GetUsersInRoleAsync("Admin"))
            .Select(x => x.UserName);
        var editors = (await _userManager.GetUsersInRoleAsync("Editor"))
            .Select(x => x.UserName);
        ViewBag.Admins = admins;
        ViewBag.Editors = editors;
        return View(users);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel userReg)
    {
        if(!ModelState.IsValid)
            return View(userReg);

        // check if email exists
        var userDB = await _userManager.FindByEmailAsync(userReg.Email);
        if (userDB != null)
        {
            ModelState.AddModelError("Email",
                "Já existe um usuário cadastrado com este e-mail!");
            return View();
        }

        var user = new User
        {
            Name = userReg.Name,
            Email = userReg.Email,
            NormalizedEmail = userReg.Email.ToLower().Trim(),
            UserName = userReg.Email,
            NormalizedUserName = userReg.Email.ToLower().Trim()
        };
        
        var result = await _userManager.CreateAsync(user, userReg.Password);
        if (result.Succeeded)
        {
            this.ShowMessage(
                "Usuário cadastrado com sucesso. " +
                "Use suas credenciais para entrar no sistema.");
            return RedirectToAction("Login");
        }
        else
        {
            this.ShowMessage("Erro ao cadastrar usuário.", true);
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(userReg);
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return NotFound();

        return View(user);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(RegisterViewModel user)
    {
        if (!ModelState.IsValid)
            return View(user);

        // check if email exists
        var userDB = await _userManager.FindByEmailAsync(user.Email);
        if (userDB == null)
        {
            ModelState.AddModelError("Email",
                "Não existe um usuário cadastrado com este e-mail!");
            return View();
        }

        var result = await _userManager.UpdateAsync(userDB);
        if (result.Succeeded)
        {
            this.ShowMessage("Usuário editado com sucesso.");
            return RedirectToAction("Index");
        }
        else
        {
            this.ShowMessage("Não foi possível alterar o usuário.", true);
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(user);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel login)
    {
        if (!ModelState.IsValid)
            return View(login);

        var result = await _signInManager.PasswordSignInAsync(
        login.Email, login.Password, true, false);

        if (result.Succeeded)
        {
            return LocalRedirect(login.ReturnUrl ?? "~/");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos!");
            return View(login);
        }
    }

    [Authorize]
    public async Task<IActionResult> Logout(string? returnUrl)
    {
        await _signInManager.SignOutAsync();

        if (returnUrl != null)
            return LocalRedirect(returnUrl);
        else
            return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddEditor(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.AddToRoleAsync(user, "Editor");

        if (result.Succeeded)
        {
            this.ShowMessage(
                $"Perfil editor adicionado para {user.UserName}", true);

        }
        else
        {
            this.ShowMessage(
                $"Não foi possível adicionar o editor para {user.UserName}", true);
        }
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddAdmin(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.AddToRoleAsync(user, "Admin");

        if (result.Succeeded)
        {
            this.ShowMessage(
                $"Perfil adm adicionado para {user.UserName}", true);

        }
        else
        {
            this.ShowMessage(
                $"Não foi possível adicionar o adm para {user.UserName}", true);
        }
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemEditor(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.RemoveFromRoleAsync(user, "Editor");

        if (result.Succeeded)
        {
            this.ShowMessage(
                $"Perfil editor removido para {user.UserName}", true);
        }
        else
        {
            this.ShowMessage(
                $"Não foi possível remover o editor para {user.UserName}", true);
        }
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemAdmin(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.RemoveFromRoleAsync(user, "admin");

        if (result.Succeeded)
        {
            this.ShowMessage(
                $"Perfil adm removido para {user.UserName}", true);
        }
        else
        {
            this.ShowMessage(
                $"Não foi possível remover o adm para {user.UserName}", true);
        }
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            this.ShowMessage("Usuário não encontrado.", true);
            return RedirectToAction(nameof(Index));
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Admin")) {
            this.ShowMessage("Administrador não pode ser excluído!", true);
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
            this.ShowMessage("Usuário excluído com sucesso.");
        else
            this.ShowMessage("Não foi possível excluir o usuário.", true);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult RestrictAccess(string returnUrl)
    {
        return View(model: returnUrl);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadImage(UploadImageViewModel model, int id)
    {
        var filename = $"{Guid.NewGuid().ToString()}.jpg";
        var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");
        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{filename}", bytes);
        }
        catch
        {
            this.ShowMessage("Upload falhou.", true);
            return RedirectToAction(nameof(Index));
        }

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            this.ShowMessage("Usuário não encontrado.", true);
            return RedirectToAction(nameof(Index));
        }

        user.Image = $"https://localhost:7218/images/{filename}";

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            this.ShowMessage("Usuário editado com sucesso.");
            return RedirectToAction("Index");
        }
        else
        {
            this.ShowMessage("Não foi possível alterar o usuário.", true);
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(user);
    } 
}
