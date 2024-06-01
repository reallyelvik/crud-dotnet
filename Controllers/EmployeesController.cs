using crud.Data;
using crud.Models;
using crud.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crud.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MyDbContext crudDbContext;

        public EmployeesController(MyDbContext crudDbContext)
        {
            this.crudDbContext = crudDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await crudDbContext.Employees.ToListAsync();
            return View(employees);

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest) 
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
            };
            await crudDbContext.Employees.AddAsync(employee);
            await crudDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id) 
        {
            var employee = await crudDbContext.Employees.FirstOrDefaultAsync(x=>x.Id == id);
            if(employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth,
                };
                return await Task.Run(()=>View("View",viewModel));
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult>View(UpdateEmployeeViewModel model)
        {
            var employee = await crudDbContext.Employees.FindAsync(model.Id);

            if (employee != null) {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await crudDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
;            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await crudDbContext.Employees.FindAsync(model.Id);
            if(employee != null)
            {
                crudDbContext.Employees.Remove(employee);
                await crudDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
