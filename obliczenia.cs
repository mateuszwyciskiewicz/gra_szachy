using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    /// <summary>
    /// ta klasa słuzy do obliczen poruszania się pionków
    /// </summary>
    public static class obliczenia
    {
        /// <summary>
        /// zmienia
        /// </summary>
        /// <param name="argument">który gracz wykonuje ruch </param>
        /// <returns>zwraca prawde albo fałsz w zależnosci od tego która gracz wykonuje ruch</returns>
        public static bool ZmianaGracza(int argument)
        {
            if (argument == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// ta metoda słuzy do okreslania poprwnosci połozenia pionków na planszy
        /// </summary>
        /// <remarks></remarks>
        /// <param name="ti">ta zmienna okresla polozenie pionka w osi</param>
        /// <param name="tj"> ta zmienna okresla polozenie pionka w osi</param>
        /// <returns> zwraca czy pionek jest dobrze połozony</returns>
        public static bool NaPlanszy(int ti, int tj)
        {
            if (ti >= 8 || tj >= 8 || ti < 0 || tj < 0)
                return false;
            return true;
        }
    }
}
