using HRManagment.Models;
using HRManagment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using AutoMapper;
namespace HRManagment.Controllers
{
    public class EmployeeController : Controller
    {
        readonly IMapper _mapper;
        readonly HRManagmentContext _context;
        readonly DropDownService _dropDownService;
        readonly ILogger<EmployeeController> _logger;

        public EmployeeController
            (HRManagmentContext context, 
            IMapper mapper, 
            DropDownService dropDownService, 
            ILogger<EmployeeController> logger)
        {
            _context = context;
            _mapper = mapper;
            _dropDownService = dropDownService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }
        public IActionResult Details(int id)
        {
            var employee = _context.Employees.Include(e => e.Department).SingleOrDefault(employee => employee.Id == id);
            if (employee == null)
            {
                _logger.LogWarning($"Employee with ID: {id} not found");
                return NotFound();
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            var viewModel = new EmployeeViewModel();
            //AddPositionGenderDepartmentListsForEmployeeViewModel(ref viewModel);
            viewModel.Departments = _dropDownService.GetDepartments();
            viewModel.Positions = _dropDownService.GetPositions();
            viewModel.Genders = _dropDownService.GetGenders();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeViewModel viewModel)
        {
            ModelState.Remove(nameof(EmployeeViewModel.Departments));
            ModelState.Remove(nameof(EmployeeViewModel.Genders));
            ModelState.Remove(nameof(EmployeeViewModel.Positions));
            if (!ModelState.IsValid)
            {
                //AddPositionGenderDepartmentListsForEmployeeViewModel(ref viewModel);
                viewModel.Departments = _dropDownService.GetDepartments();
                viewModel.Positions = _dropDownService.GetPositions();
                viewModel.Genders = _dropDownService.GetGenders();
                return View("AddEmployee", viewModel);
            }

            var employee = _mapper.Map<Employee>(viewModel);

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            var viewModel = new EmployeeViewModel();
            var employee = _context.Employees.SingleOrDefault(e => e.Id == id);

            if(employee == null)
            {
                _logger.LogWarning($"Employee with ID: {id} not found");
                return NotFound();
            }

            viewModel = _mapper.Map<EmployeeViewModel>(employee);

            //AddPositionGenderDepartmentListsForEmployeeViewModel(ref viewModel);
            viewModel.Departments = _dropDownService.GetDepartments();
            viewModel.Positions = _dropDownService.GetPositions();
            viewModel.Genders = _dropDownService.GetGenders();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditEmployee(EmployeeViewModel viewModel)
        {
            var oldEmp = _context.Employees.SingleOrDefault(e => e.Id == viewModel.Id);

            if (oldEmp == null)
            {
                _logger.LogWarning($"Employee with ID: {viewModel.Id} not found");
                return NotFound();
            }

            //oldEmp = mapper.Map<Employee>(viewModel);
            _mapper.Map(viewModel, oldEmp);  //ensure that only updated properties will overwritten (vm->emp)

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.SingleOrDefault(e => e.Id == id);

            if (employee == null)
            {
                _logger.LogWarning($"Employee with ID: {id} not found");
                return NotFound();
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        void AddPositionGenderDepartmentListsForEmployeeViewModel(ref EmployeeViewModel employeeViewModel)
        {
            var departments = _context.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();

            var genders = Enum.GetValues<GenderType>()
                .Select(g => new SelectListItem
                {
                    Value = ((int)g).ToString(),
                    Text = g.ToString()
                }).ToList();

            var positions = Enum.GetValues<Position>()
                .Select(p => new SelectListItem
                {
                    Value = p.ToString(),
                    Text = p.ToString(),
                }).ToList();

            employeeViewModel.Positions = positions;
            employeeViewModel.Genders = genders;
            employeeViewModel.Departments = departments;

        }
        //(IEnumerable<SelectListItem> departments, IEnumerable<SelectListItem> positions, IEnumerable<SelectListItem> genders)
        //    PrepareEmployee()
        //{
        //    var departments = context.Departments
        //        .Select(d => new SelectListItem
        //        {
        //            Value = d.Id.ToString(),
        //            Text = d.Name
        //        }).ToList();

        //    var genders = Enum.GetValues<GenderType>()
        //        .Select(g => new SelectListItem
        //        {
        //            Value = g.ToString(),
        //            Text = g.ToString()
        //        }).ToList();

        //    var positions = Enum.GetValues<Position>()
        //        .Select(p => new SelectListItem
        //        {
        //            Value = p.ToString(),
        //            Text = p.ToString(),
        //        }).ToList();

        //    return (departments, positions, genders);
        //}
        public bool IsValidEmployee(Employee employee)
        {
            var emailPattern = @"^[a-zA-Z0-9._%+]+@[a-zA-Z0-9.-]\.[a-zA-Z]{2,}$";
            if (employee.FName == "" || employee.FName == null || employee.FName.Length > 49)
                return false;
            if (employee.LName == "" || employee.LName == null || employee.LName.Length > 49)
                return false;
            if (!Regex.IsMatch(employee.Email, emailPattern))
                return false;
            if (employee.Salary < 0)
                return false;
            if (employee.Position == "" || employee.Position == null || employee.Position.Length > 49)
                return false;
            //if (employee.Gender != GenderType.Male || employee.Gender != GenderType.Female)
            //    return false;
            if (Enum.IsDefined<GenderType>(employee.Gender))
                return false;

            return true;
        }
    }
}
