﻿namespace Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Age { get; set; }

        public string Biography { get; set; }
        public string City { get; set; }
    }
}
