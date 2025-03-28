using HRManagment.Models;
using HRManagment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using AutoMapper;
using HRManagment.Services;
using Azure.Core;
using System.Globalization;
namespace HRManagment.Controllers
{
    public class EmployeeController(IMapper mapper,
        ILogger<EmployeeController> logger,
        IEmployeeService employeeService,
        IAuditLogService auditLogService) : Controller
    {
        readonly IEmployeeService _employeeService = employeeService;
        readonly IAuditLogService _auditLogService = auditLogService;
        readonly IMapper _mapper = mapper;
        readonly ILogger<EmployeeController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.Page = page;
            (var employees, var employeesCount) = await _employeeService.GetEmployeesAsync(pageNumber: page);
            ViewBag.Emps = employeesCount;
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            var operationResult = await _employeeService.GetEmployeeByIdAsync(id);
            //will use cookies for temp message for user and redirect back
            if (!operationResult.Success)
                return NotFound();

            var employee = operationResult.Data;
            return View(employee);
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            var viewModel = new EmployeeViewModel();
            _employeeService.PopulateDropDownLists(viewModel);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(EmployeeViewModel viewModel)
        {

            await _auditLogService.SubscribeToOperationEvent();

            // not the best case and must replace with one
            ModelState.Remove(nameof(viewModel.Governorates));
            ModelState.Remove(nameof(viewModel.Positions));
            ModelState.Remove(nameof(viewModel.Genders));
            ModelState.Remove(nameof(viewModel.Departments));

            var operationResult = new OperationResult<Employee>();
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(viewModel);
                operationResult = await _employeeService.AddEmployeeAsync(employee);

                if (operationResult.Success)
                    return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = operationResult.Message;
            _employeeService.PopulateDropDownLists(viewModel);
            return View("AddEmployee", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var operationResult = await _employeeService.GetEmployeeByIdAsync(id);

            //will use cookies for temp message for user and redirect back
            if (!operationResult.Success)
                return NotFound(id);

            var employee = operationResult.Data;
            var viewModel = _mapper.Map<EmployeeViewModel>(employee);

            _employeeService.PopulateDropDownLists(viewModel);

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeViewModel viewModel)
        {
            await _auditLogService.SubscribeToOperationEvent();

            // not the best case and must replace with one
            ModelState.Remove(nameof(viewModel.Governorates));
            ModelState.Remove(nameof(viewModel.Positions));
            ModelState.Remove(nameof(viewModel.Genders));
            ModelState.Remove(nameof(viewModel.Departments));

            var operationResult = new OperationResult<Employee>();

            if (ModelState.IsValid)
            {
                operationResult = await _employeeService.GetEmployeeByIdAsync(viewModel.Id);

                if (operationResult.Success)
                {
                    var employee = operationResult.Data;
                    Employee oldEmp = Employee.Clone(operationResult.Data);

                    _mapper.Map(viewModel, employee);
                    operationResult = await _employeeService.UpdateEmployeeAsync(employee, oldEmp);

                    if (operationResult.Success)
                        return RedirectToAction("Index");
                }

            }
            TempData["ErrorMessage"] = operationResult.Message;
            _employeeService.PopulateDropDownLists(viewModel);
            return RedirectToAction("EditEmployee", viewModel.Id);

        }

        public async Task<IActionResult> Delete(int id)
        {
            _auditLogService.SubscribeToOperationEvent();

            var operationResult = await _employeeService.DeleteEmployeeAsync(id);

            //will use cookies for temp message for user and redirect back
            if (!operationResult.Success)
                return NotFound();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult positionSalary(string position)
        {
            object? intPosition;
            if (Enum.TryParse(typeof(Position), position, true, out intPosition))
            {
                int maximumSalary = (int)intPosition switch
                {
                    (int)Position.Manager => 20000,
                    (int)Position.Junior => 2000,
                    _ => 3500
                };

                return Ok(maximumSalary);
            }
            return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}
