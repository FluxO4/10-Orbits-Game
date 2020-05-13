using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Forces
{
    public static Vector3 Gravity(Transform body, float maxRange, float planetRange)
    {
        Vector3 finalForce = new Vector3(0, 0, 0);
        for (int i = 0; i < Game.Planets.Count; i++)
        {
            if (((Game.Planets[i].position - body.position).magnitude + Game.PlanetMasses[i]) < planetRange)
            {
                return ((Game.Constants.gravityConstant * Mathf.Pow(Game.PlanetMasses[i], 1f / 3f) / (Mathf.Pow((Game.Planets[i].position - body.position).magnitude, 2f))) * (Game.Planets[i].position - body.position).normalized);
            }
            if ((Game.Planets[i].position - body.position).magnitude < maxRange)
            {

                finalForce += ((Game.Constants.gravityConstant * Mathf.Pow(Game.PlanetMasses[i], 1f / 3f) / (Mathf.Pow((Game.Planets[i].position - body.position).magnitude, 2f))) * (Game.Planets[i].position - body.position).normalized);
            }
        }
        return finalForce;
    }
}