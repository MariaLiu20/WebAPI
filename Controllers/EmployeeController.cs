using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            DataTable table = new DataTable();
            string query = @"SELECT * FROM dbo.Employees";

            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return StatusCode(500, "Connection string 'DefaultConnection' is not configured.");
            }

            using (var con = new SqlConnection(connectionString))
            {
                await con.OpenAsync();
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }
            }

            var result = table.AsEnumerable()
                .Select(row => table.Columns.Cast<DataColumn>()
                .ToDictionary(col => col.ColumnName, col => row[col]))
                .ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee emp)
        {
            try
            {
                DataTable table = new DataTable();
                string query = @"INSERT INTO dbo.Employees (EmployeeName, Employee, MailID, DOJ) values 
                ('" + emp.EmployeeName + @"',
                '" + emp.Department + @"',
                '" + emp.MailID + @"',
                '" + emp.DOJ + @"')";

                string? connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return StatusCode(500, "Connection string 'DefaultConnection' is not configured.");
                }

                long insertedId = 0;
                using (var con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@name", emp.EmployeeName);
                        var scalar = await cmd.ExecuteScalarAsync();
                        if (scalar != null && long.TryParse(scalar.ToString(), out var id))
                        {
                            insertedId = id;
                        }
                    }
                }

                return Ok(new { success = true, id = insertedId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Employee emp)
        {
            try
            {
                DataTable table = new DataTable();
                string query = @"UPDATE dbo.Employees set EmployeeName = '" + emp.EmployeeName + @"',
                    Department = '" + emp.Department + @"',
                    MailID = '" + emp.MailID + @"',
                    DOJ = '" + emp.DOJ + @"'
                    WHERE EmployeeID = " + emp.EmployeeId + @"";

                string? connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return StatusCode(500, "Connection string 'DefaultConnection' is not configured.");
                }

                long insertedId = 0;
                using (var con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@name", emp.EmployeeName);
                        var scalar = await cmd.ExecuteScalarAsync();
                        if (scalar != null && long.TryParse(scalar.ToString(), out var id))
                        {
                            insertedId = id;
                        }
                    }
                }

                return Ok(new { success = true, id = insertedId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid id.");
            try
            {
                DataTable table = new DataTable();
                string query = @"DELETE FROM dbo.Employees WHERE EmployeeID = @id";

                string? connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return StatusCode(500, "Connection string 'DefaultConnection' is not configured.");
                }

                long insertedId = 0;
                using (var con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        var scalar = await cmd.ExecuteScalarAsync();
                        if (scalar != null && long.TryParse(scalar.ToString(), out var idResult))
                        {
                            insertedId = idResult;
                        }
                    }
                }

                return Ok(new { success = true, id = insertedId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
