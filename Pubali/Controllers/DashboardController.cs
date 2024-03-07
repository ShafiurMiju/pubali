using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Pubali.Models;
using System.Data;
using System.Drawing;

namespace Pubali.Controllers
{
    public class DashboardController : Controller
    {

        private readonly ILogger<DashboardController> _logger;
        private readonly string connectionString;

        public DashboardController(ILogger<DashboardController> logger, IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
            _logger = logger;
        }


        public IActionResult Dashboard()
        {
            try
            {
                var cases = new List<Case>();

                var area_bar = new List<AreaBar>();

                using (var _connectionString = new OracleConnection(connectionString))
                {
                    _connectionString.Open();

                    using (var command = new OracleCommand("pubali.get_all_cases", _connectionString))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        OracleParameter pCases = new OracleParameter("p_cases", OracleDbType.RefCursor){Direction = ParameterDirection.Output};
                        command.Parameters.Add(pCases);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var caseItem = new Case
                                {
                                    CaseId = Convert.ToInt32(reader["CaseId"]),
                                    CaseName = reader["CaseName"].ToString(),
                                    AccountName = reader["AccountName"].ToString(),
                                    CourtDate = Convert.ToDateTime(reader["CourtDate"]),
                                    Status = reader["Status"].ToString()
                                };

                                cases.Add(caseItem);
                            }
                        }
                    }

                    using (OracleCommand cmd = new OracleCommand("pubali.get_chart_data", _connectionString))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter refCursor = new OracleParameter
                        {
                            OracleDbType = OracleDbType.RefCursor,
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(refCursor);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                area_bar.Add(new AreaBar
                                {
                                    AreaName = reader["area_name"].ToString(),
                                    Percentage = (decimal)reader["percentage"],
                                    Status = reader["status"].ToString()
                                });
                            }
                        }
                    }
                }

                ViewBag.Cases = cases;

                return View(area_bar);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public IActionResult Law()
        {
            return View();
        }

        public IActionResult Case()
        {
            return View();
        }
    }
}
