namespace Delivery_App.UtilityClass
{
    // extenstion method for toman format when displying Price
    public static class TomanExtenstionMethod
    {
        public static string Toman(this double value)
        {
            return value.ToString("#,0") + " تومان";
        }
    }
}
