// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Neodroid.FBS.State
{

using global::System;
using global::FlatBuffers;

public enum FObservation : byte
{
 NONE = 0,
 FSingle = 1,
 FDouble = 2,
 FTriple = 3,
 FQuadruple = 4,
 FArray = 5,
 FRB = 6,
 FET = 7,
 FQT = 8,
 FString = 9,
 FByteArray = 10,
};

public enum FByteDataType : byte
{
 PNG = 0,
 JPEG = 1,
 Other = 2,
};

public struct FSingle : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FSingle GetRootAsFSingle(ByteBuffer _bb) { return GetRootAsFSingle(_bb, new FSingle()); }
  public static FSingle GetRootAsFSingle(ByteBuffer _bb, FSingle obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FSingle __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public double Value { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetDouble(o + __p.bb_pos) : (double)0.0; } }
  public Neodroid.FBS.FRange? Range { get { int o = __p.__offset(6); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFSingle(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddValue(FlatBufferBuilder builder, double value) { builder.AddDouble(0, value, 0.0); }
  public static void AddRange(FlatBufferBuilder builder, Offset<Neodroid.FBS.FRange> rangeOffset) { builder.AddStruct(1, rangeOffset.Value, 0); }
  public static Offset<FSingle> EndFSingle(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FSingle>(o);
  }
};

public struct FDouble : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FDouble GetRootAsFDouble(ByteBuffer _bb) { return GetRootAsFDouble(_bb, new FDouble()); }
  public static FDouble GetRootAsFDouble(ByteBuffer _bb, FDouble obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FDouble __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Neodroid.FBS.FVector2? Vec2 { get { int o = __p.__offset(4); return o != 0 ? (Neodroid.FBS.FVector2?)(new Neodroid.FBS.FVector2()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Neodroid.FBS.FRange? XRange { get { int o = __p.__offset(6); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Neodroid.FBS.FRange? YRange { get { int o = __p.__offset(8); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFDouble(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddVec2(FlatBufferBuilder builder, Offset<Neodroid.FBS.FVector2> vec2Offset) { builder.AddStruct(0, vec2Offset.Value, 0); }
  public static void AddXRange(FlatBufferBuilder builder, Offset<Neodroid.FBS.FRange> xRangeOffset) { builder.AddStruct(1, xRangeOffset.Value, 0); }
  public static void AddYRange(FlatBufferBuilder builder, Offset<Neodroid.FBS.FRange> yRangeOffset) { builder.AddStruct(2, yRangeOffset.Value, 0); }
  public static Offset<FDouble> EndFDouble(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FDouble>(o);
  }
};

public struct FTriple : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FTriple GetRootAsFTriple(ByteBuffer _bb) { return GetRootAsFTriple(_bb, new FTriple()); }
  public static FTriple GetRootAsFTriple(ByteBuffer _bb, FTriple obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FTriple __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Neodroid.FBS.FVector3? Vec3 { get { int o = __p.__offset(4); return o != 0 ? (Neodroid.FBS.FVector3?)(new Neodroid.FBS.FVector3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Neodroid.FBS.FRange? XRange { get { int o = __p.__offset(6); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Neodroid.FBS.FRange? YRange { get { int o = __p.__offset(8); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Neodroid.FBS.FRange? ZRange { get { int o = __p.__offset(10); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFTriple(FlatBufferBuilder builder) { builder.StartObject(4); }
  public static void AddVec3(FlatBufferBuilder builder, Offset<Neodroid.FBS.FVector3> vec3Offset) { builder.AddStruct(0, vec3Offset.Value, 0); }
  public static void AddXRange(FlatBufferBuilder builder, Offset<Neodroid.FBS.FRange> xRangeOffset) { builder.AddStruct(1, xRangeOffset.Value, 0); }
  public static void AddYRange(FlatBufferBuilder builder, Offset<Neodroid.FBS.FRange> yRangeOffset) { builder.AddStruct(2, yRangeOffset.Value, 0); }
  public static void AddZRange(FlatBufferBuilder builder, Offset<Neodroid.FBS.FRange> zRangeOffset) { builder.AddStruct(3, zRangeOffset.Value, 0); }
  public static Offset<FTriple> EndFTriple(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // vec3
    return new Offset<FTriple>(o);
  }
};

public struct FQuadruple : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FQuadruple GetRootAsFQuadruple(ByteBuffer _bb) { return GetRootAsFQuadruple(_bb, new FQuadruple()); }
  public static FQuadruple GetRootAsFQuadruple(ByteBuffer _bb, FQuadruple obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FQuadruple __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Neodroid.FBS.FQuaternion? Quat { get { int o = __p.__offset(4); return o != 0 ? (Neodroid.FBS.FQuaternion?)(new Neodroid.FBS.FQuaternion()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Neodroid.FBS.FRange? XRange { get { int o = __p.__offset(6); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Neodroid.FBS.FRange? YRange { get { int o = __p.__offset(8); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Neodroid.FBS.FRange? ZRange { get { int o = __p.__offset(10); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Neodroid.FBS.FRange? WRange { get { int o = __p.__offset(12); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFQuadruple(FlatBufferBuilder builder) { builder.StartObject(5); }
  public static void AddQuat(FlatBufferBuilder builder, Offset<Neodroid.FBS.FQuaternion> quatOffset) { builder.AddStruct(0, quatOffset.Value, 0); }
  public static void AddXRange(FlatBufferBuilder builder, Offset<Neodroid.FBS.FRange> xRangeOffset) { builder.AddStruct(1, xRangeOffset.Value, 0); }
  public static void AddYRange(FlatBufferBuilder builder, Offset<Neodroid.FBS.FRange> yRangeOffset) { builder.AddStruct(2, yRangeOffset.Value, 0); }
  public static void AddZRange(FlatBufferBuilder builder, Offset<Neodroid.FBS.FRange> zRangeOffset) { builder.AddStruct(3, zRangeOffset.Value, 0); }
  public static void AddWRange(FlatBufferBuilder builder, Offset<Neodroid.FBS.FRange> wRangeOffset) { builder.AddStruct(4, wRangeOffset.Value, 0); }
  public static Offset<FQuadruple> EndFQuadruple(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // quat
    return new Offset<FQuadruple>(o);
  }
};

public struct FArray : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FArray GetRootAsFArray(ByteBuffer _bb) { return GetRootAsFArray(_bb, new FArray()); }
  public static FArray GetRootAsFArray(ByteBuffer _bb, FArray obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FArray __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public double Array(int j) { int o = __p.__offset(4); return o != 0 ? __p.bb.GetDouble(__p.__vector(o) + j * 8) : (double)0; }
  public int ArrayLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }
  public ArraySegment<byte>? GetArrayBytes() { return __p.__vector_as_arraysegment(4); }
  public Neodroid.FBS.FRange? Ranges(int j) { int o = __p.__offset(6); return o != 0 ? (Neodroid.FBS.FRange?)(new Neodroid.FBS.FRange()).__assign(__p.__vector(o) + j * 12, __p.bb) : null; }
  public int RangesLength { get { int o = __p.__offset(6); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<FArray> CreateFArray(FlatBufferBuilder builder,
      VectorOffset arrayOffset = default(VectorOffset),
      VectorOffset rangesOffset = default(VectorOffset)) {
    builder.StartObject(2);
    FArray.AddRanges(builder, rangesOffset);
    FArray.AddArray(builder, arrayOffset);
    return FArray.EndFArray(builder);
  }

  public static void StartFArray(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddArray(FlatBufferBuilder builder, VectorOffset arrayOffset) { builder.AddOffset(0, arrayOffset.Value, 0); }
  public static VectorOffset CreateArrayVector(FlatBufferBuilder builder, double[] data) { builder.StartVector(8, data.Length, 8); for (int i = data.Length - 1; i >= 0; i--) builder.AddDouble(data[i]); return builder.EndVector(); }
  public static void StartArrayVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(8, numElems, 8); }
  public static void AddRanges(FlatBufferBuilder builder, VectorOffset rangesOffset) { builder.AddOffset(1, rangesOffset.Value, 0); }
  public static void StartRangesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(12, numElems, 4); }
  public static Offset<FArray> EndFArray(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // array
    return new Offset<FArray>(o);
  }
};

public struct FRB : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FRB GetRootAsFRB(ByteBuffer _bb) { return GetRootAsFRB(_bb, new FRB()); }
  public static FRB GetRootAsFRB(ByteBuffer _bb, FRB obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FRB __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Neodroid.FBS.FBody? Body { get { int o = __p.__offset(4); return o != 0 ? (Neodroid.FBS.FBody?)(new Neodroid.FBS.FBody()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFRB(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddBody(FlatBufferBuilder builder, Offset<Neodroid.FBS.FBody> bodyOffset) { builder.AddStruct(0, bodyOffset.Value, 0); }
  public static Offset<FRB> EndFRB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // body
    return new Offset<FRB>(o);
  }
};

public struct FET : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FET GetRootAsFET(ByteBuffer _bb) { return GetRootAsFET(_bb, new FET()); }
  public static FET GetRootAsFET(ByteBuffer _bb, FET obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FET __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Neodroid.FBS.FEulerTransform? Transform { get { int o = __p.__offset(4); return o != 0 ? (Neodroid.FBS.FEulerTransform?)(new Neodroid.FBS.FEulerTransform()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFET(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddTransform(FlatBufferBuilder builder, Offset<Neodroid.FBS.FEulerTransform> transformOffset) { builder.AddStruct(0, transformOffset.Value, 0); }
  public static Offset<FET> EndFET(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // transform
    return new Offset<FET>(o);
  }
};

public struct FQT : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FQT GetRootAsFQT(ByteBuffer _bb) { return GetRootAsFQT(_bb, new FQT()); }
  public static FQT GetRootAsFQT(ByteBuffer _bb, FQT obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FQT __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Neodroid.FBS.FQuaternionTransform? Transform { get { int o = __p.__offset(4); return o != 0 ? (Neodroid.FBS.FQuaternionTransform?)(new Neodroid.FBS.FQuaternionTransform()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFQT(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddTransform(FlatBufferBuilder builder, Offset<Neodroid.FBS.FQuaternionTransform> transformOffset) { builder.AddStruct(0, transformOffset.Value, 0); }
  public static Offset<FQT> EndFQT(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // transform
    return new Offset<FQT>(o);
  }
};

public struct FString : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FString GetRootAsFString(ByteBuffer _bb) { return GetRootAsFString(_bb, new FString()); }
  public static FString GetRootAsFString(ByteBuffer _bb, FString obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FString __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Str { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetStrBytes() { return __p.__vector_as_arraysegment(4); }

  public static Offset<FString> CreateFString(FlatBufferBuilder builder,
      StringOffset strOffset = default(StringOffset)) {
    builder.StartObject(1);
    FString.AddStr(builder, strOffset);
    return FString.EndFString(builder);
  }

  public static void StartFString(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddStr(FlatBufferBuilder builder, StringOffset strOffset) { builder.AddOffset(0, strOffset.Value, 0); }
  public static Offset<FString> EndFString(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // str
    return new Offset<FString>(o);
  }
};

public struct FByteArray : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FByteArray GetRootAsFByteArray(ByteBuffer _bb) { return GetRootAsFByteArray(_bb, new FByteArray()); }
  public static FByteArray GetRootAsFByteArray(ByteBuffer _bb, FByteArray obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FByteArray __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public byte Bytes(int j) { int o = __p.__offset(4); return o != 0 ? __p.bb.Get(__p.__vector(o) + j * 1) : (byte)0; }
  public int BytesLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }
  public ArraySegment<byte>? GetBytesBytes() { return __p.__vector_as_arraysegment(4); }
  public FByteDataType Type { get { int o = __p.__offset(6); return o != 0 ? (FByteDataType)__p.bb.Get(o + __p.bb_pos) : FByteDataType.PNG; } }

  public static Offset<FByteArray> CreateFByteArray(FlatBufferBuilder builder,
      VectorOffset bytesOffset = default(VectorOffset),
      FByteDataType type = FByteDataType.PNG) {
    builder.StartObject(2);
    FByteArray.AddBytes(builder, bytesOffset);
    FByteArray.AddType(builder, type);
    return FByteArray.EndFByteArray(builder);
  }

  public static void StartFByteArray(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddBytes(FlatBufferBuilder builder, VectorOffset bytesOffset) { builder.AddOffset(0, bytesOffset.Value, 0); }
  public static VectorOffset CreateBytesVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static void StartBytesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddType(FlatBufferBuilder builder, FByteDataType type) { builder.AddByte(1, (byte)type, 0); }
  public static Offset<FByteArray> EndFByteArray(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // bytes
    return new Offset<FByteArray>(o);
  }
};


}
