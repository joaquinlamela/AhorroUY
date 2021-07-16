using DomainException;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public virtual List<Coupon> Coupons { get; set; }
        public virtual List<Product> Favorites { get; set; }

        public virtual List<Purchase> Purchases { get; set; }

        public User()
        {
            Coupons = new List<Coupon>();
            Favorites = new List<Product>();
            Purchases = new List<Purchase>(); 
            
        }

        public void Validate()
        {
            if (IsInvalidName())
            {
                throw new UserException(MessageExceptionDomain.ErrorNameIsEmpty);
            }
            if (IsInvalidUserName())
            {
                throw new UserException(MessageExceptionDomain.ErrorUserNameIsEmpty);
            }
            if (IsInvalidPassword())
            {
                throw new UserException(MessageExceptionDomain.ErrorPasswordIsEmpty);
            }
        }

        public void ValidateLogin()
        {
            if (IsInvalidUserName())
            {
                throw new UserException(MessageExceptionDomain.ErrorUserNameIsEmpty);
            }
            if (IsInvalidPassword())
            {
                throw new UserException(MessageExceptionDomain.ErrorPasswordIsEmpty);
            }
        }

        private bool IsInvalidName()
        {
            return string.IsNullOrEmpty(Name);
        }

        private bool IsInvalidUserName()
        {
            return string.IsNullOrEmpty(Username);
        }

        private bool IsInvalidPassword()
        {
            return string.IsNullOrEmpty(Password);
        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Username == user.Username;
        }
    }
}
