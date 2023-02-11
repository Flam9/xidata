<?php

// List of roms to extract
$roms = [
    'Zones_BaseGame.txt',
    'Zones_RizeOfZilart.txt',
    'Zones_ChainsOfPromathia.txt',
    'Zones_TreasuresOfAhtUrhgan.txt',
    'Zones_WingsOfTheGoddess.txt',
    'Zones_Abyssea.txt',
    'Zones_Rhapsodies.txt',
    'Zones_SeekersOfAdoulin.txt'
];

// folders to everything
$saveto  = 'X:\\project-vr\\Zones\\[[EXPANSION]]\\[[NAME]]\\[[NAME]].fbx';
$romdir  = 'E:\\SquareEnix\\SquareEnix\\FINAL FANTASY XI\\[[ROM]]';
$noesis  = 'X:\\FF11 Movie\\3D - Noesis\\Noesis64.exe';

// noesis arguments
$args    = '-ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11nolodchange -ff11optimizegeo -ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 0 -scale 100';

// command structure
$command = "\"{$noesis}\" ?cmode \"{$romdir}\" \"{$saveto}\" {$args}";

// find replacements
$replace = [ "[[ROM]]", "[[EXPANSION]]", "[[NAME]]" ];


foreach ($roms as $romfile) {
    // grab all roms
    $dats = file_get_contents($romfile);
    $dats = explode(PHP_EOL, $dats);

    $expansion = explode("_", $romfile)[1];
    $expansion = explode(".", $expansion)[0];

    echo ("----- Starting: {$expansion} -----\n");

    foreach ($dats as $dat) {
        $dat = trim($dat);

        if (empty($dat)) {
            continue;
        }

        [$zonename, $datfile] = explode('	', $dat);

        // format the zone name
        $zonename = str_ireplace(' ', '_', $zonename);
        $zonename = preg_replace("/[^a-z0-9._]+/i", "", $zonename);

        // buld the command
        $run = str_ireplace($replace, [$datfile, $expansion, $zonename], $command);

        // create the required folder for the zone
        $directory = dirname(str_ireplace($replace, [null, $expansion, $zonename], $saveto));

        if (!is_Dir($directory)) {
            mkdir($directory, 0777, true);
        }
        
        echo ("Extracting: {$expansion} - {$zonename}\n");

        shell_exec($run);

        echo ("> Complete! \n");
    }
}