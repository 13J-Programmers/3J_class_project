
3J class project
================

Todo
----

- Leap Motionを使うこと
- Unityを使うこと
- なんらかのゲームを作ること

Draft
-----

### 3Dテトリス
- 3Dテトリス : テトリスに奥行きを加えたゲーム
- Leap Motionによる操作（移動・回転）
- 独自の工夫 (+α)
	- ブロックを積むフィールドの回転 ?
	- 中心に向かう重力の追加 ?
- "Ecoris"
 	- 独自の要素として実装する方向で検討中

3Dテトリスに関する詳細な設計案
- [3Dテトリス設計草案](https://github.com/13J-Programmers/3J_class_project/blob/master/overview.md)
	

to Committer
------------

Unity内で生成される以下のファイルは中間ファイルであり、管理する必要がないので、コミットしないことにします。(.gitignoreに追加)
	
	# temporary files

	/[Ll]ibrary/
	/[Tt]emp/
	/[Oo]bj/
	/[Bb]uild/
	
	# Autogenerated VS/MD solution and project files
	*.csproj
	*.unityproj
	*.sln
	*.suo
	*.tmp
	*.user
	*.userprefs
	*.pidb
	*.booproj
	
	# Unity3D generated meta files
	*.pidb.meta
	
	# Unity3D Generated File On Crash Reports
	sysinfo.txt

Unityが自動生成するmetaファイルは、必ずコミットすることにします。

Contact us
----------

クラス企画について、質問や気になる点があれば、コメントしてください。(issues, class mail, line)


