namespace Delivery_App.UtilityClass
{
    // PriceHolder is a static class that serves as a global storage
    // for prices. It contains two static properties: PaidPrice and NotPaidPrice.
    // Being static, these properties retain their values for the lifetime of the
    // application and can be accessed and modified from any part of the code without
    // creating an instance of the class.
    public static class PriceHolder
    {
        public static string PaidPrice { get; set; }
        public static string NotPaidPrice { get; set; }
    }
}
