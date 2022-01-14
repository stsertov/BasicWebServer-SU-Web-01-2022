﻿namespace BasicWebServer.Server.Common
{
    public class Guard
    {
        public static void AgainstNull(object value, string name = null)
        {
            if (value == null)
            {
                name ??= "Value";

                throw new ArgumentNullException($"{name} cannot be null.");
            }
        }
    }
}
