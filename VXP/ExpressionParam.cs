using VRC.Playables;

namespace VXP
{
  public class ExpressionParam<T> where T: unmanaged
  {
    public readonly string Name;
    public readonly VRCPlayer Player;
    
    private AvatarPlayableController PlayableController => Player?.field_Private_VRC_AnimationController_0?.field_Private_IkController_0?.field_Private_AvatarAnimParamController_0?.field_Private_AvatarPlayableController_0;
    private AvatarParameter mParameter;

    public ExpressionParam(string name, VRCPlayer player = null)
    {
      Name = name;
      Player = player ? player : VRCPlayer.field_Internal_Static_VRCPlayer_0;
      
      if (PlayableController != null) {
        foreach (var kv in PlayableController.Method_Public_Dictionary_2_Int32_AvatarParameter_0()) {
          if (kv.Value.prop_String_0 == Name) {
            mParameter = kv.Value;
            break;
          }
        }
      }
    }

    public bool Valid => mParameter != null;

    public T? Value
    {
      get {
        if (mParameter == null) {
          return null;
        }

        if (mParameter.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0 == AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Float) {
          return (T)(object)mParameter.prop_Single_0;
        } else if (mParameter.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0 == AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Int) {
          return (T)(object)mParameter.prop_Int32_1;
        } else if (mParameter.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0 == AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Bool) {
          return (T)(object)mParameter.prop_Boolean_0;
        } else {
          throw new System.ArgumentException();
        }
      }

      set {
        if (mParameter == null) {
          return;
        }

        if (mParameter.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0 == AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Float) {
          mParameter.prop_Single_0 = (float)(object)value;
        } else if (mParameter.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0 == AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Int) {
          mParameter.prop_Int32_1 = (int)(object)value;
        } else if (mParameter.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0 == AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Bool) {
          mParameter.prop_Boolean_0 = (bool)(object)value;
        } else {
          throw new System.ArgumentException();
        }
      }
    }

    public bool Prioritized {
      get {
        var index = GetIndex();
        if (index != -1) {
          return PlayableController.Method_Private_Boolean_Int32_0(index);
        } else {
          return false;
        }
      }

      set {
        var index = GetIndex();
        if (index != -1) {
          if (value) {
            PlayableController.AssignPuppetChannel(index);
          } else {
            PlayableController.ClearPuppetChannel(index);
          }
        }
      }
    }

    private int GetIndex()
    {
      var parameters = Player?.prop_VRCAvatarManager_0?.prop_VRCAvatarDescriptor_0?.expressionParameters?.parameters;
      if (parameters != null) {
        for (var i = 0; i < parameters.Length; i++) {
          if (parameters[i].name == this.Name) {
            return i;
          }
        }
      }
      return -1;
    }
  }
}
