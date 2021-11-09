using System;
using System.Collections;
using MelonLoader;
using UnityEngine;
using VXP.TensorProcessor;
using static VXP.ParamManager;

namespace VXP.Components
{
    [RegisterTypeInIl2Cpp]
    public class ParameterDriver : MonoBehaviour
    {
        public ParameterDriver(IntPtr ptr) : base(ptr) {}
        private int _expression;
        private float _confidence;
        private IEnumerator _processorCoroutine;
        
        private void Awake()
        {
            _processorCoroutine = ProcessCoroutine();
            MelonLogger.Msg("Awake");
        }

        private IEnumerator ProcessCoroutine()
        {
            for (;;)
            {
                (_expression, _confidence) = VocalProcessor.Process();
                MelonLogger.Msg("expression: {0}, confidence: {1}", _expression, _confidence);
                Mod.OnParamsUpdated.Invoke(_expression, _confidence);
                yield return new WaitForSeconds(Settings.UpdateInterval.Value);
            }
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Alpha0)) {
            //     _expression = (int) Expressions.None;
            //     MelonLogger.Msg("Set 0");
            // } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
            //     _expression = (int) Expressions.Neutral;
            //     MelonLogger.Msg("Set 1");
            // } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            //     _expression = (int) Expressions.Calm;
            //     MelonLogger.Msg("Set 2");
            // } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            //     _expression = (int) Expressions.Happy;
            //     MelonLogger.Msg("Set 3");
            // } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            //     _expression = (int) Expressions.Sad;
            //     MelonLogger.Msg("Set 4");
            // } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            //     _expression = (int) Expressions.Angry;
            //     MelonLogger.Msg("Set 5");
            // } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            //     _expression = (int) Expressions.Fearful;
            //     MelonLogger.Msg("Set 6");
            // } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            //     _expression = (int) Expressions.Disgust;
            //     MelonLogger.Msg("Set 7");
            // } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            //     _expression = (int) Expressions.Surprised;
            //     MelonLogger.Msg("Set 8");
            // }
            
            //Mod.OnExpressionUpdated.Invoke(_expression, _confidence);
        }

        private void OnEnable()
        {
            Parameters.Add(new IntParameter(values => values.expression, "VXP_Expression"));
            Parameters.Add(new FloatParameter(values => values.confidence, "VXP_Confidence"));
            
            MelonCoroutines.Start(_processorCoroutine);
            
            // foreach (var param in Parameters)
            //     param.ZeroParam();
            MelonLogger.Msg("OnEnable");
        }

        private void OnDisable()
        {
            MelonCoroutines.Stop(_processorCoroutine);
            Parameters.Clear();
            MelonLogger.Msg("OnDisable");
        }

        private void OnDestroy()
        {
            OnDisable();
            MelonLogger.Msg("OnDestroy");
        }
    }
}