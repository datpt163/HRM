﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Module.Account.Model
{
    public class RegisterRedisData
    {
        public int Otp { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;

        public RegisterRedisData(int otp, string password, string phone, string fullname, string avatar)
        {
            Otp = otp;
            Password = password;
            Phone = phone;
            Fullname = fullname;
            Avatar = avatar;
        }
    }
}
