using FlatBuffers;

namespace Neodroid.Messaging.CustomFBS {
  public static class CustomFlatBufferImplementation {
    //Custom implementation of copying bytearray, faster than generated code
    public static VectorOffset CreateDataVectorAndAddAllDataAtOnce(FlatBufferBuilder builder, byte[] data) {
      builder.StartVector(
                          elemSize : 1,
                          count : data.Length,
                          alignment : 1);
      var additional_bytes = data.Length - 2;
      builder.Prep(
                   size : sizeof(byte),
                   additionalBytes : additional_bytes * sizeof(byte));
      //Buffer.BlockCopy (data, 0, builder.DataBuffer.Data, builder.Offset, data.Length); // Would be even better
      for (var i = data.Length - 1; i >= 0; i--)
        builder.PutByte(x : data[i]);
      return builder.EndVector();
    }
  }
}
