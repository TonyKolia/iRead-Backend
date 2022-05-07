﻿using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class User
    {
        public User()
        {
            Favorites = new HashSet<Favorite>();
            Orders = new HashSet<Order>();
            Ratings = new HashSet<Rating>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
        public DateTime LastLogin { get; set; }
        public int? UserCategory { get; set; }
        public int Active { get; set; }

        public virtual UserCategory? UserCategoryNavigation { get; set; }
        public virtual MemberContactInfo MemberContactInfo { get; set; } = null!;
        public virtual MemberPersonalInfo MemberPersonalInfo { get; set; } = null!;
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
