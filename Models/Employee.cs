using HRManagment.Validation;
using HRManagment.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Protocol;

namespace HRManagment.Models
{
    public class Employee /*: IEnumerator<Employee>, IEnumerable<Employee>*/
    {
        //public Employee(EmployeeViewModel viewModel)
        //{
        //    Id = viewModel.Id;
        //    SSN = viewModel.SSN;
        //    FName = viewModel.FName;
        //    LName = viewModel.LName;
        //    Email = viewModel.Email;
        //    Phone = viewModel.Phone;
        //    HireDate = viewModel.HireDate;
        //    DateOfBirth = viewModel.DateOfBirth;
        //    EmergencyContact = viewModel.EmergencyContact;
        //    Position = viewModel.Position.ToString();
        //    Salary = viewModel.Salary;
        //    Address = viewModel.Address;
        //    Gender = viewModel.Gender;
        //    DepartmentId = viewModel.DepartmentId;
        //}

        [Key]
        public int Id { get; set; }

        [Required]
        [Length(14, 14)]
        public string SSN { get; set; }

        [Required]
        [StringLength(50)]
        public string FName { get; set; }

        [Required]
        [StringLength(50)]
        public string LName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        public string Position { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Salary { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public GenderType Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Phone]
        public string EmergencyContact { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        //Navigation Property
        [JsonIgnore]
        public virtual Department Department { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<LeaveRequest> RequestedLeaveRequests { get; set; }
        public virtual ICollection<LeaveRequest> ApprovededLeaveRequests { get; set; }


        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
            return $"Name: {FName} {LName}, SSN: {SSN}, " +
                $"Email: {Salary}, Phone: {Phone}, Salary: {Salary}, " +
                $"Position: {Position}, Gender: {Gender}, Address: {Address}, " +
                $"Department: {Department.Name}";
        }

        public static string operator -(Employee? newEmp, Employee? oldEmp)
        {
            if (newEmp != null && oldEmp == null)
                return JsonSerializer.Serialize(newEmp);
            else if (newEmp == null && oldEmp != null)
                return "No data";

            var changedItemsList = new Dictionary<string, string>();
            //Type type = newEmp?.GetType();
            var Properties = typeof(Employee).GetProperties();

            foreach (var property in Properties)
            {
                if (property.GetValue(newEmp) != null
                    && property.GetValue(newEmp)?.ToString() != property.GetValue(oldEmp)?.ToString())
                {
                    changedItemsList.Add(property.Name, property.GetValue(newEmp).ToString());
                }
            }

            return JsonSerializer.Serialize(changedItemsList);
            //return JsonConvert.SerializeObject(changedItemsList, 
            //    new JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });

        }

        public static Employee Clone(Employee originEmployee)
        {
            Type type = originEmployee.GetType();
            var Properties = type.GetProperties();
            var employeeClone = new Employee();
            foreach (var prop in Properties)
            {
                prop.SetValue(employeeClone, prop.GetValue(originEmployee));
            }
            return employeeClone;
        }
    }
}
