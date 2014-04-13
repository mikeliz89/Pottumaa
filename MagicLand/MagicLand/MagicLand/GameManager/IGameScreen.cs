using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicLand
{
    /// <summary>
    /// Rajapinta määrittää Screen-ominaisuuden, jonka avulla pelinäyttö-luokissa
    /// voidaan tutkia onko kyseinen näyttö päällimmäisenä pinossa.
    /// </summary>
    public interface IGameScreen
    {
        GameScreen Screen
        {
            get;
        }
    }
}
