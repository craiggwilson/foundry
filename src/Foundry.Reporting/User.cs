﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Reporting
{
    public class User
    {
        private string _hashedPassword;
        private string _salt;
        private PasswordFormat _passwordFormat;

        public Guid Id { get; set; }

        public string Username { get; set; }

        public bool IsValidPassword(string plainTextPassword)
        {
            return true;
        }

        public static string HashPassword(PasswordFormat passwordFormat, string plainTextPassword)
        {
            return plainTextPassword;
        }
    }
}