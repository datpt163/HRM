﻿namespace Capstone.Application.Module.Users.Response
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Avatar { get; set; } = null!;

        public string? Address { get; set; }
        public int Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? BankAccount { get; set; }
        public string? BankAccountName { get; set; }
        public int Status { get; set; }
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UserName { get; set;}
        public string? PositionName { get; set; }
    }
}
