using System;
using System.Collections.Generic;
using MelonLoader;

//using ParamLib;

namespace VXP
{
    public static class ParamManager
    {
        public static readonly List<HRParameter> Parameters = new List<HRParameter>()
        {
            //new IntParameter(hro => hro.expression, "VXP_Expression"),
            //new FloatParameter(hro => hro.confidence, "VXP_Confidence")
        };

        public class IntParameter : ParamLib.IntBaseParam, HRParameter
        {
            private string SetParamName = String.Empty;

            public IntParameter(Func<HROutput, int> getVal, string parameterName) : base(paramName: parameterName)
            {
                SetParamName = parameterName;
                MelonLogger.Msg($"IntParameter with ParameterName: {parameterName}, has been created!");
                Mod.OnParamsUpdated += (expression, confidence) =>
                {
                    HROutput hro = new HROutput()
                    {
                        expression = expression,
                        confidence = confidence
                    };
                    int valueToSet = getVal.Invoke(hro);
                    ParamValue = valueToSet;
                };
            }

            string HRParameter.GetParamName() => SetParamName;
            void HRParameter.ResetParam() => ResetParam();
            float HRParameter.GetParamValue() => ParamValue;
        }
        
        public class FloatParameter : ParamLib.FloatBaseParam, HRParameter
        {
            private string SetParamName = String.Empty;

            public FloatParameter(Func<HROutput, float> getVal, string parameterName) : base(parameterName)
            {
                SetParamName = parameterName;
                MelonLogger.Msg($"FloatParameter with ParameterName: {parameterName}, has been created!");
                Mod.OnParamsUpdated += (expression, confidence) =>
                {
                    HROutput hro = new HROutput()
                    {
                        expression = expression,
                        confidence = confidence
                    };
                    float valueToSet = getVal.Invoke(hro);
                    ParamValue = valueToSet;
                };
            }

            string HRParameter.GetParamName() => SetParamName;
            void HRParameter.ResetParam() => ResetParam();
            float HRParameter.GetParamValue() => ParamValue;
        }

        public class HROutput
        {
            public int expression;
            public float confidence;
        }

        public interface HRParameter
        {
            public string GetParamName();
            public float GetParamValue();
            public void ResetParam();
        }
    }
}