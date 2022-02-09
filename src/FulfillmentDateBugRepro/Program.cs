using Stripe;
using Stripe.Checkout;

var stripeApiKey = Environment.GetEnvironmentVariable("StripeSeasdasdcretApiKey");
if (String.IsNullOrEmpty(stripeApiKey)) {
    Console.WriteLine("Please supply a Stripe Secret API key:");
    stripeApiKey = Console.ReadLine();
} else {
    Console.WriteLine($"Using Stripe API key: {stripeApiKey}");
}
StripeConfiguration.ApiKey = stripeApiKey;

var lineItems = new List<SessionLineItemOptions> {
    new SessionLineItemOptions { Name = "Apples", Amount = 10000, Quantity = 3, Currency = "GBP" },
    new SessionLineItemOptions { Name = "Bananas", Amount = 10000, Quantity = 6, Currency = "GBP" },
};

var fulfillmentDate = new DateTimeOffset(new DateTime(2022, 3, 4));

// If you use THIS chunk of code, it works just fine:
// var intent = new SessionPaymentIntentDataOptions {
//     Description = $"Test Order",
//     ReceiptEmail = "dylan@ndcconferences.com"
// };

// If you use THIS chunk of code, Stripe returns an HTTP 400 Bad Request:
// Unhandled exception. Stripe.StripeException: Received unknown parameter: payment_intent_data[fulfillment_date]
//    at Stripe.StripeClient.ProcessResponse[T](StripeResponse response) in D:\Projects\github\dylanbeattie\stripe-dotnet\src\Stripe.net\Infrastructure\Public\StripeClient.cs:line 153

var intent = new SessionPaymentIntentDataWithFulfillmentDateOptions {
    Description = $"Test Order",
    ReceiptEmail = "dylan@ndcconferences.com",
    FulfillmentDateTimeOffset = fulfillmentDate
};

var options = new SessionCreateOptions {
    PaymentIntentData = intent,
    CustomerEmail = "customer@example.com",
    ClientReferenceId = Guid.NewGuid().ToString(),
    PaymentMethodTypes = new List<string> { "card" },
    LineItems = lineItems,
    SuccessUrl = "https://example.com/success?stripeSessionId={CHECKOUT_SESSION_ID}",
    CancelUrl = "https://example.com/cancel?stripeSessionId={CHECKOUT_SESSION_ID}"
};

var sessionService = new SessionService();
var session = sessionService.Create(options);
Console.WriteLine("Session created.");
Console.WriteLine($"Payment URL: {session.Url}");
