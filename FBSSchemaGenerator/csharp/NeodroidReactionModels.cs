// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Neodroid.Messaging.Models.Reaction
{

using global::System;
using global::FlatBuffers;

public struct FBSReaction : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSReaction GetRootAsFBSReaction(ByteBuffer _bb) { return GetRootAsFBSReaction(_bb, new FBSReaction()); }
  public static FBSReaction GetRootAsFBSReaction(ByteBuffer _bb, FBSReaction obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool FBSReactionBufferHasIdentifier(ByteBuffer _bb) { return Table.__has_identifier(_bb, "REAC"); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSReaction __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string EnvironmentName { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetEnvironmentNameBytes() { return __p.__vector_as_arraysegment(4); }
  public FBSMotion? Motions(int j) { int o = __p.__offset(6); return o != 0 ? (FBSMotion?)(new FBSMotion()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int MotionsLength { get { int o = __p.__offset(6); return o != 0 ? __p.__vector_len(o) : 0; } }
  public FBSConfiguration? Configurations(int j) { int o = __p.__offset(8); return o != 0 ? (FBSConfiguration?)(new FBSConfiguration()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int ConfigurationsLength { get { int o = __p.__offset(8); return o != 0 ? __p.__vector_len(o) : 0; } }
  public bool Reset { get { int o = __p.__offset(10); return o != 0 ? 0!=__p.bb.Get(o + __p.bb_pos) : (bool)false; } }

  public static Offset<FBSReaction> CreateFBSReaction(FlatBufferBuilder builder,
      StringOffset environment_nameOffset = default(StringOffset),
      VectorOffset motionsOffset = default(VectorOffset),
      VectorOffset configurationsOffset = default(VectorOffset),
      bool reset = false) {
    builder.StartObject(4);
    FBSReaction.AddConfigurations(builder, configurationsOffset);
    FBSReaction.AddMotions(builder, motionsOffset);
    FBSReaction.AddEnvironmentName(builder, environment_nameOffset);
    FBSReaction.AddReset(builder, reset);
    return FBSReaction.EndFBSReaction(builder);
  }

  public static void StartFBSReaction(FlatBufferBuilder builder) { builder.StartObject(4); }
  public static void AddEnvironmentName(FlatBufferBuilder builder, StringOffset environmentNameOffset) { builder.AddOffset(0, environmentNameOffset.Value, 0); }
  public static void AddMotions(FlatBufferBuilder builder, VectorOffset motionsOffset) { builder.AddOffset(1, motionsOffset.Value, 0); }
  public static VectorOffset CreateMotionsVector(FlatBufferBuilder builder, Offset<FBSMotion>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartMotionsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddConfigurations(FlatBufferBuilder builder, VectorOffset configurationsOffset) { builder.AddOffset(2, configurationsOffset.Value, 0); }
  public static VectorOffset CreateConfigurationsVector(FlatBufferBuilder builder, Offset<FBSConfiguration>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartConfigurationsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddReset(FlatBufferBuilder builder, bool reset) { builder.AddBool(3, reset, false); }
  public static Offset<FBSReaction> EndFBSReaction(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSReaction>(o);
  }
  public static void FinishFBSReactionBuffer(FlatBufferBuilder builder, Offset<FBSReaction> offset) { builder.Finish(offset.Value, "REAC"); }
};

public struct FBSMotion : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSMotion GetRootAsFBSMotion(ByteBuffer _bb) { return GetRootAsFBSMotion(_bb, new FBSMotion()); }
  public static FBSMotion GetRootAsFBSMotion(ByteBuffer _bb, FBSMotion obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSMotion __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string ActorName { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetActorNameBytes() { return __p.__vector_as_arraysegment(4); }
  public string MotorName { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetMotorNameBytes() { return __p.__vector_as_arraysegment(6); }
  public float Strength { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static Offset<FBSMotion> CreateFBSMotion(FlatBufferBuilder builder,
      StringOffset actor_nameOffset = default(StringOffset),
      StringOffset motor_nameOffset = default(StringOffset),
      float strength = 0.0f) {
    builder.StartObject(3);
    FBSMotion.AddStrength(builder, strength);
    FBSMotion.AddMotorName(builder, motor_nameOffset);
    FBSMotion.AddActorName(builder, actor_nameOffset);
    return FBSMotion.EndFBSMotion(builder);
  }

  public static void StartFBSMotion(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddActorName(FlatBufferBuilder builder, StringOffset actorNameOffset) { builder.AddOffset(0, actorNameOffset.Value, 0); }
  public static void AddMotorName(FlatBufferBuilder builder, StringOffset motorNameOffset) { builder.AddOffset(1, motorNameOffset.Value, 0); }
  public static void AddStrength(FlatBufferBuilder builder, float strength) { builder.AddFloat(2, strength, 0.0f); }
  public static Offset<FBSMotion> EndFBSMotion(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSMotion>(o);
  }
};

public struct FBSConfiguration : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSConfiguration GetRootAsFBSConfiguration(ByteBuffer _bb) { return GetRootAsFBSConfiguration(_bb, new FBSConfiguration()); }
  public static FBSConfiguration GetRootAsFBSConfiguration(ByteBuffer _bb, FBSConfiguration obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSConfiguration __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string ConfigurableName { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetConfigurableNameBytes() { return __p.__vector_as_arraysegment(4); }
  public float ConfigurableValue { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static Offset<FBSConfiguration> CreateFBSConfiguration(FlatBufferBuilder builder,
      StringOffset configurable_nameOffset = default(StringOffset),
      float configurable_value = 0.0f) {
    builder.StartObject(2);
    FBSConfiguration.AddConfigurableValue(builder, configurable_value);
    FBSConfiguration.AddConfigurableName(builder, configurable_nameOffset);
    return FBSConfiguration.EndFBSConfiguration(builder);
  }

  public static void StartFBSConfiguration(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddConfigurableName(FlatBufferBuilder builder, StringOffset configurableNameOffset) { builder.AddOffset(0, configurableNameOffset.Value, 0); }
  public static void AddConfigurableValue(FlatBufferBuilder builder, float configurableValue) { builder.AddFloat(1, configurableValue, 0.0f); }
  public static Offset<FBSConfiguration> EndFBSConfiguration(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSConfiguration>(o);
  }
};


}