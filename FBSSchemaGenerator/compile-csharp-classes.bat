rd /s /q csharp\
flatc -n -o csharp\ NeodroidSharedModels.fbs --gen-onefile
flatc -n -o csharp\ NeodroidReactionModels.fbs --gen-onefile
flatc -n -o csharp\ NeodroidObservationModels.fbs --gen-onefile
flatc -n -o csharp\ NeodroidStateModels.fbs --gen-onefile
::@pause
