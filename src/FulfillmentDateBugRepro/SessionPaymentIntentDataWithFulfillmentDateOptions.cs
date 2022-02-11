using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
// This class extends the built-in SessionPaymentIntentDataOptions class, because the classes
// available in the Stripe.NET API don't appear to support the fulfillment_date field.
public class SessionPaymentIntentDataWithFulfillmentDateOptions : SessionPaymentIntentDataOptions {
    [JsonProperty("fulfillment_date")]
    public string FulfillmentDate => FulfillmentDateTimeOffset.ToUnixTimeSeconds().ToString();

    [JsonIgnore]
    public DateTimeOffset FulfillmentDateTimeOffset { get; set; }
}

public class PaymentIntentWithFulfillmentDateUpdateOptions : PaymentIntentUpdateOptions {
    [JsonProperty("fulfillment_date")]
    public string FulfillmentDate => FulfillmentDateTimeOffset.ToUnixTimeSeconds().ToString();

    [JsonIgnore]
    public DateTimeOffset FulfillmentDateTimeOffset { get; set; }
}
