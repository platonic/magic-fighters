using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class AI
    {
        int enemyX, enemyY;
        int heroX, heroY;

        //this will be edited to contain the sprite names
        //chase algorithm
        public void chaseAlgorithm()
        {
            if (enemyX > heroX)
            {
                enemyX--;
            }
            else if (enemyX < heroX)
            {
                enemyX++;
            }

            if (enemyY > heroY)
            {
                enemyY++;
            }
            else if (enemyY < heroY)
            {
                enemyY--;
            }
        }
    }
}
