using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CRUD_MVC_NET_Angular.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string EmpName { get; set; }

        public int Salary { get; set; }

        [StringLength(200)]
        public string DeptName { get; set; }

        [StringLength(200)]
        public string Designation { get; set; }
    }
}