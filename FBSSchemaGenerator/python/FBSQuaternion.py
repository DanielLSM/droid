# automatically generated by the FlatBuffers compiler, do not modify

# namespace: FBS

import flatbuffers

class FBSQuaternion(object):
    __slots__ = ['_tab']

    # FBSQuaternion
    def Init(self, buf, pos):
        self._tab = flatbuffers.table.Table(buf, pos)

    # FBSQuaternion
    def X(self): return self._tab.Get(flatbuffers.number_types.Float64Flags, self._tab.Pos + flatbuffers.number_types.UOffsetTFlags.py_type(0))
    # FBSQuaternion
    def Y(self): return self._tab.Get(flatbuffers.number_types.Float64Flags, self._tab.Pos + flatbuffers.number_types.UOffsetTFlags.py_type(8))
    # FBSQuaternion
    def Z(self): return self._tab.Get(flatbuffers.number_types.Float64Flags, self._tab.Pos + flatbuffers.number_types.UOffsetTFlags.py_type(16))
    # FBSQuaternion
    def W(self): return self._tab.Get(flatbuffers.number_types.Float64Flags, self._tab.Pos + flatbuffers.number_types.UOffsetTFlags.py_type(24))

def CreateFBSQuaternion(builder, x, y, z, w):
    builder.Prep(8, 32)
    builder.PrependFloat64(w)
    builder.PrependFloat64(z)
    builder.PrependFloat64(y)
    builder.PrependFloat64(x)
    return builder.Offset()
