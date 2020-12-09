using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvencaLib
{
    public static class AvencaTools
    {
        // --------- Public Methods --------------------------------------------------------
        public static bool VerifyCPF(string cpf)
        {
            if (cpf.Length != 11)
                return false;
            if (cpf == Modulo11(Modulo11(cpf.Substring(0, 9))))
                return true;
            else return false;
        }



        // --------- Private Methods -------------------------------------------------------
        private static string Modulo11(string cpf)
        {
            long nCpf;
            int iResult = 0;

            if (Int64.TryParse(cpf, out nCpf))
            {
                int m = 2;
                for (int i = cpf.Length - 1; i >= 0; i--, m++)
                {
                    iResult += Int32.Parse(cpf[i].ToString()) * m;
                }
                iResult = 11 - (iResult % 11 > 1 ? iResult % 11 : 11);
                return cpf + iResult.ToString();
            }
            else return "ERROR";
        }
    }
}
