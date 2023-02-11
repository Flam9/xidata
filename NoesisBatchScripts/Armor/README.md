# Batch Exporting ARMOR

_Note: NOT Weapons_

This one is quite simple in that it can be done via `?cmode` however each time you export a piece of gear it will as you to provide the Skeleton, this obviously prevents full automation.

First, we need to setup an AHK that will detect the "Open" dialog and handle it. 

- AHK Code: [NoesisDialogue.ahk](NoesisDialogue.ahk)

Open this file and scroll down to the function `DetectNoesisWindow`

You will see I have hard coded a dat file:
- `E:\SquareEnix\SquareEnix\FINAL FANTASY XI\ROM\27\82.DAT`

This is for the Hume M, You'd obviously want to swap this depending on the race you are exporting. For this I am just going to explain Hume_Male, but the process is the same for all of them. You can find the correct Skeleton from AtlanaViewer. 

We will not be exporting animations, since you can do them separately later on.

Ensure that `AddSkeleton := true` 

This code works by simply looking for ANY Windows Open File dialog box. (Even if it's not Noesis)

Now go into your script code, in this case: 
- PHP Code: [NoesisArmor.php](NoesisArmor.php)

Edit the line: `$romfile  = 'Hume_Male.txt';` to whatever rom text file you're using, in this case it's fine as `Hume_Male`, I've included this file: [Hume_Male.txt](Hume_Male.txt) which is simply just the AtlanaViewer list files all bundled into 1 txt file and the `@none` entries removed.

Next, edit the following lines to your hard drive paths:

```php
$saveto   = 'X:\\project-vr\\HumeMaleArmor\\[[TYPE]]\\[[NAME]]\\[[NAME]].fbx';
$romdir   = 'E:\\SquareEnix\\SquareEnix\\FINAL FANTASY XI\\ROM\\[[ROM]]';
$noesis   = 'X:\\FF11 Movie\\3D - Noesis\\Noesis64.exe';
```

In this case I am saving to `HumeMaleArmor`, the `[[TYPE]]` will convert things like Head, Body, Legs etc.

These are the arguments I have included, which work good for UE4/5:

```
-noanims -ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11nolodchange -ff11optimizegeo -ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 270 -scale 60
```

It is important we do `-noanims` because we do not want to export animations with these files, those will be done separately as a batch process. 

I also highly recommend `-fbxtexrelonly -fbxtexext .png ` which will fix file path names in the output so you do not need to assign the textures yourself, it'll be done for you.

Now the fun begins!

- Start the **AHK NoesisDialogue.ahk** Script and process `F3` 
- You can stop it by pressing `F8`

Next:
- Start the `NoesisArmor.php` (or your own), eg: `php NoesisArmor.php`

You should see that it is extracting each piece of gear and automatically filling in the Noesis Dialog box.

Now you wait an hour for it to finish :D