# automatically generated by the FlatBuffers compiler, do not modify

# namespace: FBS

import flatbuffers

class FBSEulerTransform(object):
    __slots__ = ['_tab']

    @classmethod
    def GetRootAsFBSEulerTransform(cls, buf, offset):
        n = flatbuffers.encode.Get(flatbuffers.packer.uoffset, buf, offset)
        x = FBSEulerTransform()
        x.Init(buf, n + offset)
        return x

    # FBSEulerTransform
    def Init(self, buf, pos):
        self._tab = flatbuffers.table.Table(buf, pos)

    # FBSEulerTransform
    def Position(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(4))
        if o != 0:
            x = o + self._tab.Pos
            from .FBSVector3 import FBSVector3
            obj = FBSVector3()
            obj.Init(self._tab.Bytes, x)
            return obj
        return None

    # FBSEulerTransform
    def Rotation(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(6))
        if o != 0:
            x = o + self._tab.Pos
            from .FBSVector3 import FBSVector3
            obj = FBSVector3()
            obj.Init(self._tab.Bytes, x)
            return obj
        return None

    # FBSEulerTransform
    def Direction(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(8))
        if o != 0:
            x = o + self._tab.Pos
            from .FBSVector3 import FBSVector3
            obj = FBSVector3()
            obj.Init(self._tab.Bytes, x)
            return obj
        return None

def FBSEulerTransformStart(builder): builder.StartObject(3)
def FBSEulerTransformAddPosition(builder, position): builder.PrependStructSlot(0, flatbuffers.number_types.UOffsetTFlags.py_type(position), 0)
def FBSEulerTransformAddRotation(builder, rotation): builder.PrependStructSlot(1, flatbuffers.number_types.UOffsetTFlags.py_type(rotation), 0)
def FBSEulerTransformAddDirection(builder, direction): builder.PrependStructSlot(2, flatbuffers.number_types.UOffsetTFlags.py_type(direction), 0)
def FBSEulerTransformEnd(builder): return builder.EndObject()
