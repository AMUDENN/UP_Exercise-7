using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP_Exercise_7
{
    internal class ExceptionFunctions
    {
        public static Exception Ex_Double(string input_str, string data, double min = -1.7976931348623158e+308, double max = 1.7976931348623158e+308)
        {
            try
            {
                double result = Convert.ToDouble(input_str);
                if (result < min)
                {
                    throw new Exception($"Ошибка! Введено число {data} меньше минимального предела: {min}");
                }
                if (result > max)
                {
                    throw new Exception($"Ошибка! Введено число {data} больше максимального предела: {max}");
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }

        public static Exception Ex_Int(string input_str, string data, int min = -2147483648, int max = 2147483647)
        {
            try
            {
                int result = Convert.ToInt32(input_str);
                if (result < min)
                {
                    throw new Exception($"Ошибка! Введено число {data} меньше минимального предела: {min}");
                }
                if (result > max)
                {
                    throw new Exception($"Ошибка! Введено число {data} больше максимального предела: {max}");
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }
    }

}
