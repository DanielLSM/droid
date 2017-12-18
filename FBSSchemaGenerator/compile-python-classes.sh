rm python/ -r
mkdir python
flatc -p -o python/ NeodroidReactionModels.fbs --gen-onefile
flatc -p -o python/ NeodroidStateModels.fbs --gen-onefile
