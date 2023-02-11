# Batch Exporting ZONES

This is the easiest one :D You do not need AHK for this batch process.

The `Zone_**.txt` files you see in this directory are basically built from this list: https://www.reddit.com/r/ffximodding/comments/9ndg2d/complete_list_of_zone_dats_by_zone_id/

I have split them into files per expansion.

Open up the  [NoesisZones.php](NoesisZones.php) code and edit the paths to your noesis, FF11 and where you'd like things to save:

```php
// folders to everything
$saveto  = 'X:\\project-vr\\Zones\\[[EXPANSION]]\\[[NAME]]\\[[NAME]].fbx';
$romdir  = 'E:\\SquareEnix\\SquareEnix\\FINAL FANTASY XI\\[[ROM]]';
$noesis  = 'X:\\FF11 Movie\\3D - Noesis\\Noesis64.exe';
```

The Advanced Arguments I have used are as follows:

```
-ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11nolodchange -ff11optimizegeo -ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 0 -scale 100
```

Edit them to your liking, the options I have picked are because:

- `-ff11bumpdir normals`: This formats bumps into normals, which provides better mesh light smoothing
- `-fbxtexrelonly -fbxtexext .png` This ensures textures get the correct paths
- `-rotate 180 0 0 -scale 100` Flips the map to the correct position and will also scale to 100 which is perfect for VR.

Once you've modified it how you like, run the code:

- `php NoesisZones.php`

This uses `?cmode` and will do everything for you, you can still use your PC.