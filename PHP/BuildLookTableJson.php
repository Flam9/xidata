<?php

$lookTable = [];

function buildLookTable() {
    global $lookTable;

    $dirJson = __DIR__ .'/LookDataJsons';
    $dirJsonScan = array_diff(scandir($dirJson), ['..', '.']);

    foreach ($dirJsonScan as $folderRace) {
        $jsonGearFiles = array_diff(scandir("{$dirJson}/{$folderRace}"), ['..', '.']);

        foreach ($jsonGearFiles as $jsonGearFile) {
            $jsonGear = file_get_contents("{$dirJson}/{$folderRace}/{$jsonGearFile}");
            $jsonGear = json_decode($jsonGear, true);

            $slot =  pathinfo($jsonGearFile, PATHINFO_FILENAME);

            $lookTable[$folderRace][$slot] = $jsonGear;
        }
    }
}

// Build look table ...
buildLookTable();

// Store for simplicity
file_put_contents(__DIR__ .'/LookTable.json', json_encode($lookTable, JSON_PRETTY_PRINT));