﻿using System;
using System.Linq;
using Caliburn.Micro;
using InstantDelivery.Core.Entities;
using InstantDelivery.Services;
using InstantDelivery.ViewModel.ViewModels.EmployeesViewModels;

namespace InstantDelivery.ViewModel
{
    public class VehicleManageViewModel : EmployeesViewModelBase
    {
        private readonly IEmployeeService employeesService;
        private readonly IVehiclesService vehiclesService;
        private readonly IWindowManager windowManager;
        private Employee selectedEmployee;
        private IQueryable<Employee> employees;
        private int currentPage = 1;

        public VehicleManageViewModel(IEmployeeService employeesService, IWindowManager windowManager, IVehiclesService vehiclesService)
        {
            this.employeesService = employeesService;
            this.windowManager = windowManager;
            this.vehiclesService = vehiclesService;
            Employees = employeesService.GetAll();
        }

        protected override IQueryable<Employee> GetEmployees()
        {
            return employeesService.GetAll();
        }

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                NotifyOfPropertyChange();
            }
        }

        public void EditVehicleForEmployee()
        {
            if (SelectedEmployee == null)
            {
                return;
            }
            var result = windowManager.ShowDialog(new SelectVehicleForEmployeeViewModel(employeesService, vehiclesService) {SelectedEmployee=SelectedEmployee, employeeService=employeesService, vehicleService=vehiclesService, SelectedVehicle=SelectedEmployee.Vehicle, Vehicles=vehiclesService.GetAllAvailableAndCurrent(SelectedEmployee.Vehicle)});
            if (result != true)
            {
                employeesService.Reload(SelectedEmployee);
                //TryClose(false);
            }
            else
            {
                employeesService.Save();
            }
        }
       

    }
}