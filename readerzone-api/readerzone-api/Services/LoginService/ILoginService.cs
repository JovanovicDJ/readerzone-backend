﻿using readerzone_api.Models;

namespace readerzone_api.Services.LoginService
{
    public interface ILoginService
    {       
        public string Login(string email, string password);

        public Customer RegisterCustomer(Customer customer, string password);

        public Employee RegisterEmployee(Employee employee, string password);

        public string ActivateAccount(int id);

        public void ForgottenPassword(string email);
        public void ResetPassword(string password, string token);
    }
}
