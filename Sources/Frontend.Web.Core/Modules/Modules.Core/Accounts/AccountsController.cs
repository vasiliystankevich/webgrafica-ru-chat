using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dal;
using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.Common;
using Libraries.Core.Backend.WebApi.Repositories;
using Project.Kernel;
using WebMatrix.WebData;

namespace Modules.Core.Accounts
{

    [InitializeSimpleMembership]
    public class AccountsController : BaseController
    {
        public AccountsController(IIdentityRepository identityRepository, IWrapper<Random> random, ISystemMessagesRepository systemMessagesRepository)
        {
            Random = random;
            IdentityRepository = identityRepository;
            SystemMessagesRepository = systemMessagesRepository;
        }

        [AllowAnonymous]
        [InitializeSimpleMembership]
        public async Task<ActionResult> Login(string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return await GeneratorActionResult("~/Accounts/Login/action.cshtml", new LoginModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [InitializeSimpleMembership]
        public async Task<ActionResult> Login(LoginModel model)
        {
            try
            {
                IdentityRepository.RemoveUsers(new TimeSpan(0, 30, 0));
                var password = $"{Random.Instance.Next(100000, 1000000)}";
                IdentityRepository.CreateUser(
                    new AccountModel {AccountName = model.Account, IsActivate = true, Role = ERoles.User}, password);
                if (WebSecurity.Login(model.Account, password))
                {
                    SystemMessagesRepository.Send($"Пользователь {model.Account} вошел в чат.");
                    var result = TempData["ReturnUrl"] != null
                        ? (ActionResult)Redirect((string)TempData["ReturnUrl"])
                        : RedirectToAction("Index", "Home");
                    return await GetAsyncResult(result);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", exception.Message);
            }
            return await GeneratorActionResult("~/Accounts/Login/action.cshtml", new LoginModel());
        }

        [AllowAnonymous]
        [InitializeSimpleMembership]
        public async Task<RedirectToRouteResult> Logout()
        {
            var userName = User.Identity.Name;
            WebSecurity.Logout();
            IdentityRepository.DeleteUser(userName);
            var action = RedirectToAction("Index", "Home");
            return await GetAsyncResult(action);
        }
        public IWrapper<Random> Random { get; set; }
        public ISystemMessagesRepository SystemMessagesRepository { get; set; }
        public IIdentityRepository IdentityRepository { get; set; }
    }
}
