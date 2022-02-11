using Stripe;
using Stripe.Checkout;

var stripeApiKey = Environment.GetEnvironmentVariable("StripeSecretApiKey");
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

var intent = new SessionPaymentIntentDataOptions {
    Description = $"Test Order",
    ReceiptEmail = "dylan@ndcconferences.com"
};

var sessionCreateOptions = new SessionCreateOptions {
    PaymentIntentData = intent,
    CustomerEmail = "customer@example.com",
    ClientReferenceId = Guid.NewGuid().ToString(),
    PaymentMethodTypes = new List<string> { "card" },
    LineItems = lineItems,
    SuccessUrl = "https://example.com/success?stripeSessionId={CHECKOUT_SESSION_ID}",
    CancelUrl = "https://example.com/cancel?stripeSessionId={CHECKOUT_SESSION_ID}"
};

var sessionService = new SessionService();
var createdSession = sessionService.Create(sessionCreateOptions);
Console.WriteLine($"Session created! Session ID is {createdSession.Id}");
Console.WriteLine($"Payment URL: {createdSession.Url}");
Console.WriteLine($"Calling SessionService.Get({createdSession.Id}");
var retrievedSession = sessionService.Get(createdSession.Id);

var paymentIntentService = new PaymentIntentService();
var paymentIntentId = retrievedSession.PaymentIntentId;

var paymentIntentUpdateOptions = new PaymentIntentWithFulfillmentDateUpdateOptions {
    FulfillmentDateTimeOffset = fulfillmentDate
};
paymentIntentService.Update(paymentIntentId, paymentIntentUpdateOptions);
