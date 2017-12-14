rm python/ -r
flatc -p -o python/ NeodroidReactionModels.fbs --gen-onefile
flatc -p -o python/ NeodroidStateModels.fbs --gen-onefile
