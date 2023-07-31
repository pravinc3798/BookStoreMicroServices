using BookStore.Orders.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.Orders.Entity
{
    public class OrderEntity
    {
        [Key]
        public string OrderId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPaid { get; set; } = false;
        public string OrderStatus { get; set; } = "Awaiting Payment";
        [NotMapped]
        public decimal OrderAmount { get; set; }
        [NotMapped]
        public UserEntity User { get; set; }
        [NotMapped]
        public BookEntity Book { get; set; }

    }
}
