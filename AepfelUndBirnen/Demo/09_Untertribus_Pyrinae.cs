using System;
using System.Collections.Generic;

namespace AepfelUndBirnen
{
    /// <summary>
    /// http://de.wikipedia.org/wiki/Kernobstgew%C3%A4chse
    /// </summary>
    public abstract class Untertribus_Pyrinae : Tribus_Pyreae, IComparable<Untertribus_Pyrinae>
    {
        /// <summary />
        public ushort AnzahlKerne { get; protected set; }

        /// <summary>
        /// For <see cref="List{T}.Sort()"/>.
        /// </summary>
        /// <param name="other" />
        /// <returns />
        public int CompareTo(Untertribus_Pyrinae other)
        {
            if (other == null)
            {
                return 1;
            }

            var result = this.AnzahlKerne.CompareTo(other.AnzahlKerne);

            return result;
        }
    }
}