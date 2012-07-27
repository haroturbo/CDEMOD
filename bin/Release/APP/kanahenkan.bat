SET KAKASI="c:\kakasi\bin"
SET MECAB="C:\Program Files\MeCab\bin"
SET KANWADICTPATH=C:\kakasi\share\kakasi\kanwadict
SET ITAIJIDICTPATH=C:\kakasi\share\kakasi\itaijidict
%MECAB%\mecab CHEATsjis.DB -Oyomi -o mecab.db
%KAKASI%\kakasi -Jk -Hk -Kk -Ea < mecab.db > cheatmecab.db