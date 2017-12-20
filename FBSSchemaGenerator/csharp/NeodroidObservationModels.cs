// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Neodroid.FBS
{

using global::System;
using global::FlatBuffers;

public enum FBSObserverData : byte
{
 NONE = 0,
 FBSByteArray = 1,
 FBSNumeral = 2,
 FBSString = 3,
 FBSPosition = 4,
 FBSRotation = 5,
 FBSEulerTransform = 6,
 FBSQuaternionTransformObservation = 7,
 FBSBodyObservation = 8,
};

public enum FBSByteDataType : byte
{
 PNG = 0,
 JPEG = 1,
 Other = 2,
};

public struct FBSPosition : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSPosition GetRootAsFBSPosition(ByteBuffer _bb) { return GetRootAsFBSPosition(_bb, new FBSPosition()); }
  public static FBSPosition GetRootAsFBSPosition(ByteBuffer _bb, FBSPosition obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSPosition __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public FBSVector3? Position { get { int o = __p.__offset(4); return o != 0 ? (FBSVector3?)(new FBSVector3()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFBSPosition(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddPosition(FlatBufferBuilder builder, Offset<FBSVector3> positionOffset) { builder.AddStruct(0, positionOffset.Value, 0); }
  public static Offset<FBSPosition> EndFBSPosition(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSPosition>(o);
  }
};

public struct FBSRotation : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSRotation GetRootAsFBSRotation(ByteBuffer _bb) { return GetRootAsFBSRotation(_bb, new FBSRotation()); }
  public static FBSRotation GetRootAsFBSRotation(ByteBuffer _bb, FBSRotation obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSRotation __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public FBSVector3? Rotation { get { int o = __p.__offset(4); return o != 0 ? (FBSVector3?)(new FBSVector3()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFBSRotation(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddRotation(FlatBufferBuilder builder, Offset<FBSVector3> rotationOffset) { builder.AddStruct(0, rotationOffset.Value, 0); }
  public static Offset<FBSRotation> EndFBSRotation(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSRotation>(o);
  }
};

public struct FBSEulerTransform : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSEulerTransform GetRootAsFBSEulerTransform(ByteBuffer _bb) { return GetRootAsFBSEulerTransform(_bb, new FBSEulerTransform()); }
  public static FBSEulerTransform GetRootAsFBSEulerTransform(ByteBuffer _bb, FBSEulerTransform obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSEulerTransform __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public FBSVector3? Position { get { int o = __p.__offset(4); return o != 0 ? (FBSVector3?)(new FBSVector3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public FBSVector3? Rotation { get { int o = __p.__offset(6); return o != 0 ? (FBSVector3?)(new FBSVector3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public FBSVector3? Direction { get { int o = __p.__offset(8); return o != 0 ? (FBSVector3?)(new FBSVector3()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFBSEulerTransform(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddPosition(FlatBufferBuilder builder, Offset<FBSVector3> positionOffset) { builder.AddStruct(0, positionOffset.Value, 0); }
  public static void AddRotation(FlatBufferBuilder builder, Offset<FBSVector3> rotationOffset) { builder.AddStruct(1, rotationOffset.Value, 0); }
  public static void AddDirection(FlatBufferBuilder builder, Offset<FBSVector3> directionOffset) { builder.AddStruct(2, directionOffset.Value, 0); }
  public static Offset<FBSEulerTransform> EndFBSEulerTransform(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSEulerTransform>(o);
  }
};

public struct FBSQuaternionTransformObservation : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSQuaternionTransformObservation GetRootAsFBSQuaternionTransformObservation(ByteBuffer _bb) { return GetRootAsFBSQuaternionTransformObservation(_bb, new FBSQuaternionTransformObservation()); }
  public static FBSQuaternionTransformObservation GetRootAsFBSQuaternionTransformObservation(ByteBuffer _bb, FBSQuaternionTransformObservation obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSQuaternionTransformObservation __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public FBSQuaternionTransform? Transform { get { int o = __p.__offset(4); return o != 0 ? (FBSQuaternionTransform?)(new FBSQuaternionTransform()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFBSQuaternionTransformObservation(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddTransform(FlatBufferBuilder builder, Offset<FBSQuaternionTransform> transformOffset) { builder.AddStruct(0, transformOffset.Value, 0); }
  public static Offset<FBSQuaternionTransformObservation> EndFBSQuaternionTransformObservation(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSQuaternionTransformObservation>(o);
  }
};

public struct FBSBodyObservation : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSBodyObservation GetRootAsFBSBodyObservation(ByteBuffer _bb) { return GetRootAsFBSBodyObservation(_bb, new FBSBodyObservation()); }
  public static FBSBodyObservation GetRootAsFBSBodyObservation(ByteBuffer _bb, FBSBodyObservation obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSBodyObservation __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public FBSBody? Body { get { int o = __p.__offset(4); return o != 0 ? (FBSBody?)(new FBSBody()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartFBSBodyObservation(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddBody(FlatBufferBuilder builder, Offset<FBSBody> bodyOffset) { builder.AddStruct(0, bodyOffset.Value, 0); }
  public static Offset<FBSBodyObservation> EndFBSBodyObservation(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSBodyObservation>(o);
  }
};

public struct FBSByteArray : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSByteArray GetRootAsFBSByteArray(ByteBuffer _bb) { return GetRootAsFBSByteArray(_bb, new FBSByteArray()); }
  public static FBSByteArray GetRootAsFBSByteArray(ByteBuffer _bb, FBSByteArray obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSByteArray __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public byte ByteArray(int j) { int o = __p.__offset(4); return o != 0 ? __p.bb.Get(__p.__vector(o) + j * 1) : (byte)0; }
  public int ByteArrayLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }
  public ArraySegment<byte>? GetByteArrayBytes() { return __p.__vector_as_arraysegment(4); }
  public FBSByteDataType DataType { get { int o = __p.__offset(6); return o != 0 ? (FBSByteDataType)__p.bb.Get(o + __p.bb_pos) : FBSByteDataType.PNG; } }

  public static Offset<FBSByteArray> CreateFBSByteArray(FlatBufferBuilder builder,
      VectorOffset byte_arrayOffset = default(VectorOffset),
      FBSByteDataType data_type = FBSByteDataType.PNG) {
    builder.StartObject(2);
    FBSByteArray.AddByteArray(builder, byte_arrayOffset);
    FBSByteArray.AddDataType(builder, data_type);
    return FBSByteArray.EndFBSByteArray(builder);
  }

  public static void StartFBSByteArray(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddByteArray(FlatBufferBuilder builder, VectorOffset byteArrayOffset) { builder.AddOffset(0, byteArrayOffset.Value, 0); }
  public static VectorOffset CreateByteArrayVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static void StartByteArrayVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddDataType(FlatBufferBuilder builder, FBSByteDataType dataType) { builder.AddByte(1, (byte)dataType, 0); }
  public static Offset<FBSByteArray> EndFBSByteArray(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSByteArray>(o);
  }
};

public struct FBSNumeral : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSNumeral GetRootAsFBSNumeral(ByteBuffer _bb) { return GetRootAsFBSNumeral(_bb, new FBSNumeral()); }
  public static FBSNumeral GetRootAsFBSNumeral(ByteBuffer _bb, FBSNumeral obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSNumeral __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public double Value { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetDouble(o + __p.bb_pos) : (double)0.0; } }

  public static Offset<FBSNumeral> CreateFBSNumeral(FlatBufferBuilder builder,
      double value = 0.0) {
    builder.StartObject(1);
    FBSNumeral.AddValue(builder, value);
    return FBSNumeral.EndFBSNumeral(builder);
  }

  public static void StartFBSNumeral(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddValue(FlatBufferBuilder builder, double value) { builder.AddDouble(0, value, 0.0); }
  public static Offset<FBSNumeral> EndFBSNumeral(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSNumeral>(o);
  }
};

public struct FBSString : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FBSString GetRootAsFBSString(ByteBuffer _bb) { return GetRootAsFBSString(_bb, new FBSString()); }
  public static FBSString GetRootAsFBSString(ByteBuffer _bb, FBSString obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FBSString __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Value { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetValueBytes() { return __p.__vector_as_arraysegment(4); }

  public static Offset<FBSString> CreateFBSString(FlatBufferBuilder builder,
      StringOffset valueOffset = default(StringOffset)) {
    builder.StartObject(1);
    FBSString.AddValue(builder, valueOffset);
    return FBSString.EndFBSString(builder);
  }

  public static void StartFBSString(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddValue(FlatBufferBuilder builder, StringOffset valueOffset) { builder.AddOffset(0, valueOffset.Value, 0); }
  public static Offset<FBSString> EndFBSString(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FBSString>(o);
  }
};


}
