# CRUD_MVC_NET_Angular
Tutorial on implementing CRUD with MVC and Angular

The AngularJS & Web API Implementation

Step 1
Using Visual Studio 2015 - New Project - Visual C# (Web) - ASP.NET Web Application- MVC

Step 2
Create the Class Employee under the Models/

```
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
```

Step 3
On your Visual Studio 2015 - go to Tools -> NuGet Package Manager -> Package Manager Console

Run the initial migration on the controllers
pm> enable-migrations

pm> add-migration {name_of_your_migration}

pm> update-database

Step 4:

Add the following code -  public DbSet<Employee> Employees { get; set; }
in the Models/IdentityModels.cs
```
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Employee> Employees { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
```

Step 5:
Create the database from the model
pm> add-migration [name of your migration]
pm> update-database

Step 6:
Add Web API project under Controllers/Api/EmployeesController
Right click on Controller/
Add - Controller (Web API 2 Controllers with actions, using Entity Framework)

Step 7:
We need to add AngularJS to the current project. To do so, right-click on the project name and select ‘Manager NuGet Package’. From the Package Nuget Manager Window search for AngularJS as shown below and add it to the current project.

AngularJs Core - enhanced for web apps!

Step 8:
Add 3 files under Scripts/ :
- Module.js

```
/// <reference path="../angular.js" />
var app;
(function () {
    app = angular.module("crudModule", []);
})();
```

- Service.js
```
/// <reference path="../angular.js" />
/// <reference path="Module.js" />


app.service('crudService', function ($http) {


    //Create new record
    this.post = function (Employee) {
        var request = $http({
            method: "post",
            url: "/api/Employees",
            data: Employee
        });
        return request;
    }
    //Get Single Records
    this.get = function (EmpNo) {
        return $http.get("/api/Employees/" + EmpNo);
    }

    //Get All Employees
    this.getEmployees = function () {
        return $http.get("/api/Employees");
    }


    //Update the Record
    this.put = function (EmpNo, Employee) {
        var request = $http({
            method: "put",
            url: "/api/Employees/" + EmpNo,
            data: Employee
        });
        return request;
    }
    //Delete the Record
    this.delete = function (EmpNo) {
        var request = $http({
            method: "delete",
            url: "/api/Employees/" + EmpNo
        });
        return request;
    }

});
```

- Controller.js
```

/// <reference path="../angular.js" />


/// <reference path="Module.js" />

//The controller is having 'crudService' dependency.
//This controller makes call to methods from the service 
app.controller('crudController', function ($scope, crudService) {

    $scope.IsNewRecord = 1; //The flag for the new record

    loadRecords();

    //Function to load all Employee records
    function loadRecords() {
        var promiseGet = crudService.getEmployees(); //The MEthod Call from service

        promiseGet.then(function (pl) { $scope.Employees = pl.data },
              function (errorPl) {
                  $log.error('failure loading Employee', errorPl);
              });
    }



    //The Save scope method use to define the Employee object.
    //In this method if IsNewRecord is not zero then Update Employee else 
    //Create the Employee information to the server
    $scope.save = function () {
        var Employee = {
            EmpNo: $scope.EmpNo,
            EmpName: $scope.EmpName,
            Salary: $scope.Salary,
            DeptName: $scope.DeptName,
            Designation: $scope.Designation
        };
        //If the flag is 1 the it si new record
        if ($scope.IsNewRecord === 1) {
            var promisePost = crudService.post(Employee);
            promisePost.then(function (pl) {
                $scope.EmpNo = pl.data.EmpNo;
                loadRecords();
            }, function (err) {
                console.log("Err" + err);
            });
        } else { //Else Edit the record
            var promisePut = crudService.put($scope.EmpNo, Employee);
            promisePut.then(function (pl) {
                $scope.Message = "Updated Successfuly";
                loadRecords();
            }, function (err) {
                console.log("Err" + err);
            });
        }



    };

    //Method to Delete
    $scope.delete = function () {
        var promiseDelete = crudService.delete($scope.EmpNo);
        promiseDelete.then(function (pl) {
            $scope.Message = "Deleted Successfuly";
            $scope.EmpNo = 0;
            $scope.EmpName = "";
            $scope.Salary = 0;
            $scope.DeptName = "";
            $scope.Designation = "";
            loadRecords();
        }, function (err) {
            console.log("Err" + err);
        });
    }

    //Method to Get Single Employee based on EmpNo
    $scope.get = function (Emp) {
        var promiseGetSingle = crudService.get(Emp.EmpNo);

        promiseGetSingle.then(function (pl) {
            var res = pl.data;
            $scope.EmpNo = res.EmpNo;
            $scope.EmpName = res.EmpName;
            $scope.Salary = res.Salary;
            $scope.DeptName = res.DeptName;
            $scope.Designation = res.Designation;

            $scope.IsNewRecord = 0;
        },
                  function (errorPl) {
                      console.log('failure loading Employee', errorPl);
                  });
    }
    //Clear the Scopr models
    $scope.clear = function () {
        $scope.IsNewRecord = 1;
        $scope.EmpNo = 0;
        $scope.EmpName = "";
        $scope.Salary = 0;
        $scope.DeptName = "";
        $scope.Designation = "";
    }
});
```
