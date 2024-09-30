using HarmonyLib;
using System;

namespace BuffMe
{
    public class BuffMe
    {
        public const string MONEY_VARID = "Money_Multiplier";
        public const string MONEY_DEFAULT_VAR = "200";

        public const string FAN_VARID = "Fan_Multiplier";
        public const string FAN_DEFAULT_VAR = "200";

        public const string STAMINA_USAGE_VARID = "Stamina_Usage_Multiplier";
        public const string STAMINA_USAGE_DEFAULT_VAR = "50";
    }

    [HarmonyPatch(typeof(resources), "_Add")]
    public class Resources_Add
    {
        public static bool Prefix(ref resources.type _type, ref long val)
        {
            double money_mult = double.Parse(variables.Get(BuffMe.MONEY_VARID) ?? BuffMe.MONEY_DEFAULT_VAR) / 100;
            double fan_mult = double.Parse(variables.Get(BuffMe.FAN_VARID) ?? BuffMe.FAN_DEFAULT_VAR) / 100;

            // Check that type is money
            if (_type == resources.type.money)
            {
                // Apply the multiplier only if the value is positive
                if (val > 0)
                {
                    val = (long)(val * money_mult);
                }
            }

            // Check that type is fan
            if (_type == resources.type.fans)
            {
                // Apply the multiplier only if the value is positive
                if (val > 0)
                {
                    val = (long)(val * fan_mult);
                }
            }

            return true;
        }
    }


    [HarmonyPatch(typeof(data_girls.girls), "AddFans", new Type[] { typeof(long), typeof(resources.fanType) })]
    public class DataGirls_Girls_AddFans
    {
        public static bool Prefix(ref long val, ref resources.fanType? fanType)
        {
            double fan_mult = double.Parse(variables.Get(BuffMe.FAN_VARID) ?? BuffMe.FAN_DEFAULT_VAR) / 100;

            if (val > 0)
            {
                val = (long)(val * fan_mult);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(data_girls.girls), "addParam")]
    public class DataGirls_Girls_addParam
    {
        public static bool Prefix(ref data_girls._paramType type, ref float val, ref bool IgnorePotential)
        {
            double staminaMultiplier = double.Parse(variables.Get(BuffMe.STAMINA_USAGE_VARID) ?? BuffMe.STAMINA_USAGE_DEFAULT_VAR) / 100;

            // Apply the multiplier only for negative values of physical or mental stamina
            if (val < 0 && (type == data_girls._paramType.physicalStamina || type == data_girls._paramType.mentalStamina))
            {
                val = (float)(val * staminaMultiplier);
            }

            return true;
        }
    }
}
