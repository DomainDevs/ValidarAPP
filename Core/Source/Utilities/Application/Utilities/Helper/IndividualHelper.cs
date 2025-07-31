using System;

namespace Sistran.Core.Application.Utilities.Helper
{
    public class IndividualHelper
    {
        public static int CalculateDigitVerify(string documentNumber)
        {
            Int64 nit1;
            int[] vpri;
            int dv1 = 0, x = 0, y = 0, z;
            if (!string.IsNullOrEmpty(documentNumber))
            {
                if (documentNumber.Length > 9)
                {
                    vpri = new int[15] { 3, 7, 13, 17, 19, 23, 29, 37, 41, 43, 47, 53, 59, 67, 71 };
                    nit1 = Convert.ToInt64(documentNumber);
                    z = documentNumber.Length;
                    for (int i = 0; i < z; i++)
                    {
                        y = Convert.ToInt32(documentNumber.Substring(i, 1));
                        x += (y * vpri[z - i]);
                    }

                    y = x % 11;

                    if (y > 1)
                    {
                        dv1 = 11 - y;
                    }
                    else
                    {
                        dv1 = y;
                    }
                }
                else
                {
                    dv1 = (int)Convert.ToInt64(documentNumber) % 10;
                }
            }
            return dv1;
        }

        public static string CalculateDocumentNumberWithOutDigitVerify(string documentNumber)
        {
            if (!string.IsNullOrEmpty(documentNumber))
            {
                if (documentNumber.Length > 9)
                {
                    documentNumber.Substring(0, 9);
                }

            }
            return documentNumber;
        }
    }
}
