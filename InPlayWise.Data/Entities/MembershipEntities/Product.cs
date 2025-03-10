using System.Text.Json.Serialization;

namespace InPlayWise.Data.Entities.MembershipEntities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StripeId { get; set; }
		//public Guid FeatureId { get; set; }
        public Guid SubscriptionId { get; set; }

        [JsonIgnore]
        public List<Subscription> Subscriptions { get; set; }

		[JsonIgnore]
        public List<Price> Price { get; set; }

        [JsonIgnore]
        public PlanFeatures Features { get; set; }

    }
}
