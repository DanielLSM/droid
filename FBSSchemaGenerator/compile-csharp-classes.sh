rm csharp/ -r
mkdir csharp
flatc -n -o csharp/ NeodroidReactionModels.fbs --gen-onefile
flatc -n -o csharp/ NeodroidStateModels.fbs --gen-onefile
