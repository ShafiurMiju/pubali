using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Pubali.Models;
using System.Data;
using System.Drawing;
using System.Reflection.Metadata;

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
            try
            {
                var documents = new List<Law>();

                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new OracleCommand("select * from law", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                documents.Add(new Law
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    FileName = reader["file_name"].ToString(),
                                    ContentType = reader["content_type"].ToString(),
                                    FileSize = (Convert.ToInt32(reader["file_size"])/1024),
                                    UploadedBy = reader["uploaded_by"].ToString(),
                                    UploadDate = Convert.ToDateTime(reader["upload_date"])
                                });
                            }
                        }
                    }

                }
                return View(documents);
            }
            catch { return View("Error"); }
        }

        public IActionResult Download(int id)
        {
            using (var connection = new OracleConnection(connectionString))
            {
                connection.Open();
                var command = new OracleCommand("SELECT id, file_name, file_content, content_type, file_size, uploaded_by, upload_date FROM Law WHERE id = :id", connection);
                command.Parameters.Add(new OracleParameter("id", id));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return File(reader["file_content"] as byte[], reader["content_type"].ToString(), reader["file_name"].ToString());
                    }
                }
            }

            return NotFound();
        }

        public IActionResult Case()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return RedirectToAction("law");
            }

            var fileName = Path.GetFileName(file.FileName);
            var contentType = file.ContentType;
            long fileSize = file.Length;
            var uploadedBy = "User";
            var uploadDate = DateTime.Now;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyToAsync(memoryStream);

                if (memoryStream.Length == 0)
                {
                    return RedirectToAction("law");
                }

                var fileContent = memoryStream.ToArray();

                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    var command = new OracleCommand("INSERT INTO Law (file_name, file_content, content_type, file_size, uploaded_by, upload_date) VALUES (:fileName, :fileContent, :contentType, :fileSize, :uploadedBy, :uploadDate)", connection);

                    command.Parameters.Add("fileName", OracleDbType.Varchar2).Value = fileName;
                    command.Parameters.Add("fileContent", OracleDbType.Blob).Value = fileContent;
                    command.Parameters.Add("contentType", OracleDbType.Varchar2).Value = contentType;
                    command.Parameters.Add("fileSize", OracleDbType.Int64).Value = fileSize;
                    command.Parameters.Add("uploadedBy", OracleDbType.Varchar2).Value = uploadedBy;
                    command.Parameters.Add("uploadDate", OracleDbType.Date).Value = uploadDate;

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("law");
        }

        public IActionResult LawOfficer()
        {
            return View();
        }

    }
}
