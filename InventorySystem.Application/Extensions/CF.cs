namespace InventorySystem.Application.Extensions
{
    public static class CF
    {
        public static int GetInt(object? value, int defaultValue = 0)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            if (value is int i)
                return i;

            if (int.TryParse(value.ToString(), out var result))
                return result;

            return defaultValue;
        }

        public static decimal GetDecimal(object? value, decimal defaultValue = 0m)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            if (value is decimal d)
                return d;

            if (decimal.TryParse(value.ToString(), out var result))
                return result;

            return defaultValue;
        }

        public static bool GetBool(object? value, bool defaultValue = false)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            if (value is bool b)
                return b;

            // SQL bit (0 / 1)
            if (value is int i)
                return i == 1;

            var str = value.ToString()?.Trim().ToLower();

            if (str == "1" || str == "true" || str == "yes" || str == "y")
                return true;

            if (str == "0" || str == "false" || str == "no" || str == "n")
                return false;

            return defaultValue;
        }

    }
}
