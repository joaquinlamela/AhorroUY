using System;

namespace Domain
{
    public class Coupon
    {
        public Guid Id { get; set; }
        public DateTime Deadline { get; set; }
        public virtual Market Market { get; set; }
        public int Value { get; set; }

        public Coupon() { }

    }
}
