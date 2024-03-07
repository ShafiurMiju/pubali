using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Pubali.Models;
using System.Data;
using System.Diagnostics;

namespace Pubali.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly string connectionString;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login_User u)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    using (var _connectionString = new OracleConnection(connectionString))
                    {
                        _connectionString.Open();

                        using (var command = new OracleCommand("pubali.login_user", _connectionString))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.Add("p_username", OracleDbType.Varchar2, direction: ParameterDirection.Input).Value = u.Email;
                            command.Parameters.Add("p_password", OracleDbType.Varchar2, direction: ParameterDirection.Input).Value = u.Password;

                            command.Parameters.Add("p_result", OracleDbType.Int32, ParameterDirection.Output);

                            command.ExecuteNonQuery();

                            if ((command.Parameters["p_result"].Value).ToString() == "1")
                            {
                                HttpContext.Session.SetString("LoggedInUser", u.Email);
                                return RedirectToAction("Dashboard", "Dashboard");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Invalid Username or Password.");
                                return View(u);
                            }
                        }
                    }
                }
                return View(u);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
