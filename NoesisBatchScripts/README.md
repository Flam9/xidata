# Noesis Batch Scripts

To batch export from Noesis you can either use the in-built GUI "batch process" tool or you can use the command line, or if you're like me and neither of those work you can then move onto AutoHotKey to completely automate the GUI to batch export everything!

Some things to get familiar with, Noesis has a command line argument called: `?cmode` which allows you to run an export command with the Input Source and the Output filename, then you can pass along your usual Advanced Arguments. The one downside is: You cannot pass skeletons and you cannot define Outputs (Texture or Animation), it will output as PNG but that's a non-issue.

Documentation for `?cmode` can be found here: http://richwhitehouse.com/noesis/nms/index.php?content=userman#sect_16

Different types of content can be batch exported in different ways, I primarilly use AtlanaViewer/Model Viewer as the "source" of my data, You can find my lists for both on this repository or just Google them!

All Advanced Arguments I post here are stuff I personally use for my VR Project as they are the perfect scale to fix the alphablend issue and they meet your real-life size scale. Change them how you like. I will add notes to some to explain why I've added specific arguments.

For these scripts you will need some kind of programming language that can run shell. I use PHP because I used to be a PHP Dev and it's just silly easy to script in it, but you can choose python or whatever and convert the code yourself.

Also remember: For those that use AHK you cannot use your PC while it's exporting!!

> Note: You cannot use **cmder**, you must use Powershell or Command Prompt. Cmder seems to mess up windows open dialog boxes being detected by AHK.

# Batch Scripts:

- Armor
- NPC
- Weapons
- Zones