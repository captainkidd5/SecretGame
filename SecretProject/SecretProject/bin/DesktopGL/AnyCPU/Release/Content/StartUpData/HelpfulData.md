## Animation Keys
1 = CutGrassDown

2 = right

3 = left

4 = up

5 = MiningDown

6 = right

7 = left

8 = up

9 = ChoppingDown

10 = right

11 = left

12 = up

## Tool Keys

0.  : no item held
1. Keys 1-20 reserved for special cases. 20-30+ for tool interaction types.

## Destructable Key Code

W,X,Y,Z

W = Tool Required
X = Hitpoints
Y = Color
Z = Sound

## To Fix XML Content Load not found thing:
Simply run the project in Debug mode first, then load content


## Delete chunks on startup - paste this into pre build event command line:
cd "Content\SaveFiles\Chunks"
del *.* /q

overwrite - https://stackoverflow.com/questions/1125968/how-do-i-force-git-pull-to-overwrite-local-files