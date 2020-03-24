using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using APBDCw3.DAL;
using APBDCw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBDCw3.Controllers
{


    [Route("api/students")]
    [ApiController]   

    public class StudentsController : ControllerBase
    {
        
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            var list = new List<Student>();
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18812; Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = client;

                command.CommandText = "select * from Student";
                client.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Student()
                    {
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        IndexNumber = reader["IndexNumber"].ToString(),
                        BirthDate = DateTime.Parse(reader["BirthDate"].ToString())

                    });
                }
            }
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            var list = new List<Object>();
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18812; Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = client;

                command.CommandText = "select e.Semester Semester," +
                                      "  st.Name StudiesName from Student s" +
                                      "  inner join Enrollment e on s.IdEnrollment=e.IdEnrollment" +
                                      "  inner join Studies st on e.IdStudy=st.IdStudy" +
                                      "  where s.IndexNumber=@id";
                command.Parameters.AddWithValue("id", id);


                client.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var studia = new
                    {
                        Semestr = reader["Semester"].ToString(),
                        StudiesName = reader["StudiesName"].ToString()
                    };
                    list.Add(studia);
                }
            }

            return Ok(list);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {        
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(int id)
        {
            return Ok("Aktualizacja dokończona");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Usuwanie ukończone");
        }
    }
}
