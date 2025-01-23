using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Projectapi.Models;
using AppContext = Projectapi.Models.appContext;

namespace Projectapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        AppContext context;
        public DepartmentController(AppContext _context)
        {
            context = _context;
        }

        //[HttpGet("count")]
        //public ActionResult<List<DeptWithEmpCountDTO>> DeptwITHEmpCount() 
        //{
        //    List<Department> list = context.Department.Include(d=>d.Employees).ToList();
        //    List<DeptWithEmpCountDTO> DDTOlist = new List<DeptWithEmpCountDTO>();
        //    foreach (Department department in list) 
        //    { 
        //        DeptWithEmpCountDTO dto = new DeptWithEmpCountDTO();
        //        dto.Id = department.Id;
        //        dto.Name = department.Name;
        //        dto.count = department.Employees.Count();
        //        DDTOlist.Add(dto);
        //    }
        //    return Ok(DDTOlist);
        //}
        [HttpGet]
        public IActionResult displayAll()
        {
            List<Department> list = context.Department.ToList();
            return Ok(list);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetbyId(int id)
        {
            Department dept = context.Department.FirstOrDefault(d => d.Id == id);
            return Ok(dept);
        }

        [HttpPost]
        public IActionResult AddDept(Department dept)
        {

            context.Department.Add(dept);
            context.SaveChanges();
            // return Created($"http://localhost:5000/api/Department/{dept.Id}", dept);
            return CreatedAtAction("GetbyId", dept);
        }
    }
}
