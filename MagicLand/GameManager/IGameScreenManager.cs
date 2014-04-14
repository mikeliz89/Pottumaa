using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicLand
{
    /// <summary>
    /// Tämä rajapinta määrittelee metodit, joiden avulla voidaan näyttöjä viedä pinoon
    /// eli tuoda esille ja poistaa pinosta, vaihtaa näyttöä ja tutkia mikä on pinossa päällimmäisenä.
    /// </summary>
    public interface IGameScreenManager
    {
        // Tutkii mikä näyttö on päällimmäisenä
        GameScreen TopScreen
        {
            get;
        }

        void PopScreen();   // Poistaa näytön pinosta
        void PushScreen(GameScreen Screen); // Vie näytön pinoo
        void ChangeScreen(GameScreen NewScreen);    // Vaihtaa näyttö
        bool ContainsScreen(GameScreen Screen); // Tutkii onko näyttöä pinossa
        event EventHandler OnScreenChange;
    }
}
