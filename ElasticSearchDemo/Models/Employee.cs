using System;
using System.Collections.Generic;
namespace ElasticSearchDemo.Models
{

    public class Employee
    {
  
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsEmployeePermanent { get; set; }

        public List<Skill> Skills { get; set; }

        public List<Employee> Employees { get; set; }

        public int Salary { get; set; }

        public string Email { get; set; }

    }
    public class Skill
    {
        public string Name { get; set; }

        public int Proficiency { get; set; }
    }
}
