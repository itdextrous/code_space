using InPlayWise.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities.MembershipEntities
{
	public class Subscription
    {
        [Key]
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
		public string UserId { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public string CurrentSubscription { get; set; }
        public string PreviousSubscription { get; set; }
        public bool IsUpgrade { get; set; }
        public bool IsDowngrade { get; set; }
        public DateTime SubscriptionStart { get; set; }
        public DateTime SubscriptionEnd { get; set; }

    }
}